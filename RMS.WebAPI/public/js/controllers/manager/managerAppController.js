"use strict";

var app = angular.module("controllers");
app.controller("managerAppCtrl", [
    "$scope",
    "$state",
    "helperService",
    "managerAreaService",
    "employeeService",
    "companyObject",
    "blockUI",
    function ($scope, $state, helperService, managerAreaService, employeeService, companyObject,
        blockUI) {
        // Block And Auth
        helperService.blockAndAuth($scope, blockUI);


        // Event Subscription


        // Declarations
        $scope.company = angular.copy(companyObject[0]);
        helperService.setLocalStorageObject("companyStore", companyObject[0]);
        helperService.setLocalStorageObject("managerStore", companyObject[1]);

        var allEmployeesCollection = [];
        var activeEmployeesCollection = [];
        var inactiveEmployeesCollection = [];

        _.forEach(companyObject[2], function (employee) {

            allEmployeesCollection.push(employee);

            if (employee.IsActive) {
                activeEmployeesCollection.push(employee);
            } else {
                inactiveEmployeesCollection.push(employee);
            }
        });

        helperService.setLocalStorageObject("allSiteEmployeesStore", allEmployeesCollection);
        helperService.setLocalStorageObject("siteEmployeesStore", activeEmployeesCollection);
        //helperService.setLocalStorageObject("siteEmployeesStore", companyObject[2]);

        // Functions
        

        // Init
        $scope.init = function () {
            if (!$scope.authentication.isAuth) {
                helperService.notAuth();
                return;
            }

            if ($state.$current.name === "manager") {
                $state.go(helperService.setDashState());
            }

            blockUI.stop();
        };

        $scope.init();
    }
]);