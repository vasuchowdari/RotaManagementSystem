"use strict";

var app = angular.module("controllers");
app.controller("adminMonthlyPayrollReportCtrl", [
    "$scope",
    "helperService",
    "adminAreaService",
    "blockUI",
    function($scope, helperService, adminAreaService, blockUI) {
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

        $scope.runMonthlyPayrollReport = function() {
            var model = adminAreaService.returnNewMonthlyPayrollReportParamsModel();

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

            adminAreaService.runMonthlyPayrollReport(model).then(function(res) {
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