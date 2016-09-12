"use strict";

var app = angular.module("controllers");
app.controller("profileCtrl", [
    "$scope",
    "$state",
    "helperService",
    "userService",
    "blockUI",
    function($scope, $state, helperService, userService, blockUI) {
        // Block And Auth
        helperService.blockAndAuth($scope, blockUI);


        // Event Subscriptions


        // Declarations
        $scope.user = null;
        $scope.newPassword = null;
        $scope.confirmNewPassword = null;
        $scope.existingPassword = null;
        $scope.submitBtnDisabled = false;


        // Functions
        $scope.error = function() {
            helperService.toasterError();
        };

        $scope.success = function() {
            helperService.toasterSuccess(
                "Success - Password changed",
                "Your password has been succesfully changed.");
        };

        $scope.updatePassword = function () {

            $scope.submitBtnDisabled = helperService.toggleButtonDisable($scope.submitBtnDisabled);

            $scope.user.Password = $scope.confirmNewPassword;

            if (angular.equals($scope.newPassword, $scope.confirmNewPassword)) {
                userService.updateUserPassword($scope.user).then(function(res) {
                    $scope.success();
                    $scope.submitBtnDisabled = helperService.toggleButtonDisable($scope.submitBtnDisabled);

                    setTimeout(function() {
                        $state.go(helperService.setLoginState());
                    }, 2000);
                }).catch(function(error) {
                    blockUI.stop();
                    $scope.error();
                    $scope.submitBtnDisabled = helperService.toggleButtonDisable($scope.submitBtnDisabled);
                });
                return;
            }

            helperService.toasterError("Change Password Error", "Your new password and the confirm value do not match.");
            blockUI.stop();
            $scope.submitBtnDisabled = helperService.toggleButtonDisable($scope.submitBtnDisabled);
        };

        $scope.getUser = function() {
            userService.getById($scope.authentication.userId).then(function(res) {
                $scope.user = res.data;

                blockUI.stop();
            }).catch(function(error) {
                blockUI.stop();
                $scope.error();
            });
        };


        // Init
        $scope.init = function() {
            if (!$scope.authentication.isAuth) {
                helperService.notAuth();
            }

            $scope.getUser();
            blockUI.stop();
        };

        $scope.init();
    }
]);