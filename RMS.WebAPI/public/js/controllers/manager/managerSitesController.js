"use strict";

var app = angular.module("controllers");
app.controller("managerSitesCtrl", [
    "$scope",
    "$state",
    "helperService",
    "managerAreaService",
    "siteService",
    "sitePersonnelLookupService",
    "shiftProfileService",
    "blockUI",
    function ($scope, $state, helperService, managerAreaService, siteService,
        sitePersonnelLookupService, shiftProfileService, blockUI) {
        // Block And Auth
        helperService.blockAndAuth($scope, blockUI);


        // Declarations
        var companyStore = helperService.getLocalStorageObject("companyStore");
        $scope.howtoTitle = "1. Using the Rota";
        var managerStore = helperService.getLocalStorageObject("managerStore");
        $scope.panelExpanded = true;


        // Functions
        $scope.filterSitesStoreForManager = function() {
            $scope.filteredSites = helperService.getLocalStorageObject("sitesStore");
        };

        $scope.getSiteAccessRights = function(employeeId) {
            sitePersonnelLookupService.getForEmployee(employeeId).then(function(res) {
                helperService.setLocalStorageObject("accessRightsStore", res.data);
                $scope.filterSitesStoreForManager();
            }).catch(function(error) {
                blockUI.stop();
                helperService.toasterError(null, error);
            });
        };

        $scope.getSites = function() {
            siteService.getSitesForCompany(companyStore.Id).then(function(res) {
                helperService.setLocalStorageObject("sitesStore", res.data);
                $scope.sites = angular.copy(helperService.getLocalStorageObject("sitesStore"));

                $scope.getSiteAccessRights(managerStore.Id);
                blockUI.stop();
            }).catch(function(error) {
                blockUI.stop();
                helperService.toasterError(null, error);
            });
        };

        $scope.showAddShiftsHowTo = function() {
            $scope.slides = [];
            $scope.howtoTitle = "3. Add Shifts to existing Role";


            $scope.slides.push({
                image: "http://80.229.27.248:8093/RMSV2Dev/public/content/add-shifts-howto/add_shifts_existing_role.png",
                id: 0
            });

            $scope.slides.push({
                image: "http://80.229.27.248:8093/RMSV2Dev/public/content/add-shifts-howto/add_new_role_2.png",
                id: 1
            });

            $scope.slides.push({
                image: "http://80.229.27.248:8093/RMSV2Dev/public/content/add-shifts-howto/add_new_role_4.png",
                id: 2
            });

            $scope.slides.push({
                image: "http://80.229.27.248:8093/RMSV2Dev/public/content/add-shifts-howto/add_new_role_5.png",
                id: 3
            });

            $scope.slides.push({
                image: "http://80.229.27.248:8093/RMSV2Dev/public/content/add-shifts-howto/add_new_role_6.png",
                id: 4
            });

            $scope.slides.push({
                image: "http://80.229.27.248:8093/RMSV2Dev/public/content/add-shifts-howto/add_new_role_7.png",
                id: 5
            });

            $scope.slides.push({
                image: "http://80.229.27.248:8093/RMSV2Dev/public/content/add-shifts-howto/add_new_role_8.png",
                id: 6
            });

            $scope.slides.push({
                image: "http://80.229.27.248:8093/RMSV2Dev/public/content/add-shifts-howto/add_new_role_9.png",
                id: 7
            });

            $scope.slides.push({
                image: "http://80.229.27.248:8093/RMSV2Dev/public/content/add-shifts-howto/add_new_role_10.png",
                id: 8
            });

            $scope.slides.push({
                image: "http://80.229.27.248:8093/RMSV2Dev/public/content/add-shifts-howto/add_new_role_11.png",
                id: 9
            });

            $scope.slides.push({
                image: "http://80.229.27.248:8093/RMSV2Dev/public/content/add-shifts-howto/add_new_role_12.png",
                id: 10
            });
        };

        $scope.showDeleteShiftHowTo = function() {
            $scope.slides = [];
            $scope.howtoTitle = "5. Delete Shift or Role from Rota";

            $scope.slides.push({
                image: "http://80.229.27.248:8093/RMSV2Dev/public/content/delete-shifts-howto/delete_1.png",
                id: 0
            });

            $scope.slides.push({
                image: "http://80.229.27.248:8093/RMSV2Dev/public/content/delete-shifts-howto/delete_2.png",
                id: 1
            });

            $scope.slides.push({
                image: "http://80.229.27.248:8093/RMSV2Dev/public/content/delete-shifts-howto/delete_3.png",
                id: 2
            });
        };

        $scope.showNewRoleHowTo = function() {
            $scope.slides = [];
            $scope.howtoTitle = "2. Add New Role to the Rota";

            $scope.slides.push({
                image: "http://80.229.27.248:8093/RMSV2Dev/public/content/new-role-howto/no_role_step.png",
                id: 0
            });

            $scope.slides.push({
                image: "http://80.229.27.248:8093/RMSV2Dev/public/content/new-role-howto/add_new_role.png",
                id: 1
            });

            $scope.slides.push({
                image: "http://80.229.27.248:8093/RMSV2Dev/public/content/new-role-howto/add_new_role_2.png",
                id: 2
            });

            $scope.slides.push({
                image: "http://80.229.27.248:8093/RMSV2Dev/public/content/new-role-howto/add_new_role_3.png",
                id: 3
            });

            $scope.slides.push({
                image: "http://80.229.27.248:8093/RMSV2Dev/public/content/new-role-howto/add_new_role_4.png",
                id: 4
            });

            $scope.slides.push({
                image: "http://80.229.27.248:8093/RMSV2Dev/public/content/new-role-howto/add_new_role_5.png",
                id: 5
            });

            $scope.slides.push({
                image: "http://80.229.27.248:8093/RMSV2Dev/public/content/new-role-howto/add_new_role_6.png",
                id: 6
            });

            $scope.slides.push({
                image: "http://80.229.27.248:8093/RMSV2Dev/public/content/new-role-howto/add_new_role_7.png",
                id: 7
            });

            $scope.slides.push({
                image: "http://80.229.27.248:8093/RMSV2Dev/public/content/new-role-howto/add_new_role_8.png",
                id: 8
            });

            $scope.slides.push({
                image: "http://80.229.27.248:8093/RMSV2Dev/public/content/new-role-howto/add_new_role_9.png",
                id: 9
            });

            $scope.slides.push({
                image: "http://80.229.27.248:8093/RMSV2Dev/public/content/new-role-howto/add_new_role_10.png",
                id: 10
            });

            $scope.slides.push({
                image: "http://80.229.27.248:8093/RMSV2Dev/public/content/new-role-howto/add_new_role_11.png",
                id: 11
            });

            $scope.slides.push({
                image: "http://80.229.27.248:8093/RMSV2Dev/public/content/new-role-howto/add_new_role_12.png",
                id: 12
            });
        };

        $scope.showReassignShiftHowTo = function() {
            $scope.slides = [];
            $scope.howtoTitle = "4. Re-assign a Shift on Rota";

            $scope.slides.push({
                image: "http://80.229.27.248:8093/RMSV2Dev/public/content/reassign-shifts-howto/edit_shift_1.png",
                id: 0
            });

            $scope.slides.push({
                image: "http://80.229.27.248:8093/RMSV2Dev/public/content/reassign-shifts-howto/edit_shift_2.png",
                id: 1
            });

            $scope.slides.push({
                image: "http://80.229.27.248:8093/RMSV2Dev/public/content/reassign-shifts-howto/edit_shift_3.png",
                id: 2
            });

            $scope.slides.push({
                image: "http://80.229.27.248:8093/RMSV2Dev/public/content/reassign-shifts-howto/edit_shift_4.png",
                id: 3
            });

            $scope.slides.push({
                image: "http://80.229.27.248:8093/RMSV2Dev/public/content/reassign-shifts-howto/edit_shift_5.png",
                id: 4
            });
        };

        $scope.showStarterHowTo = function() {
            $scope.slides = [];
            $scope.howtoTitle = "1. Using the Rota";

            $scope.slides.push({
                image: "http://80.229.27.248:8093/RMSV2Dev/public/content/step_1.png",
                id: 0
            });

            $scope.slides.push({
                image: "http://80.229.27.248:8093/RMSV2Dev/public/content/site_step.png",
                id: 1
            });

            $scope.slides.push({
                image: "http://80.229.27.248:8093/RMSV2Dev/public/content/sub_site_step.png",
                id: 2
            });

            $scope.slides.push({
                image: "http://80.229.27.248:8093/RMSV2Dev/public/content/sub_site_step_two.png",
                id: 3
            });
        };

        $scope.toggleHowToPanel = function() {
            if (!$scope.panelExpanded) {
                $scope.panelExpanded = true;
            } else {
                $scope.panelExpanded = false;
            }
        };


        // Init
        $scope.init = function() {
            if (!$scope.authentication.isAuth) {
                helperService.notAuth();
                return;
            }

            $scope.showStarterHowTo();
            $scope.getSites();
            blockUI.stop();
        };

        $scope.init();
    }
]);