"use strict";

var app = angular.module("controllers");
app.controller("managerSiteCalendarCtrl", [
    "$scope",
    "$state",
    "$stateParams",
    "helperService",
    "managerAreaService",
    "rotaService",
    "blockUI",
    function ($scope, $state, $stateParams, helperService, managerAreaService,
        rotaService, blockUI) {
        // Block and Auth
        helperService.blockAndAuth($scope, blockUI);


        // Declarations
        $scope.calendarId = Number($stateParams.calendarId);
        $scope.siteEmployees = helperService.getLocalStorageObject("allSiteEmployeesStore");
        //$scope.siteEmployees = helperService.getLocalStorageObject("siteEmployeesStore");
        var siteCalendarsStore = helperService.getLocalStorageObject("siteCalendarsStore");


        // Functions
        $scope.addNewResRq = function () {
            managerAreaService.openRotaResourceExplanationDialog();
        };


        // Init
        $scope.init = function () {
            if (!$scope.authentication.isAuth) {
                helperService.notAuth();
                return;
            }
            
            $scope.siteCalendar = _.head(_.filter(siteCalendarsStore, { "Id": $scope.calendarId }));
            blockUI.stop();
        };

        $scope.init();
    }
]);