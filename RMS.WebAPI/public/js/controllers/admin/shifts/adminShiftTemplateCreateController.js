"use strict";

var app = angular.module("controllers");
app.controller("adminShiftTemplateCreateCtrl", [
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
        $scope.baldock = [];
        $scope.formObj = null;
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.shiftTemplateModel = null;
        $scope.shiftTemplateModelCollection = [];
        $scope.psycare = [];
        $scope.resources = [];
        $scope.sites = [];


        // Functions
        $scope.clearBaldockChildren = function() {
            adminAreaService.clearBaldockChildren($scope.baldock);
        };

        $scope.hydrateBaldock = function() {
            adminAreaService.hydrateBaldock($scope);
        };

        $scope.hydratePsycare = function() {
            adminAreaService.hydratePsycare($scope);
        };

        $scope.openCreateNewShiftTemplateDialog = function() {
            adminAreaService.openCreateNewShiftTemplateDialog();
        };

        $scope.selectBaldockParentSite = function() {
            adminAreaService.selectBaldockParent($scope.baldock);
        };

        $scope.submitShiftTemplateCreateForm = function() {
            $scope.shiftTemplateModelCollection = [];
            var tempShiftTemplateModel = angular.copy($scope.shiftTemplateModel);

            tempShiftTemplateModel.StartTime = moment(tempShiftTemplateModel.StartTime).format("YYYY-MM-DD HH:mm");
            tempShiftTemplateModel.EndTime = moment(tempShiftTemplateModel.EndTime).format("YYYY-MM-DD HH:mm");

            // If End time is before Start time, then overnight shift, 
            // so add one day to end and then calculate difference in milliseconds
            if (moment(tempShiftTemplateModel.EndTime).isBefore(tempShiftTemplateModel.StartTime)) {
                tempShiftTemplateModel.EndTime = moment(tempShiftTemplateModel.EndTime).add(1, "days").format("YYYY-MM-DD HH:mm");
            }
            tempShiftTemplateModel.Duration = moment(tempShiftTemplateModel.EndTime).diff(tempShiftTemplateModel.StartTime);
            tempShiftTemplateModel.UnpaidBreakDuration = helperService.returnCalculatedMillisecondsFromMinutesValue(tempShiftTemplateModel.UnpaidBreakDuration);

            if ($scope.baldock.isChecked) {
                tempShiftTemplateModel.SiteId = $scope.baldock.Id;
                $scope.shiftTemplateModelCollection.push(angular.copy(tempShiftTemplateModel));

                _.forEach($scope.baldock.SubSiteModels, function(subsite) {
                    if (subsite.isChecked) {
                        tempShiftTemplateModel.SiteId = $scope.baldock.Id;
                        tempShiftTemplateModel.SubSiteId = subsite.Id;
                        $scope.shiftTemplateModelCollection.push(angular.copy(tempShiftTemplateModel));
                    }
                });
            } else {
                _.forEach($scope.baldock.SubSiteModels, function(subsite) {
                    if (subsite.isChecked) {
                        tempShiftTemplateModel.SiteId = $scope.baldock.Id;
                        tempShiftTemplateModel.SubSiteId = subsite.Id;
                        $scope.shiftTemplateModelCollection.push(angular.copy(tempShiftTemplateModel));
                    }
                });
            }

            _.forEach($scope.psycare, function(site) {
                if (site.isChecked) {
                    tempShiftTemplateModel.SiteId = site.Id;
                    tempShiftTemplateModel.SubSiteId = null;
                    $scope.shiftTemplateModelCollection.push(angular.copy(tempShiftTemplateModel));
                }
            });

            // send off single or collection
            if ($scope.shiftTemplateModelCollection.length > 1) {
                adminAreaService.batchCreateShiftTemplates($scope.shiftTemplateModelCollection).then(function(res) {
                    helperService.toasterSuccess(null, res);
                    blockUI.stop();
                }).catch(function(error) {
                    helperService.toasterError(null, error);
                    blockUI.stop();
                });
            } else if ($scope.shiftTemplateModelCollection.length === 1) {
                adminAreaService.createShiftTemplate($scope.shiftTemplateModelCollection[0]).then(function(res) {
                    helperService.toasterSuccess(null, res);
                    blockUI.stop();
                }).catch(function(error) {
                    helperService.toasterError(null, error);
                    blockUI.stop();
                });
            } else {
                helperService.toasterWarning(null, "Please select at least 1 location or ward to create the Shift Template.");
                return;
            }
        };


        // Init
        $scope.init = function() {
            if (!$scope.authentication.isAuth) {
                helperService.notAuth();
            }

            $scope.shiftTemplateModel = adminAreaService.returnNewShiftTemplateModel();
            $scope.resources = helperService.getLocalStorageObject("resourcesStore");
            $scope.sites = helperService.getLocalStorageObject("sitesStore");

            $scope.hydrateBaldock();
            $scope.hydratePsycare();

            blockUI.stop();
        };

        $scope.init();
    }
]);