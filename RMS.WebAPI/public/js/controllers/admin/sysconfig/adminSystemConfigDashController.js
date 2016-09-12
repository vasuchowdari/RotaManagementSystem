"use strict";

var app = angular.module("controllers");
app.controller("adminSystemConfigDashCtrl", [
    "$scope",
    "$state",
    "helperService",
    "adminAreaService",
    "adminActionStatusService",
    "blockUI",
    function ($scope, $state, helperService, adminAreaService, adminActionStatusService, blockUI) {
        // Block And Auth
        helperService.blockAndAuth($scope, blockUI);


        // Event Subscriptions
        adminAreaService.subscribe($scope, function() {
            adminActionStatusService.handleSystemConfigUdateSuccess();
        }, "sysconfig-update-complete-event");

        adminAreaService.subscribe($scope, function(errorMsg) {
            adminActionStatusService.handleSystemConfigUdateError(errorMsg);
        }, "sysconfig-update-failure-event");


        // Declarations
        $scope.defaultSystemConfig = {};
        $scope.formObj = null;
        $scope.systemConfigs = [];


        // Functions
        $scope.getUserDefinedSystemConfigs = function() {
            adminAreaService.getUserDefinedSystemConfigs().then(function(res) {
                $scope.systemConfigs = res;
                // TODO: This is a hack FTM, in order to circumvent multiple companies but only one base code
                $scope.defaultSystemConfig = _.head($scope.systemConfigs);
            }).catch(function(error) {
                helperService.toasterError(null, error);
            }).finally(function () {
                blockUI.stop();
            });
        };

        $scope.submitSystemConfigUpdateForm = function() {
            var self = this;
            $scope.formObj = angular.copy(self.systemConfigUpdateForm);

            if (!adminActionStatusService.validateSystemConfigFormObject($scope.formObj)) {
                return;
            }

            adminAreaService.updateSystemConfig($scope.defaultSystemConfig);
        };


        // Init
        $scope.init = function () {
            if (!$scope.authentication.isAuth) {
                helperService.notAuth();
            }

            $scope.getUserDefinedSystemConfigs();
        };

        $scope.init();
    }
]);