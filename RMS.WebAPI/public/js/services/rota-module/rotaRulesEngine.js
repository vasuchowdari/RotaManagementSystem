"use strict";

var app = angular.module("services");
app.factory("rotaRulesEngine", [
    "$rootScope",
    "shiftService",
    "shiftProfileService",
    "helperService",
    function($rootScope, shiftService, shiftProfileService, helperService) {
        var rotaRulesEngineFactory = {};

        // Event Handling
        var _subscribe = function(scope, callback, eventName) {
            var handler = $rootScope.$on(eventName, callback);
            scope.$on("$destroy", handler);
        };

        var _notify = function(data, eventName) {
            $rootScope.$emit(eventName, data);
        };

        rotaRulesEngineFactory.notify = _notify;
        rotaRulesEngineFactory.subscribe = _subscribe;


        // Event Subscription
        rotaRulesEngineFactory.subscribe($rootScope, function() {
            helperService.toasterError();
        }, "general-error-event");

        rotaRulesEngineFactory.subscribe($rootScope, function() {
            helperService.toasterError(
                "Consecutive Days Worked Error",
                "Staff member has exceeded maximum allowed number of consecutive working days.");
        }, "consecutive-working-days-error-event");


        // Pre-Check
        var _filterEmployees = function(resFilteredEmployees, startDate, endDate) {

            var tempArr = [];
            var rulesFilteredEmployees = angular.copy(resFilteredEmployees);

            _.forEach(resFilteredEmployees, function(employee) {
                var model = {
                    StaffId: employee.Id,
                    IsEmployee: true,
                    StartDate: startDate,
                    EndDate: endDate
                };


                // CONSECUTIVE DAYS WORKED RULE
                rotaRulesEngineFactory.returnNumberOfDaysConsecutivelyWorkedByRange(model).then(function(res) {
                    var days = res.data;
                    if (days >= 13) {
                        tempArr = _.filter(rulesFilteredEmployees, function(employee) {
                            if (employee.Id !== model.StaffId) {
                                return employee;
                            }
                        });

                        rulesFilteredEmployees = angular.copy(tempArr);
                        rotaRulesEngineFactory.notify(rulesFilteredEmployees, "employee-pre-filtered-event");
                    }
                }).catch(function(error) {
                    rotaRulesEngineFactory.notify(null, "general-error-event");
                    return;
                });
            });
        };

        var _consecutiveDaysWorkedRuleForSavingShifts = function(staffId, diffVal, startDate, endDate) {
            
            var model = {
                StaffId: staffId,
                IsEmployee: true,
                StartDate: startDate,
                EndDate: endDate
            };


            // CONSECUTIVE DAYS WORKED RULE
            rotaRulesEngineFactory.returnNumberOfDaysConsecutivelyWorkedByRange(model).then(function(res) {
                var days = (res.data + diffVal);
                if (days > 13) {
                    rotaRulesEngineFactory.notify(null, "consecutive-working-days-error-event");
                    return;
                }

                rotaRulesEngineFactory.notify(model.StaffId, "rules-passed-event");
            }).catch(function (error) {
                rotaRulesEngineFactory.notify(null, "general-error-event");
                return;
            });
        };

        var _shiftChecker = function(filteredStaff, startDate, endDate) {
            var copyOfFilteredStaff = angular.copy(filteredStaff);
            var staffIdsToRemove = [];
            var filteredStaffIds = [];

            _.forEach(copyOfFilteredStaff, function(mos) {
                filteredStaffIds.push(mos.Id);
            });

            var dataModel = {
                employeeIds: filteredStaffIds,
                shiftRangeStartDate: startDate,
                shiftRangeEndDate: endDate
            };

            shiftService.checkIfStaffAssignedToShiftByDateRange(dataModel).then(function(res) {
                staffIdsToRemove = res.data;

                _.forEach(staffIdsToRemove, function(idForRemoval) {
                    _.forEach(filteredStaff, function(mos) {
                        _.remove(copyOfFilteredStaff, function(cmos) {
                            if (cmos.Id === idForRemoval) {
                                return cmos;
                            }
                        });
                    });
                });

                rotaRulesEngineFactory.notify(copyOfFilteredStaff, "staff-shift-filtered-event");
            }).catch(function(error) {
                rotaRulesEngineFactory.notify(null, "general-error-event");
                return;
            });
        };

        var _leaveChecker = function(scope) {
            var tempArr1 = angular.copy(scope.siteEmployees);
            var tempArr2 = [];

            _.forEach(scope.siteEmployees, function(staffMember) {
                // first, do they have any approved, untaken leave
                _.forEach(staffMember.LeaveRequestModels, function(leaveModel) {
                    if (leaveModel.IsApproved) {

                        // [SRSD---SRED]
                        //				 [LRSD----LRED]
                        // IF shift range end date is before leave start date THEN keep
                        if (moment(scope.endDate) <= moment(leaveModel.StartDateTime)) {
                            // do nothing
                        }


                        // [SRSD-----------SRED]
                        //  	[LRSD----LRED]
                        // IF shift range start date is before leave start date AND 
                        // shift range end date is after leave end date THEN remove
                        if (moment(scope.startDate) <= moment(leaveModel.StartDateTime) &&
                            moment(scope.endDate) >= moment(leaveModel.EndDateTime)) {

                            tempArr2 = _.filter(tempArr1, function(mos) {
                                if (mos.Id !== staffMember.Id) {
                                    return mos;
                                }
                            });

                            tempArr1 = angular.copy(tempArr2);
                        }


                        // [SRSD-----SRED]
                        //		[LRSD----LRED]
                        // IF shift range start date is before leave start date AND 
                        // shift range end date is before leave end date AND
                        // shift range end date is after leave start date THEN remove
                        if (moment(scope.startDate) <= moment(leaveModel.StartDateTime) &&

                            (moment(scope.endDate) >= moment(leaveModel.StartDateTime) &&
                            moment(scope.endDate <= moment(leaveModel.EndDateTime)))
                            ) {

                            tempArr2 = _.filter(tempArr1, function(mos) {
                                if (mos.Id !== staffMember.Id) {
                                    return mos;
                                }
                            });

                            tempArr1 = angular.copy(tempArr2);
                        }


                        //		[SRSD---SRED]
                        // [LRSD-----------LRED]
                        // IF shift range start date is after leave start date AND 
                        // shift range end date is before leave end date THEN remove
                        if (moment(scope.startDate) >= moment(leaveModel.StartDateTime) &&
                            moment(scope.endDate) <= moment(leaveModel.EndDateTime)) {

                            tempArr2 = _.filter(tempArr1, function(mos) {
                                if (mos.Id !== staffMember.Id) {
                                    return mos;
                                }
                            });

                            tempArr1 = angular.copy(tempArr2);
                        }


                        //		[SRSD-----------SRED]
                        // [LRSD----LRED]
                        // IF shift range start date is after leave start date AND
                        // shift range start date is before leave end date AND
                        // shift range end date is after leave end date THEN remove
                        if (((moment(scope.startDate) >= moment(leaveModel.StartDateTime)) && (moment(scope.startDate) <= moment(leaveModel.EndDateTime))) &&
                            (moment(scope.endDate) >= moment(leaveModel.EndDateTime))) {
                                tempArr2 = _.filter(tempArr1, function(mos) {
                                    if (mos.Id !== staffMember.Id) {
                                        return mos;
                                    }
                                });

                                tempArr1 = angular.copy(tempArr2);
                        }


                        //				  [SRSD----SRED]
                        // [LRSD----LRED]
                        // IF shift range start date is after leave end date THEN keep
                        if (moment(scope.startDate) >= moment(leaveModel.EndDateTime)) {
                            // do nothing
                        }
                    }
                });
            });
            
            scope.filteredStaff = angular.copy(tempArr1);
        };


        // Init
        var _init = function(staffId, diffVal) {

            var model = {
                StaffId: staffId,
                IsEmployee: true
            };

            // CONSECUTIVE DAYS WORKED RULE
            rotaRulesEngineFactory.returnNumberOfDaysConsecutivelyWorked(model).then(function(res) {
                var days = (res.data + diffVal);
                if (days > 13) {
                    rotaRulesEngineFactory.notify(null, "consecutive-working-days-error-event");
                    return;
                }

                rotaRulesEngineFactory.notify(model.StaffId, "rules-passed-event");
                return;
            }).catch(function(error) {
                rotaRulesEngineFactory.notify(null, "general-error-event");
                return;
            });
        };


        // CONSECUTIVE DAYS WORKED RULE WITH DEFAULT DATE RANGE (TODAY MINUS 13 DAYS)
        var _returnNumberOfDaysConsecutivelyWorked = function(consDaysWorkedModel) {
            return shiftProfileService.consecutiveDaysWorked(consDaysWorkedModel);
        };

        // CONSECUTIVE DAYS WORKED WITH USER SPECIFIED DATE RANGE FROM UI
        var _returnNumberOfDaysConsecutivelyWorkedByRange = function(consDaysWorkedModel) {
            return shiftProfileService.consecutiveDaysWorkedByRange(consDaysWorkedModel);
        };


        // TODO: RENAME THIS
        rotaRulesEngineFactory.init = _init;

        rotaRulesEngineFactory.consecutiveDaysWorkedRuleForSavingShifts = _consecutiveDaysWorkedRuleForSavingShifts;
        rotaRulesEngineFactory.filterEmployees = _filterEmployees;
        rotaRulesEngineFactory.leaveChecker = _leaveChecker;
        rotaRulesEngineFactory.returnNumberOfDaysConsecutivelyWorked = _returnNumberOfDaysConsecutivelyWorked;
        rotaRulesEngineFactory.returnNumberOfDaysConsecutivelyWorkedByRange = _returnNumberOfDaysConsecutivelyWorkedByRange;
        rotaRulesEngineFactory.shiftChecker = _shiftChecker;

        return rotaRulesEngineFactory;
    }
]);