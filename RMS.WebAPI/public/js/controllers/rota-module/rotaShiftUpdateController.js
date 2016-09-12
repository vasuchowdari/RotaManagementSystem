"use strict";

var app = angular.module("controllers");
app.controller("rotaShiftUpdateCtrl", [
    "$scope",
    "$state",
    "$stateParams",
    "helperService",
    "shiftService",
    "rotaDialogService",
    "rotaRulesEngine",
    "rotaService",
    "blockUI",
    function ($scope, $state, $stateParams, helperService, shiftService, rotaDialogService, rotaRulesEngine, rotaService, blockUI) {
        // Block And Auth
        helperService.blockAndAuth($scope, blockUI);


        // Event Subscription
        rotaRulesEngine.subscribe($scope, function(e, data) {
            $scope.filteredStaff = data;
            $scope.shiftModel.Employee = _.head($scope.filteredStaff);

            if ($scope.filteredStaff.length === 0) {
                helperService.toasterWarning("Careful now!", "Using Bank Staff.");
                $scope.getBankEmployees();
            }
        }, "employee-pre-filtered-event");
        
        rotaRulesEngine.subscribe($scope, function(e, data) {
            $scope.filteredStaff = data;
            $scope.filteredStaff = _.orderBy($scope.filteredStaff, ["displayName"], ["asc"]);

            $scope.shiftModel.Employee = _.head($scope.filteredStaff);

            if ($scope.filteredStaff.length === 0) {
                helperService.toasterWarning("Careful now!", "Using Bank Staff.");
                $scope.getBankEmployees();
            }
        }, "staff-shift-filtered-event");

        // Declarations
        // TEMP FOR BALDOCK
        var shiftStore = helperService.getLocalStorageObject("shiftStore");
        var stringHolder = shiftStore.text;

        var strArr = stringHolder.split("<br />");
        stringHolder = strArr[0] + " " + strArr[1];
        $scope.currentlyAssignedStaffMember = stringHolder;
        // TEMP FOR BALDOCK


        $scope.filteredStaff = [];
        $scope.shift = shiftStore;
        $scope.shiftAssigned = false;
        $scope.startDate = moment(shiftStore.start, "YYYY-MM-DD HH:mm:ss Z");
        $scope.endDate = moment(shiftStore.end, "YYYY-MM-DD HH:mm:ss Z");
        $scope.quantity = 1;
        $scope.formDisable = false;
        $scope.hideDeleteWarning = true;
        
        $scope.shiftModel = {
            Id: $scope.shift.id,
            IsActive: true,
            IsAssigned: false,
            ShiftTemplateId: shiftStore.shiftTemplateId,
            //CalendarResourceRequirementId: shiftStore.calResRqId,
            CalendarResourceRequirementId: shiftStore.resource,
            EmployeeId: null,
            StartDate: shiftStore.start,
            EndDate: shiftStore.end,
            IsEmployee: shiftStore.isEmployee,
            Resource: { Id: shiftStore.resourceId },

            TempCurrentStaffMember: $scope.currentlyAssignedStaffMember, // <-- TEMP FOR BALDOCK
            TempResourceTypeName: shiftStore.resourceName // <-- TEMP FOR BALDOCK
        };

        $scope.staffId = null;


        // Functions
        $scope.cancel = function () {
            $scope.formDisable = false;
            $scope.hideDeleteWarning = true;
        };

        $scope.continueDelete = function() {
            $scope.deleteShift();
        };

        $scope.deleteShift = function() {
            shiftService.delete($scope.shiftModel.Id).then(function(res) {
                rotaService.notify(null, "dpresources-store-updated");

                // clear down resources and events stores
                rotaService.dpResourcesStore = [];
                rotaService.eventsStore = [];

                helperService.statusHandler($scope, res.status, null, "The rota has been updated.");
                helperService.closeDialog($scope.ngDialogId);
            }).catch(function(error) {
                blockUI.stop();
                helperService.toasterError(null, error);
            });
        };

        $scope.getEmployees = function() {
            rotaDialogService.filterStaffOnLeave($scope);
            rotaDialogService.filterEmployeesOnConsecutiveDaysWorked($scope);

            $scope.startDate = helperService.convertBackToMomentObject($scope.startDate);
            $scope.endDate = helperService.convertBackToMomentObject($scope.endDate);

            rotaDialogService.filterEmployeesAlreadyAssignedToShift($scope);
        };

        $scope.triggerDeleteWarning = function() {
            $scope.hideDeleteWarning = false;
        };

        $scope.updateShift = function() {
            var checkedStaff = rotaDialogService.validateCheckedStaffAgainstQuantity($scope.filteredStaff, $scope.quantity);
            if (!rotaDialogService.validateCheckedStaff(checkedStaff)) {
                return;
            }

            if (checkedStaff[0]) {
                $scope.shiftModel.IsAssigned = true;
                $scope.staffId = checkedStaff[0].Id;
            }
            
            $scope.shiftModel.EmployeeId = $scope.staffId;

            shiftService.update($scope.shiftModel).then(function(res) {
                rotaService.notify(null, "dpresources-store-updated");

                // clear down resources and events stores
                rotaService.dpResourcesStore = [];
                rotaService.eventsStore = [];

                helperService.statusHandler($scope, res.status, null, "The rota has been updated.");
                helperService.closeDialog($scope.ngDialogId);
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

            $scope.getEmployees();
            blockUI.stop();
        };

        $scope.init();
    }
]);