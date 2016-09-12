"use strict";

var app = angular.module("controllers");
app.controller("managerDashCtrl", [
    "$scope",
    "$state",
    "managerAreaService",
    "helperService",
    "blockUI",
    function ($scope, $state, managerAreaService, helperService, blockUI) {
        // Block And Auth
        helperService.blockAndAuth($scope, blockUI);


        // Event Subscription
        

        // Declarations
        $scope.leaveRequests = [];
        $scope.timeAdjustmentForms = [];
        var staffIds = [];


        // Functions
        $scope.getLeaveRequests = function() {
            managerAreaService.getLeaveRequests(staffIds).then(function(res) {
                $scope.leaveRequests = res.data;
            }).catch(function (error) {
                helperService.toasterError(null, error);
                blockUI.stop();
            });
        };

        $scope.getTimeAdjustmentForms = function() {
            managerAreaService.getTimeAdjustmentForms(staffIds).then(function(res) {
                $scope.timeAdjustmentForms = res;
            }).catch(function(error) {
                helperService.toasterError(null, error);
                blockUI.stop();
            });
        };

        $scope.navToAdjustmentForm = function(timeAdjustmentForm) {
            $state.go("manager.adjustment", { timeAdjustmentFormData: timeAdjustmentForm });
        };


        // Init
        $scope.init = function() {
            if (!$scope.authentication.isAuth) {
                helperService.notAuth();
                return;
            }

            _.forEach(helperService.getLocalStorageObject("siteEmployeesStore"), function(mos) {
                staffIds.push(mos.Id);
            });

            $scope.getTimeAdjustmentForms();
            $scope.getLeaveRequests();
            blockUI.stop();
        };

        $scope.init();
    }
]);