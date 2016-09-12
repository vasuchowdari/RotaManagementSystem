"use strict";

var app = angular.module("controllers");
app.controller("userDashCtrl", [
    "$scope",
    "$state",
    "helperService",
    "userAreaService",
    "blockUI",
    function ($scope, $state, helperService, userAreaService, blockUI) {
        // Block And Auth
        helperService.blockAndAuth($scope, blockUI);


        // Declarations
        $scope.approvedLeaveRequests = [];
        $scope.assignedShifts = [];
        $scope.employeeModel = {};

        $scope.panelsExpanded = true;
        $scope.hideApprovedLeaveRequestPanel = false;
        $scope.hideAssignedShiftsPanel = false;
        $scope.hideTimeCorrectionPanel = false;
        $scope.hidePendingLeaveRequestPanel = false;

        $scope.hoursScheduledThisWeek = 40;
        $scope.hoursRemainingThisWeek = 12.5;
        $scope.hoursWorkedThisWeek = 27.5;

        $scope.pendingLeaveRequests = [];
        $scope.shiftsForApproval = [];
        $scope.weekStartDate = helperService.getWeekStartDate();
        

        // Functions
        $scope.getInvalidShiftProfilesForStaff = function() {
            // ONLY EMPLOYEE ID AT THE MOMENT
            userAreaService.getInvalidShiftProfilesForStaff($scope.employeeModel.Id).then(function(res) {
                $scope.shiftsForApproval = res;

                _.forEach($scope.shiftsForApproval, function(shiftProfile) {
                    shiftProfile.TimeWorked = userAreaService.calculateHoursMinutesSecondsFromMilliSec(shiftProfile.HoursWorked);
                    userAreaService.setShiftProfileActualsByStatus(shiftProfile);
                });
            });
        };

        $scope.getLeaveRequestsForEmployee = function() {
            userAreaService.getLeaveRequestsForEmployee($scope.employeeModel.Id).then(function(res) {
                _.forEach(res, function (leaveRequest) {

                    leaveRequest.numberOfDays = userAreaService.calculatedAmountRequested(leaveRequest.StartDateTime, leaveRequest.EndDateTime, null, null);

                    if (!leaveRequest.IsApproved) {
                        $scope.pendingLeaveRequests.push(leaveRequest);
                    } else {
                        $scope.approvedLeaveRequests.push(leaveRequest);
                    }
                });
            });
        };

        $scope.getAssignedShiftsForEmployee = function() {
            userAreaService.getAssignedShiftsForEmployee($scope.employeeModel.Id).then(function(res) {
                _.forEach(res, function(shift) {
                    shift.hours = helperService.returnDateTimeDifferenceAsHours(shift.StartDate, shift.EndDate);
                });

                $scope.assignedShifts = _.orderBy(res, ["StartDate"], ["asc"]);
            });
        }

        $scope.navToAdjustmentForm = function(shiftProfile) {
            $state.go("user.adjustment", { shiftData: shiftProfile });
        };

        $scope.toggleAllPanels = function() {
            if (!$scope.panelsExpanded) {
                $scope.panelsExpanded = true;

                $scope.hideApprovedLeaveRequestPanel = false;
                $scope.hideAssignedShiftsPanel = false;
                $scope.hideTimeCorrectionPanel = false;
                $scope.hidePendingLeaveRequestPanel = false;
            } else {
                $scope.panelsExpanded = false;

                $scope.hideApprovedLeaveRequestPanel = true;
                $scope.hideAssignedShiftsPanel = true;
                $scope.hideTimeCorrectionPanel = true;
                $scope.hidePendingLeaveRequestPanel = true;
            }
        };

        $scope.toggleApprovedLeaveRequestPanel = function() {
            $scope.hideApprovedLeaveRequestPanel = helperService.toggleShowHide($scope.hideApprovedLeaveRequestPanel);
        };

        $scope.toggleAssignedShiftsPanel = function() {
            $scope.hideAssignedShiftsPanel = helperService.toggleShowHide($scope.hideAssignedShiftsPanel);
        };

        $scope.togglePendingLeaveRequestPanel = function() {
            $scope.hidePendingLeaveRequestPanel = helperService.toggleShowHide($scope.hidePendingLeaveRequestPanel);
        };

        $scope.toggleTimeCorrectionPanel = function() {
            $scope.hideTimeCorrectionPanel = helperService.toggleShowHide($scope.hideTimeCorrectionPanel);
        };


        // Init
        $scope.init = function() {
            if (!$scope.authentication.isAuth) {
                helperService.notAuth();
            }

            if (!helperService.getLocalStorageObject("employeeModelStore")) {
                userAreaService.getEmployeeByUserId($scope.authentication.userId).then(function(res) {
                    helperService.setLocalStorageObject("employeeModelStore", res);
                    $scope.employeeModel = res;
                    //$scope.getInvalidShiftProfilesForStaff();
                    //$scope.getLeaveRequestsForEmployee();
                    $scope.getAssignedShiftsForEmployee();
                }).catch(function(error) {
                    helperService.toasterError(null, error);
                });
            } else {
                $scope.employeeModel = helperService.getLocalStorageObject("employeeModelStore");
                //$scope.getInvalidShiftProfilesForStaff();
                //$scope.getLeaveRequestsForEmployee();
                $scope.getAssignedShiftsForEmployee();
            }

            blockUI.stop();
        };

        $scope.init();
    }
]);