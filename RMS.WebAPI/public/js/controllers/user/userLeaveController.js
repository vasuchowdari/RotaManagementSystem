"use strict";

var ap = angular.module("controllers");
app.controller("userLeaveCtrl", [
    "$scope",
    "$state",
    "helperService",
    "userAreaService",
    "blockUI",
    function ($scope, $state, helperService, userAreaService, blockUI) {
        // Block And Auth
        helperService.blockAndAuth($scope, blockUI);


        // Declarations
        $scope.dtp = userAreaService.dateTimePickerConfig;
        $scope.endHalfDay = false;
        $scope.formDisable = false;
        $scope.formModel = userAreaService.newLeaveRequestModel();
        $scope.leaveTypes = null;
        $scope.startHalfDay = false;


        // Functions
        $scope.calculateAmountRequested = function () {
            $scope.formModel.amountRequested =
                userAreaService.calculatedAmountRequested(
                    $scope.formModel.startDateTime,
                    $scope.formModel.endDateTime,
                    $scope.startHalfDay,
                    $scope.endHalfDay);
        };

        $scope.getLeaveTypes = function() {
            userAreaService.getLeaveTypes().then(function(res) {
                $scope.leaveTypes = res;
            });
        };

        $scope.sendLeaveRequestForm = function () {
            $scope.formDisable = true;

            var tempLeaveTypeObj = _.find($scope.leaveTypes, { "Id": $scope.selectedLeaveTypeId });
            $scope.formModel.leaveTypeName = tempLeaveTypeObj.Name;
            $scope.formModel.leaveTypeId = $scope.selectedLeaveTypeId;

            $scope.formModel.startDateTime = helperService.formatDateTimeStringForServer($scope.formModel.startDateTime);
            $scope.formModel.endDateTime = helperService.formatDateTimeStringForServer($scope.formModel.endDateTime);

            var isBefore = moment($scope.formModel.endDateTime).isBefore($scope.formModel.startDateTime);
            if (!isBefore) {
                userAreaService.createLeaveRequest($scope.formModel).then(function(res) {
                    $scope.formDisable = res;
                });
            } else {
                helperService.toasterError("Leave Dates Error", "You cannot have an End Date before the Start Date.");
                $scope.formDisable = false;
            }
        };


        // Init
        $scope.init = function () {
            if (!$scope.authentication.isAuth) {
                helperService.notAuth();
            }

            var lsObj = helperService.getLocalStorageObject("employeeModelStore");
            $scope.formModel.employeeId = lsObj.Id;
            $scope.formModel.staffName = lsObj.UserModel.Firstname + " " + lsObj.UserModel.Lastname;

            $scope.getLeaveTypes();

            blockUI.stop();
        };

        $scope.init();
    }
]);