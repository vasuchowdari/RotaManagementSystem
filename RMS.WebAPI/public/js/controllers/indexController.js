"use strict";

var app = angular.module("controllers");
app.controller("indexCtrl", [
    "$scope",
    "$state",
    "authService",
    function ($scope, $state, authService) {
        // Auth
        $scope.authentication = authService.authentication;


        // Declarations
        

        // Functions
        $scope.logOut = function () {
            authService.logOut();
            $state.go("login");
        };
    }
]);