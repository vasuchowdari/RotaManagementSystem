"use strict";

var app = angular.module("controllers");
app.controller("adminAppCtrl", [
    "$scope",
    "$state",
    "helperService",
    "blockUI",
    function ($scope, $state, helperService, blockUI) {
        // Block And Auth
        helperService.blockAndAuth($scope, blockUI);


        // Event Subscription


        // Init
        $scope.init = function () {
            if (!$scope.authentication.isAuth) {
                helperService.notAuth();
            }

            // this is for an F5 refresh by user
            if ($state.$current.name === "admin") {
                $state.go(helperService.setDashState());
            }

            blockUI.stop();
        };

        $scope.init();
    }
]);