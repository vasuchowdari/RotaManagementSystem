"use strict";

var app = angular.module("controllers");
app.controller("adminDashCtrl", [
    "$scope",
    "$state",
    "helperService",
    "adminAreaService",
    "blockUI",
    function ($scope, $state, helperService, adminAreaService, blockUI) {
        // Block And Auth
        helperService.blockAndAuth($scope, blockUI);


        // Event Subscription
        adminAreaService.subscribe($scope, function() {
            adminAreaService.populateEmployeeNameAndIdStore();
        }, "sites-store-populated-event");

        adminAreaService.subscribe($scope, function() {
            $scope.getInvalidShiftProfiles();
        }, "employee-name-id-store-populated-event");


        // Declarations
        $scope.invalidShiftProfiles = [];


        // Functions
        $scope.getInvalidShiftProfiles = function() {
            adminAreaService.getInvalidShiftProfiles().then(function (res) {
                var empNameIdStore = helperService.getLocalStorageObject("employeeNameAndIdStore");
                var sitesStore = helperService.getLocalStorageObject("sitesStore");
                _.forEach(res, function(shiftProfile) {
                    var invalidShiftProfileModel = adminAreaService.returnNewInvalidShiftProfileViewModel();
                    var tempSite = null;
                    var tempSubSite = null;

                    invalidShiftProfileModel.id = shiftProfile.Id;
                    invalidShiftProfileModel.isActive = shiftProfile.IsActive;
                    invalidShiftProfileModel.startDateTime = shiftProfile.StartDateTime;
                    invalidShiftProfileModel.endDateTime = shiftProfile.EndDateTime;
                    invalidShiftProfileModel.actualStartDateTime = shiftProfile.ActualStartDateTime;
                    invalidShiftProfileModel.actualEndDateTime = shiftProfile.ActualEndDateTime;
                    invalidShiftProfileModel.zktStartDateTime = shiftProfile.ZktStartDateTime;
                    invalidShiftProfileModel.zktEndDateTime = shiftProfile.ZktEndDateTime;
                    invalidShiftProfileModel.hoursWorked = shiftProfile.HoursWorked;
                    invalidShiftProfileModel.timeWorked = shiftProfile.TimeWorked;
                    invalidShiftProfileModel.employeeId = shiftProfile.EmployeeId;
                    invalidShiftProfileModel.shiftId = shiftProfile.ShiftId;
                    invalidShiftProfileModel.isApproved = shiftProfile.IsApproved;
                    invalidShiftProfileModel.status = adminAreaService.returnShiftStatusString(shiftProfile.Status);
                    invalidShiftProfileModel.reason = shiftProfile.Reason;
                    invalidShiftProfileModel.notes = shiftProfile.Notes;
                    invalidShiftProfileModel.isModified = shiftProfile.IsModified;

                    // view model properties
                    invalidShiftProfileModel.shiftDate = shiftProfile.StartDateTime;

                    var tempResources = helperService.getLocalStorageObject("resourcesStore");
                    var tempResource = _.head(_.filter(tempResources, { "Id": shiftProfile.ShiftModel.ShiftTemplateModel.ResourceId }));
                    invalidShiftProfileModel.resourceName = tempResource.Name;

                    if (shiftProfile.ShiftModel.ShiftTemplateModel.SiteId && !shiftProfile.ShiftModel.ShiftTemplateModel.SubSiteId) {
                        tempSite = _.head(_.filter(sitesStore, { "Id": shiftProfile.ShiftModel.ShiftTemplateModel.SiteId }));
                        invalidShiftProfileModel.shiftLocation = tempSite.Name;
                    }

                    if (shiftProfile.ShiftModel.ShiftTemplateModel.SiteId && shiftProfile.ShiftModel.ShiftTemplateModel.SubSiteId) {
                        tempSite = _.head(_.filter(sitesStore, { "Id": shiftProfile.ShiftModel.ShiftTemplateModel.SiteId }));
                        tempSubSite = _.head(_.filter(tempSite.SubSiteModels, { "Id": shiftProfile.ShiftModel.ShiftTemplateModel.SubSiteId }));
                        invalidShiftProfileModel.shiftLocation = tempSubSite.Name;
                    }

                    if (shiftProfile.EmployeeId) {
                        var employee = _.find(empNameIdStore, { "Id": shiftProfile.EmployeeId });
                        invalidShiftProfileModel.staffMember = employee.Firstname + " " + employee.Lastname;
                    } else {
                        invalidShiftProfileModel.staffMember = "Unassigned";
                    }

                    $scope.invalidShiftProfiles.push(invalidShiftProfileModel);
                });
            }).catch(function(error) {
                helperService.toasterError(null, error);
                blockUI.stop();
            });
        };

        $scope.navToShiftAdjustment = function(shiftProfile) {
            $state.go("admin.adjustment", { shiftProfileData: shiftProfile });
        };


        // Init
        $scope.init = function() {
            if (!$scope.authentication.isAuth) {
                helperService.notAuth();
            }

            adminAreaService.populateCompaniesStore();
            adminAreaService.populateEmployeeTypesStore();
            adminAreaService.populateResourcesStore();
            adminAreaService.populateSitesStore();

            blockUI.stop();
        };

        $scope.init();
    }
]);