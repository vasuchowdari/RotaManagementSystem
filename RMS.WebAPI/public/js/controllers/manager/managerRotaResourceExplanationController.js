"use strict";

var app = angular.module("controllers");
app.controller("managerRotaResourceExplanationCtrl", [
    "$scope",
    "$state",
    "helperService",
    "managerAreaService",
    "blockUI",
    function ($scope, $state, helperService, managerAreaService, blockUI) {
        // Block And Auth
        helperService.blockAndAuth($scope, blockUI);


        // Functions
        $scope.openNewCalResRqDialog = function() {
            var self = this;
            self.closeThisDialog();
            managerAreaService.openNewCalResRqDialog();
        };


        // Init
        $scope.init = function() {
            if (!$scope.authentication.isAuth) {
                helperService.notAuth();
            }

            blockUI.stop();
        };

        $scope.init();
    }
]);