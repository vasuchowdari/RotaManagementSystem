"use strict";

var app = angular.module("services");
app.factory("adminAreaService", [
    "$rootScope",
    "helperService",
    "calendarService",
    "configService",
    "companyService",
    "contractService",
    "employeeService",
    "employeeTypeService",
    "leaveRequestService",
    "leaveTypeService",
    "reportService",
    "resourceService",
    "siteService",
    "sitePersonnelLookupService",
    "shiftService",
    "shiftProfileService",
    "shiftTemplateService",
    "systemAccessRoleService",
    "timeAdjustFormService",
    "userDefinedSystemConfigService",
    "userService",
    "zktService",
    "ngDialog",
    function ($rootScope, helperService, calendarService, configService, companyService,
        contractService, employeeService, employeeTypeService, leaveRequestService, leaveTypeService,
        reportService, resourceService, siteService, sitePersonnelLookupService, shiftService,
        shiftProfileService, shiftTemplateService, systemAccessRoleService, timeAdjustFormService,
        userDefinedSystemConfigService, userService, zktService, ngDialog) {
        var adminAreaServiceFactory = {};

        // Event Handling
        var _notify = function(data, eventName) {
            $rootScope.$emit(eventName, data);
        };

        var _subscribe = function(scope, callback, eventName) {
            var handler = $rootScope.$on(eventName, callback);
            scope.$on("$destroy", handler);
        };


        // Declarations
        var _userUpdateStatusObject = {
            userUpdateSuccessful: null,
            userUpdateFailure: null,
            employeeUpdateSuccessful: null,
            employeeUpdateFailure: null,
            contractUpdateSuccessful: null,
            contractUpdateFailure: null,

            siteAccessUpdateSuccessful: null,
            siteAccessUpdateFailure: null,
            siteAccessCreateSuccessful: null,
            siteAccessCreateFailure: null
        };


        // Functions
        var _clearBaldockChildren = function(baldockSiteObject) {
            if (!baldockSiteObject.isChecked) {
                _.forEach(baldockSiteObject.SubSiteModels, function (subsite) {
                    subsite.isChecked = false;
                });
            }
        };

        var _checkMvmComponentsAreDirty = function(masterViewModel, userUpdateForm) {
            if (userUpdateForm.userLogin.$dirty ||
                userUpdateForm.userEmail.$dirty ||
                userUpdateForm.userFirstname.$dirty ||
                userUpdateForm.userLastname.$dirty ||
                userUpdateForm.userAccess.$dirty ||
                userUpdateForm.userExtTimeSysId.$dirty ||
                userUpdateForm.userPayrollRef.$dirty ||
                userUpdateForm.userIsActive.$dirty) {
                masterViewModel.userIsDirty = true;
            }

            if (userUpdateForm.employeeCompany.$dirty ||
                userUpdateForm.employeeType.$dirty ||
                userUpdateForm.employeeBaseSite.$dirty ||
                userUpdateForm.employeeBaseSubSite.$dirty ||
                userUpdateForm.employeeAddress.$dirty ||
                userUpdateForm.employeeCity.$dirty ||
                userUpdateForm.employeePostcode.$dirty ||
                userUpdateForm.employeeTelephone.$dirty ||
                userUpdateForm.employeeMobile.$dirty) {
                masterViewModel.employeeIsDirty = true;
            }

            if (userUpdateForm.contractResource.$dirty ||
                userUpdateForm.contractWeeklyHours.$dirty ||
                userUpdateForm.contractBaseRateOverride.$dirty ||
                userUpdateForm.contractOvertimeOverride.$dirty ||
                userUpdateForm.contractIsActive.$dirty) {
                masterViewModel.contractIsDirty = true;
            }
        };

        var _hydrateBaldock = function(scope) {
            scope.baldock = _.head(_.filter(scope.sites, { "CompanyId": 1 }));
            scope.baldock.isChecked = false;

            _.forEach(scope.baldock.SubSiteModels, function (subsite) {
                subsite.isChecked = false;
            });
        };

        var _hydratePsycare = function(scope) {
            scope.psycare = _.filter(scope.sites, { "CompanyId": 2 });

            _.forEach(scope.psycare, function (site) {
                site.isChecked = false;
            });
        };

        var _returnLeaveRequestDeleteViewModel = function() {
            return leaveRequestService.generateDeleteLeaveViewModel();
        };

        var _returnNewDailyActivityReportParamsModel = function() {
            return reportService.returnNewDailyActivityReportParamsModel();
        };

        var _returnNewInvalidShiftProfileViewModel = function() {
            var model = {
                id: null,
                isActive: true,
                startDateTime: null,
                endDateTime: null,
                actualStartDateTime: null,
                actualEndDateTime: null,
                zktStartDateTime: null,
                zktEndDateTime: null,
                hoursWorked: null,
                timeWorked: null,
                employeeId: null,
                shiftId: null,
                isApproved: false,
                status: null,
                reason: null,
                notes: null,
                isModified: false,

                shiftDate: null,
                resourceName: null
            };

            return model;
        };

        var _returnNewLeaveRequestModel = function() {
            return leaveRequestService.generateNewLeaveRequestModel();
        };

        var _returnNewMonthlyPayrollReportParamsModel = function() {
            return reportService.returnNewMonthlyPayrollReportParamsModel();
        };

        var _returnNewResourceModel = function() {
            return resourceService.generateNewResourceModel();
        };

        var _returnNewShiftTemplateModel = function() {
            return shiftTemplateService.returnNewShiftTemplateModel();
        };

        var _returnNewTimeAdjustmentFormUpdateModel = function() {
            return timeAdjustFormService.returnNewTimeAdjustmentUpdateFormModel();
        };

        var _returnNewTrainingShiftModel = function() {
            return leaveRequestService.generateNewLeaveRequestModel();
        };

        var _returnNewUserViewModel = function() {
            var mvm = {
                userIsDirty: false,
                employeeIsDirty: false,
                contractIsDirty: false,
                accessIsDirty: false,
                userModel: {
                    Id: null,
                    Login: null,
                    Email: null,
                    Firstname: null,
                    Lastname: null,
                    SystemAccessRoleId: null,
                    ExternalTimeSystemId: null,
                    PayrollReferenceNumber: null,
                    IsActive: null
                },
                employeeModel: {
                    Id: null,
                    CompanyId: null,
                    EmployeeTypeId: null,
                    BaseSiteId: null,
                    BaseSubSiteId: null,
                    Address: null,
                    City: null,
                    Postcode: null,
                    Telephone: null,
                    Mobile: null,
                    IsActive: null
                },
                contractModel: {
                    Id: null,
                    ResourceId: null,
                    EmployeeId: null,
                    WeeklyHours: null,
                    BaseRateOverride: null,
                    OvertimeModifierOverride: null,
                    IsActive: null
                },
                siteAccessModels: []
            };

            return mvm;
        };

        var _returnShiftStatusString = function(statusInt) {
            var strToReturn = null;

            switch(statusInt) {
                case 0:
                    strToReturn = "Valid";
                    break;
                case 1:
                    strToReturn = "Late In";
                    break;
                case 2:
                    strToReturn = "Early Out";
                    break;
                case 3:
                    strToReturn = "Early In";
                    break;
                case 4:
                    strToReturn = "Late Out";
                    break;
                case 5:
                    strToReturn = "Missing Clock In";
                    break;
                case 6:
                    strToReturn = "Missing Clock Out";
                    break;
                case 7:
                    strToReturn = "No Show";
                    break;
                case 8:
                    strToReturn = "Over Stayed";
                    break;
                case 9:
                    strToReturn = "Huh?";
                    break;
            }

            return strToReturn;
        };

        var _returnStaffCalendarConfig = function(startDate) {
            var config = {
                startDate: startDate,
                eventMoveHandling: "disabled",
                eventResizeHandling: "disabled",
                moveBy: "None",
                onBeforeHeaderRender: function(args) {
                    args.header.html = adminAreaServiceFactory.staffCalendarDaysOfWeekHeaders(args.header.dayOfWeek);
                },
                onEventClick: function(args) {
                    //delete behaviour
                    if (args.e.data.deletable) {
                        adminAreaServiceFactory.openStaffCalendarLeaveDialog(args.e.data);
                    }
                },
                bubble: new DayPilot.Bubble({
                    onLoad: function(args) {
                        var event = args.source;
                        args.async = true;

                        setTimeout(function() {
                            args.html = event.tag();
                            args.loaded();
                        }, 50);
                    }
                })
            }

            return config;
        };

        var _returnUserDefinedSystemConfigModel = function() {
            return userDefinedSystemConfigService.udscModel;
        };

        var _selectBaldockParent = function(baldockSiteObject) {
            baldockSiteObject.isChecked = true;
        };

        var _staffCalendarDaysOfWeekHeaders = function(dayOfWeekInt) {
            var dayOfWeekStr = "";
            switch (dayOfWeekInt) {
                case 0:
                    dayOfWeekStr = "Sun";
                    break;
                case 1:
                    dayOfWeekStr = "Mon";
                    break;
                case 2:
                    dayOfWeekStr = "Tue";
                    break;
                case 3:
                    dayOfWeekStr = "Wed";
                    break;
                case 4:
                    dayOfWeekStr = "Thu";
                    break;
                case 5:
                    dayOfWeekStr = "Fri";
                    break;
                case 6:
                    dayOfWeekStr = "Sat";
                    break;
                default:
                    break;
            }

            return dayOfWeekStr;
        };


        // Dialogs
        var _openCreateNewShiftTemplateDialog = function() {
            ngDialog.open({
                showClose: true,
                closeByNavigation: false,
                closeByDocument: false,
                template: configService.path + "/public/views/dialog-tmpls/add-new-shift-template.html",
                controller: "adminShiftTemplateCreateCtrl"
            });
        };

        var _openDailyActivityReportDialog = function() {
            ngDialog.open({
                showClose: true,
                closeByNavigation: false,
                closeByDocument: false,
                template: configService.path + "/public/views/dialog-tmpls/daily-activity-report-form.html",
                controller: "adminDailyActivityReportCtrl"
            });
        };

        var _openMonthlyPayrollReportDialog = function() {
            ngDialog.open({
                showClose: true,
                closeByNavigation: false,
                closeByDocument: false,
                template: configService.path + "/public/views/dialog-tmpls/monthly-payroll-report-form.html",
                controller: "adminMonthlyPayrollReportCtrl"
            });
        };

        var _openStaffCalendarLeaveDialog = function(eventData) {
            ngDialog.open({
                showClose: true,
                closeByNavigation: false,
                closeByDocument: false,
                data: eventData,
                template: configService.path + "/public/views/dialog-tmpls/staff-calendar-leave-form.html",
                controller: "adminStaffCalenderLeaveCtrl"
            });
        };


        // API Calls
        var _batchCreateShiftTemplates = function(shiftTemplateModelCollection) {
            return shiftTemplateService.createBatch(shiftTemplateModelCollection).then(function(res) {
                return res.data;
            }).catch(function(error) {
                return error;
            });
        };

        var _createLeaveRequest = function(leaveRequestModel) {
            leaveRequestService.adminCreate(leaveRequestModel).then(function(res) {
                if (res.status != 200) {
                    var errorMsg = res.data.Message || "Staff Calender Update error.";
                    adminAreaServiceFactory.notify(errorMsg, "leave-request-create-failure-event");
                } else {
                    adminAreaServiceFactory.notify(null, "leave-request-create-success-event");
                }
            }).catch(function(error) {
                adminAreaServiceFactory.notify(error, "leave-request-create-failure-event");
            });
        };

        var _createResource = function(resourceModel) {
            resourceService.create(resourceModel).then(function(res) {
                adminAreaServiceFactory.notify(null, "resource-create-complete-event");
            }).catch(function(error) {
                adminAreaServiceFactory.notify(error, "resource-create-failure-event");
            });
        };

        var _createShiftProfileForUnmatchedZkt = function(shiftProfileModel) {
            shiftProfileService.createForUnmantchedZkt(shiftProfileModel).then(function(res) {
                helperService.toasterSuccess(null, res.data, "admin.dash");
            }).catch(function(error) {
                helperService.toasterError(null, error);
            });
        };

        var _createShiftTemplate = function(shiftTemplateModel) {
            return shiftTemplateService.create(shiftTemplateModel).then(function(res) {
                return res.data;
            }).catch(function(error) {
                return error;
            });
        };

        var _createUser = function(userViewModel) {
            return userService.registerUser(userViewModel).then(function(res) {
                return res;
            }).catch(function(error) {
                return error;
            });
        };

        var _deleteLeaveRequest = function(leaveRequestId) {
            leaveRequestService.adminDelete(leaveRequestId).then(function (res) {
                adminAreaServiceFactory.notify(res.data, "leave-request-delete-success-event");
            }).catch(function(error) {
                adminAreaServiceFactory.notify(error, "leave-request-delete-failure-event");
            });
        };

        var _getAllEmployeesByCompanyId = function(companyId) {
            return employeeService.getAllByCompanyId(companyId).then(function(res) {
                return res.data;
            }).catch(function(error) {
                return error;
            });
        };

        var _getCalendarId = function(siteId, subsiteId) {
            if (!subsiteId) {
                return calendarService.getForSite(siteId).then(function(res) {
                    return res.data;
                }).catch(function(error) {
                    return error;
                });
            } else {
                return calendarService.getForSubSite(subsiteId).then(function(res) {
                    return res.data;
                }).catch(function(error) {
                    return error;
                });
            }
        };

        var _getCompanies = function() {
            return companyService.getAll().then(function(res) {
                return res.data;
            }).catch(function(error) {
                return error;
            });
        };

        var _getContractDetails = function(employeeId) {
            return contractService.getContractForEmployee(employeeId).then(function(res) {
                return res.data;
            }).catch(function(error) {
                return error;
            });
        };

        var _getEmployeeDetails = function(userId) {
            return employeeService.getByUserId(userId).then(function(res) {
                return res.data;
            }).catch(function(error) {
                return error;
            });
        };

        var _getEmployeeNameAndId = function() {
            return employeeService.getEmployeeNameAndId().then(function(res) {
                return res.data;
            }).catch(function(error) {
                return error;
            });
        };

        var _getEmployeeTypes = function() {
            return employeeTypeService.getAll().then(function(res) {
                return res.data;
            }).catch(function(error) {
                return error;
            });
        };

        var _getInvalidShiftProfiles = function() {
            return shiftProfileService.returnInvalidShiftProfiles().then(function(res) {
                return res.data;
            }).catch(function(error) {
                return error;
            });
        };

        var _getLeaveTypes = function() {
            return leaveTypeService.getAll().then(function(res) {
                return res.data;
            }).catch(function(error) {
                return error;
            });
        };

        var _getPersonalCalendarObjectsByDateRange = function(staffCalObjByDateRangeModel) {
            return employeeService.getPersonalCalendarObjectsByDateRange(staffCalObjByDateRangeModel).then(function(res) {
                return res.data;
            }).catch(function(error) {
                return error;
            });
        };

        var _getResources = function() {
            return resourceService.getAll().then(function(res) {
                return res.data;
            }).catch(function(error) {
                return error;
            });
        };

        var _getShiftProfileData = function(shiftProfileId) {
            return shiftProfileService.getForId(shiftProfileId).then(function(res) {
                return res.data;
            }).catch(function(error) {
                return error;
            });
        };

        var _getShiftTemplateBySearchCriteria = function(searchCriteriaModel) {
            return shiftTemplateService.getBySearchCriteria(searchCriteriaModel).then(function(res) {
                return res.data;
            }).catch(function(error) {
                return error;
            });
        };

        var _getSiteAccessDetails = function(staffId) {
            return sitePersonnelLookupService.getAllPlusInactiveForEmployee(staffId).then(function (res) {
                return res.data;
            }).catch(function(error) {
                return error;
            });
        };

        var _getSitesAndSubSites = function() {
            return siteService.getAll().then(function(res) {
                return res.data;
            }).catch(function(error) {
                return error;
            });
        };

        var _getSystemAccessRoles = function() {
            return systemAccessRoleService.getAll().then(function(res) {
                return res.data;
            }).catch(function(error) {
                return error;
            });
        };

        var _getTimeAdjustmentForms = function() {
            return timeAdjustFormService.getAllUnapproved().then(function(res) {
                return res.data;
            }).catch(function(error) {
                return error;
            });
        };

        var _getUnmatchedZkTimeDataForEmployee = function(zktUnmatchedModel) {
            return zktService.getUnmatchedZkTimeDataForEmployee(zktUnmatchedModel).then(function(res) {
                return res.data;
            }).catch(function(error) {
                return error;
            });
        };

        var _getUserDefinedSystemConfigs = function() {
            return userDefinedSystemConfigService.getAll().then(function(res) {
                return res.data;
            }).catch(function(error) {
                return error;
            });
        };

        var _getUserDetails = function(userId) {
            return userService.getById(userId).then(function(res) {
                return res.data;
            }).catch(function(error) {
                return error;
            });
        };

        var _getUsernameAndId = function() {
            return userService.getUsernameAndId().then(function(res) {
                return res.data;
            }).catch(function(error) {
                return error;
            });
        };

        var _runDailyActivityReport = function(paramsModel) {
            return reportService.runDailyActivityReport(paramsModel).then(function(res) {
                return res;
            }).catch(function(error) {
                return error;
            });
        };

        var _runMonthlyPayrollReport = function(paramsModel) {
            return reportService.runMonthlyPayrollReport(paramsModel).then(function(res) {
                //return res.data;
                return res;
            }).catch(function(error) {
                return error;
            });
        };

        var _updateLeaveRequest = function(leaveRequestModel) {
            leaveRequestService.adminUpdate(leaveRequestModel).then(function (res) {
                if (res.status != 200) {
                    var errorMsg = res.data.Message || "Staff Calender Update error.";
                    adminAreaServiceFactory.notify(errorMsg, "leave-request-update-failure-event");
                } else {
                    adminAreaServiceFactory.notify(null, "leave-request-update-success-event");
                }
            }).catch(function (error) {
                adminAreaServiceFactory.notify(error, "leave-request-update-failure-event");
            });
        };

        var _updateResource = function(resourceModel) {
            resourceService.update(resourceModel).then(function(res) {
                adminAreaServiceFactory.notify(null, "resource-update-complete-event");
            }).catch(function(error) {
                adminAreaServiceFactory.notify(error, "resource-update-failure-event");
            });
        };

        var _updateShiftProfile = function(shiftProfileModel) {
            shiftProfileService.update(shiftProfileModel).then(function(res) {
                helperService.toasterSuccess(null, res.data, "admin.dash");
            }).catch(function(error) {
                helperService.toasterError(null, error);
            });
        };

        var _updateShiftTemplate = function(shiftTemplateModel) {
            return shiftTemplateService.update(shiftTemplateModel).then(function(res) {
                return res.data;
            }).catch(function(error) {
                return error;
            });
        };

        var _updateSystemConfig = function(udscModel) {
            userDefinedSystemConfigService.update(udscModel).then(function(res) {
                adminAreaServiceFactory.notify(null, "sysconfig-update-complete-event");
            }).catch(function(error) {
                adminAreaServiceFactory.notify(error, "sysconfig-update-failure-event");
            });
        };

        var _updateTimeAdjustmentFormAndShiftProfile = function(tafUpdateModel) {
            return timeAdjustFormService.updateFormAndShiftProfile(tafUpdateModel).then(function(res) {
                helperService.toasterSuccess(null, res.data, "admin.dash");
            }).catch(function(error) {
                helperService.toasterError(null, error);
                return false;
            });
        };

        var _updateUser = function(masterViewModel) {
            if (masterViewModel.userIsDirty) {
                // call to user service
                userService.update(masterViewModel.userModel).then(function() {
                    adminAreaServiceFactory.userUpdateStatusObject.userUpdateSuccessful = true;
                    adminAreaServiceFactory.notify(null, "user-update-complete-event");
                }).catch(function(error) {
                    adminAreaServiceFactory.userUpdateStatusObject.userUpdateFailure = true;
                    adminAreaServiceFactory.notify(error, "user-update-failure-event");
                });
            }

            if (masterViewModel.employeeIsDirty) {
                // call to employee service
                if (masterViewModel.employeeModel.Id) {
                    employeeService.update(masterViewModel.employeeModel).then(function() {
                        adminAreaServiceFactory.userUpdateStatusObject.employeeUpdateSuccessful = true;
                        adminAreaServiceFactory.notify(null, "user-update-complete-event");
                    }).catch(function(error) {
                        adminAreaServiceFactory.userUpdateStatusObject.employeeUpdateFailure = true;
                        adminAreaServiceFactory.notify(error, "user-update-failure-event");
                    });
                }

                // if employee isActive is false, fire off a process where
                // all their future (i.e. from the datetime this code is called)
                // shifts are et to unassigned (and their manager emailed to
                // know to go fix up the rota).

                if (!masterViewModel.employeeModel.IsActive) {
                    shiftService.unassignEmployeeFromFutureShifts(masterViewModel.employeeModel.Id).then(function() {
                        // no need to do anything more
                    });
                }
            }

            if (masterViewModel.contractIsDirty) {
                // call to contract service
                if (masterViewModel.contractModel.Id) {
                    contractService.update(masterViewModel.contractModel).then(function() {
                        adminAreaServiceFactory.userUpdateStatusObject.contractUpdateSuccessful = true;
                        adminAreaServiceFactory.notify(null, "user-update-complete-event");
                    }).catch(function(error) {
                        adminAreaServiceFactory.userUpdateStatusObject.contractUpdateFailure = true;
                        adminAreaServiceFactory.notify(error, "user-update-failure-event");
                    });
                }
            }
            
            // call to site personnel lookups service
            if (masterViewModel.accessIsDirty) {
                var createCollection = [];
                var updateCollection = [];

                _.forEach(masterViewModel.siteAccessModels, function(record) {
                    // create or to update
                    if (!record.Id) {
                        // new
                        createCollection.push(record);
                    } else {
                        updateCollection.push(record);
                    }
                });


                if (createCollection.length > 0) {
                    sitePersonnelLookupService.createBatch(createCollection).then(function (res) {
                        //return res.data;
                        adminAreaServiceFactory.userUpdateStatusObject.siteAccessCreateSuccessful = true;
                        adminAreaServiceFactory.notify(null, "user-update-complete-event");
                    }).catch(function (error) {
                        adminAreaServiceFactory.userUpdateStatusObject.siteAccessCreateFailure = true;
                        adminAreaServiceFactory.notify(error, "user-update-failure-event");
                    });
                }
                
                if (updateCollection.length > 0) {
                    sitePersonnelLookupService.updateBatch(updateCollection).then(function (res) {
                        adminAreaServiceFactory.userUpdateStatusObject.siteAccessUpdateSuccessful = true;
                        adminAreaServiceFactory.notify(null, "user-update-complete-event");
                    }).catch(function (error) {
                        adminAreaServiceFactory.userUpdateStatusObject.siteAccessUpdateFailure = true;
                        adminAreaServiceFactory.notify(error, "user-update-failure-event");
                    });
                }
            }
        };


        // Local Storage
        var _populateCompaniesStore = function() {
            adminAreaServiceFactory.getCompanies().then(function(res) {
                helperService.setLocalStorageObject("companiesStore", res);
                adminAreaServiceFactory.notify(res, "companies-store-populated-event");
            }).catch(function(error) {
                helperService.toasterError(null, error);
            });
        };

        var _populateEmployeeNameAndIdStore = function() {
            adminAreaServiceFactory.getEmployeeNameAndId().then(function(res) {
                helperService.setLocalStorageObject("employeeNameAndIdStore", res);
                adminAreaServiceFactory.notify(res, "employee-name-id-store-populated-event");
            }).catch(function(error) {
                helperService.toasterError(null, error);
            });
        };

        var _populateEmployeeTypesStore = function() {
            adminAreaServiceFactory.getEmployeeTypes().then(function(res) {
                helperService.setLocalStorageObject("employeeTypesStore", res);
                adminAreaServiceFactory.notify(res, "employee-types-store-populated-event");
            }).catch(function(error) {
                helperService.toasterError(null, error);
            });
        };

        var _populateResourcesStore = function() {
            adminAreaServiceFactory.getResources().then(function(res) {
                helperService.setLocalStorageObject("resourcesStore", res);
                adminAreaServiceFactory.notify(res, "resources-store-populated-event");
            }).catch(function(error) {
                helperService.toasterError(null, error);
            });
        };
        
        var _populateSitesStore = function() {
            adminAreaServiceFactory.getSitesAndSubSites().then(function (res) {
                var orderedSitesArr = _.orderBy(res, ["Name"], ["asc"]);

                helperService.setLocalStorageObject("sitesStore", orderedSitesArr);
                adminAreaServiceFactory.notify(res, "sites-store-populated-event");
            }).catch(function(error) {
                helperService.toasterError(null, error);
            });
        };


        adminAreaServiceFactory.batchCreateShiftTemplates = _batchCreateShiftTemplates;

        adminAreaServiceFactory.checkMvmComponentsAreDirty = _checkMvmComponentsAreDirty;
        adminAreaServiceFactory.clearBaldockChildren = _clearBaldockChildren;
        adminAreaServiceFactory.createLeaveRequest = _createLeaveRequest;
        adminAreaServiceFactory.createResource = _createResource;
        adminAreaServiceFactory.createShiftProfileForUnmatchedZkt = _createShiftProfileForUnmatchedZkt;
        adminAreaServiceFactory.createShiftTemplate = _createShiftTemplate;
        adminAreaServiceFactory.createUser = _createUser;

        adminAreaServiceFactory.deleteLeaveRequest = _deleteLeaveRequest;

        adminAreaServiceFactory.getAllEmployeesByCompanyId = _getAllEmployeesByCompanyId;
        adminAreaServiceFactory.getCalendarId = _getCalendarId;
        adminAreaServiceFactory.getCompanies = _getCompanies;
        adminAreaServiceFactory.getContractDetails = _getContractDetails;
        adminAreaServiceFactory.getEmployeeDetails = _getEmployeeDetails;
        adminAreaServiceFactory.getEmployeeNameAndId = _getEmployeeNameAndId;
        adminAreaServiceFactory.getEmployeeTypes = _getEmployeeTypes;
        adminAreaServiceFactory.getInvalidShiftProfiles = _getInvalidShiftProfiles;
        adminAreaServiceFactory.getLeaveTypes = _getLeaveTypes;
        adminAreaServiceFactory.getPersonalCalendarObjectsByDateRange = _getPersonalCalendarObjectsByDateRange;
        adminAreaServiceFactory.getResources = _getResources;
        adminAreaServiceFactory.getShiftProfileData = _getShiftProfileData;
        adminAreaServiceFactory.getShiftTemplateBySearchCriteria = _getShiftTemplateBySearchCriteria;
        adminAreaServiceFactory.getSiteAccessDetails = _getSiteAccessDetails;
        adminAreaServiceFactory.getSitesAndSubSites = _getSitesAndSubSites;
        adminAreaServiceFactory.getSystemAccessRoles = _getSystemAccessRoles;
        adminAreaServiceFactory.getTimeAdjustmentForms = _getTimeAdjustmentForms;
        adminAreaServiceFactory.getUnmatchedZkTimeDataForEmployee = _getUnmatchedZkTimeDataForEmployee;
        adminAreaServiceFactory.getUserDefinedSystemConfigs = _getUserDefinedSystemConfigs;
        adminAreaServiceFactory.getUserDetails = _getUserDetails;
        adminAreaServiceFactory.getUsernameAndId = _getUsernameAndId;

        adminAreaServiceFactory.hydrateBaldock = _hydrateBaldock;
        adminAreaServiceFactory.hydratePsycare = _hydratePsycare;

        adminAreaServiceFactory.openCreateNewShiftTemplateDialog = _openCreateNewShiftTemplateDialog;
        adminAreaServiceFactory.openDailyActivityReportDialog = _openDailyActivityReportDialog;
        adminAreaServiceFactory.openMonthlyPayrollReportDialog = _openMonthlyPayrollReportDialog;
        adminAreaServiceFactory.openStaffCalendarLeaveDialog = _openStaffCalendarLeaveDialog;

        adminAreaServiceFactory.populateCompaniesStore = _populateCompaniesStore;
        adminAreaServiceFactory.populateEmployeeNameAndIdStore = _populateEmployeeNameAndIdStore;
        adminAreaServiceFactory.populateEmployeeTypesStore = _populateEmployeeTypesStore;
        adminAreaServiceFactory.populateResourcesStore = _populateResourcesStore;
        adminAreaServiceFactory.populateSitesStore = _populateSitesStore;

        adminAreaServiceFactory.returnLeaveRequestDeleteViewModel = _returnLeaveRequestDeleteViewModel;
        adminAreaServiceFactory.returnNewDailyActivityReportParamsModel = _returnNewDailyActivityReportParamsModel;
        adminAreaServiceFactory.returnNewInvalidShiftProfileViewModel = _returnNewInvalidShiftProfileViewModel;
        adminAreaServiceFactory.returnNewLeaveRequestModel = _returnNewLeaveRequestModel;
        adminAreaServiceFactory.returnNewMonthlyPayrollReportParamsModel = _returnNewMonthlyPayrollReportParamsModel;
        adminAreaServiceFactory.returnNewResourceModel = _returnNewResourceModel;
        adminAreaServiceFactory.returnNewShiftTemplateModel = _returnNewShiftTemplateModel;
        adminAreaServiceFactory.returnNewTimeAdjustmentFormUpdateModel = _returnNewTimeAdjustmentFormUpdateModel;
        adminAreaServiceFactory.returnNewTrainingShiftModel = _returnNewTrainingShiftModel;
        adminAreaServiceFactory.returnNewUserViewModel = _returnNewUserViewModel;
        adminAreaServiceFactory.returnShiftStatusString = _returnShiftStatusString;
        adminAreaServiceFactory.returnStaffCalendarConfig = _returnStaffCalendarConfig;
        adminAreaServiceFactory.returnUserDefinedSystemConfigModel = _returnUserDefinedSystemConfigModel;
        adminAreaServiceFactory.runDailyActivityReport = _runDailyActivityReport;
        adminAreaServiceFactory.runMonthlyPayrollReport = _runMonthlyPayrollReport;

        adminAreaServiceFactory.selectBaldockParent = _selectBaldockParent;
        adminAreaServiceFactory.staffCalendarDaysOfWeekHeaders = _staffCalendarDaysOfWeekHeaders;

        adminAreaServiceFactory.updateLeaveRequest = _updateLeaveRequest;
        adminAreaServiceFactory.updateResource = _updateResource;
        adminAreaServiceFactory.updateShiftProfile = _updateShiftProfile;
        adminAreaServiceFactory.updateShiftTemplate = _updateShiftTemplate;
        adminAreaServiceFactory.updateSystemConfig = _updateSystemConfig;
        adminAreaServiceFactory.updateTimeAdjustmentFormAndShiftProfile = _updateTimeAdjustmentFormAndShiftProfile;
        adminAreaServiceFactory.updateUser = _updateUser;
        adminAreaServiceFactory.userUpdateStatusObject = _userUpdateStatusObject;
        
        adminAreaServiceFactory.notify = _notify;
        adminAreaServiceFactory.subscribe = _subscribe;

        return adminAreaServiceFactory;
    }
]);