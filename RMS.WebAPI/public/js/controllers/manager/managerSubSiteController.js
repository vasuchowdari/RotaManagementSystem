"use strict";

var app = angular.module("controllers");
app.controller("managerSubSiteCtrl", [
    "$scope",
    "$state",
    "$stateParams",
    "helperService",
    "managerAreaService",
    "calendarService",
    "blockUI",
    function ($scope, $state, $stateParams, helperService, managerAreaService,
        calendarService, blockUI) {
        // Block and Auth
        helperService.blockAndAuth($scope, blockUI);


        // Declarations
        $scope.currentlySelectedSubSiteId = Number($stateParams.subsiteId);
        var subsites = helperService.getLocalStorageObject("subsitesStore");


        // Functions
        $scope.getCalendarsForSubSite = function() {
            calendarService.getForSubSite($scope.subsite.Id).then(function(res) {
                // TODO: Service
                _.forEach(res.data, function(calendar) {
                    calendar.StartDate = moment(calendar.StartDate).format("DD/MM/YYYY");
                    calendar.EndDate = moment(calendar.EndDate).format("DD/MM/YYYY");

                    _.forEach(calendar.CalendarPeriodModels, function(period) {
                        period.StartDate = moment(period.StartDate).format("DD/MM/YYYY");
                        period.EndDate = moment(period.EndDate).format("DD/MM/YYYY");
                    });
                });

                $scope.subsiteCalendars = res.data;
                helperService.setLocalStorageObject("subsiteCalendarsStore", res.data);

            }).catch(function(error) {
                blockUI.stop();
                helperService.toasterError(null, error);
            });
        };


        // Init
        $scope.init = function () {
            if (!$scope.authentication.isAuth) {
                helperService.notAuth();
                return;
            }

            $scope.subsite = _.head(_.filter(subsites, { "Id": $scope.currentlySelectedSubSiteId }));

            $scope.getCalendarsForSubSite();
            blockUI.stop();
        };

        $scope.init();
    }
]);