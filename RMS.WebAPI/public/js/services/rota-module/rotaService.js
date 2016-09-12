"use strict";

var app = angular.module("services");
app.factory("rotaService", [
    "$rootScope",
    "helperService",
    "configService",
    "ngDialog",
    function ($rootScope, helperService, configService, ngDialog) {
        // Declarations
        var rotaServiceFactory = {};
        var _eventsStore = [];
        var _dpResourcesStore = [];


        // Event Handling
        var _notify = function (data, eventName) {
            $rootScope.$emit(eventName, data);
        };

        var _subscribe = function (scope, callback, eventName) {
            var handler = $rootScope.$on(eventName, callback);
            scope.$on("$destroy", handler);
        };


        // API Calls


        // Dialogs
        var _openNewCalResRqDialog = function() {
            ngDialog.open({
                showClose: true,
                closeByNavigation: false,
                closeByDocument: false,
                template: configService.path + "/public/views/dialog-tmpls/add-new-cal-resource-requirement.html",
                controller: "rotaResRqCtrl"
            });
        };

        var _openEditShiftDialog = function () {
            ngDialog.open({
                showClose: true,
                closeByNavigation: false,
                closeByDocument: false,
                template: configService.path + "/public/views/dialog-tmpls/edit-shift.html",
                controller: "rotaShiftUpdateCtrl"
            });
        };

        var _openEditCalResRqDialog = function () {
            ngDialog.open({
                showClose: false,
                closeByNavigation: false,
                closeByDocument: false,
                template: configService.path + "/public/views/dialog-tmpls/edit-cal-resoure-requirement.html",
                controller: "rotaResourceResRqUpdateCtrl"
            });
        };


        // Functions
        var _addToDayPilotEventStore = function(shiftModel, resourceName, resourceId, calPerResRqId, staffModel) {
            var startDateTime = helperService.shiftDateTimeFormatter(shiftModel.StartDate, shiftModel.ShiftTemplateModel.StartTime);
            var endDateTime = helperService.shiftDateTimeFormatter(shiftModel.EndDate, shiftModel.ShiftTemplateModel.EndTime);

            var startTimeStr = helperService.shiftTimeFormatter(shiftModel.ShiftTemplateModel.StartTime);
            var endTimeStr = helperService.shiftTimeFormatter(shiftModel.ShiftTemplateModel.EndTime);

            var shiftObjectColour = "#F5BCBC";
            var displayText = "Unassigned" + "<br />" + startTimeStr + "<br />" + endTimeStr + "<br />" + shiftModel.ShiftTemplateModel.Name;
            var isEmployee = false;
            if (staffModel) {
                displayText = staffModel.displayName + "<br />" + startTimeStr + "<br />" + endTimeStr + "<br />" + shiftModel.ShiftTemplateModel.Name;

                if (staffModel.EmployeeTypeId !== 3) {
                    shiftObjectColour = "#BCE7F5";
                    isEmployee = true;
                } else if (staffModel.EmployeeTypeId === 3) {
                    shiftObjectColour = "#FFFB00";
                }
            }

            // Night shift check
            var overnightCheckEndDate = moment(shiftModel.EndDate, 'YYYY-MM-DD');
            var overnightCheckStartDate = moment(shiftModel.StartDate, 'YYYY-MM-DD');
            if (moment(overnightCheckEndDate).isAfter(overnightCheckStartDate) && staffModel) {
                if (staffModel.EmployeeTypeId === 3) {
                    shiftObjectColour = "#FFFB00";
                } else {
                    shiftObjectColour = "#FBFFC9";
                }
            }

            // isBefore check needed
            var now = moment();
            var isBefore = moment(endDateTime).isBefore(now);
            if (isBefore) {
                if (!shiftModel.IsApproved) {
                    shiftObjectColour = "#D4BB90";

                    if (staffModel) {
                        if (staffModel.EmployeeTypeId === 3) {
                            shiftObjectColour = "#FFFB00";
                        }
                    }
                } else {
                    shiftObjectColour = "#A1D490";

                    if (staffModel) {
                        if (staffModel.EmployeeTypeId === 3) {
                            shiftObjectColour = "#FFFB00";
                        }
                    }
                }
            }

            rotaServiceFactory.eventsStore.push({
                start: new DayPilot.Date(startDateTime),
                end: new DayPilot.Date(endDateTime),
                isAssigned: shiftModel.IsAssigned,
                id: shiftModel.Id,
                shiftTemplateId: shiftModel.ShiftTemplateModel.Id,
                text: displayText,
                resource: String(shiftModel.CalendarResourceRequirementId),
                //calResRqId: String(shiftModel.CalendarResourceRequirementId),
                resourceName: resourceName,
                resourceId: resourceId,
                calPerResRqId: calPerResRqId,
                backColor: shiftObjectColour,
                isEmployee: isEmployee
            });

            rotaServiceFactory.notify(rotaServiceFactory.eventsStore, "events-store-updated");
        };

        var _addToDayPilotResourcesStore = function (resource, resRqId, testId) {
            rotaServiceFactory.dpResourcesStore.push({
                id: String(resRqId),
                //name: resource.Name + " (" + resRqId + ")"
                name: resource.Name
            });
        };

        var _returnSiteStoreId = function() {
            var siteStore = helperService.getLocalStorageObject("siteStore");
            return siteStore.Id;
        };

        var _schedulerDayScaleConfig = function(dpResources, startDate) {
            var config = {
                cssOnly: true,
                scale: "Day",
                days: 1,
                startDate: moment().format("YYYY-MM-DD"),
                cellWidth: 1150,
                eventHeight: 80,
                allowEventOverlap: true,
                eventStackingLineHeight: 25,
                eventMoveHandling: "disabled",
                eventResizeHandling: "disabled",
                moveBy: "None",
                useEventBoxes: "Never",
                timeHeaders: [
                    { groupBy: "Month", format: "MMMM yyyy" },
                    { groupBy: "Day", format: "d" }
                ],
                resources: dpResources
            };
            return config;
        };

        var _schedulerWeekScaleConfig = function(dpResources, startDate) {
            var config = {
                cssOnly: true,
                scale: "Day",
                days: 7,
                startDate: startDate,
                cellWidth: 164,
                eventHeight: 80,
                allowEventOverlap: true,
                eventStackingLineHeight: 25,
                eventMoveHandling: "disabled",
                eventResizeHandling: "disabled",
                moveBy: "None",
                useEventBoxes: "Never",
                rowClickHandling: "Select",
                onBeforeCellRender: function(args) {
                    if (args.cell.start <= DayPilot.Date.today() && DayPilot.Date.today() < args.cell.end) {
                        args.cell.backColor = "#F3EBFF";
                    }
                },
                onRowSelected: function(args) {
                    var authObj = helperService.getLocalStorageObject("authorizationData");
                    if (authObj.sysAccess !== "Manager") {
                        return;
                    }

                    helperService.setLocalStorageObject("calendarResourceRequirementStore", args.row);
                    rotaServiceFactory.openEditCalResRqDialog();
                },
                onEventClick: function(args) {
                    var authObj = helperService.getLocalStorageObject("authorizationData");
                    if (authObj.sysAccess !== "Manager") {
                        return;
                    }
                    
                    helperService.setLocalStorageObject("shiftStore", args.e.data);
                    rotaServiceFactory.openEditShiftDialog();
                },
                timeHeaders: [
                    { groupBy: "Month", format: "MMMM yyyy" },
                    { groupBy: "Day", format: "dddd d" }
                ],
                resources: dpResources
            };
            return config;
        };

        var _schedulerMonthScaleConfig = function(dpResources, startDate) {
            var endDate = moment(startDate).add(1, "months");
            var milliDiff = endDate.diff(startDate);
            var daysVal = moment.duration(milliDiff).asDays();

            var config = {
                cssOnly: true,
                scale: "Week",
                days: daysVal, // <-- this is the offending item (in all the config objects)
                startDate: startDate,
                cellWidth: 360,
                eventHeight: 80,
                allowEventOverlap: true,
                eventStackingLineHeight: 25,
                eventMoveHandling: "disabled",
                eventResizeHandling: "disabled",
                useEventBoxes: "Never",
                timeHeaders: [
                    { groupBy: "Month", format: "MMMM yyyy" },
                    { groupBy: "Day", format: "d" }
                ],
                resources: dpResources
            };
            return config;
        };

        var _schedulerYearScaleConfig = function(dpResources, startDate) {
            var config = {
                cssOnly: true,
                scale: "Month",
                days: 365,
                startDate: startDate,
                cellWidth: 1340,
                eventHeight: 80,
                allowEventOverlap: true,
                eventStackingLineHeight: 25,
                eventMoveHandling: "disabled",
                eventResizeHandling: "disabled",
                useEventBoxes: "Never",
                timeHeaders: [
                    { groupBy: "Month", format: "MMMM yyyy" },
                    { groupBy: "Day", format: "d" }
                ],
                resources: dpResources
            };
            return config;
        };

        var _zeroDaysSelectedValidation = function (shiftTemplateObj) {
            if (!shiftTemplateObj.Mon && !shiftTemplateObj.Tue && !shiftTemplateObj.Wed && !shiftTemplateObj.Thu &&
                !shiftTemplateObj.Fri && !shiftTemplateObj.Sat && !shiftTemplateObj.Sun) {
                return false;
            }

            return true;
        };


        // Wire up
        rotaServiceFactory.addToDayPilotEventStore = _addToDayPilotEventStore;
        rotaServiceFactory.addToDayPilotResourcesStore = _addToDayPilotResourcesStore;
        rotaServiceFactory.dpResourcesStore = _dpResourcesStore;
        rotaServiceFactory.eventsStore = _eventsStore;
        rotaServiceFactory.openNewCalResRqDialog = _openNewCalResRqDialog;
        rotaServiceFactory.openEditShiftDialog = _openEditShiftDialog;
        rotaServiceFactory.openEditCalResRqDialog = _openEditCalResRqDialog;
        rotaServiceFactory.returnSiteStoreId = _returnSiteStoreId;
        rotaServiceFactory.schedulerDayScaleConfig = _schedulerDayScaleConfig;
        rotaServiceFactory.schedulerWeekScaleConfig = _schedulerWeekScaleConfig;
        rotaServiceFactory.schedulerMonthScaleConfig = _schedulerMonthScaleConfig;
        rotaServiceFactory.schedulerYearScaleConfig = _schedulerYearScaleConfig;
        rotaServiceFactory.zeroDaysSelectedValidation = _zeroDaysSelectedValidation;

        rotaServiceFactory.notify = _notify;
        rotaServiceFactory.subscribe = _subscribe;

        return rotaServiceFactory;
    }
]);