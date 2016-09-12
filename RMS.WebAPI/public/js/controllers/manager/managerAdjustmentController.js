"use script";

var app = angular.module("controllers");
app.controller("managerAdjustmentCtrl", [
    "$scope",
    "$state",
    "helperService",
    "managerAreaService",
    "blockUI",
    function($scope, $state, helperService, managerAreaService, blockUI) {
        // Block And Auth
        helperService.blockAndAuth($scope, blockUI);


        // Declarations
        $scope.formModel = {};
        $scope.shiftDate = null;
        $scope.tafUpdateModel = managerAreaService.returnNewTimeAdjustmentFormUpdateModel();
        $scope.reasons = ["ApprOvertime", "Sickness", "StaffLate", "Other"];


        // Functions
        $scope.actionTimeAdjustment = function() {
            $scope.tafUpdateModel.timeAdjustmentFormId = $state.params.timeAdjustmentFormData.Id;
            $scope.tafUpdateModel.shiftProfileId = $state.params.timeAdjustmentFormData.ShiftProfileId;
            $scope.tafUpdateModel.isManagerApproved = true;
            $scope.tafUpdateModel.isAdminApproved = false;

            // defaulted here as these values don't get updated by the manager,
            // but the C# view model needs valid datetimes passed to it
            $scope.tafUpdateModel.actualStartDateTime = moment().format("YYYY-MM-DD HH:mm:ss");
            $scope.tafUpdateModel.actualEndDateTime = moment().format("YYYY-MM-DD HH:mm:ss");
            $scope.tafUpdateModel.zktStartDateTime = moment().format("YYYY-MM-DD HH:mm:ss");
            $scope.tafUpdateModel.zktEndDateTime = moment().format("YYYY-MM-DD HH:mm:ss");

            managerAreaService.updateTimeAdjustmentFormAndShiftProfile($scope.tafUpdateModel)
                .then(function(res) { })
                .catch(function(error) { });
        };

        $scope.mapStateParamsToModel = function() {
            $scope.shiftDate = moment($state.params.timeAdjustmentFormData.ShiftStartDateTime).format("DD/MM/YYYY");
            
            $scope.formModel.staffName = $state.params.timeAdjustmentFormData.StaffName;
            $scope.formModel.shiftLocation = $state.params.timeAdjustmentFormData.ShiftLocation;

            $scope.formModel.shiftStart = moment($state.params.timeAdjustmentFormData.ShiftStartDateTime).format("HH:mm");
            $scope.formModel.shiftEnd = moment($state.params.timeAdjustmentFormData.ShiftEndDateTime).format("HH:mm");

            $scope.formModel.clockIn = moment($state.params.timeAdjustmentFormData.ZktStartDateTime).format("HH:mm:ss");
            $scope.formModel.clockOut = moment($state.params.timeAdjustmentFormData.ZktEndDateTime).format("HH:mm:ss");

            $scope.formModel.actualIn = moment($state.params.timeAdjustmentFormData.ActualStartDateTime).format("HH:mm:ss");
            $scope.formModel.actualOut = moment($state.params.timeAdjustmentFormData.ActualEndDateTime).format("HH:mm:ss");

            $scope.formModel.missedClockIn = $state.params.timeAdjustmentFormData.MissedClockIn;
            $scope.formModel.missedClockOut = $state.params.timeAdjustmentFormData.MissedClockOut;
            $scope.formModel.lateIn = $state.params.timeAdjustmentFormData.LateIn;
            $scope.formModel.earlyOut = $state.params.timeAdjustmentFormData.EarlyOut;
            $scope.formModel.notes = $state.params.timeAdjustmentFormData.Notes;
        };


        // Init
        $scope.init = function() {
            if (!$scope.authentication.isAuth) {
                helperService.notAuth();
            }

            if (!$state.params.timeAdjustmentFormData) {
                $state.params.timeAdjustmentFormData = helperService.getLocalStorageObject("timeAdjustmentFormData");
            } else {
                helperService.setLocalStorageObject("timeAdjustmentFormData", $state.params.timeAdjustmentFormData);
            }

            $scope.mapStateParamsToModel();
            blockUI.stop();
        };

        $scope.init();
    }
]);