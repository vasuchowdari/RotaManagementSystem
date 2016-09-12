"use strict";

var app = angular.module("controllers");
app.controller("adminUsersDashCtrl", [
    "$scope",
    "$state",
    "helperService",
    "adminAreaService",
    "blockUI",
    function($scope, $state, helperService, adminAreaService, blockUI) {
        // Block And Auth
        helperService.blockAndAuth($scope, blockUI);


        // Event Subscriptions
        

        // Declarations
        $scope.userList = [];


        // Functions
        $scope.getUserList = function () {
            adminAreaService.getUsernameAndId().then(function(res) {
                $scope.userList = res;
            }).catch(function(error) {
                helperService.toasterError(null, error);
            }).finally(function() {
                blockUI.stop();
            });
        };


        // Init
        $scope.init = function() {
            if (!$scope.authentication.isAuth) {
                helperService.notAuth();
            }

            $scope.getUserList();
        };

        $scope.init();
    }
]);