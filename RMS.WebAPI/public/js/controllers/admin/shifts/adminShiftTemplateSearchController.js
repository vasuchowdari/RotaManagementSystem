"use strict";

var app = angular.module("controllers");
app.controller("adminShiftTemplateSearchCtrl", [
    "$scope",
    "$state",
    "helperService",
    "adminAreaService",
    "adminActionStatusService",
    "blockUI",
    "$timeout",
    function ($scope, $state, helperService, adminAreaService, adminActionStatusService,
        blockUI, $timeout) {
        // Block And Auth
        helperService.blockAndAuth($scope, blockUI);


        // Event Subscriptions


        // Declarations
        $scope.editBtnDisabled = true;
        $scope.formObj = null;
        $scope.hideLocationWarning = true;
        $scope.hideRoleWarning = true;
        $scope.hideShiftTemplateLabel = true;
        $scope.resources = [];
        $scope.returnedShiftTemplates = [];
        $scope.selectedResourceId = null;
        $scope.selectedSiteId = null;
        $scope.selectedShiftTemplateId = null;
        $scope.selectedSubSiteId = null;
        $scope.sites = [];
        $scope.shiftTemplateDropdownDisabled = true;
        $scope.subsites = [];
        $scope.subsiteDropdownDisabled = true;
        var INVALID_SEARCH = adminActionStatusService.SHIFT_TEMPLATE_SEARCH_VAL_ENUM;


        // Functions
        var autoHideShiftTemplateLabel = function() {
            $scope.hideShiftTemplateLabel = true;
        };

        $scope.refreshSubSites = function() {
            $scope.hideLocationWarning = true;

            $scope.subsites = [];

            _.forEach($scope.sites, function(site) {
                if (site.Id === $scope.selectedSiteId) {
                    $scope.subsites = site.SubSiteModels;
                }
            });

            if ($scope.subsites.length > 0) {
                $scope.subsiteDropdownDisabled = false;
            } else {
                $scope.subsiteDropdownDisabled = true;
            }
        };

        $scope.resetForm = function() {
            $scope.formObj = null;
            $scope.resources = [];
            $scope.returnedShiftTemplates = [];
            $scope.selectedResourceId = null;
            $scope.selectedSiteId = null;
            $scope.selectedShiftTemplateId = null;
            $scope.selectedSubSiteId = null;
            $scope.sites = [];
            $scope.shiftTemplateDropdownDisabled = true;
            $scope.subsites = [];
            $scope.subsiteDropdownDisabled = true;

            $scope.init();
        };

        $scope.removeCss = function() {
            $scope.shiftTemplateDropdownDisabled = true;
            $scope.hideRoleWarning = true;
        };

        $scope.submitSearchCriteriaForm = function() {
            // false means we have shift templates
            // so we want to select the users choice and pass
            // the template in the state params
            if (!$scope.shiftTemplateDropdownDisabled) {
                var templateData = _.head(_.filter($scope.returnedShiftTemplates, { "Id": $scope.selectedShiftTemplateId }));

                $state.go("admin.shifts.templates.detail", { shiftTemplateData: templateData });
            } else {
                $scope.shiftTemplateDropdownDisabled = true;

                var self = this;
                $scope.formObj = angular.copy(self.shiftTemplateSearchParamsForm);

                var valObj = adminActionStatusService.validateShiftTemplateSearchCriteriaForm($scope.formObj);
                if (!valObj[0]) {

                    if (valObj[1] === INVALID_SEARCH.role) {
                        $scope.hideRoleWarning = false;
                    }

                    if (valObj[1] === INVALID_SEARCH.location) {
                        $scope.hideLocationWarning = false;
                    }

                    return;
                }

                var searchCriteriaModel = {};
                searchCriteriaModel.resourceId = $scope.selectedResourceId;
                searchCriteriaModel.siteId = $scope.selectedSiteId;
                searchCriteriaModel.subsiteId = $scope.selectedSubSiteId;

                adminAreaService.getShiftTemplateBySearchCriteria(searchCriteriaModel).then(function(res) {
                    if (res.length > 0) {
                        $scope.returnedShiftTemplates = res;
                        $scope.shiftTemplateDropdownDisabled = false;
                        $scope.hideShiftTemplateLabel = false;

                        $timeout(autoHideShiftTemplateLabel, 2000);
                    }

                    blockUI.stop();
                }).catch(function(error) {
                    helperService.toasterError(null, error);
                    blockUI.stop();
                });
            }
        };

        $scope.toggleEditBtnDisable = function() {
            if (!$scope.selectedShiftTemplateId) {
                $scope.editBtnDisabled = true;
            } else {
                $scope.editBtnDisabled = false;
            }
        };


        // Init
        $scope.init = function() {
            if (!$scope.authentication.isAuth) {
                helperService.notAuth();
            }

            $scope.resources = helperService.getLocalStorageObject("resourcesStore");
            $scope.sites = helperService.getLocalStorageObject("sitesStore");

            blockUI.stop();
        };

        $scope.init();
    }
]);