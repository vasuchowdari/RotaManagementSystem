"use strict";

var app = angular.module("controllers");
app.controller("adminPayrollDashCtrl", [
    "$scope",
    "$state",
    "helperService",
    "adminAreaService",
    "blockUI",
    function($scope, $state, helperService, adminAreaService, blockUI) {
        // Block And Auth
        helperService.blockAndAuth($scope, blockUI);


        // Event Subscription


        // Declarations


        // Functions
        $scope.openDailyActivityReportDialog = function() {
            adminAreaService.openDailyActivityReportDialog();
        };

        $scope.openMonthlyPayrollReportDialog = function() {
            adminAreaService.openMonthlyPayrollReportDialog();
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