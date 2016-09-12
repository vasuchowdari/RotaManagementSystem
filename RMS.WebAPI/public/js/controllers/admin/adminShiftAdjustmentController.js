"use strict";

var app = angular.module("controllers");
app.controller("adminShiftAdjustmentCtrl", [
    "$scope",
    "$state",
    "helperService",
    "adminAreaService",
    "blockUI",
    function ($scope, $state, helperService, adminAreaService, blockUI) {
        // Block And Auth
        helperService.blockAndAuth($scope, blockUI);


        // Declarations
        $scope.employees = [];
        $scope.formModel = {};
        $scope.hstep = 1;
        $scope.hideUnmatchedSection = true;
        $scope.mstep = 1;
        $scope.reasons = ["ApprOvertime", "Sickness", "StaffLate", "Other"];
        $scope.selectedUnmatchedEmployee = null;
        $scope.shiftDate = null;
        $scope.shiftProfile = {};
        $scope.shiftViewModel = {};
        $scope.shiftUpdateViewModel = {
            actualStartDateTime: null,
            actualEndDateTime: null,
            reason: null,
            notes: null
        };
        $scope.unmatchedDataViewModel = {
            staffMember: null,
            employeeId: null,
            startDateTime: null,
            endDateTime: null
        };
        $scope.unmatchedStartDateTimes = [];
        $scope.selectedUSDT = null;
        $scope.selectedUEDT = null;
        $scope.unmatchedEndDateTimes = [];
        $scope.unmatchedZktDisabled = true;

        var employeeArray = [];


        // Functions
        $scope.actionTimeAdjustment = function() {
            if ($scope.selectedUnmatchedEmployee != null && $scope.shiftViewModel.shiftStatus === "No Show") {
                var employee = _.head(_.filter($scope.employees, { "staffName": $scope.selectedUnmatchedEmployee }));

                if (!$scope.selectedUSDT) {
                    $scope.selectedUSDT = "01/01/1900 00:00:00";
                }
                if (!$scope.selectedUEDT) {
                    $scope.selectedUEDT = "01/01/1900 00:00:00";
                }

                var shiftProfileCreateModel = {
                    startDateTime: $state.params.shiftProfileData.startDateTime,
                    endDateTime: $state.params.shiftProfileData.endDateTime,
                    actualStartDateTime: helperService.formatDateTimeStringForServer($scope.shiftUpdateViewModel.actualStartDateTime),
                    actualEndDateTime: helperService.formatDateTimeStringForServer($scope.shiftUpdateViewModel.actualEndDateTime),
                    hoursWorked: 0,
                    employeeId: employee.id,
                    shiftId: $state.params.shiftProfileData.shiftId,
                    isApproved: true,
                    status: 0,
                    isActive: true,
                    reason: $scope.shiftUpdateViewModel.reason,
                    notes: $scope.shiftUpdateViewModel.notes,
                    isModified: true,
                    zktStartDateTime: helperService.formatUnmatchedClockingTimeStringForServer($scope.selectedUSDT),
                    zktEndDateTime: helperService.formatUnmatchedClockingTimeStringForServer($scope.selectedUEDT)
                };

                adminAreaService.createShiftProfileForUnmatchedZkt(shiftProfileCreateModel);

            } else {
                var shiftProfileUpdateModel = {
                    id: $scope.shiftViewModel.shiftProfileId,
                    startDateTime: $state.params.shiftProfileData.startDateTime,
                    endDateTime: $state.params.shiftProfileData.endDateTime,
                    actualStartDateTime: helperService.formatDateTimeStringForServer($scope.shiftUpdateViewModel.actualStartDateTime),
                    actualEndDateTime: helperService.formatDateTimeStringForServer($scope.shiftUpdateViewModel.actualEndDateTime),
                    hoursWorked: 0,
                    employeeId: 0,
                    shiftId: 0,
                    isApproved: true,
                    status: 0,
                    isActive: true,
                    reason: $scope.shiftUpdateViewModel.reason,
                    notes: $scope.shiftUpdateViewModel.notes,
                    isModified: true,
                    zktStartDateTime: helperService.formatDateTimeStringForServer($state.params.shiftProfileData.zktStartDateTime),
                    zktEndDateTime: helperService.formatDateTimeStringForServer($state.params.shiftProfileData.zktEndDateTime)
                };

                adminAreaService.updateShiftProfile(shiftProfileUpdateModel);
            }
        };

        $scope.getUnmatchedZkTimeData = function() {
            var model = {
                employeeName: $scope.selectedUnmatchedEmployee,
                startDateTime: helperService.formatDateTimeStringForServer($state.params.shiftProfileData.startDateTime),
                endDateTime: helperService.formatDateTimeStringForServer($state.params.shiftProfileData.endDateTime)
            };

            adminAreaService.getUnmatchedZkTimeDataForEmployee(model).then(function(res) {
                if (res.length > 0) {
                    $scope.unmatchedStartDateTimes = res;
                    $scope.unmatchedEndDateTimes = res;
                    $scope.unmatchedZktDisabled = false;
                } else {
                    $scope.unmatchedStartDateTimes = res;
                    $scope.unmatchedEndDateTimes = res;
                    $scope.unmatchedZktDisabled = true;
                }

                blockUI.stop();
            }).catch(function(error) {
                helperService.toasterError(null, error);
                blockUI.stop();
            });
        };

        $scope.hydrateShiftUpdateViewModel = function() {

            if (moment($state.params.shiftProfileData.actualStartDateTime).isBefore($state.params.shiftProfileData.startDateTime)) {
                $scope.shiftUpdateViewModel.actualStartDateTime = moment($state.params.shiftProfileData.startDateTime, moment.ISO_8601);
            } else {
                $scope.shiftUpdateViewModel.actualStartDateTime = moment($state.params.shiftProfileData.actualStartDateTime, moment.ISO_8601);
            }

            if (moment($state.params.shiftProfileData.actualEndDateTime).isAfter($state.params.shiftProfileData.endDateTime)) {
                $scope.shiftUpdateViewModel.actualEndDateTime = moment($state.params.shiftProfileData.endDateTime, moment.ISO_8601);
            } else {
                $scope.shiftUpdateViewModel.actualEndDateTime = moment($state.params.shiftProfileData.actualEndDateTime, moment.ISO_8601);
            }
        };

        $scope.mapStateParamsToModel = function() {
            $scope.shiftViewModel.shiftDate = moment($state.params.shiftProfileData.shiftDate).format("DD/MM/YYYY");
            $scope.shiftViewModel.shiftProfileId = $state.params.shiftProfileData.id;

            $scope.shiftViewModel.staffName = $state.params.shiftProfileData.staffMember;
            $scope.shiftViewModel.shiftLocation = $state.params.shiftProfileData.shiftLocation;
            $scope.shiftViewModel.shiftStatus = $state.params.shiftProfileData.status;
            $scope.shiftViewModel.resourceName = $state.params.shiftProfileData.resourceName;

            $scope.shiftViewModel.startDateTime = moment($state.params.shiftProfileData.startDateTime).format("HH:mm");
            $scope.shiftViewModel.endDateTime = moment($state.params.shiftProfileData.endDateTime).format("HH:mm");

            $scope.shiftViewModel.zktStartDateTime = moment($state.params.shiftProfileData.zktStartDateTime).format("HH:mm:ss");
            $scope.shiftViewModel.zktEndDateTime = moment($state.params.shiftProfileData.zktEndDateTime).format("HH:mm:ss");

            $scope.shiftViewModel.actualStartDateTime = moment($state.params.shiftProfileData.actualStartDateTime).format("HH:mm:ss");
            $scope.shiftViewModel.actualEndDateTime = moment($state.params.shiftProfileData.actualEndDateTime).format("HH:mm:ss");
        };


        // Init
        $scope.init = function() {
            if (!$scope.authentication.isAuth) {
                helperService.notAuth();
            }

            if (!$state.params.shiftProfileData) {
                $state.params.shiftProfileData = helperService.getLocalStorageObject("shiftProfileAdjustmentData");
            } else {
                helperService.setLocalStorageObject("shiftProfileAdjustmentData", $state.params.shiftProfileData);
            }

            $scope.mapStateParamsToModel();
            $scope.hydrateShiftUpdateViewModel();

            // Unmatched Data Section
            if ($scope.shiftViewModel.shiftStatus === "No Show") {
                employeeArray = helperService.getLocalStorageObject("employeeNameAndIdStore");
                _.forEach(employeeArray, function (employee) {
                    $scope.employees.push({
                        id: employee.Id,
                        staffName: employee.Firstname + " " + employee.Lastname
                    });
                });
                $scope.employees = _.orderBy($scope.employees, ["staffName"], ["asc"]);
                
                $scope.hideUnmatchedSection = false;
            }

            blockUI.stop();
        };

        $scope.init();
    }
]);