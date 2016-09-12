"use strict";

var app = angular.module("controllers");
app.controller("adminRotasDashCtrl", [
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
        $scope.rotaSearchParamsObj = {};
        $scope.selectedSiteId = null;
        $scope.selectedSubSiteId = null;
        $scope.sites = [];
        $scope.siteEmployees = helperService.getLocalStorageObject("allSiteEmployeesStore");
        $scope.subsites = [];
        $scope.subsiteDropdownDisabled = true;


        // Functions
        $scope.refreshSubSites = function() {
            $scope.subsites = [];

            _.forEach($scope.sites, function(site) {
                if (site.Id === $scope.selectedSiteId) {
                    $scope.subsites = _.orderBy(site.SubSiteModels, ["Name"], ["asc"]);
                }
            });

            if ($scope.subsites.length > 0) {
                $scope.subsiteDropdownDisabled = false;
            } else {
                $scope.subsiteDropdownDisabled = true;
            }
        };

        $scope.resetForm = function() {
            $scope.rotaSearchParamsObj = {};
            $scope.selectedSiteId = null;
            $scope.selectedSubSiteId = null;
            $scope.sites = [];
            $scope.subsites = [];
            $scope.subsiteDropdownDisabled = true;

            $scope.init();
        };

        $scope.submitSearchCriteriaForm = function() {
            var tempSite = _.head(_.filter($scope.sites, { "Id": $scope.selectedSiteId }));

            $scope.rotaSearchParamsObj.siteId = $scope.selectedSiteId;
            $scope.rotaSearchParamsObj.siteName = tempSite.Name;

            adminAreaService.getAllEmployeesByCompanyId(tempSite.CompanyId).then(function(res) {
                helperService.setLocalStorageObject("allSiteEmployeesStore", res);

                if (!$scope.selectedSubSiteId) {
                    adminAreaService.getCalendarId($scope.selectedSiteId, null).then(function(res) {
                        helperService.setLocalStorageObject("calendarIdStore", res[0].Id);
                    }).catch(function(error) {
                        blockUI.stop();
                    });
                } else {
                    adminAreaService.getCalendarId(
                        $scope.selectedSiteId,
                        $scope.selectedSubSiteId).then(function(res) {
                            helperService.setLocalStorageObject("calendarIdStore", res[0].Id);

                            $scope.rotaSearchParamsObj.subsiteId = $scope.selectedSubSiteId;
                            var tempSubSite = _.head(_.filter(tempSite.SubSiteModels, { "Id": $scope.selectedSubSiteId }));
                            $scope.rotaSearchParamsObj.subsiteName = tempSubSite.Name;

                            $state.go("admin.rotas.detail", { rotaSearchParamsData: $scope.rotaSearchParamsObj }, { reload: "admin.rotas.detail" });
                        }).catch(function(error) {
                            blockUI.stop();
                        });
                }
            }).catch(function(error) {
                blockUI.stop();
            });
        };

        // Init
        $scope.init = function() {
            if (!$scope.authentication.isAuth) {
                helperService.notAuth();
            }

            $scope.sites = helperService.getLocalStorageObject("sitesStore");
            
            blockUI.stop();
        };

        $scope.init();
    }
]);