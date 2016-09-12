"use strict";

var app = angular.module("controllers");
app.controller("adminShiftTemplateDetailsCtrl", [
    "$scope",
    "$state",
    "helperService",
    "adminAreaService",
    "adminActionStatusService",
    "blockUI",
    function ($scope, $state, helperService, adminAreaService, adminActionStatusService, blockUI) {
        // Block And Auth
        helperService.blockAndAuth($scope, blockUI);


        // Event Subscription


        // Declarations
        var formObj = null;
        $scope.hideNameWarning = true;
        $scope.hideShiftRateWarning = true;
        $scope.hideUnpaidBreakWarning = true;
        $scope.hideUpdateRoleWarning = true;
        $scope.resources = [];
        $scope.shiftTemplateModel = null;
        var INVALID_SHIFT_TEMPLATE = adminActionStatusService.SHIFT_TEMPLATE_VAL_ENUM;


        // Functions
        $scope.removeNameWarning = function () {
            if (!$scope.hideNameWarning) {
                $scope.hideNameWarning = true;
            }
        };

        $scope.removeShiftRateWarning = function () {
            if (!$scope.hideShiftRateWarning) {
                $scope.hideShiftRateWarning = true;
            }
        };

        $scope.removeUnpaidBreakWarning = function () {
            if (!$scope.hideUnpaidBreakWarning) {
                $scope.hideUnpaidBreakWarning = true;
            }
        };

        $scope.removeUpdateRoleWarning = function () {
            if (!$scope.hideUpdateRoleWarning) {
                $scope.hideUpdateRoleWarning = true;
            }
        };

        $scope.submitShiftTemplateUpdateForm = function () {
            var self = this;

            formObj = angular.copy(self.updateShiftTemplateForm);
            var tempShiftTemplateModel = angular.copy(self.shiftTemplateModel);

            var valObj = adminActionStatusService.validateShiftTemplateUpdateForm(formObj);

            if (!valObj[0]) {
                if (valObj[1] === INVALID_SHIFT_TEMPLATE.name) {
                    $scope.hideNameWarning = false;
                }

                if (valObj[1] === INVALID_SHIFT_TEMPLATE.role) {
                    $scope.hideUpdateRoleWarning = false;
                }

                if (valObj[1] === INVALID_SHIFT_TEMPLATE.shiftRate) {
                    $scope.hideShiftRateWarning = false;
                }

                if (valObj[1] === INVALID_SHIFT_TEMPLATE.unpaidBreak) {
                    $scope.hideUnpaidBreakWarning = false;
                }

                return;
            }

            tempShiftTemplateModel.StartTime = moment(tempShiftTemplateModel.StartTime).format("YYYY-MM-DD HH:mm");
            tempShiftTemplateModel.EndTime = moment(tempShiftTemplateModel.EndTime).format("YYYY-MM-DD HH:mm");

            if (moment(tempShiftTemplateModel.EndTime).isBefore(tempShiftTemplateModel.StartTime)) {
                tempShiftTemplateModel.EndTime = moment(tempShiftTemplateModel.EndTime).add(1, "days").format("YYYY-MM-DD HH:mm");
            }

            tempShiftTemplateModel.Duration = moment(tempShiftTemplateModel.EndTime).diff(tempShiftTemplateModel.StartTime);
            tempShiftTemplateModel.UnpaidBreakDuration = helperService.returnCalculatedMillisecondsFromMinutesValue(tempShiftTemplateModel.UnpaidBreakDuration);

            adminAreaService.updateShiftTemplate(tempShiftTemplateModel).then(function(res) {
                helperService.toasterSuccess("Update Success", res);
                blockUI.stop();
            }).catch(function(error) {
                helperService.toasterError(null, error);
                blockUI.stop();
            });
        };


        // Init
        $scope.init = function () {
            if (!$scope.authentication.isAuth) {
                helperService.notAuth();
            }

            if (!$state.params.shiftTemplateData) {
                $state.params.shiftTemplateData = helperService.getLocalStorageObject("shiftTemplateData");
            } else {
                helperService.setLocalStorageObject("shiftTemplateData", $state.params.shiftTemplateData);
            }

            $scope.shiftTemplateModel = $state.params.shiftTemplateData;
            $scope.shiftTemplateModel.UnpaidBreakDuration = helperService.returnCalculatedMinutesFromMillisecondsValue($scope.shiftTemplateModel.UnpaidBreakDuration);

            $scope.shiftTemplateModel.StartTime = moment($scope.shiftTemplateModel.StartTime, moment.ISO_8601);
            $scope.shiftTemplateModel.EndTime = moment($scope.shiftTemplateModel.EndTime, moment.ISO_8601);

            $scope.resources = helperService.getLocalStorageObject("resourcesStore");

            blockUI.stop();
        };

        $scope.init();
    }
]);