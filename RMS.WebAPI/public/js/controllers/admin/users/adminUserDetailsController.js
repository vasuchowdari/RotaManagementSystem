"use strict";

var app = angular.module("controllers");
app.controller("adminUserDetailsCtrl", [
    "$scope",
    "$state",
    "$stateParams",
    "helperService",
    "adminAreaService",
    "adminActionStatusService",
    "blockUI",
    function ($scope, $state, $stateParams, helperService, adminAreaService,
        adminActionStatusService, blockUI) {
        // Block And Auth
        helperService.blockAndAuth($scope, blockUI);


        // Event Subscription
        adminAreaService.subscribe($scope, function() {
            adminActionStatusService.handleUserUpdateSuccess();
            $scope.setFormToPristine();
        }, "user-update-complete-event");

        adminAreaService.subscribe($scope, function(errorMsg) {
            adminActionStatusService.handleUserUpdateError(errorMsg);

            // this might end up being removed in the future. see how users get on first
            $scope.setFormToPristine();
        }, "user-update-failure-event");

        adminAreaService.subscribe($scope, function() {
            $scope.getStaffCalendarData();
        }, "reload-staff-calendar-event");


        // Declarations
        $scope.baldock = [];
        $scope.baseSubLocationDropdownDisabled = true;
        $scope.companies = [];
        $scope.employeeTypes = [];
        var formObj = null;
        $scope.hideAddressWarning = true;
        $scope.hideBaseSiteWarning = true;
        $scope.hideCityWarning = true;
        $scope.hideCompanyWarning = true;
        $scope.hideEmailWarning = true;
        $scope.hideEmployeeTypeWarning = true;
        $scope.hideExtTimeSysIdWarning = true;
        $scope.hideFirstnameWarning = true;
        $scope.hideLastnameWarning = true;
        $scope.hideLoginWarning = true;
        $scope.hideMobileWarning = true;
        $scope.hidePayrollRefWarning = true;
        $scope.hidePostcodeWarning = true;
        $scope.hideRoleWarning = true;
        $scope.hideSysAccessWarning = true;
        $scope.hideTelephoneWarning = true;
        $scope.hideWeeklyHoursWarning = true;
        $scope.leaveRequestModels = [];
        $scope.psycare = [];
        $scope.mvm = adminAreaService.returnNewUserViewModel();
        $scope.openContractDetails = false;
        $scope.openEmployeeDetails = false;
        $scope.openLocationAccess = false;
        $scope.openUserDetails = true;
        $scope.sites = [];
        $scope.subsites = [];
        $scope.shiftModels = [];
        $scope.systemAccessRoles = [];
        var USER_INVALID = adminActionStatusService.USER_VAL_ENUM;

        // Staff Calendar Declarations
        $scope.currMth = {
            startDate: helperService.getMonthStartDate(),
            text: moment().format("MMMM YYYY")
        };
        $scope.currMonthConfig = adminAreaService.returnStaffCalendarConfig($scope.currMth.startDate);
        $scope.currMonthEvents = [];

        $scope.nextMth = {
            startDate: moment(helperService.getMonthStartDate()).add(1, "M").format("YYYY-MM-DD"),
            text: moment().add(1, "M").format("MMMM YYYY")
        };
        $scope.nextMonthConfig = adminAreaService.returnStaffCalendarConfig($scope.nextMth.startDate);
        $scope.nextMonthEvents = [];

        $scope.prevMth = {
            startDate: moment(helperService.getMonthStartDate()).subtract(1, "M").format("YYYY-MM-DD"),
            text: moment().subtract(1, "M").format("MMMM YYYY")
        };
        $scope.prevMonthConfig = adminAreaService.returnStaffCalendarConfig($scope.prevMth.startDate);
        $scope.prevMonthEvents = [];


        // Functions
        $scope.clearBaldockChildren = function() {
            adminAreaService.clearBaldockChildren($scope.baldock);
        };

        $scope.getStaffCalendarData = function() {
            var staffCalObjDateRangeModel = {
                employeeId: $scope.mvm.employeeModel.Id,
                startDate: $scope.prevMth.startDate,
                endDate: moment($scope.nextMth.startDate).endOf("month").format("YYYY-MM-DD")
            };

            adminAreaService.getPersonalCalendarObjectsByDateRange(staffCalObjDateRangeModel).then(function(res) {
                $scope.currMonthEvents = [];
                $scope.prevMonthEvents = [];
                $scope.nextMonthEvents = [];

                var shiftModels = res.ShiftModels;
                $scope.shiftModels = res.ShiftModels;

                _.forEach(shiftModels, function(shiftModel) {

                    var bubbleText = "<br> Start: " + moment(shiftModel.StartDate).format("DD/MM/YYYY HH:mm:ss") + "<br> End: " + moment(shiftModel.EndDate).format("DD/MM/YYYY HH:mm:ss");
                    var eventBackColour = "#BCE7F5";
                    var eventText = "Day Shift";
                    var overnightCheckEndDate = null;
                    var overnightCheckStartDate = null;
                    var now = moment().format("YYYY-MM-DD");
                    var tagText = eventText + bubbleText;

                    if (moment(shiftModel.StartDate).isBefore($scope.currMth.startDate)) {
                        // Previous Month
                        overnightCheckEndDate = moment(shiftModel.EndDate, "YYYY-MM-DD");
                        overnightCheckStartDate = moment(shiftModel.StartDate, "YYYY-MM-DD");
                        if (moment(overnightCheckEndDate).isAfter(overnightCheckStartDate)) {
                            eventBackColour = "#FBFFC9";
                            eventText = "Night Shift";
                            tagText = eventText + bubbleText;;
                        }

                        // historical shift?
                        if (moment(shiftModel.StartDate).isBefore(now)) {
                            eventBackColour = "#CCCCCC";
                        }

                        $scope.prevMonthEvents.push({
                            start: new DayPilot.Date(shiftModel.StartDate),
                            end: new DayPilot.Date(shiftModel.StartDate), // Shifts are set to this to represent night shifts on one calendar day rather than spanning two
                            id: DayPilot.guid(),
                            text: eventText,
                            tag: tagText,
                            backColor: eventBackColour,
                            deletable: false
                        });
                    } else if (moment(shiftModel.StartDate).isAfter($scope.nextMth.startDate)) {
                        // Next Month
                        overnightCheckEndDate = moment(shiftModel.EndDate, "YYYY-MM-DD");
                        overnightCheckStartDate = moment(shiftModel.StartDate, "YYYY-MM-DD");
                        if (moment(overnightCheckEndDate).isAfter(overnightCheckStartDate)) {
                            eventBackColour = "#FBFFC9";
                            eventText = "Night Shift";
                            tagText = eventText + bubbleText;
                        }

                        // historical shift?
                        if (moment(shiftModel.StartDate).isBefore(now)) {
                            eventBackColour = "#CCCCCC";
                        }

                        $scope.nextMonthEvents.push({
                            start: new DayPilot.Date(shiftModel.StartDate),
                            end: new DayPilot.Date(shiftModel.StartDate), // Shifts are set to this to represent night shifts on one calendar day rather than spanning two
                            id: DayPilot.guid(),
                            text: eventText,
                            tag: tagText,
                            backColor: eventBackColour,
                            deletable: false
                        });
                    } else {
                        // Cuurent Month
                        overnightCheckEndDate = moment(shiftModel.EndDate, "YYYY-MM-DD");
                        overnightCheckStartDate = moment(shiftModel.StartDate, "YYYY-MM-DD");
                        if (moment(overnightCheckEndDate).isAfter(overnightCheckStartDate)) {
                            eventBackColour = "#FBFFC9";
                            eventText = "Night Shift";
                            tagText = eventText + bubbleText;
                        }

                        // historical shift?
                        if (moment(shiftModel.StartDate).isBefore(now)) {
                            eventBackColour = "#CCCCCC";
                        }

                        $scope.currMonthEvents.push({
                            start: new DayPilot.Date(shiftModel.StartDate),
                            end: new DayPilot.Date(shiftModel.StartDate), // Shifts are set to this to represent night shifts on one calendar day rather than spanning two
                            id: DayPilot.guid(),
                            text: eventText,
                            tag: tagText,
                            backColor: eventBackColour,
                            deletable: false
                        });
                    }
                });

                var leaveRequestModels = res.LeaveRequestModels;
                $scope.leaveRequestModels = res.LeaveRequestModels;

                _.forEach(leaveRequestModels, function(leaveRequestModel) {
                    var bubbleText = "<br> Start: " + moment(leaveRequestModel.StartDateTime).format("DD/MM/YYYY HH:mm:ss") + "<br> End: " + moment(leaveRequestModel.EndDateTime).format("DD/MM/YYYY HH:mm:ss");

                    var leaveEvent = {
                        start: new DayPilot.Date(leaveRequestModel.StartDateTime),
                        end: new DayPilot.Date(leaveRequestModel.EndDateTime),
                        id: leaveRequestModel.Id,
                        deletable: true,
                        amountRequested: leaveRequestModel.AmountRequested,
                        leaveTypeId: leaveRequestModel.LeaveTypeId
                        //isApproved: leaveRequestModel.IsApproved,
                        //isTaken: leaveRequestModel.IsTaken,
                        //isActive: leaveRequestModel.IsActive
                    };

                    var eventBackColour = null;
                    var eventText = null;
                    var tagText = null;

                    switch (leaveRequestModel.LeaveTypeId) {
                        case 1:
                            eventText = "Leave";
                            tagText = eventText + bubbleText; 
                            eventBackColour = "#A1D490";

                            break;
                        case 2:
                            eventText = "Maternity";
                            tagText = eventText + bubbleText;
                            eventBackColour = "#FFB8FA";

                            break;
                        case 3:
                            eventText = "Paternity";
                            tagText = eventText + bubbleText;
                            eventBackColour = "#2E4AFF";

                            break;
                        case 4:
                            eventText = "Sick";
                            tagText = eventText + bubbleText;
                            eventBackColour = "#FF9C45";

                            break;
                        case 5:
                            eventText = "Suspension";
                            tagText = eventText + bubbleText;
                            eventBackColour = "#FF576D";

                            break;
                        case 6:
                            eventText = "Other";
                            tagText = eventText + bubbleText;
                            eventBackColour = "#FF7A7A";

                            break;
                        case 7:
                            eventText = "Training";
                            tagText = eventText + bubbleText;
                            eventBackColour = "#D4BB90";

                            break;
                        default:
                            break;
                    }

                    leaveEvent.backColor = eventBackColour;
                    leaveEvent.tag = tagText;
                    leaveEvent.text = eventText;

                    if (moment(leaveRequestModel.StartDateTime).isBefore($scope.currMth.startDate)) {
                        $scope.prevMonthEvents.push(leaveEvent);
                    } else if (moment(leaveRequestModel.StartDateTime).isAfter($scope.nextMth.startDate)) {
                        $scope.nextMonthEvents.push(leaveEvent);
                    } else {
                        $scope.currMonthEvents.push(leaveEvent);
                    }
                });

            }).catch(function(error) {
                helperService.toasterError(null, error);
                blockUI.stop();
            });
        };

        $scope.hydrateBaldock = function() {
            adminAreaService.hydrateBaldock($scope);
        };

        $scope.hydratePsycare = function() {
            adminAreaService.hydratePsycare($scope);
        };

        $scope.hydrateUserMasterViewModel = function() {
            adminAreaService.getSystemAccessRoles().then(function(res) {
                $scope.systemAccessRoles = res;

                // user table
                adminAreaService.getUserDetails($stateParams.userId).then(function(res) {
                    $scope.mvm.userModel = res;

                    // employee table
                    adminAreaService.getEmployeeDetails($scope.mvm.userModel.Id).then(function(res) {
                        if (!res) {
                            // no employee record for user, so no point wiring rest up
                            return;
                        }

                        $scope.mvm.employeeModel = res;

                        if ($scope.mvm.employeeModel.BaseSiteId === 2) {
                            $scope.baseSubLocationDropdownDisabled = false;
                        }

                        if (res.ContractModels) {
                            $scope.mvm.contractModel = res.ContractModels[0];
                        }

                        // sitePersonnelLookup table
                        adminAreaService.getSiteAccessDetails($scope.mvm.employeeModel.Id).then(function(res) {
                            $scope.mvm.siteAccessModels = res;
                        }).catch(function(error) {
                            helperService.toasterError(null, error);
                            blockUI.stop();
                        }).finally(function() {
                            $scope.companies = helperService.getLocalStorageObject("companiesStore");
                            $scope.employeeTypes = helperService.getLocalStorageObject("employeeTypesStore");
                            $scope.resources = helperService.getLocalStorageObject("resourcesStore");

                            _.forEach($scope.mvm.siteAccessModels, function(accessRecord) {
                                if (accessRecord.SiteId && !accessRecord.SubSiteId) {
                                    // Baldock
                                    if (accessRecord.SiteId === $scope.baldock.Id && accessRecord.IsActive) {
                                        $scope.baldock.isChecked = true;
                                    }

                                    // Psycare
                                    _.forEach($scope.psycare, function (site) {
                                        if (accessRecord.SiteId === site.Id && accessRecord.IsActive) {
                                            site.isChecked = true;
                                        }
                                    });
                                }

                                if (accessRecord.SiteId && accessRecord.SubSiteId) {
                                    // Baldock
                                    _.forEach($scope.baldock.SubSiteModels, function(subsite) {
                                        if (accessRecord.SubSiteId === subsite.Id && accessRecord.IsActive) {
                                            subsite.isChecked = true;
                                        }
                                    });
                                }
                            });

                            // set this to use on staff calendar popup
                            helperService.setLocalStorageObject("userMasterViewModel", $scope.mvm);
                            $scope.getStaffCalendarData();
                        });
                    }).catch(function(error) {
                        helperService.toasterError(null, error);
                        blockUI.stop();
                    });
                }).catch(function(error) {
                    helperService.toasterError(null, error);
                    blockUI.stop();
                });
            }).catch(function(error) {
                helperService.toasterError(null, error);
                blockUI.stop();
            });
        };

        $scope.openStaffCalendarLeaveDialog = function() {
            adminAreaService.openStaffCalendarLeaveDialog(null);
        };

        $scope.prevMonth = function() {
            $scope.currMonthConfig.startDate = moment($scope.currMonthConfig.startDate).subtract(1, "M").format("YYYY-MM-DD");
            $scope.nextMonthConfig.startDate = moment($scope.nextMonthConfig.startDate).subtract(1, "M").format("YYYY-MM-DD");
            $scope.prevMonthConfig.startDate = moment($scope.prevMonthConfig.startDate).subtract(1, "M").format("YYYY-MM-DD");

            $scope.currMth.startDate = moment($scope.currMth.startDate).subtract(1, "M").format("YYYY-MM-DD");
            $scope.currMth.text = moment($scope.currMth.startDate).format("MMMM YYYY");
            $scope.prevMth.startDate = moment($scope.prevMth.startDate).subtract(1, "M").format("YYYY-MM-DD");
            $scope.prevMth.text = moment($scope.prevMth.startDate).format("MMMM YYYY");
            $scope.nextMth.startDate = moment($scope.nextMth.startDate).subtract(1, "M").format("YYYY-MM-DD");
            $scope.nextMth.text = moment($scope.nextMth.startDate).format("MMMM YYYY");

            $scope.getStaffCalendarData();
        };

        $scope.nextMonth = function () {
            $scope.currMonthConfig.startDate = moment($scope.currMonthConfig.startDate).add(1, "M").format("YYYY-MM-DD");
            $scope.nextMonthConfig.startDate = moment($scope.nextMonthConfig.startDate).add(1, "M").format("YYYY-MM-DD");
            $scope.prevMonthConfig.startDate = moment($scope.prevMonthConfig.startDate).add(1, "M").format("YYYY-MM-DD");

            $scope.currMth.startDate = moment($scope.currMth.startDate).add(1, "M").format("YYYY-MM-DD");
            $scope.currMth.text = moment($scope.currMth.startDate).format("MMMM YYYY");
            $scope.prevMth.startDate = moment($scope.prevMth.startDate).add(1, "M").format("YYYY-MM-DD");
            $scope.prevMth.text = moment($scope.prevMth.startDate).format("MMMM YYYY");
            $scope.nextMth.startDate = moment($scope.nextMth.startDate).add(1, "M").format("YYYY-MM-DD");
            $scope.nextMth.text = moment($scope.nextMth.startDate).format("MMMM YYYY");

            $scope.getStaffCalendarData();
        };

        $scope.removeAddressWarning = function() {
            if (!$scope.hideAddressWarning) {
                $scope.hideAddressWarning = true;
            }
        };

        $scope.removeBaseSiteWarning = function() {
            if (!$scope.hideBaseSiteWarning) {
                $scope.hideBaseSiteWarning = true;
            }
        };

        $scope.removeCityWarning = function() {
            if (!$scope.hideCityWarning) {
                $scope.hideCityWarning = true;
            }
        };

        $scope.removeCompanyWarning = function() {
            if (!$scope.hideCompanyWarning) {
                $scope.hideCompanyWarning = true;
            }
        };

        $scope.removeEmailWarning = function() {
            if (!$scope.hideEmailWarning) {
                $scope.hideEmailWarning = true;
            }
        };

        $scope.removeEmployeeTypeWarning = function() {
            if (!$scope.hideEmployeeTypeWarning) {
                $scope.hideEmployeeTypeWarning = true;
            }
        };

        $scope.removeExtTimeSysIdWarning = function() {
            if (!$scope.hideExtTimeSysIdWarning) {
                $scope.hideExtTimeSysIdWarning = true;
            }
        };

        $scope.removeFirstnameWarning = function() {
            if (!$scope.hideFirstnameWarning) {
                $scope.hideFirstnameWarning = true;
            }
        };

        $scope.removeLastnameWarning = function() {
            if (!$scope.hideLastnameWarning) {
                $scope.hideLastnameWarning = true;
            }
        };

        $scope.removeLoginWarning = function() {
            if (!$scope.hideLoginWarning) {
                $scope.hideLoginWarning = true;
            }
        };

        $scope.removeMobileWarning = function() {
            if (!$scope.hideMobileWarning) {
                $scope.hideMobileWarning = true;
            }
        };

        $scope.removePayrollRefWarning = function() {
            if (!$scope.hidePayrollRefWarning) {
                $scope.hidePayrollRefWarning = true;
            }
        };

        $scope.removePostcodeWarning = function() {
            if (!$scope.hidePostcodeWarning) {
                $scope.hidePostcodeWarning = true;
            }
        };

        $scope.removeRoleWarning = function() {
            if (!$scope.hideRoleWarning) {
                $scope.hideRoleWarning = true;
            }
        };

        $scope.removeSysAccessWarning = function() {
            if (!$scope.hideSysAccessWarning) {
                $scope.hideSysAccessWarning = true;
            }
        };

        $scope.removeTelephoneWarning = function() {
            if (!$scope.hideTelephoneWarning) {
                $scope.hideTelephoneWarning = true;
            }
        };

        $scope.removeWeeklyHoursWarning = function() {
            if (!$scope.hideWeeklyHoursWarning) {
                $scope.hideWeeklyHoursWarning = true;
            }
        };

        $scope.selectBaldockParentSite = function() {
            adminAreaService.selectBaldockParent($scope.baldock);
        };

        $scope.setFormToPristine = function() {
            formObj.$setPristine();

            $scope.mvm.userIsDirty = false;
            $scope.mvm.employeeIsDirty = false;
            $scope.mvm.contractIsDirty = false;
            $scope.mvm.accessIsDirty = false;

            adminAreaService.userUpdateStatusObject.userUpdateSuccessful = false;
            adminAreaService.userUpdateStatusObject.userUpdateFailure = false;
            adminAreaService.userUpdateStatusObject.employeeUpdateSuccessful = false;
            adminAreaService.userUpdateStatusObject.employeeUpdateFailure = false;
            adminAreaService.userUpdateStatusObject.contractUpdateSuccessful = false;
            adminAreaService.userUpdateStatusObject.contractUpdateFailure = false;
            adminAreaService.userUpdateStatusObject.siteAccessUpdateSuccessful = false;
            adminAreaService.userUpdateStatusObject.siteAccessUpdateFailure = false;
            adminAreaService.userUpdateStatusObject.siteAccessCreateSuccessful = false;
            adminAreaService.userUpdateStatusObject.siteAccessCreateFailure = false;

            // reset and reload
            $scope.mvm = adminAreaService.returnNewUserViewModel();
            $scope.hydrateUserMasterViewModel();
        };

        $scope.submitUserUpdateForm = function() {
            var self = this;
            formObj = angular.copy(self.userUpdateForm);

            var valObj = adminActionStatusService.validateUserObjectForm(formObj, $scope);

            if (!valObj[0]) {
                if (valObj[1] === USER_INVALID.login) {
                    $scope.hideLoginWarning = false;
                    $scope.openUserDetails = true;
                }

                if (valObj[1] === USER_INVALID.email) {
                    $scope.hideEmailWarning = false;
                    $scope.openUserDetails = true;
                }

                if (valObj[1] === USER_INVALID.firstname) {
                    $scope.hideFirstnameWarning = false;
                    $scope.openUserDetails = true;
                }

                if (valObj[1] === USER_INVALID.lastname) {
                    $scope.hideLastnameWarning = false;
                    $scope.openUserDetails = true;
                }

                if (valObj[1] === USER_INVALID.sysAccess) {
                    $scope.hideSysAccessWarning = false;
                    $scope.openUserDetails = true;
                }

                if (valObj[1] === USER_INVALID.extTimeSysId) {
                    $scope.hideExtTimeSysIdWarning = false;
                    $scope.openUserDetails = true;
                }

                if (valObj[1] === USER_INVALID.payroll) {
                    $scope.hidePayrollRefWarning = false;
                    $scope.openUserDetails = true;
                }

                if (valObj[1] === USER_INVALID.company) {
                    $scope.hideCompanyWarning = false;
                    $scope.openEmployeeDetails = true;
                }

                if (valObj[1] === USER_INVALID.employeeType) {
                    $scope.hideEmployeeTypeWarning = false;
                    $scope.openEmployeeDetails = true;
                }

                if (valObj[1] === USER_INVALID.baseSite) {
                    $scope.hideBaseSiteWarning = false;
                    $scope.openEmployeeDetails = true;
                }

                if (valObj[1] === USER_INVALID.address) {
                    $scope.hideAddressWarning = false;
                    $scope.openEmployeeDetails = true;
                }

                if (valObj[1] === USER_INVALID.city) {
                    $scope.hideCityWarning = false;
                    $scope.openEmployeeDetails = true;
                }

                if (valObj[1] === USER_INVALID.postcode) {
                    $scope.hidePostcodeWarning = false;
                    $scope.openEmployeeDetails = true;
                }

                if (valObj[1] === USER_INVALID.telephone) {
                    $scope.hideTelephoneWarning = false;
                    $scope.openEmployeeDetails = true;
                }

                if (valObj[1] === USER_INVALID.mobile) {
                    $scope.hideMobileWarning = false;
                    $scope.openEmployeeDetails = true;
                }

                if (valObj[1] === USER_INVALID.role) {
                    $scope.hideRoleWarning = false;
                    $scope.openContractDetails = true;
                }

                if (valObj[1] === USER_INVALID.weeklyHours) {
                    $scope.hideWeeklyHoursWarning = false;
                    $scope.openContractDetails = true;
                }

                if (valObj[1] === USER_INVALID.locationAccess) {
                    $scope.openLocationAccess = true;
                }

                return;
            }

            // TODO: BEEF UP THE DE-ACTIVATE OF USER / EMPLOYEE PROCESS
            if (!$scope.mvm.userModel.IsActive) {
                $scope.mvm.employeeModel.IsActive = false;
                $scope.mvm.employeeIsDirty = true;
            }

            $scope.updateUserSitePermissions();
            adminAreaService.checkMvmComponentsAreDirty($scope.mvm, formObj);
            adminAreaService.updateUser($scope.mvm);
        };

        $scope.toggleBaseSubLocationDropdown = function() {
            if (!$scope.baseSubLocationDropdownDisabled) {
                $scope.baseSubLocationDropdownDisabled = true;
                $scope.subsites = [];
            } else {
                $scope.baseSubLocationDropdownDisabled = false;
                var tempSite = _.head(_.filter($scope.sites, { "Id": 2 }));
                $scope.subsites = tempSite.SubSiteModels;
            }
        };

        $scope.tripAccessIsDirtyFlag = function() {
            $scope.mvm.accessIsDirty = true;
        };

        $scope.updateUserSitePermissions = function() {
            var tempSite = null;
            // Baldock main site
            if ($scope.baldock.isChecked) {
                if (_.find($scope.mvm.siteAccessModels, { "SiteId": $scope.baldock.Id, "SubSiteId": null, "IsActive": true })) {
                    // BALDOCK ACCESS RECORD ALREADY EXISTS AND ACTIVE
                } else if (_.find($scope.mvm.siteAccessModels, { "SiteId": $scope.baldock.Id, "SubSiteId": null, "IsActive": false })) {
                    // BALDOCK ACCESS RECORD ALREADY EXISTS BUT IS INACTIVE
                    tempSite = _.find($scope.mvm.siteAccessModels, { "SiteId": $scope.baldock.Id, "SubSiteId": null, "IsActive": false });
                    tempSite.IsActive = true;
                } else {
                    // OTHERWISE NO RECORD EXISTS AND NEEDS TO BE ADDED TO ARRAY
                    $scope.mvm.siteAccessModels.push({
                        IsActive: true,
                        EmployeeId: $scope.mvm.employeeModel.Id,
                        SiteId: $scope.baldock.Id,
                        SubSiteId: null
                    });
                }
            }

            if (!$scope.baldock.isChecked) {
                if (_.find($scope.mvm.siteAccessModels, { "SiteId": $scope.baldock.Id, "SubSiteId": null, "IsActive": true })) {
                    // BALDOCK ACCESS RECORD ALREADY EXISTS AND ACTIVE
                    tempSite = _.find($scope.mvm.siteAccessModels, { "SiteId": $scope.baldock.Id, "SubSiteId": null, "IsActive": true });
                    tempSite.IsActive = false;
                } else if (_.find($scope.mvm.siteAccessModels, { "SiteId": $scope.baldock.Id, "SubSiteId": null, "IsActive": false })) {
                    // BALDOCK ACCESS RECORD ALREADY EXISTS BUT IS INACTIVE
                }
            }

            // Baldock wards
            _.forEach($scope.baldock.SubSiteModels, function(subsite) {
                if (subsite.isChecked) {
                    if (_.find($scope.mvm.siteAccessModels, { "SiteId": $scope.baldock.Id, "SubSiteId": subsite.Id, "IsActive": true })) {
                        // BALDOCK WARD ACCESS RECORD ALREADY EXISTS AND ACTIVE
                    } else if (_.find($scope.mvm.siteAccessModels, { "SiteId": $scope.baldock.Id, "SubSiteId": subsite.Id, "IsActive": false })) {
                        // BALDOCK WARD ACCESS RECORD ALREADY EXISTS BUT IS INACTIVE
                        tempSite = _.find($scope.mvm.siteAccessModels, { "SiteId": $scope.baldock.Id, "SubSiteId": subsite.Id, "IsActive": false });
                        tempSite.IsActive = true;
                    } else {
                        // OTHERWISE NO RECORD EXISTS AND NEEDS TO BE ADDED TO ARRAY
                        $scope.mvm.siteAccessModels.push({
                            IsActive: true,
                            EmployeeId: $scope.mvm.employeeModel.Id,
                            SiteId: $scope.baldock.Id,
                            SubSiteId: subsite.Id
                        });
                    }
                }

                if (!subsite.isChecked) {
                    if (_.find($scope.mvm.siteAccessModels, { "SiteId": $scope.baldock.Id, "SubSiteId": subsite.Id, "IsActive": true })) {
                        // BALDOCK ACCESS RECORD ALREADY EXISTS AND ACTIVE
                        tempSite = _.find($scope.mvm.siteAccessModels, { "SiteId": $scope.baldock.Id, "SubSiteId": subsite.Id, "IsActive": true });
                        tempSite.IsActive = false;
                    } else if (_.find($scope.mvm.siteAccessModels, { "SiteId": $scope.baldock.Id, "SubSiteId": subsite.Id, "IsActive": false })) {
                        // BALDOCK ACCESS RECORD ALREADY EXISTS BUT IS INACTIVE
                    }
                }
            });


            // Psycare
            _.forEach($scope.psycare, function(site) {
                if (site.isChecked) {
                    if (_.find($scope.mvm.siteAccessModels, { "SiteId": site.Id, "IsActive": true })) {
                        // PSYCARE SITE ACCESS RECORD ALREADY EXISTS AND ACTIVE
                    } else if (_.find($scope.mvm.siteAccessModels, { "SiteId": site.Id, "IsActive": false })) {
                        // PSYCARE SITE ACCESS RECORD ALREADY EXISTS BUT IS INACTIVE
                        tempSite = _.find($scope.mvm.siteAccessModels, { "SiteId": site.Id, "IsActive": false });
                        tempSite.IsActive = true;
                    } else {
                        $scope.mvm.siteAccessModels.push({
                            IsActive: true,
                            EmployeeId: $scope.mvm.employeeModel.Id,
                            SiteId: site.Id,
                            SubSiteId: null
                        });
                    }
                }

                if (!site.isChecked) {
                    if (_.find($scope.mvm.siteAccessModels, { "SiteId": site.Id, "IsActive": true })) {
                        // PSYCARE SITE ACCESS RECORD ALREADY EXISTS AND ACTIVE
                        tempSite = _.find($scope.mvm.siteAccessModels, { "SiteId": site.Id, "IsActive": true });
                        tempSite.IsActive = false;
                    } else if (_.find($scope.mvm.siteAccessModels, { "SiteId": site.Id, "IsActive": false })) {
                        // PSYCARE SITE ACCESS RECORD ALREADY EXISTS BUT IS INACTIVE
                    }
                }
            });
        };

        $scope.updateUserSystemAccessRole = function() { };

        
        // Init
        $scope.init = function() {
            if (!$scope.authentication.isAuth) {
                helperService.notAuth();
            }

            $scope.sites = helperService.getLocalStorageObject("sitesStore");
            var tempSite = _.head(_.filter($scope.sites, { "Id": 2 }));
            $scope.subsites = tempSite.SubSiteModels;

            $scope.hydrateBaldock();
            $scope.hydratePsycare();
            $scope.hydrateUserMasterViewModel();

            blockUI.stop();
        };

        $scope.init();
    }
]);