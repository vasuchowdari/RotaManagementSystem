"use strict";

var app = angular.module("services");
app.factory("rotaDialogService", [
    "$rootScope",
    "helperService",
    "managerAreaService",
    "localStorageService",
    "rotaRulesEngine",
    function($rootScope, helperService, managerAreaService, localStorageService, rotaRulesEngine) {
        var rotaDialogServiceFactory = {};

        // Objects
        var _dateTimePickerConfig = {
            startView: "day",
            minView: "day"
        };

        var _rotaModel = {
            CalendarResourceRequirementModel: {
                IsActive: true,
                CalendarId: null,
                ResourceId: null,
                StartDate: moment(),
                EndDate: moment()
            },
            ShiftPatternModel: {
                IsActive: true,
                CalendarResourceRequirementId: null,
                ShiftModels: []
            }
        };

        var _newShiftModel = {
            Resource: {},
            ShiftTemplate: {},
            Employee: {}
        };


        // Functions
        // REDUNDANT. REFACTOR
        var _addShiftModel = function(rotaModel, staffId, shiftAssigned,
            startDate, endDate, shiftTemplateId) {

            if (!shiftAssigned) {
                rotaModel.CalendarResourceRequirementModel.ShiftModels.push({
                    IsActive: true,
                    IsAssigned: shiftAssigned,
                    ShiftTemplateId: shiftTemplateId,
                    ShiftPatternId: null,
                    EmployeeId: null,
                    StartDate: startDate,
                    EndDate: endDate
                });
            } else {
                rotaModel.CalendarResourceRequirementModel.ShiftModels.push({
                    IsActive: true,
                    IsAssigned: shiftAssigned,
                    ShiftTemplateId: shiftTemplateId,
                    ShiftPatternId: null,
                    EmployeeId: staffId,
                    StartDate: startDate,
                    EndDate: endDate
                });
            }
        };

        var _applyToDayOfWeek = function(startDate, shiftTemplate) {
            var dow = startDate.weekday();
            var skip = false;

            switch (dow) {
            case 0:
                // Sunday
                if (!shiftTemplate.Sun) {
                    skip = true;
                    break;
                }

                skip = false;
                break;
            case 1:
                // Monday
                if (!shiftTemplate.Mon) {
                    skip = true;
                    break;
                }

                skip = false;
                break;
            case 2:
                // Tuesday
                if (!shiftTemplate.Tue) {
                    skip = true;
                    break;
                }

                skip = false;
                break;
            case 3:
                // Wednesday
                if (!shiftTemplate.Wed) {
                    skip = true;
                    break;
                }

                skip = false;
                break;
            case 4:
                // Thursday
                if (!shiftTemplate.Thu) {
                    skip = true;
                    break;
                }

                skip = false;
                break;
            case 5:
                // Friday
                if (!shiftTemplate.Fri) {
                    skip = true;
                    break;
                }

                skip = false;
                break;
            case 6:
                // Saturday
                if (!shiftTemplate.Sat) {
                    skip = true;
                    break;
                }

                skip = false;
                break;
            }

            return skip;
        };

        var _checkIfContractResourceIdEqualsShiftModelResourceId = function(scope, workerModel) {
            if (workerModel.ContractModels[0].ResourceId === scope.shiftModel.Resource.Id) {
                return true;
            }

            return false;
        };

        var _checkIfOvernightShift = function(shiftModel) {
            if (shiftModel.ShiftTemplate.EndTime < shiftModel.ShiftTemplate.StartTime) {
                return true;
            }

            return false;
        };

        // CALL TO RULES ENGINE FOR CONSECUTIVE DAYS WORKED RULE VALIDATION
        var _filterEmployeesOnConsecutiveDaysWorked = function (scope) {
            rotaDialogServiceFactory.populateFilteredStaffByResource(scope);

            // remove Bank Employees from filtered and store for use later
            //managerAreaService.filteredBankEmployeesStore = _.remove(scope.filteredStaff, { "EmployeeTypeId": 4 });

            // RULES ENGINE TO FILTER FURTHER
            rotaRulesEngine.filterEmployees(scope.filteredStaff, scope.startDate, scope.endDate);
        };

        // CALL TO RULES ENGINE FOR LEAVE RULES VALIDATION
        var _filterStaffOnLeave = function(scope) {
            managerAreaService.generateMemberOfStaffDisplayNamesFromEmployee();
            scope.siteEmployees = helperService.getLocalStorageObject("siteEmployeesStore");

            rotaRulesEngine.leaveChecker(scope);
        };

        var _filterEmployeesAlreadyAssignedToShift = function(scope) {
            var startTime = null;
            var endTime = null;
            var startDate = null;
            var endDate = null;
            var startDateTime = null;
            var endDateTime = null;

            if (scope.shiftModel.ShiftTemplate) {
                startTime = moment(scope.shiftModel.ShiftTemplate.StartTime, "HH:mm");
                endTime = moment(scope.shiftModel.ShiftTemplate.EndTime, "HH:mm");

                if (endTime < startTime) {
                    startDate = moment(scope.startDate).format("YYYY-MM-DD");
                    startDateTime = startDate + "T" + scope.shiftModel.ShiftTemplate.StartTime + ":00";
                    startDateTime = helperService.formatDateTimeStringForServer(startDateTime);

                    var overnightEndDate = moment(scope.endDate, "YYYY-MM-DD").add(1, "days");
                    endDate = moment(overnightEndDate).format("YYYY-MM-DD");
                    endDateTime = endDate + "T" + scope.shiftModel.ShiftTemplate.EndTime + ":00";
                    endDateTime = helperService.formatDateTimeStringForServer(endDateTime);
                } else {
                    startDate = moment(scope.startDate).format("YYYY-MM-DD");
                    startDateTime = startDate + "T" + scope.shiftModel.ShiftTemplate.StartTime + ":00";
                    startDateTime = helperService.formatDateTimeStringForServer(startDateTime);

                    endDate = moment(scope.endDate).format("YYYY-MM-DD");
                    endDateTime = endDate + "T" + scope.shiftModel.ShiftTemplate.EndTime + ":00";
                    endDateTime = helperService.formatDateTimeStringForServer(endDateTime);
                }
            } else {
                startDateTime = scope.startDate;
                endDateTime = scope.endDate;
            }

            rotaRulesEngine.shiftChecker(scope.filteredStaff, startDateTime, endDateTime);
        };

        var _filterShiftTemplatesByResource = function (scope, stateParams) {
            if (scope.filteredTemplates.length === 0) {
                rotaDialogServiceFactory.filterShiftTemplatesBySite(scope, stateParams);
            } else {
                scope.filteredTemplates = _.filter(scope.filteredTemplates, { "ResourceId": scope.shiftModel.Resource.Id });
            }
        };

        var _filterShiftTemplatesBySite = function(scope, stateParams) {
            if (!stateParams.subsiteId) {
                scope.filteredTemplates = scope.siteShiftTemplates;
            } else {
                scope.filteredTemplates = _.filter(scope.subsiteShiftTemplates, { "SubSiteId": Number(stateParams.subsiteId) });
            }
        };

        var _formatShiftTimes = function(shiftTemplates) {
            _.forEach(shiftTemplates, function(template) {
                template.StartTime = helperService.shiftTimeFormatter(template.StartTime);
                template.EndTime = helperService.shiftTimeFormatter(template.EndTime);
            });
        };

        var _getValidateReturnLargestBlock = function(shiftTemplate, diffVal) {
            var largestBlock = rotaDialogServiceFactory.returnLargestBlock(shiftTemplate);
            largestBlock = rotaDialogServiceFactory.setLargestBlock(largestBlock, diffVal);

            return largestBlock;
        };

        var _newRotaModel = function() {
            var newRotaModel = {
                CalendarResourceRequirementModel: {
                    IsActive: true,
                    CalendarId: null,
                    ResourceId: null,
                    StartDate: moment(),
                    EndDate: moment(),
                    ShiftTemplateId: null,
                    ShiftModels: []
                }
            };

            return newRotaModel;
        };

        var _populateFilteredStaffByResource = function(scope) {
            var tempArr1 = angular.copy(scope.filteredStaff);
            var tempArr2 = [];

            _.forEach(tempArr1, function(employee) {
                if (employee.ContractModels[0].ResourceId === scope.shiftModel.Resource.Id) {
                    tempArr2.push(employee);
                }
            });

            scope.filteredStaff = angular.copy(tempArr2);
        };

        var _populateShiftTemplateCollections = function(scope) {
            _.forEach(scope.shiftTemplates, function(template) {
                if (!template.SubSiteId) {
                    scope.siteShiftTemplates.push(template);
                } else {
                    scope.subsiteShiftTemplates.push(template);
                }
            });
        };

        var _returnLargestBlock = function(shiftTemplate) {
            var blockCounter = 0;
            var myBreak = false;
            var myBlockValuesHolderArr = [];

            if (!shiftTemplate.Sun) {
                myBreak = true;
            } else {
                blockCounter++;
                myBreak = false;
            }

            if (!shiftTemplate.Mon) {
                myBlockValuesHolderArr.push(blockCounter);

                myBreak = true;
            } else {
                if (myBreak) {
                    blockCounter = 0;
                }

                blockCounter++;
                myBreak = false;
            }

            if (!shiftTemplate.Tue) {
                myBlockValuesHolderArr.push(blockCounter);

                myBreak = true;
            } else {
                if (myBreak) {
                    blockCounter = 0;
                }

                blockCounter++;
                myBreak = false;
            }

            if (!shiftTemplate.Wed) {
                myBlockValuesHolderArr.push(blockCounter);
                myBreak = true;
            } else {
                if (myBreak) {
                    blockCounter = 0;
                }

                blockCounter++;
                myBreak = false;
            }

            if (!shiftTemplate.Thu) {
                myBlockValuesHolderArr.push(blockCounter);

                myBreak = true;
            } else {
                if (myBreak) {
                    blockCounter = 0;
                }

                blockCounter++;
                myBreak = false;
            }

            if (!shiftTemplate.Fri) {
                myBlockValuesHolderArr.push(blockCounter);

                myBreak = true;
            } else {
                if (myBreak) {
                    blockCounter = 0;
                }

                blockCounter++;
                myBreak = false;
            }

            if (!shiftTemplate.Sat) {
                myBlockValuesHolderArr.push(blockCounter);
            } else {
                if (myBreak) {
                    blockCounter = 0;
                }
                blockCounter++;
            }

            var valToReturn = _.last(_.sortBy(myBlockValuesHolderArr)) || blockCounter;

            return valToReturn;
        };

        var _returnCheckedStaff = function(filteredStaff) {
            return _.filter(filteredStaff, { "isChecked": true });
        };

        var _returnDifferenceInDays = function (scope) {
            var sd = moment(scope.startDate);
            var ed = moment(scope.endDate);
            var diffVal = moment.duration(ed.diff(sd));
            var tempDiffVal = diffVal.asDays();

            if (tempDiffVal !== parseInt(tempDiffVal, 10)) {
                diffVal = Math.ceil(tempDiffVal);

                return diffVal;
            } else {
                return diffVal.asDays();
            }
        };

        var _returnNewRotaModel = function(scope, stateParams) {
            var newRotaModel = rotaDialogServiceFactory.newRotaModel();

            newRotaModel.CalendarResourceRequirementModel.CalendarId = Number(stateParams.calendarId) || Number(stateParams.subsiteCalendarId);
            newRotaModel.CalendarResourceRequirementModel.ResourceId = scope.shiftModel.Resource.Id;
            
            // set the startDate to a Sunday and the time to be 00:00:00
            var submittedStartDate = moment(scope.startDate);
            newRotaModel.CalendarResourceRequirementModel.StartDate = submittedStartDate.day("Sunday");

            newRotaModel.CalendarResourceRequirementModel.StartDate =
                moment(newRotaModel.CalendarResourceRequirementModel.StartDate).hour(0).minute(0).second(0);

            newRotaModel.CalendarResourceRequirementModel.StartDate =
                helperService.formatDateTimeStringForServer(newRotaModel.CalendarResourceRequirementModel.StartDate);

            // set the endDate to a Saturday and the time to be 23:59:59
            var submittedEndDate = moment(scope.endDate);
            newRotaModel.CalendarResourceRequirementModel.EndDate = submittedEndDate.day("Saturday");

            newRotaModel.CalendarResourceRequirementModel.EndDate =
                moment(newRotaModel.CalendarResourceRequirementModel.EndDate).hour(23).minute(59).second(59);

            newRotaModel.CalendarResourceRequirementModel.EndDate =
                helperService.formatDateTimeStringForServer(newRotaModel.CalendarResourceRequirementModel.EndDate);

            return newRotaModel;
        };

        var _returnNewShiftTemplateOnChange = function(filteredTemplates, shiftTemplateId) {
            return _.head(_.filter(filteredTemplates, { "Id": shiftTemplateId }));
        };

        var _returnShiftStartDate = function(dateVal, daysVal) {
            return moment(dateVal).add(daysVal, "days");
        };

        var _returnShiftEndDate = function(dateVal, daysVal) {
            return moment(dateVal).add(daysVal, "days");
        };

        var _setLargestBlock = function(largestBlock, diffVal) {
            if (largestBlock >= 7) {
                largestBlock = diffVal;
            }

            return largestBlock;
        };

        var _setNewShiftModelResource = function (resourceId) {
            var resourcesStore = helperService.getLocalStorageObject("resourcesStore");
            return _.head(_.filter(resourcesStore, { "Id": resourceId }));
        };

        var _setShiftTemplate = function(filteredTemplates) {
            return _.head(filteredTemplates);
        };

        var _setShiftTemplate2 = function(filteredTemplates, resourceId) {
            return _.head(_.filter(filteredTemplates, {"ResourceId": resourceId}));
        };

        var _throwTooManyStaffError = function() {
            helperService.toasterError("Resource Quantity Error", "You have selected too many staff members for the quantity specified.");
            return;
        };

        var _validateCheckedStaffAgainstQuantity = function(staffList, qty) {
            var checkedStaff = _.filter(staffList, { "isChecked": true });
            if (checkedStaff.length > qty) {
                return null;
            }

            return checkedStaff;
        };

        var _validateCheckedStaff = function(checkedStaff) {
            if (!checkedStaff) {
                rotaDialogServiceFactory.throwTooManyStaffError();
                return false;
            }

            return true;
        };


        // Objects
        rotaDialogServiceFactory.dateTimePickerConfig = _dateTimePickerConfig;
        rotaDialogServiceFactory.rotaModel = _rotaModel;
        rotaDialogServiceFactory.shiftModel = _newShiftModel;

        // Functions
        rotaDialogServiceFactory.addShiftModel = _addShiftModel;
        rotaDialogServiceFactory.applyToDayOfWeek = _applyToDayOfWeek;
        rotaDialogServiceFactory.checkIfContractResourceIdEqualsShiftModelResourceId = _checkIfContractResourceIdEqualsShiftModelResourceId;
        rotaDialogServiceFactory.checkIfOvernightShift = _checkIfOvernightShift;
        rotaDialogServiceFactory.filterShiftTemplatesByResource = _filterShiftTemplatesByResource;
        rotaDialogServiceFactory.filterShiftTemplatesBySite = _filterShiftTemplatesBySite;
        rotaDialogServiceFactory.filterStaffOnLeave = _filterStaffOnLeave;
        rotaDialogServiceFactory.filterEmployeesAlreadyAssignedToShift = _filterEmployeesAlreadyAssignedToShift;
        rotaDialogServiceFactory.formatShiftTimes = _formatShiftTimes;
        rotaDialogServiceFactory.getValidateReturnLargestBlock = _getValidateReturnLargestBlock;
        rotaDialogServiceFactory.newRotaModel = _newRotaModel;
        rotaDialogServiceFactory.populateFilteredStaffByResource = _populateFilteredStaffByResource;
        rotaDialogServiceFactory.populateShiftTemplateCollections = _populateShiftTemplateCollections;
        rotaDialogServiceFactory.returnLargestBlock = _returnLargestBlock;
        rotaDialogServiceFactory.returnCheckedStaff = _returnCheckedStaff;
        rotaDialogServiceFactory.returnDifferenceInDays = _returnDifferenceInDays;
        rotaDialogServiceFactory.returnNewRotaModel = _returnNewRotaModel;
        rotaDialogServiceFactory.returnNewShiftTemplateOnChange = _returnNewShiftTemplateOnChange;
        rotaDialogServiceFactory.returnShiftStartDate = _returnShiftStartDate;
        rotaDialogServiceFactory.returnShiftEndDate = _returnShiftEndDate;
        rotaDialogServiceFactory.filterEmployeesOnConsecutiveDaysWorked = _filterEmployeesOnConsecutiveDaysWorked;
        rotaDialogServiceFactory.setLargestBlock = _setLargestBlock;
        rotaDialogServiceFactory.setNewShiftModelResource = _setNewShiftModelResource;
        rotaDialogServiceFactory.setShiftTemplate = _setShiftTemplate;
        //TODO: RENAME THIS
        rotaDialogServiceFactory.setShiftTemplate2 = _setShiftTemplate2;
        rotaDialogServiceFactory.throwTooManyStaffError = _throwTooManyStaffError;
        rotaDialogServiceFactory.validateCheckedStaffAgainstQuantity = _validateCheckedStaffAgainstQuantity;
        rotaDialogServiceFactory.validateCheckedStaff = _validateCheckedStaff;

        return rotaDialogServiceFactory;
    }
]);