"use strict";

var app = angular.module("controllers");
app.controller("adminRoleDetailsCtrl", [
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
            adminAreaService.populateResourcesStore();
            adminActionStatusService.handleResourceUpdateSuccess();
        }, "resource-update-complete-event");

        adminAreaService.subscribe($scope, function(errorMsg) {
            adminActionStatusService.handleResourceUpdateError(errorMsg);
        }, "resource-update-failure-event");


        // Declarations
        var formObj = null;
        $scope.hideBaseRateWarning = true;
        $scope.hideNameWarning = true;
        $scope.resourceModel = null;
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

        $scope.submitResourceUpdateForm = function() {
            var self = this;
            formObj = angular.copy(self.updateResourceForm);

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

            adminAreaService.updateResource($scope.resourceModel);
        };


        // Init
        $scope.init = function () {
            if (!$scope.authentication.isAuth) {
                helperService.notAuth();
            }

            var resourcesStore = helperService.getLocalStorageObject("resourcesStore");
            $scope.resourceModel = _.head(_.filter(resourcesStore, { "Id": Number($stateParams.resourceId) }));
            blockUI.stop();
        };

        $scope.init();
    }
]);