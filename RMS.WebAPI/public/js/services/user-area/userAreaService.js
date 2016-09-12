"use strict";

var app = angular.module("services");
app.factory("userAreaService", [
    "$rootScope",
    "localStorageService",
    "mailerService",
    "leaveTypeService",
    "leaveRequestService",
    "employeeService",
    "shiftProfileService",
    "shiftService",
    "timeAdjustFormService",
    "helperService",
    function ($rootScope, localStorageService, mailerService, leaveTypeService,
        leaveRequestService, employeeService, shiftProfileService, shiftService,
        timeAdjustFormService, helperService) {
        var userAreaServiceFactory = {};

        // Event Handling
        var _notify = function(data, eventName) {
            $rootScope.$emit(eventName, data);
        };

        var _subscribe = function(scope, callback, eventName) {
            var handler = $rootScope.$on(eventName, callback);
            scope.$on("$destroy", handler);
        };


        // Declarations
        var _dateTimePickerConfig = {
            startView: "day",
            minView: "day"
        };


        // Functions
        var _calculatedAmountRequested = function(startDateTime, endDateTime, startHalfDay, endHalfDay) {
            var sd = moment(startDateTime);
            var ed = moment(endDateTime);
            var duration = moment.duration(ed.diff(sd));

            var valToReturn = duration.asDays();

            if (!startHalfDay && !endHalfDay) {
                valToReturn += 1;
            }

            if (!startHalfDay && endHalfDay) {
                valToReturn += 1;
                valToReturn -= 0.5;
            }

            if (startHalfDay && !endHalfDay) {
                valToReturn += 1;
                valToReturn -= 0.5;
            }

            return valToReturn;
        };

        var _calculateHoursMinutesSecondsFromMilliSec = function(millisecondVal) {
            var tempTime = moment.duration(millisecondVal);

            if (tempTime.seconds() >= 10) {
                return tempTime.hours() + ":" + tempTime.minutes() + ":" + tempTime.seconds();

            } else {
                return tempTime.hours() + ":" + tempTime.minutes() + ":0" + tempTime.seconds();
            }
        };

        var _createLeaveRequest = function(leaveRequestFormModel) {
            return leaveRequestService.create(leaveRequestFormModel).then(function(res) {
                helperService.toasterSuccess(null, res.data);
                return false;
            }).catch(function(error) {
                helperService.toasterError(null, error);
                return false;
            });
        };

        var _getEmployeeByUserId = function(userId) {
            return employeeService.getByUserId(userId).then(function(res) {
                return res.data;
            }).catch(function(error) {
                return error;
            });
        };

        var _getInvalidShiftProfilesForStaff = function(employeeId) {
            return shiftProfileService.getInvalidShiftProfilesForEmployee(employeeId).then(function(res) {
                return res.data;
            }).catch(function(error) {
                return error;
            });
        };

        var _getLeaveTypes = function() {
            return leaveTypeService.getAll().then(function(res) {
                return res.data;
            }).catch(function(error) {
                return error;
            });
        };

        var _getAssignedShiftsForEmployee = function(employeeId) {
            return shiftService.getForEmployee(employeeId).then(function(res) {
                return res.data;
            }).catch(function(error) {
                return error;
            });
        };

        var _getLeaveRequestsForEmployee = function(employeeId) {
            return leaveRequestService.getLeaveRequestsForEmployee(employeeId).then(function(res) {
                return res.data;
            }).catch(function(error) {
                return error;
            });
        };

        var _getShiftLocationName = function(shiftId) {
            return shiftService.getShiftLocationName(shiftId).then(function(res) {
                return res.data;
            }).catch(function(error) {
                return error;
            });
        };

        var _newLeaveRequestModel = function() {
            var model = {
                isActive: true,
                staffName: null,
                amountRequested: null,
                employeeId: null,
                leaveTypeName: null,
                leaveTypeId: null,
                startDateTime: null,
                endDateTime: null,
                notes: null,
                isApproved: false,
                isTaken: false
            };

            return model;
        };

        var _newTimeAdjustmentModel = function() {
            var model = {
                shiftId: null,
                shiftProfileId: null,
                employeeId: null,
                staffName: null,
                shiftLocation: null,
                shiftStartDateTime: null,
                shiftEndDateTime: null,
                actualStartDateTime: null,
                actualEndDateTime: null,
                missedClockIn: false,
                missedClockOut: false,
                lateIn: false,
                earlyOut: false,
                notes: null
            };

            return model;
        };

        var _sendTimeAdjustmentFormEmail = function(timeAdjustmentFormModel) {
            return timeAdjustFormService.create(timeAdjustmentFormModel).then(function(res) {
                helperService.toasterSuccess(null, res.data, "user.dash");
                return false;
            }).catch(function(error) {
                helperService.toasterError(null, error);
                return false;
            });
        };

        var _setShiftProfileActualsByStatus = function(shiftProfile) {
            switch (shiftProfile.Status) {
                case 5:
                    shiftProfile.ZktStartDateTime = "-";
                    shiftProfile.TimeWorked = "00:00:00";
                    break;
                case 6:
                    shiftProfile.ZktEndDateTime = "-";
                    shiftProfile.TimeWorked = "00:00:00";
                    break;
                case 7:
                    shiftProfile.ZktStartDateTime = "-";
                    shiftProfile.ZktEndDateTime = "-";
                    shiftProfile.TimeWorked = "00:00:00";
                    break;
            }
        };

        userAreaServiceFactory.calculatedAmountRequested = _calculatedAmountRequested;
        userAreaServiceFactory.calculateHoursMinutesSecondsFromMilliSec = _calculateHoursMinutesSecondsFromMilliSec;
        userAreaServiceFactory.createLeaveRequest = _createLeaveRequest;
        userAreaServiceFactory.dateTimePickerConfig = _dateTimePickerConfig;
        userAreaServiceFactory.getEmployeeByUserId = _getEmployeeByUserId;
        userAreaServiceFactory.getInvalidShiftProfilesForStaff = _getInvalidShiftProfilesForStaff;
        userAreaServiceFactory.getLeaveTypes = _getLeaveTypes;
        userAreaServiceFactory.getAssignedShiftsForEmployee = _getAssignedShiftsForEmployee;
        userAreaServiceFactory.getLeaveRequestsForEmployee = _getLeaveRequestsForEmployee;
        userAreaServiceFactory.getShiftLocationName = _getShiftLocationName;
        userAreaServiceFactory.newLeaveRequestModel = _newLeaveRequestModel;
        userAreaServiceFactory.newShiftTimeAdjustmentModel = _newTimeAdjustmentModel;
        userAreaServiceFactory.sendTimeAdjustmentFormEmail = _sendTimeAdjustmentFormEmail;
        userAreaServiceFactory.setShiftProfileActualsByStatus = _setShiftProfileActualsByStatus;

        userAreaServiceFactory.notify = _notify;
        userAreaServiceFactory.subscribe = _subscribe;

        return userAreaServiceFactory;
    }
]);