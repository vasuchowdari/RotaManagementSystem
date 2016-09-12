"use strict";

var app = angular.module("controllers");
app.controller("adminStaffCalenderLeaveCtrl", [
    "$scope",
    "$state",
    "helperService",
    "adminAreaService",
    "adminActionStatusService",
    "blockUI",
    function ($scope, $state, helperService, adminAreaService, adminActionStatusService,
        blockUI) {
        // Block And Auth
        helperService.blockAndAuth($scope, blockUI);


        // Event Subscription
        adminAreaService.subscribe($scope, function() {
            $scope.reset();
            adminActionStatusService.handleLeaveRequestCreateSuccess();

            adminAreaService.notify(null, "reload-staff-calendar-event");
        }, "leave-request-create-success-event");

        adminAreaService.subscribe($scope, function(e, errorMsg) {
            adminActionStatusService.handleLeaveRequestCreateError(errorMsg);
        }, "leave-request-create-failure-event");

        adminAreaService.subscribe($scope, function(e, successMsg) {
            adminActionStatusService.handleLeaveRequestDeleteSuccess(successMsg);
            adminAreaService.notify(null, "reload-staff-calendar-event");

            $scope.closeDeleteLeaveDialog();
        }, "leave-request-delete-success-event");

        adminAreaService.subscribe($scope, function(e, errorMsg) {
            adminActionStatusService.handleLeaveRequestDeleteError(errorMsg);
        }, "leave-request-delete-failure-event");

        adminAreaService.subscribe($scope, function() {
            adminActionStatusService.handleLeaveRequestUpdateSuccess();

            adminAreaService.notify(null, "reload-staff-calendar-event");
        }, "leave-request-update-success-event");

        adminAreaService.subscribe($scope, function(e, errorMsg) {
            adminActionStatusService.handleLeaveRequestUpdateError(errorMsg);
        }, "leave-request-update-failure-event");


        // Declarations
        $scope.dateFormat = "DD/MM/YYYY HH:mm";
        $scope.disableDelete = true;
        $scope.dtpStart = {
            startView: "day",
            minView: "minute",
            dropdownSelector: ".dropdown-toggle"
        };

        var formObj = null;
        $scope.hours = 0;
        $scope.leaveRequestModel = adminAreaService.returnNewLeaveRequestModel();
        $scope.leaveTypes = [];
        $scope.minutes = 0;
        var mvm = helperService.getLocalStorageObject("userMasterViewModel");
        $scope.unpaidBreakDuration = 0;
        $scope.unpaidBreakDurationDisabled = true;
        $scope.username = null;


        // Functions
        $scope.closeDeleteLeaveDialog = function() {
            var self = this;
            self.closeThisDialog();
        };

        $scope.submitLeaveRequest = function () {
            var self = this;
            formObj = angular.copy(self.staffLeaveForm);

            if (!adminActionStatusService.validateLeaveRequestObjectForm(formObj)) {
                return;
            }

            $scope.leaveRequestModel.StartDateTime = helperService.formatDateTimeStringForServer($scope.leaveRequestModel.StartDateTime);
            $scope.leaveRequestModel.EndDateTime = helperService.formatDateTimeStringForServer($scope.leaveRequestModel.EndDateTime);
            $scope.leaveRequestModel.AmountRequested = moment.duration(($scope.hours * 60) + $scope.minutes, "m").asMilliseconds();

            if (!$scope.disableDelete) {
                adminAreaService.updateLeaveRequest($scope.leaveRequestModel);
            } else {
                adminAreaService.createLeaveRequest($scope.leaveRequestModel);
            }
        };

        $scope.deleteLeaveRequest = function() {
            adminAreaService.deleteLeaveRequest($scope.ngDialogData.id);
        };

        $scope.getLeaveTypes = function() {
            adminAreaService.getLeaveTypes().then(function(res) {
                $scope.leaveTypes = res;
                $scope.leaveTypes = _.orderBy($scope.leaveTypes, ["Name"], ["asc"]);
                blockUI.stop();
            }).catch(function(error) {
                helperService.toasterError(null, error);
                blockUI.stop();
            });
        };

        $scope.reset = function() {
            // maybe an angular form pristine call here too?
            $scope.hours = 0;
            $scope.leaveRequestModel = adminAreaService.returnNewLeaveRequestModel();
            $scope.leaveTypes = [];
            $scope.minutes = 0;
            $scope.unpaidBreakDuration = 0;
            $scope.unpaidBreakDurationDisabled = true;

            $scope.getLeaveTypes();
        };

        $scope.toggleUnpaidBreakDuration = function() {
            if ($scope.leaveRequestModel.LeaveTypeId !== 7) {
                $scope.unpaidBreakDurationDisabled = true;
            } else {
                $scope.unpaidBreakDurationDisabled = false;
            }
        };


        // Init
        $scope.init = function() {
            if (!$scope.authentication.isAuth) {
                helperService.notAuth();
            }

            if ($scope.ngDialogData !== undefined) {
                $scope.username = mvm.userModel.Firstname + " " + mvm.userModel.Lastname;
                $scope.leaveRequestModel.EmployeeId = mvm.employeeModel.Id;
                $scope.leaveRequestModel.StartDateTime = moment($scope.ngDialogData.start.value);
                $scope.leaveRequestModel.EndDateTime = moment($scope.ngDialogData.end.value);

                var tempTime = moment.duration($scope.ngDialogData.amountRequested);
                $scope.hours = tempTime.hours();
                $scope.minutes = tempTime.minutes();

                $scope.leaveRequestModel.LeaveTypeId = $scope.ngDialogData.leaveTypeId;
                $scope.leaveRequestModel.Id = $scope.ngDialogData.id;
                //$scope.leaveRequestModel.IsAppproved = $scope.ngDialogData.isApproved;
                //$scope.leaveRequestModel.IsTaken = $scope.ngDialogData.isTaken;
                //$scope.leaveRequestModel.IsActive = $scope.ngDialogData.isActive;

                $scope.disableDelete = false;
                $scope.toggleUnpaidBreakDuration();
            } else {
                $scope.username = mvm.userModel.Firstname + " " + mvm.userModel.Lastname;
                $scope.leaveRequestModel.EmployeeId = mvm.employeeModel.Id;
            }
            
            $scope.getLeaveTypes();
            blockUI.stop();
        };

        $scope.init();
    }
]);