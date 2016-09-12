"use strict";

var ap = angular.module("controllers");
app.controller("userAdjustmentCtrl", [
    "$scope",
    "$state",
    "helperService",
    "userAreaService",
    "blockUI",
    function ($scope, $state, helperService, userAreaService, blockUI) {
        // Block And Auth
        helperService.blockAndAuth($scope, blockUI);


        // Declarations
        $scope.formDisable = false;
        $scope.formModel = userAreaService.newShiftTimeAdjustmentModel();
        $scope.hstep = 1;
        $scope.mstep = 1;


        // Functions
        $scope.sendTimeAdjustmentForm = function() {
            $scope.formDisable = true;

            $scope.formModel.actualStartDateTime = helperService.formatDateTimeStringForServer($scope.formModel.actualStartDateTime);
            $scope.formModel.actualEndDateTime = helperService.formatDateTimeStringForServer($scope.formModel.actualEndDateTime);

            $scope.formModel.zktStartDateTime = helperService.formatDateTimeStringForServer($scope.formModel.zktStartDateTime);
            $scope.formModel.zktEndDateTime = helperService.formatDateTimeStringForServer($scope.formModel.zktEndDateTime);

            userAreaService.sendTimeAdjustmentFormEmail($scope.formModel).then(function(res) {
                $scope.formDisable = res;
            });
        };

        $scope.mapStateParamsToModel = function() {
            $scope.shiftDate = moment($state.params.shiftData.StartDateTime).format("DD/MM/YYYY");

            $scope.formModel.shiftId = $state.params.shiftData.ShiftId;
            $scope.formModel.shiftProfileId = $state.params.shiftData.Id;
            $scope.formModel.shiftStartDateTime = $state.params.shiftData.StartDateTime;
            $scope.formModel.shiftEndDateTime = $state.params.shiftData.EndDateTime;
            $scope.formModel.employeeId = $state.params.shiftData.EmployeeId;

            if ($state.params.shiftData.ZktStartDateTime === "-") {
                $scope.formModel.actualStartDateTime = moment($state.params.shiftData.StartDateTime, moment.ISO_8601);
                $scope.formModel.zktStartDateTime = moment($state.params.shiftData.StartDateTime, moment.ISO_8601);
            } else {
                $scope.formModel.actualStartDateTime = moment($state.params.shiftData.ActualStartDateTime, moment.ISO_8601);
                $scope.formModel.zktStartDateTime = moment($state.params.shiftData.ZktStartDateTime, moment.ISO_8601);
            }

            if ($state.params.shiftData.ZktEndDateTime === "-") {
                $scope.formModel.actualEndDateTime = moment($state.params.shiftData.EndDateTime, moment.ISO_8601);
                $scope.formModel.zktEndDateTime = moment($state.params.shiftData.EndDateTime, moment.ISO_8601);
            } else {
                $scope.formModel.actualEndDateTime = moment($state.params.shiftData.ActualEndDateTime, moment.ISO_8601);
                $scope.formModel.zktEndDateTime = moment($state.params.shiftData.ZktEndDateTime, moment.ISO_8601);
            }

            userAreaService.getShiftLocationName($state.params.shiftData.ShiftId).then(function(res) {
                $scope.formModel.shiftLocation = res;
            });
        };


        // Init
        $scope.init = function() {
            if (!$scope.authentication.isAuth) {
                helperService.notAuth();
            }

            var lsObj = helperService.getLocalStorageObject("employeeModelStore");
            $scope.formModel.employeeId = lsObj.Id;
            $scope.formModel.staffName = lsObj.UserModel.Firstname + " " + lsObj.UserModel.Lastname;

            if (!$state.params.shiftData) {
                $state.params.shiftData = helperService.getLocalStorageObject("employeeShiftData");
            } else {
                helperService.setLocalStorageObject("employeeShiftData", $state.params.shiftData);
            }

            $scope.mapStateParamsToModel();
            blockUI.stop();
        };

        $scope.init();
    }
]);