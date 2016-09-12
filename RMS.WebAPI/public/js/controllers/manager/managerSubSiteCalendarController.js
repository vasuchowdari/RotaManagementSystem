"use strict";

var app = angular.module("controllers");
app.controller("managerSubSiteCalendarCtrl", [
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
        $scope.calendarId = Number($stateParams.subsiteCalendarId);
        $scope.siteEmployees = helperService.getLocalStorageObject("allSiteEmployeesStore");
        //$scope.siteEmployees = helperService.getLocalStorageObject("siteEmployeesStore");
        var subsiteCalendarsStore = helperService.getLocalStorageObject("subsiteCalendarsStore");


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

            $scope.subsiteCalendar = _.head(_.filter(subsiteCalendarsStore, { "Id": Number($stateParams.subsiteCalendarId) }));
            blockUI.stop();
        };

        $scope.init();
    }
]);