"use strict";

var app = angular.module("controllers");
app.controller("adminRotaDetailsCtrl", [
    "$scope",
    "$state",
    "helperService",
    "adminAreaService",
    "blockUI",
    function ($scope, $state, helperService, adminAreaService, blockUI) {
        // Block And Auth
        helperService.blockAndAuth($scope, blockUI);


        // Event Subscription


        // Declarations
        $scope.calendarId = helperService.getLocalStorageObject("calendarIdStore");
        $scope.siteEmployees = helperService.getLocalStorageObject("allSiteEmployeesStore");
        $scope.rotaName = null;


        // Functions


        // Init
        $scope.init = function () {
            if (!$scope.authentication.isAuth) {
                helperService.notAuth();
            }

            if (!$state.params.rotaSearchParamsData) {
                $state.params.rotaSearchParamsData = helperService.getLocalStorageObject("rotaSearchParamsData");
            } else {
                helperService.setLocalStorageObject("rotaSearchParamsData", $state.params.rotaSearchParamsData);
            }

            $scope.rotaName = $state.params.rotaSearchParamsData.subsiteName || $state.params.rotaSearchParamsData.siteName;
            blockUI.stop();
        };

        $scope.init();
    }
]);