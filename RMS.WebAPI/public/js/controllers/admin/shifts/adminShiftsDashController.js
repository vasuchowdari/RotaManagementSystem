"use strict";

var app = angular.module("controllers");
app.controller("adminShiftsDashCtrl", [
    "$scope",
    "$state",
    "helperService",
    "adminAreaService",
    "blockUI",
    function ($scope, $state, helperService, adminAreaService, blockUI) {
        // Block And Auth
        helperService.blockAndAuth($scope, blockUI);


        // Event Subscriptions


        // Declarations


        // Functions
        $scope.openCreateNewShiftTemplateDialog = function() {
            adminAreaService.openCreateNewShiftTemplateDialog();
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