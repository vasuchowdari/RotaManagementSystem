"use strict";

var app = angular.module("controllers");
app.controller("managerSiteCtrl", [
    "$scope",
    "$state",
    "$stateParams",
    "helperService",
    "managerAreaService",
    "calendarService",
    "sitePersonnelLookupService",
    "blockUI",
    function ($scope, $state, $stateParams, helperService, managerAreaService,
        calendarService, sitePersonnelLookupService, blockUI) {
        // Block and Auth
        helperService.blockAndAuth($scope, blockUI);


        // Declarations
        var subsites = [];
        var tempSubSites = [];


        // Functions
        $scope.getCalendarsForSite = function() {
            calendarService.getForSite($scope.site.Id).then(function(res) {
                _.forEach(res.data, function (calendar) {
                    calendar.StartDate = moment(calendar.StartDate).format("DD/MM/YYYY");
                    calendar.EndDate = moment(calendar.EndDate).format("DD/MM/YYYY");
                });

                $scope.siteCalendars = res.data;
                helperService.setLocalStorageObject("siteCalendarsStore", res.data);

                blockUI.stop();
            }).catch(function(error) {
                blockUI.stop();
                helperService.toasterError(null, error);
            });
        };

        // Init
        $scope.init = function() {
            if (!$scope.authentication.isAuth) {
                helperService.notAuth();
                return;
            }

            $scope.sitesStore = helperService.getLocalStorageObject("sitesStore");
            helperService.setLocalStorageObject("siteStore", _.head(_.filter($scope.sitesStore, { "Id": Number($stateParams.siteId) })));

            _.forEach($scope.sitesStore, function(site) {
                _.forEach(site.SubSiteModels, function(subsite) {
                    subsites.push(subsite);
                });
            });
            helperService.setLocalStorageObject("subsitesStore", subsites);
            $scope.site = helperService.getLocalStorageObject("siteStore");

            _.forEach(helperService.getLocalStorageObject("accessRightsStore"), function (record) {
                if (record.SubSiteId) {
                    tempSubSites.push(_.head(_.filter($scope.site.SubSiteModels, { "Id": record.SubSiteId })));
                }
            });

            $scope.subsites = _.orderBy(tempSubSites, ["Name"], ["asc"]);

            $scope.getCalendarsForSite();
            blockUI.stop();
        };

        $scope.init();
    }
]);