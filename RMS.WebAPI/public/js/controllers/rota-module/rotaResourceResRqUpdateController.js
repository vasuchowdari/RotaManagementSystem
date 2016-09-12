"use strict";

var app = angular.module("controllers");
app.controller("rotaResourceResRqUpdateCtrl", [
    "$scope",
    "$state",
    "$stateParams",
    "rotaService",
    "helperService",
    "shiftTemplateService",
    "calResRqService",
    "resourceService",
    "rotaDialogService",
    "rotaRulesEngine",
    "blockUI",
    function($scope, $state, $stateParams, rotaService, helperService, shiftTemplateService,
        calResRqService, resourceService, rotaDialogService, rotaRulesEngine, blockUI) {
        // Block And Auth
        helperService.blockAndAuth($scope, blockUI);


        // Event Subscription
        rotaService.subscribe($scope, function() {
            if ($scope.triggerCounter === $scope.quantity) {
                
                calResRqService.update($scope.rotaModels).then(function(res) {
                    rotaService.notify(null, "dpresources-store-updated");

                    // clear down resources and events stores
                    rotaService.dpResourcesStore = [];
                    rotaService.eventsStore = [];

                    helperService.statusHandler($scope, res.status, null, res.data);

                    // DoW button CSS issue (only partially fixes it)
                    $scope.shiftModel.ShiftTemplate.Mon = false;
                    $scope.shiftModel.ShiftTemplate.Tue = false;
                    $scope.shiftModel.ShiftTemplate.Wed = false;
                    $scope.shiftModel.ShiftTemplate.Thu = false;
                    $scope.shiftModel.ShiftTemplate.Fri = false;
                    $scope.shiftModel.ShiftTemplate.Sat = false;
                    $scope.shiftModel.ShiftTemplate.Sun = false;

                    helperService.closeDialog($scope.ngDialogId);
                }).catch(function(error) {
                    blockUI.stop();
                    helperService.toasterError(null, error);
                });
            }
        }, "watch-event");

        rotaRulesEngine.subscribe($scope, function(e, staffId) {
            $scope.shiftAssigned = true;
            $scope.staffId = staffId;
            $scope.addShiftModels();
        }, "rules-passed-event");

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
        $scope.calendarResourceRequirement = helperService.getLocalStorageObject("calendarResourceRequirementStore");
        $scope.diffVal = null;
        $scope.endDate = moment();
        $scope.filteredTemplates = [];
        $scope.formDisable = false;
        $scope.hideWarning = true;
        $scope.hideDeleteWarning = true;
        $scope.options = {
            minDate: new Date(),
            showWeeks: false
        };
        $scope.quantity = 1;
        $scope.rotaModels = [];
        $scope.shiftAssigned = null;
        $scope.shiftModel = rotaDialogService.shiftModel;
        $scope.siteShiftTemplates = [];
        $scope.staffId = null;
        $scope.startDate = moment();
        $scope.subsiteShiftTemplates = [];
        $scope.triggerCounter = 0;
        


        // Functions
        $scope.addShiftModels = function() {
            $scope.newRotaModel = rotaDialogService.returnNewRotaModel($scope, $stateParams);

            $scope.newRotaModel.CalendarResourceRequirementModel.Id = $scope.resRqId;


            for (var x = 0; x <= $scope.diffVal; x++) {
                var startDate = rotaDialogService.returnShiftStartDate($scope.startDate, x);
                var skip = rotaDialogService.applyToDayOfWeek(startDate, $scope.shiftModel.ShiftTemplate);

                if (!skip) {
                    var endDate = rotaDialogService.returnShiftEndDate($scope.startDate, x);

                    if (rotaDialogService.checkIfOvernightShift($scope.shiftModel)) {
                        endDate = rotaDialogService.returnShiftEndDate($scope.startDate, x + 1);
                    }

                    startDate = helperService.shiftDateTimeFormatter(startDate, $scope.shiftModel.ShiftTemplate.StartTime);
                    endDate = helperService.shiftDateTimeFormatter(endDate, $scope.shiftModel.ShiftTemplate.EndTime);

                    rotaDialogService.addShiftModel(
                        $scope.newRotaModel,
                        $scope.staffId,
                        $scope.shiftAssigned,
                        startDate,
                        endDate,
                        $scope.shiftModel.ShiftTemplate.Id);
                }
            }

            $scope.rotaModels.push($scope.newRotaModel);
            $scope.triggerCounter++;
            rotaService.notify(null, "watch-event");
        };

        $scope.cancel = function() {
            $scope.formDisable = false;
            $scope.hideWarning = true;
            $scope.hideDeleteWarning = true;
        };

        $scope.continue = function() {
            $scope.updateResRq();
        };

        $scope.continueDelete = function() {
            $scope.deleteResourceReq();
        };

        $scope.deleteResourceReq = function() {
            calResRqService.delete($scope.calendarResourceRequirement.id).then(function (res) {
                rotaService.notify(null, "calendar-updated-event");
                helperService.statusHandler($scope, res.status, null, "The rota has been updated.");
                helperService.closeDialog($scope.ngDialogId);
            }).catch(function(error) {
                blockUI.stop();
                helperService.toasterError(null, error);
            });
        };

        $scope.getEmployees = function() {
            $scope.filteredStaff = [];
            
            rotaDialogService.filterStaffOnLeave($scope);
            rotaDialogService.filterEmployeesOnConsecutiveDaysWorked($scope);

            $scope.startDate = helperService.convertBackToMomentObject($scope.startDate);
            $scope.endDate = helperService.convertBackToMomentObject($scope.endDate);

            rotaDialogService.filterEmployeesAlreadyAssignedToShift($scope);
        };

        $scope.getShiftTemplates = function() {
            shiftTemplateService.getForSite(rotaService.returnSiteStoreId()).then(function(res) {
                    $scope.shiftTemplates = res.data;

                    if ($scope.shiftTemplates.length === 0) {
                        $scope.formDisable = true;
                        $scope.warning("Shift Template Warning", "There are no shift templates defined for this site. Please contact your administrator.");
                        return;
                    }

                    rotaDialogService.formatShiftTimes($scope.shiftTemplates);
                    rotaDialogService.populateShiftTemplateCollections($scope);
                    rotaDialogService.filterShiftTemplatesBySite($scope, $stateParams);

                    $scope.filteredTemplates = _.filter($scope.filteredTemplates, { "ResourceId": $scope.shiftModel.Resource.Id });
                    $scope.selectedResourceId = $scope.shiftModel.Resource.Id;

                    $scope.shiftModel.ShiftTemplate = rotaDialogService.setShiftTemplate($scope.filteredTemplates);
                    $scope.selectedShiftTemplateId = $scope.shiftModel.ShiftTemplate.Id;

                    $scope.getEmployees();
                }).catch(function(error) {
                    blockUI.stop();
                    helperService.toasterError(null, error);
                });
        };

        $scope.refreshStaff = function() {
            $scope.getEmployees();
        };

        $scope.saveForm = function() {
            if (moment($scope.endDate).isBefore($scope.startDate)) {
                helperService.toasterError("Shift Dates Error", "You cannot have an End Date before the Start Date.");
                return;
            }

            if (!rotaService.zeroDaysSelectedValidation($scope.shiftModel.ShiftTemplate)) {
                helperService.toasterError("Days Selected Error", "Please select at least one day of the week.");
                return;
            }

            var checkedStaff = _.filter($scope.filteredStaff, { "isChecked": true });
            if (checkedStaff.length === 0) {
                $scope.triggerWarning();
            } else {
                $scope.updateResRq();
            }
        };

        $scope.setResource = function() {
            var resourcesStore = helperService.getLocalStorageObject("resourcesStore");

            // Set Resource as User cannot change type
            $scope.shiftModel.Resource = _.head(_.filter(resourcesStore, { "Name": $scope.calendarResourceRequirement.name }));
            $scope.getShiftTemplates();
        };

        $scope.shiftTemplateChanged = function() {
            $scope.shiftModel.ShiftTemplate =
                rotaDialogService.returnNewShiftTemplateOnChange(
                    $scope.filteredTemplates,
                    $scope.selectedShiftTemplateId);

            $scope.refreshStaff();
        };

        $scope.triggerDeleteWarning = function() {
            $scope.hideDeleteWarning = false;
        };

        $scope.triggerWarning = function() {
            $scope.hideWarning = false;
        };

        $scope.updateResRq = function() {
            $scope.resRqId = Number($scope.calendarResourceRequirement.id);

            var checkedStaff = rotaDialogService.validateCheckedStaffAgainstQuantity($scope.filteredStaff, $scope.quantity);
            if (!rotaDialogService.validateCheckedStaff(checkedStaff)) {
                return;
            }

            $scope.diffVal = rotaDialogService.returnDifferenceInDays($scope);
            var largestBlock = rotaDialogService.getValidateReturnLargestBlock(
                $scope.shiftModel.ShiftTemplate, $scope.diffVal);

            for (var i = 0; i < $scope.quantity; i++) {
                if (checkedStaff[i]) {
                    $scope.shiftAssigned = true;
                    $scope.staffId = checkedStaff[i].Id;
                } else {
                    $scope.shiftAssigned = false;
                    $scope.staffId = null;
                }

                if ($scope.shiftAssigned) {
                    rotaRulesEngine.consecutiveDaysWorkedRuleForSavingShifts(
                        $scope.staffId, largestBlock, $scope.startDate, $scope.endDate);
                } else {
                    $scope.addShiftModels();
                }
            }
        };


        // Init
        $scope.init = function() {
            if (!$scope.authentication.isAuth) {
                helperService.notAuth();
                return;
            }

            $scope.setResource();
            blockUI.stop();
        };

        $scope.init();
    }
]);