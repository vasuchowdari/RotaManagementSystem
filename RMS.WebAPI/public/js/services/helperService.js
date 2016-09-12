"use strict";

var app = angular.module("services");
app.factory("helperService", [
    "$state",
    "blockUI",
    "toaster",
    "authService",
    "localStorageService",
    "ngDialog",
    function ($state, blockUI, toaster, authService, localStorageService, ngDialog) {
        var helperServiceFactory = {};

        var _auth = function(scope) {
            scope.authentication = authService.authentication;
        };

        var _blockAndAuth = function(scope, blockUI) {
            // Block
            blockUI.start();

            // Auth
            scope.authentication = authService.authentication;
        };

        var _formatShiftActualForDurationCalculation = function(dateTimeStr, adjTimeStr) {
            var tempDate = moment(dateTimeStr, "DD/MM/YYYY HH:mm:ss");
            tempDate = moment(tempDate).format("DD/MM/YYYY");

            var newDateTime = tempDate + " " + adjTimeStr;
            newDateTime = moment(newDateTime, "DD/MM/YYYY HH:mm:ss");

            return newDateTime;
        };

        var _changeSign = function(val) {
            return Math.abs(val);
        };

        var _checkAll = function(scopeObjects) {
            _.forEach(scopeObjects, function(scopeObject) {
                scopeObject.isChecked = true;
            });
        };

        var _closeDialog = function(dialogObjId) {
            setTimeout(function() {
                ngDialog.close(dialogObjId);
            }, 2000);
        };

        var _convertBackToMomentObject = function(dateTimeObj) {
            var yearStr = moment(dateTimeObj)._d.getFullYear();
            var monthStr = moment(dateTimeObj)._d.getMonth();
            var dateStr = moment(dateTimeObj)._d.getDate();
            var hoursStr = moment(dateTimeObj)._d.getHours();
            var minutesStr = moment(dateTimeObj)._d.getMinutes();
            var secondsStr = moment(dateTimeObj)._d.getSeconds();

            monthStr = helperServiceFactory.mapMonthIntValToString(monthStr);
            dateStr = helperServiceFactory.mapDateIntValToString(dateStr);

            if (hoursStr < 10) {
                hoursStr = "0" + hoursStr.toString();
            }

            if (minutesStr < 10) {
                minutesStr = "0" + minutesStr.toString();
            }

            if (secondsStr < 10) {
                secondsStr = "0" + secondsStr.toString();
            }

            var strToReturn =
                yearStr + "-" +
                monthStr + "-" +
                dateStr + " " +
                hoursStr + ":" +
                minutesStr + ":" +
                secondsStr;

            var newStrToRtn = moment(strToReturn).format("YYYY-MM-DD HH:mm:ss");

            return newStrToRtn;
        };

        var _formatDateTimeStringForServer = function(momentObj) {
            return moment(momentObj).format("YYYY-MM-DD HH:mm:ss");
        };

        var _formatUnmatchedClockingTimeStringForServer = function(clockingTimeStr) {
            return moment(clockingTimeStr, "DD/MM/YYYY HH:mm:ss").format("YYYY-MM-DD HH:mm:ss");
        };

        var _getLocalStorageObject = function(objNameStr) {
            return localStorageService.get(objNameStr);
        };

        var _getMonthStartDate = function() {
            // 0 based month in MOMENTJS
            var yr = moment().year();
            var mth = moment().month();
            var day = 1;
            var startDate = moment([yr, mth, day]);

            return startDate.format("YYYY-MM-DD");
        };

        var _getWeekStartDate = function() {
            // set Sunday as start of week
            var sow = moment().isoWeekday(0);
            return sow.format("YYYY-MM-DD");
        };

        var _getYearStartDate = function() {
            var yr = moment().year();
            var startDate = moment([yr, 0, 1]);
            return startDate.format("YYYY-MM-DD");
        };

        var _makeCheckable = function(scopeObjects) {
            _.forEach(scopeObjects, function(scopeObject) {
                scopeObject.IsChecked = false;
            });
        };

        var _mapDateIntValToString = function(intToMap) {
            var strToRtn = null;
            switch (intToMap) {
                case 1:
                    strToRtn = "01";
                    break;
                case 2:
                    strToRtn = "02";
                    break;
                case 3:
                    strToRtn = "03";
                    break;
                case 4:
                    strToRtn = "04";
                    break;
                case 5:
                    strToRtn = "05";
                    break;
                case 6:
                    strToRtn = "06";
                    break;
                case 7:
                    strToRtn = "07";
                    break;
                case 8:
                    strToRtn = "08";
                    break;
                case 9:
                    strToRtn = "09";
                    break;
                case 10:
                    strToRtn = "10";
                    break;
                case 11:
                    strToRtn = "11";
                    break;
                case 12:
                    strToRtn = "12";
                    break;
                case 13:
                    strToRtn = "13";
                    break;
                case 14:
                    strToRtn = "14";
                    break;
                case 15:
                    strToRtn = "15";
                    break;
                case 16:
                    strToRtn = "16";
                    break;
                case 17:
                    strToRtn = "17";
                    break;
                case 18:
                    strToRtn = "18";
                    break;
                case 19:
                    strToRtn = "19";
                    break;
                case 20:
                    strToRtn = "20";
                    break;
                case 21:
                    strToRtn = "21";
                    break;
                case 22:
                    strToRtn = "22";
                    break;
                case 23:
                    strToRtn = "23";
                    break;
                case 24:
                    strToRtn = "24";
                    break;
                case 25:
                    strToRtn = "25";
                    break;
                case 26:
                    strToRtn = "26";
                    break;
                case 27:
                    strToRtn = "27";
                    break;
                case 28:
                    strToRtn = "28";
                    break;
                case 29:
                    strToRtn = "29";
                    break;
                case 30:
                    strToRtn = "30";
                    break;
                case 31:
                    strToRtn = "31";
                    break;
            }

            return strToRtn;
        };

        var _mapMonthIntValToString = function(intToMap) {
            var strToRtn = null;
            switch (intToMap) {
                case 0:
                    strToRtn = "01";
                    break;
                case 1:
                    strToRtn = "02";
                    break;
                case 2:
                    strToRtn = "03";
                    break;
                case 3:
                    strToRtn = "04";
                    break;
                case 4:
                    strToRtn = "05";
                    break;
                case 5:
                    strToRtn = "06";
                    break;
                case 6:
                    strToRtn = "07";
                    break;
                case 7:
                    strToRtn = "08";
                    break;
                case 8:
                    strToRtn = "09";
                    break;
                case 9:
                    strToRtn = "10";
                    break;
                case 10:
                    strToRtn = "11";
                    break;
                case 11:
                    strToRtn = "12";
                    break;
            }

            return strToRtn;
        };

        var _notAuth = function() {
            $state.go("login");
            blockUI.stop();
            return;
        };

        var _rateFormatter = function(rateStr) {
            var formattedRateStr;

            rateStr = rateStr.toString();

            if (rateStr.length === 1) {
                formattedRateStr = rateStr + ".00";
            }

            if (rateStr.length === 2) {
                formattedRateStr = rateStr + ".00";
            }

            if (rateStr.length === 3) {
                formattedRateStr = rateStr + "0";
            }

            if (rateStr.length === 4) {
                formattedRateStr = rateStr + "0";
            }

            return formattedRateStr;
        };

        var _returnCalculatedHoursMinutesSecondsFromMilliSec = function(millisecondVal) {
            var tempTime = moment.duration(millisecondVal);

            if (tempTime.seconds() >= 10) {
                return tempTime.hours() + ":" + tempTime.minutes() + ":" + tempTime.seconds();
            } else {
                return tempTime.hours() + ":" + tempTime.minutes() + ":0" + tempTime.seconds();
            }
        };

        var _returnCalculatedMillisecondsFromMinutesValue = function(minutesValue) {
            return minutesValue * 60000;
        };

        var _returnCalculatedMinutesFromMillisecondsValue = function(millisecondsValue) {
            return millisecondsValue / 60000;
        };

        var _returnDateTimeDifferenceAsHours = function(startDateTime, endDateTime) {
            startDateTime = moment(startDateTime, "YYYY-MM-DD HH:mm:ss");
            endDateTime = moment(endDateTime, "YYYY-MM-DD HH:mm:ss");

            var duration = moment.duration(endDateTime.diff(startDateTime));
            return helperServiceFactory.returnCalculatedHoursMinutesSecondsFromMilliSec(duration);
        };

        var _returnMonthsOfYearDesc = function() {
            var mthArr = [
                { id: 0, text: "January" },
                { id: 1, text: "February" },
                { id: 2, text: "March" },
                { id: 3, text: "April" },
                { id: 4, text: "May" },
                { id: 5, text: "June" },
                { id: 6, text: "July" },
                { id: 7, text: "August" },
                { id: 8, text: "September" },
                { id: 9, text: "October" },
                { id: 10, text: "November" },
                { id: 11, text: "December" }
            ];

            return _.orderBy(mthArr, ["id"], ["desc"]);
        };

        var _returnPrevYearsDesc = function() {
            var years = [];
            var currYr = moment().year();

            for (var i = 0; i < 5; i++) {
                years.push(currYr - i);
            }

            return years;
        };

        var _setDashState = function() {
            var stateStr = null;

            switch (authService.authentication.sysAccess) {
            case "Admin":
                stateStr = "admin.dash";
                break;
            case "Manager":
                stateStr = "manager.dash";
                break;
            default:
                stateStr = "user.dash";
                break;
            }

            return stateStr;
        };

        var _setLocalStorageObject = function(objNameStr, obj) {
            localStorageService.set(objNameStr, obj);
        };

        var _setLoginState = function() {
            var stateStr = null;

            switch (authService.authentication.sysAccess) {
            case "Admin":
                stateStr = "admin";
                break;
            case "Manager":
                stateStr = "manager";
                break;
            default:
                stateStr = "user";
                break;
            }

            return stateStr;
        };

        var _setTimeAdjustmentField = function(srcDateTimeStr) {
            var newDateTimeStr = null;
            newDateTimeStr = moment(srcDateTimeStr, "DD/MM/YYYY HH:mm:ss");
            newDateTimeStr = moment(newDateTimeStr).format("HH:mm:ss");
            newDateTimeStr = newDateTimeStr.substring(0, newDateTimeStr.length - 3);

            return newDateTimeStr;
        };

        var _setUnpaidBreakAdjustmentField = function(srcTimeStr) {
            var newTimeStr = null;
            newTimeStr = moment(srcTimeStr, "HH:mm:ss");
            newTimeStr = newTimeStr._i.substring(0, newTimeStr._i.length - 3);
            newTimeStr = "0" + newTimeStr;

            return newTimeStr;
        };

        var _shiftDateTimeFormatter = function(dateStr, timeStr) {
            var newDateTimeStr = moment(dateStr).format("YYYY-MM-DD");
            newDateTimeStr += "T";

            if (timeStr.length > 5) {
                newDateTimeStr += moment(timeStr).format("HH:mm:ss");
            } else {
                newDateTimeStr += timeStr + ":00";
            }

            return newDateTimeStr;
        };

        var _shiftTimeFormatter = function(dateTimeStr) {
            return moment(dateTimeStr).format("HH:mm");
        };

        var _statusHandler = function(scope, status, headerStr, messageStr, uiState) {
            if (status !== 200) {
                blockUI.stop();

                if (messageStr.Message !== undefined || messageStr.Message !== "" || !messageStr.Message) {
                    helperServiceFactory.toasterError(headerStr, messageStr.Message);
                } else {
                    helperServiceFactory.toasterError(headerStr, messageStr);
                }
            } else {
                blockUI.stop();
                helperServiceFactory.toasterSuccess(headerStr, messageStr, uiState);
            }
        };

        var _toasterError = function(headerStr, messageStr) {
            var toasterHeader = headerStr || "Error";
            var toasterMessage = messageStr || "Your action was unsuccessful";

            toaster.pop("error", toasterHeader, toasterMessage);
        };

        var _toasterInfo = function(headerStr, messageStr) {
            var toasterHeader = headerStr || "FYI";
            var toasterMessage = messageStr || "This is important";

            toaster.pop("info", toasterHeader, toasterMessage);
        };

        var _toasterSuccess = function(headerStr, messageStr, stateStr) {
            var toasterHeader = headerStr || "Success";
            var toasterMessage = messageStr || "Your action was successful";

            toaster.pop({
                type: "success",
                title: toasterHeader,
                body: toasterMessage,
                onHideCallback: function() {
                    if (!stateStr) {
                        return;
                    }

                    $state.go(stateStr, $state.params, { reload: true });
                }
            });
        };

        var _toasterWarning = function(headerStr, messageStr) {
            var toasterHeader = headerStr || "Warning";
            var toasterMessage = messageStr || "Careful now!";

            toaster.pop("warning", toasterHeader, toasterMessage);
        };

        var _toggleButtonDisable = function(flag) {
            if (!flag) {
                return true;
            }

            return false;
        };

        var _toggleShowHide = function(flag) {
            if (!flag) {
                return true;
            }

            return false;
        };

        var _uncheckAll = function(scopeObjects) {
            _.forEach(scopeObjects, function(scopeObject) {
                scopeObject.isChecked = false;
            });
        };


        helperServiceFactory.auth = _auth;
        helperServiceFactory.blockAndAuth = _blockAndAuth;
        helperServiceFactory.changeSign = _changeSign;
        helperServiceFactory.checkAll = _checkAll;
        helperServiceFactory.closeDialog = _closeDialog;
        helperServiceFactory.convertBackToMomentObject = _convertBackToMomentObject;
        helperServiceFactory.formatDateTimeStringForServer = _formatDateTimeStringForServer;
        helperServiceFactory.formatShiftActualForDurationCalculation = _formatShiftActualForDurationCalculation;
        helperServiceFactory.formatUnmatchedClockingTimeStringForServer = _formatUnmatchedClockingTimeStringForServer;
        helperServiceFactory.getLocalStorageObject = _getLocalStorageObject;
        helperServiceFactory.getMonthStartDate = _getMonthStartDate;
        helperServiceFactory.getWeekStartDate = _getWeekStartDate;
        helperServiceFactory.getYearStartDate = _getYearStartDate;
        helperServiceFactory.makeCheckable = _makeCheckable;
        helperServiceFactory.mapDateIntValToString = _mapDateIntValToString;
        helperServiceFactory.mapMonthIntValToString = _mapMonthIntValToString;
        helperServiceFactory.notAuth = _notAuth;
        helperServiceFactory.rateFormatter = _rateFormatter;
        helperServiceFactory.returnCalculatedHoursMinutesSecondsFromMilliSec = _returnCalculatedHoursMinutesSecondsFromMilliSec;
        helperServiceFactory.returnCalculatedMillisecondsFromMinutesValue = _returnCalculatedMillisecondsFromMinutesValue;
        helperServiceFactory.returnCalculatedMinutesFromMillisecondsValue = _returnCalculatedMinutesFromMillisecondsValue;
        helperServiceFactory.returnDateTimeDifferenceAsHours = _returnDateTimeDifferenceAsHours;
        helperServiceFactory.returnMonthsOfYearDesc = _returnMonthsOfYearDesc;
        helperServiceFactory.returnPrevYearsDesc = _returnPrevYearsDesc;
        helperServiceFactory.setDashState = _setDashState;
        helperServiceFactory.setLocalStorageObject = _setLocalStorageObject;
        helperServiceFactory.setLoginState = _setLoginState;
        helperServiceFactory.setTimeAdjustmentField = _setTimeAdjustmentField;
        helperServiceFactory.setUnpaidBreakAdjustmentField = _setUnpaidBreakAdjustmentField;
        helperServiceFactory.shiftDateTimeFormatter = _shiftDateTimeFormatter;
        helperServiceFactory.shiftTimeFormatter = _shiftTimeFormatter;

        helperServiceFactory.statusHandler = _statusHandler;
        helperServiceFactory.toasterError = _toasterError;
        helperServiceFactory.toasterInfo = _toasterInfo;
        helperServiceFactory.toasterSuccess = _toasterSuccess;
        helperServiceFactory.toasterWarning = _toasterWarning;
        helperServiceFactory.toggleButtonDisable = _toggleButtonDisable;
        helperServiceFactory.toggleShowHide = _toggleShowHide;
        helperServiceFactory.uncheckAll = _uncheckAll;

        return helperServiceFactory;
    }
]);