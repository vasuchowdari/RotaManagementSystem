"use strict";

var app = angular.module("controllers");
app.controller("loginCtrl", [
    "$scope",
    "$state",
    "authService",
    "helperService",
    "toaster",
    function ($scope, $state, authService, helperService, toaster) {
        // Declarations
        $scope.loginData = {
            login: "",
            password: ""
        };

        $scope.loginBtnDisabled = false;


        // Functions


        // Init
        $scope.login = function () {
            $scope.loginBtnDisabled = helperService.toggleButtonDisable($scope.loginBtnDisabled);

            authService.login($scope.loginData).then(function(res) {
                // redirect
                $state.go(helperService.setLoginState());
            }).catch(function(error) {
                $scope.loginBtnDisabled = helperService.toggleButtonDisable($scope.loginBtnDisabled);
                helperService.toasterError("Login Error", "There was an error logging you in.");
            });
        };
    }
]);