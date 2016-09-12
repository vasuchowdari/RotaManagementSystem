"use strict";

var app = angular.module("controllers");
app.controller("resetCtrl", [
    "$scope",
    "$state",
    "helperService",
    "userService",
    "blockUI",
    function($scope, $state, helperService, userService, blockUI) {
        // Declarations
        $scope.passwordResetModel = {
            Email: null
        };

        $scope.email = null;
        $scope.resetBtnDisabled = false;


        // Functions
        $scope.passwordReset = function() {
            $scope.resetBtnDisabled = helperService.toggleButtonDisable($scope.resetBtnDisabled);

            userService.passwordReset($scope.passwordResetModel).then(function(res) {
                helperService.statusHandler($scope, res.status, "Success - Password Reset", "Your reset password has been emailed to you");
                $scope.resetBtnDisabled = helperService.toggleButtonDisable($scope.resetBtnDisabled);
            }).catch(function(err) {
                blockUI.stop();
                helperService.toasterError();
                $scope.resetBtnDisabled = helperService.toggleButtonDisable($scope.resetBtnDisabled);
            });
        };


        // Init
        $scope.init = function() {
            blockUI.stop();
        };

        $scope.init();
    }
]);