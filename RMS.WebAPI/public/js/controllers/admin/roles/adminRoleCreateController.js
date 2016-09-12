"use strict";

var app = angular.module("controllers");
app.controller("adminRoleCreateCtrl", [
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
        adminAreaService.subscribe($scope, function () {
            adminAreaService.populateResourcesStore();
            adminActionStatusService.handleResourceCreateSuccess();
        }, "resource-create-complete-event");

        adminAreaService.subscribe($scope, function (errorMsg) {
            adminActionStatusService.handleResourceCreateError(errorMsg);
        }, "resource-create-failure-event");


        // Declarations
        var formObj = null;
        $scope.hideBaseRateWarning = true;
        $scope.hideNameWarning = true;
        $scope.resourceModel = adminAreaService.returnNewResourceModel();
        var ROLE_INVALID = adminActionStatusService.ROLE_VAL_ENUM;

        // Functions
        $scope.removeBaseRateWarning = function () {
            if (!$scope.hideBaseRateWarning) {
                $scope.hideBaseRateWarning = true;
            }
        };

        $scope.removeNameWarning = function () {
            if (!$scope.hideNameWarning) {
                $scope.hideNameWarning = true;
            }
        };

        $scope.submitResourceCreateForm = function () {
            var self = this;
            formObj = angular.copy(self.createResourceForm);

            var valObj = adminActionStatusService.validateResourceObjectForm(formObj);

            if (!valObj[0]) {
                if (valObj[1] === ROLE_INVALID.name) {
                    $scope.hideNameWarning = false;
                }

                if (valObj[1] === ROLE_INVALID.baseRate) {
                    $scope.hideBaseRateWarning = false;
                }

                return;
            }

            adminAreaService.createResource($scope.resourceModel);
        };


        // Init
        $scope.init = function () {
            if (!$scope.authentication.isAuth) {
                helperService.notAuth();
            }

            blockUI.stop();
        };

        $scope.init();
    }
]);