"use strict";

var app = angular.module("controllers");
app.controller("adminRolesDashCtrl", [
    "$scope",
    "$state",
    "helperService",
    "adminAreaService",
    "blockUI",
    function ($scope, $state, helperService, adminAreaService, blockUI) {
        // Block And Auth
        helperService.blockAndAuth($scope, blockUI);


        // Event Subscription
        adminAreaService.subscribe($scope, function(e, resources) {
            $scope.resources = resources;
        }, "resources-store-populated-event");

        // Declarations
        $scope.resources = null;


        // Functions


        // Init
        $scope.init = function () {
            if (!$scope.authentication.isAuth) {
                helperService.notAuth();
            }

            adminAreaService.populateResourcesStore();

            blockUI.stop();
        };

        $scope.init();
    }
]);