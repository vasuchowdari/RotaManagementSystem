"use strict";

var app = angular.module("controllers");
app.controller("managerShiftForApprovalCtrl", [
    "$scope",
    "$state",
    "$stateParams",
    "helperService",
    "managerAreaService",
    "shiftProfileService",
    "shiftService",
    "blockUI",
    function ($scope, $state, $stateParams, helperService, managerAreaService,
        shiftProfileService, shiftService, blockUI) {
        // Block and Auth
        helperService.blockAndAuth($scope, blockUI);


        // Declarations
        $scope.shiftProfile = angular.copy(helperService.getLocalStorageObject("shiftForApprovalStore"));

        var siteStore = helperService.getLocalStorageObject("siteStore");
        $scope.locationName = siteStore.Name;

        $scope.shiftTemplateName = "";
        $scope.shiftEndTime = moment($scope.shiftProfile.EndDateTime).format("HH:mm:ss");

        $scope.hideWarning = true;
        $scope.startTimeFieldDisabled = false;
        $scope.endTimeFieldDisabled = false;
        $scope.formDisable = false;
        $scope.startTimeAdjustment = "";
        $scope.endTimeAdjustment = "";
        $scope.unpaidBreakAdjustment = null;
        $scope.selectedReason = null;
        $scope.reasons = [
            "Reason 1",
            "Reason 2",
            "Reason 3",
            "Reason 4",
            "Reason 5",
            "Reason 6",
            "Other"
        ];
        $scope.notes = null;


        // Functions
        $scope.cancel = function() {
            $scope.formDisable = false;
            $scope.hideWarning = true;
        };

        $scope.continue = function() {
            $scope.updateShiftProfile();
        };

        $scope.saveForm = function() {
            $scope.formDisable = true;

            if (!$scope.shiftProfile.IsApproved) {
                $scope.triggerWarning();
            } else {
                $scope.updateShiftProfile();
            }
        };
        
        $scope.setTimeAdjustmentValues = function() {
            if ($scope.shiftProfile.Status !== 0) {
                if ($scope.shiftProfile.ActualStartDateTime === "-") {
                    $scope.startTimeAdjustment = helperService.setTimeAdjustmentField($scope.shiftProfile.StartDateTime);
                } else {
                    $scope.startTimeAdjustment = helperService.setTimeAdjustmentField($scope.shiftProfile.ActualStartDateTime);
                }

                if ($scope.shiftProfile.ActualEndDateTime === "-") {
                    $scope.endTimeAdjustment = helperService.setTimeAdjustmentField($scope.shiftProfile.EndDateTime);
                } else {
                    $scope.endTimeAdjustment = helperService.setTimeAdjustmentField($scope.shiftProfile.ActualEndDateTime);
                }

                $scope.calculateDuration();
            }
        };

        $scope.triggerWarning = function() {
            $scope.hideWarning = false;
        };

        $scope.updateShiftProfile = function () {

            $scope.shiftProfile.StartDateTime = moment($scope.shiftProfile.StartDateTime, "DD/MM/YYYY HH:mm:ss");
            $scope.shiftProfile.StartDateTime = helperService.formatDateTimeStringForServer($scope.shiftProfile.StartDateTime);

            $scope.shiftProfile.EndDateTime = moment($scope.shiftProfile.EndDateTime, "DD/MM/YYYY HH:mm:ss");
            $scope.shiftProfile.EndDateTime = helperService.formatDateTimeStringForServer($scope.shiftProfile.EndDateTime);

            $scope.shiftProfile.ActualStartDateTime = moment($scope.shiftProfile.ActualStartDateTime, "DD/MM/YYYY HH:mm:ss");
            $scope.shiftProfile.ActualStartDateTime = helperService.formatDateTimeStringForServer($scope.shiftProfile.ActualStartDateTime);

            $scope.shiftProfile.ActualEndDateTime = moment($scope.shiftProfile.ActualEndDateTime, "DD/MM/YYYY HH:mm:ss");
            $scope.shiftProfile.ActualEndDateTime = helperService.formatDateTimeStringForServer($scope.shiftProfile.ActualEndDateTime);

            // set HoursWorked into milliseconds for update
            $scope.shiftProfile.HoursWorked = moment.duration($scope.shiftProfile.TimeWorked).asMilliseconds();
            $scope.shiftProfile.Reason = $scope.selectedReason;
            $scope.shiftProfile.Notes = $scope.notes;

            shiftProfileService.update($scope.shiftProfile).then(function(res) {
                blockUI.stop();
                $scope.formDisable = false;
                helperService.toasterSuccess(null, res.data);
            }).catch(function(error) {
                blockUI.stop();
                helperService.toasterError(null, error);
            });
        };

        $scope.updateDuration = function() {
            var duration = null;
            var tempActualStartDateTime = angular.copy($scope.shiftProfile.ActualStartDateTime);
            var tempActualEndDateTime = angular.copy($scope.shiftProfile.ActualEndDateTime);

            tempActualStartDateTime = helperService.formatShiftActualForDurationCalculation(tempActualStartDateTime, $scope.startTimeAdjustment);
            tempActualEndDateTime = helperService.formatShiftActualForDurationCalculation(tempActualEndDateTime, $scope.endTimeAdjustment);

            duration = moment.duration(tempActualEndDateTime.diff(tempActualStartDateTime));
            //$scope.shiftProfile.TimeWorked = managerAreaService.returnCalculatedHoursMinutesSecondsFromMilliSec(duration);
            $scope.shiftProfile.TimeWorked = helperService.returnCalculatedHoursMinutesSecondsFromMilliSec(duration);
        };

        $scope.calculateDuration = function() {
            var duration = null;
            var isBefore = null;
            var isAfter = null;
            var tempActualStartDateTime = null;
            var tempActualEndDateTime = null;
            var tempShiftStartDateTime = $scope.shiftProfile.StartDateTime;
            var tempShiftEndDateTime = $scope.shiftProfile.EndDateTime;

            if ($scope.shiftProfile.ActualStartDateTime === "-") {
                $scope.shiftProfile.ActualStartDateTime = tempShiftStartDateTime;

                tempActualStartDateTime = angular.copy(tempShiftStartDateTime);
                tempActualStartDateTime =
                    helperService.formatShiftActualForDurationCalculation(
                        tempActualStartDateTime, $scope.startTimeAdjustment);
            } else {
                tempActualStartDateTime = angular.copy($scope.shiftProfile.ActualStartDateTime);

                // we have a datetime, but is it before or after shift start?
                isBefore = moment(tempActualStartDateTime).isBefore(tempShiftStartDateTime);

                if (isBefore) {
                    tempActualStartDateTime = moment(tempShiftStartDateTime, "DD/MM/YYYY HH:mm:ss");
                } else {
                    tempActualStartDateTime = moment(tempActualStartDateTime, "DD/MM/YYYY HH:mm:ss");
                }
            }

            if ($scope.shiftProfile.ActualEndDateTime === "-") {
                $scope.shiftProfile.ActualEndDateTime = tempShiftEndDateTime;

                tempActualEndDateTime = angular.copy($scope.shiftProfile.ActualEndDateTime);
                tempActualEndDateTime =
                    helperService.formatShiftActualForDurationCalculation(
                        tempActualEndDateTime, $scope.endTimeAdjustment);
            } else {
                tempActualEndDateTime = angular.copy($scope.shiftProfile.ActualEndDateTime);

                isAfter = moment(tempActualEndDateTime).isAfter(tempShiftEndDateTime);

                if (isAfter) {
                    tempActualEndDateTime = moment(tempShiftEndDateTime, "DD/MM/YYYY HH:mm:ss");
                } else {
                    tempActualEndDateTime = moment(tempActualEndDateTime, "DD/MM/YYYY HH:mm:ss");
                }
            }

            duration = moment.duration(tempActualEndDateTime.diff(tempActualStartDateTime));
            $scope.shiftProfile.TimeWorked = managerAreaService.returnCalculatedHoursMinutesSecondsFromMilliSec(duration);
        };


        // Init
        $scope.init = function() {
            if (!$scope.authentication.isAuth) {
                helperService.notAuth();
                return;
            }

            managerAreaService.formatShiftProfileDateTimesForDisplay($scope.shiftProfile);
            $scope.setTimeAdjustmentValues();

            shiftService.getById($scope.shiftProfile.ShiftId).then(function(res) {
                $scope.shiftTemplateName = res.data.ShiftTemplateModel.Name;

                var milliVal = res.data.ShiftTemplateModel.UnpaidBreakDuration;

                $scope.unpaidBreakAdjustment = managerAreaService.returnCalculatedHoursMinutesSecondsFromMilliSec(milliVal);
                $scope.unpaidBreakAdjustment = helperService.setUnpaidBreakAdjustmentField($scope.unpaidBreakAdjustment);
            }).catch(function (error) {
                helperService.toasterError(null, error);
            });

            blockUI.stop();
        }

        $scope.init();
    }
]);