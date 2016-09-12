"use strict";

var app = angular.module("controllers");
app.controller("adminUserLeaveCtrl", [
    "$scope",
    "$state",
    "leaveRequestService",
    "helperService",
    "adminAreaService",
    "blockUI",
    function ($scope, $state, leaveRequestService, helperService,
        adminAreaService, blockUI) {
        // Block And Auth
        helperService.blockAndAuth($scope, blockUI);


        // Declarations
        $scope.employeeId = adminAreaService.userIdCache;


        // Functions
        

        // Init
        $scope.init = function() {
            if (!$scope.authentication.isAuth) {
                helperService.notAuth();
            }

            blockUI.stop();
        };

        $scope.init();
    }
])