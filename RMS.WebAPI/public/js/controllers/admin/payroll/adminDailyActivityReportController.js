"use strict";

var app = angular.module("controllers");
app.controller("adminDailyActivityReportCtrl", [
    "$scope",
    "helperService",
    "adminAreaService",
    "blockUI",
    function ($scope, helperService, adminAreaService, blockUI) {
        // Block And Auth
        helperService.blockAndAuth($scope, blockUI);


        // Declarations
        $scope.defaultEndDateOfMonth = 20;
        $scope.defaultStartDateOfMonth = 21;
        $scope.edt = new Date();
        $scope.hideDownloadLink = true;
        $scope.hideOverrideCalendars = true;
        $scope.overrideDefaults = false;
        $scope.periods = helperService.returnMonthsOfYearDesc();
        $scope.resources = [];
        $scope.sdt = new Date();
        $scope.selectedLocation = null;
        $scope.selectedPeriodId = null;
        $scope.selectedResource = null;
        $scope.selectedStaffMember = null;
        $scope.selectedSubLocation = null;
        $scope.selectedYear = null;
        $scope.sites = [];
        $scope.staffMembers = [];
        $scope.subsites = [];
        $scope.years = helperService.returnPrevYearsDesc();


        // Functions
        $scope.getResources = function() {
            adminAreaService.getResources().then(function(res) {
                $scope.resources = res;
            });
        };

        $scope.getSitesAndSubSites = function() {
            adminAreaService.getSitesAndSubSites().then(function(res) {
                $scope.sites = res;
            });
        };

        $scope.getUsernameAndId = function() {
            adminAreaService.getUsernameAndId().then(function(res) {
                _.forEach(res, function(mos) {
                    mos.displayName = mos.Firstname + " " + mos.Lastname;
                });

                $scope.staffMembers = res;
            });
        };

        $scope.refreshSubSites = function() {
            $scope.subsites = [];
            _.forEach($scope.sites, function(site) {
                if (site.Id === $scope.selectedLocation) {
                    $scope.subsites = site.SubSiteModels;
                }
            });
        };

        $scope.runDailyActivityReport = function() {
            var model = adminAreaService.returnNewDailyActivityReportParamsModel();

            if (!$scope.overrideDefaults) {
                // January check
                var prevPeriod = null;
                if ($scope.selectedPeriodId !== 0) {
                    prevPeriod = $scope.selectedPeriodId - 1;
                } else {
                    prevPeriod = 11;
                }

                model.endDate = moment([$scope.selectedYear, $scope.selectedPeriodId, $scope.defaultEndDateOfMonth]).format("YYYY-MM-DD");
                model.startDate = moment([$scope.selectedYear, prevPeriod, $scope.defaultStartDateOfMonth]).format("YYYY-MM-DD");
            } else {
                model.startDate = $scope.sdt;
                model.endDate = $scope.edt;
            }

            if ($scope.selectedStaffMember) {
                var mos = _.head(_.filter($scope.staffMembers, { "Id": $scope.selectedStaffMember }));
                var staffNameStrArr = mos.displayName.split(" ");
                model.firstname = staffNameStrArr[0];
                model.lastname = staffNameStrArr[1];
            }

            if ($scope.selectedLocation) {
                var tempSite1 = _.head(_.filter($scope.sites, { "Id": $scope.selectedLocation }));
                model.siteName = tempSite1.Name;
            }

            if ($scope.selectedSubLocation) {
                var tempSite2 = _.head(_.filter($scope.sites, { "Id": $scope.selectedLocation }));
                var subsite = _.head(_.filter(tempSite2.SubSiteModels, { "Id": $scope.selectedSubLocation }));
                model.subSiteName = subsite.Name;
            }

            if ($scope.selectedResource) {
                var tempResource = _.head(_.filter($scope.resources, { "Id": $scope.selectedResource }));
                model.roleName = tempResource.Name;
            }

            adminAreaService.runDailyActivityReport(model).then(function(res) {
                $scope.hideDownloadLink = res.data;
                helperService.statusHandler($scope, res.status, null, res.statusText);
            });
        };

        $scope.toggleDownloadLink = function() {
            if (!$scope.hideDownloadLink) {
                $scope.hideDownloadLink = true;
            }
        };

        $scope.toggleOverrideCalendars = function() {
            if (!$scope.hideOverrideCalendars) {
                $scope.hideOverrideCalendars = true;
            } else {
                $scope.hideOverrideCalendars = false;
            }
        };


        // Init
        $scope.init = function() {
            if (!$scope.authentication.isAuth) {
                helperService.notAuth();
            }

            $scope.getResources();
            $scope.getSitesAndSubSites();
            $scope.getUsernameAndId();
            blockUI.stop();
        };

        $scope.init();
    }
]);