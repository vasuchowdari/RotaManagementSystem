"use strict";

var app = angular.module("services");
app.factory("managerAreaService", [
    "$rootScope",
    "helperService",
    "configService",
    "shiftProfileService",
    "leaveRequestService",
    "timeAdjustFormService",
    "rotaService",
    "ngDialog",
    function ($rootScope, helperService, configService, shiftProfileService, leaveRequestService,
        timeAdjustFormService, rotaService, ngDialog) {
        var managerAreaServiceFactory = {};


        // Event Handling
        var _notify = function(data, eventName) {
            $rootScope.$emit(eventName, data);
        };

        var _subscribe = function(scope, callback, eventName) {
            var handler = $rootScope.$on(eventName, callback);
            scope.$on("$destroy", handler);
        };


        // Functions
        var _formatShiftProfileDateTimesForDisplay = function(shiftProfile) {
            if (shiftProfile.ActualStartDateTime !== "-") {
                shiftProfile.ActualStartDateTime = moment(shiftProfile.ActualStartDateTime).format("DD/MM/YYYY HH:mm:ss");
            }

            if (shiftProfile.ActualEndDateTime !== "-") {
                shiftProfile.ActualEndDateTime = moment(shiftProfile.ActualEndDateTime).format("DD/MM/YYYY HH:mm:ss");
            }

            if (shiftProfile.StartDateTime !== "-") {
                shiftProfile.StartDateTime = moment(shiftProfile.StartDateTime).format("DD/MM/YYYY HH:mm:ss");
            }

            if (shiftProfile.EndDateTime !== "-") {
                shiftProfile.EndDateTime = moment(shiftProfile.EndDateTime).format("DD/MM/YYYY HH:mm:ss");
            }
        };

        var _generateMemberOfStaffDisplayNamesFromEmployee = function() {
            var staffList = helperService.getLocalStorageObject("siteEmployeesStore");
            _.forEach(staffList, function (staffMember) {
                staffMember.displayName = staffMember.UserModel.Firstname + " " + staffMember.UserModel.Lastname;
            });
            helperService.setLocalStorageObject("siteEmployeesStore", staffList);
        };

        var _returnNewResourceRequirementModel = function() {
            var resRqModel = {
                employeeId: null,
                resource: {},
                shiftTemplate: [],
                shift: {
                    StartDate: moment(),
                    EndDate: moment(),
                    ShiftTemplateId: null,
                    ShiftPatternId: null
                }
            };

            return resRqModel;
        };

        var _returnNewTimeAdjustmentFormUpdateModel = function() {
            return timeAdjustFormService.returnNewTimeAdjustmentUpdateFormModel();
        };

        var _returnShiftStatusString = function(statusInt) {
            var strToReturn = null;

            switch (statusInt) {
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

        var _setShiftProfileActualsByStatus = function(shiftProfile) {
            switch(shiftProfile.Status) {
                case 5:
                    shiftProfile.ActualStartDateTime = "-";
                    shiftProfile.TimeWorked = "00:00:00";
                    break;
                case 6:
                    shiftProfile.ActualEndDateTime = "-";
                    shiftProfile.TimeWorked = "00:00:00";
                    break;
                case 7:
                    shiftProfile.ActualStartDateTime = "-";
                    shiftProfile.ActualEndDateTime = "-";
                    shiftProfile.TimeWorked = "00:00:00";
                    break;
            }
        };


        // API Calls
        var _getLeaveRequests = function(staffIds) {
            return leaveRequestService.getLeaveRequestsForIds(staffIds).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        }

        var _getTimeAdjustmentForms = function(staffIds) {
            return timeAdjustFormService.getTimeAdjustmentFormsForIds(staffIds).then(function(res) {
                return res.data;
            }).catch(function(error) {
                return error;
            });
        };

        var _updateTimeAdjustmentFormAndShiftProfile = function(tafUpdateModel) {
            return timeAdjustFormService.updateFormAndShiftProfile(tafUpdateModel).then(function(res) {
                helperService.toasterSuccess(null, res.data, "manager.dash");
            }).catch(function(error) {
                helperService.toasterError(null, error);
                return false;
            });
        };


        // Dialogs
        var _openNewCalResRqDialog = function() {
            rotaService.openNewCalResRqDialog();
        };

        var _openRotaResourceExplanationDialog = function() {
            ngDialog.openConfirm({
                    showClose: true,
                    closeByNavigation: false,
                    closeByDocument: false,
                    template: configService.path + "/public/views/dialog-tmpls/rota-resource-explanation.html",
                    controller: "managerRotaResourceExplanationCtrl"
            });
        };

        var _openShiftForApprovalDialog = function() {
            ngDialog.open({
                showClose: true,
                closeByNavigation: false,
                closeByDocument: false,
                template: configService.path + "/public/views/dialog-tmpls/shift-for-approval.html",
                controller: "managerShiftForApprovalCtrl"
            });
        };


        // Wire up
        managerAreaServiceFactory.returnNewResourceRequirementModel = _returnNewResourceRequirementModel;
        managerAreaServiceFactory.formatShiftProfileDateTimesForDisplay = _formatShiftProfileDateTimesForDisplay;
        managerAreaServiceFactory.returnNewTimeAdjustmentFormUpdateModel = _returnNewTimeAdjustmentFormUpdateModel;
        managerAreaServiceFactory.returnShiftStatusString = _returnShiftStatusString;
        managerAreaServiceFactory.setShiftProfileActualsByStatus = _setShiftProfileActualsByStatus;
        managerAreaServiceFactory.updateTimeAdjustmentFormAndShiftProfile = _updateTimeAdjustmentFormAndShiftProfile;

        managerAreaServiceFactory.generateMemberOfStaffDisplayNamesFromEmployee = _generateMemberOfStaffDisplayNamesFromEmployee;
        managerAreaServiceFactory.getLeaveRequests = _getLeaveRequests;
        managerAreaServiceFactory.getTimeAdjustmentForms = _getTimeAdjustmentForms;

        managerAreaServiceFactory.notify = _notify;
        managerAreaServiceFactory.subscribe = _subscribe;
        
        managerAreaServiceFactory.openNewCalResRqDialog = _openNewCalResRqDialog;
        managerAreaServiceFactory.openRotaResourceExplanationDialog = _openRotaResourceExplanationDialog;
        managerAreaServiceFactory.openShiftForApprovalDialog = _openShiftForApprovalDialog;

        return managerAreaServiceFactory;
    }
]);