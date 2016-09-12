"use strict";

var ap = angular.module("controllers");
app.controller("userNavCtrl", [
    "$scope",
    "$state",
    "helperService",
    "blockUI",
    function($scope, $state, helperService, blockUI) {
        // Block And Auth
        helperService.blockAndAuth($scope, blockUI);


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