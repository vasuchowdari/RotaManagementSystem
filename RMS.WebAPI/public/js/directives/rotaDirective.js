"use strict";

var app = angular.module("directives");
app.directive("rota", [
    "calResRqService",
    "resourceService",
    "shiftProfileService",
    "managerAreaService",
    "helperService",
    "configService",
    "rotaService",
    "blockUI",
    function (calResRqService, resourceService, shiftProfileService, managerAreaService,
        helperService, configService, rotaService, blockUI) {
        function link(scope, elem, attrs) {
            // Declarations
            var resources = [];
            var dayPilotResources = [];

            scope.disabled = false;
            scope.scale = null;
            scope.config = {};
            scope.events = [];
            scope.agencyEventsInPeriod = 0;
            scope.unassignedEventsInPeriod = 0;

            // TODO: LOOK AT AND IMPROVE START / END DATES
            scope.startDate = helperService.getWeekStartDate();
            scope.endDate = moment(scope.startDate).add(1, "weeks").format("YYYY-MM-DD");


            // Event Subscriptions
            rotaService.subscribe(scope, function() {
                scope.events = [];
                scope.events = rotaService.eventsStore;
                scope.agencyEventsInPeriod = 0;
                scope.unassignedEventsInPeriod = 0;

                _.forEach(scope.events, function(event) {
                    var eventStartFormattedMoment = moment(event.start.value).format("YYYY-MM-DD HH:mm:ss");
                    var eventIsBeforeRangeStartDate = moment(eventStartFormattedMoment).isAfter(scope.startDate);

                    if (eventIsBeforeRangeStartDate) {
                        var eventIsAfterRangeEndDate = moment(eventStartFormattedMoment).isBefore(scope.endDate);

                        if (eventIsAfterRangeEndDate) {
                            if (!event.isAssigned) {
                                scope.unassignedEventsInPeriod++;
                            }

                            if (!event.isEmployee && event.isAssigned) {
                                scope.agencyEventsInPeriod++;
                            }
                        }
                    }
                });
            }, "events-store-updated");

            rotaService.subscribe(scope, function() {
                // reset
                scope.config = {};
                scope.events = [];
                resources = [];
                dayPilotResources = [];

                init();
            }, "dpresources-store-updated");

            rotaService.subscribe(scope, function() {
                rotaService.notify(null, "dpresources-store-updated");
            }, "calendar-updated-event");


            // Functions
            function generateStaffMemberDisplayName() {
                _.forEach(scope.staff, function(staffMember) {
                    staffMember.displayName = staffMember.UserModel.Firstname +
                        "<br />" + staffMember.UserModel.Lastname;
                });
            };

            function configSwitcher(dpResources, startDate) {
                var configObj = {};

                switch (scope.scale) {
                case "days":
                    configObj = rotaService.schedulerDayScaleConfig(dpResources, startDate);
                    break;
                case "weeks":
                    configObj = rotaService.schedulerWeekScaleConfig(dpResources, startDate);
                    break;
                case "months":
                    configObj = rotaService.schedulerMonthScaleConfig(dpResources, startDate);
                    break;
                case "years":
                    configObj = rotaService.schedulerYearScaleConfig(dpResources, startDate);
                    break;
                default:
                    configObj = rotaService.schedulerWeekScaleConfig(dpResources, startDate);
                    break;
                }

                return configObj;
            };

            function getCalendarData() {
                scope.disabled = true;
                var calendarId = scope.calendarId;

                calResRqService.getForCalendar(calendarId, scope.startDate, scope.endDate).then(function(res) {
                    // Scheduler Resources Setup
                    _.forEach(res.data, function(calResRq) {

                        var currentResourceId = null;
                        var currentResourceName = null;
                        var currentCalResRqId = calResRq.CalendarResourceRequirementModel.Id;

                        _.forEach(resources, function(resource) {
                            if (resource.Id === calResRq.CalendarResourceRequirementModel.ResourceId) {
                                rotaService.addToDayPilotResourcesStore(resource, calResRq.CalendarResourceRequirementModel.Id);

                                currentResourceName = resource.Name;
                                currentResourceId = resource.Id;
                            }
                        });

                        // Scheduler Events (Shifts) Setup
                        var shiftIds = [];
                        _.forEach(calResRq.CalendarResourceRequirementModel.ShiftModels, function (shiftModel) {
                            shiftIds.push(shiftModel.Id);
                        });

                        shiftProfileService.checkApproval(shiftIds).then(function(res) {
                            var approvedShifts = res.data;

                            _.forEach(calResRq.CalendarResourceRequirementModel.ShiftModels, function(shiftModel) {
                                shiftModel.IsApproved = false;

                                _.forEach(approvedShifts, function(approvedShift) {
                                    if (approvedShift.ShiftId === shiftModel.Id) {
                                        shiftModel.IsApproved = true;
                                    }
                                });

                                var mos = null;
                                if (!shiftModel.EmployeeId) {
                                    // unassigned
                                } else {
                                    // employee
                                    mos = _.head(_.filter(scope.staff, { "Id": shiftModel.EmployeeId }));
                                }

                                if (shiftModel.IsActive) {
                                    rotaService.addToDayPilotEventStore(shiftModel, currentResourceName, currentResourceId, currentCalResRqId, mos);
                                }
                            });
                        }).catch(function(error) {

                        });
                    });

                    dayPilotResources = rotaService.dpResourcesStore;
                    scope.config = configSwitcher(dayPilotResources, scope.startDate);
                    scope.disabled = false;

                    blockUI.stop();
                }).catch(function(error) {
                    scope.disabled = false;

                    blockUI.stop();
                });
            };

            function getResources() {
                resourceService.getAll().then(function(res) {
                    resources = res.data;
                    helperService.setLocalStorageObject("resourcesStore", res.data);

                    getCalendarData();
                }).catch(function(error) {
                    blockUI.stop();
                });
            };

            function init() {
                // clear down resources and events stores
                rotaService.dpResourcesStore = [];
                rotaService.eventsStore = [];

                var authObj = helperService.getLocalStorageObject("authorizationData");
                scope.hidePrintRotaBtn = true;

                if (authObj.sysAccess === "Admin") {
                    scope.hidePrintRotaBtn = false;
                }

                blockUI.start();
                generateStaffMemberDisplayName();
                getResources();
            };

            init();
        }

        return {
            restrict: "E",
            replace: "true",
            templateUrl: configService.path + "/public/views/directive-tmpls/rota.html",
            scope: {
                staff: "=staff",
                calendarId: "=calendarId"
            },
            link: link,
            controller: function ($scope) {
                var stepSize = 1;
                
                $scope.previousPeriod = function () {
                    if (!$scope.scale) {
                        $scope.scale = "weeks";
                    }

                    $scope.startDate = moment($scope.startDate).add(-stepSize, $scope.scale).format("YYYY-MM-DD");
                    $scope.endDate = moment($scope.endDate).add(-stepSize, $scope.scale).format("YYYY-MM-DD");
                    rotaService.notify(null, "calendar-updated-event");
                };

                $scope.nextPeriod = function() {
                    if (!$scope.scale) {
                        $scope.scale = "weeks";
                    }

                    $scope.startDate = moment($scope.startDate).add(stepSize, $scope.scale).format("YYYY-MM-DD");
                    $scope.endDate = moment($scope.endDate).add(stepSize, $scope.scale).format("YYYY-MM-DD");
                    rotaService.notify(null, "calendar-updated-event");
                };

                $scope.scaleChange = function(val) {
                    $scope.scale = val;
                    
                    // THIS IS HERE AS WITHOUT SEPERATE SCALER FOR SCALE CHANGE
                    // THE DATES NEVER CHANGED WHEN TRYING TO SCROLL BACK AND FORTH IN TIME
                    switch ($scope.scale) {
                        case "days":
                            break;
                        case "weeks":
                            $scope.startDate = helperService.getWeekStartDate();
                            break;
                        case "months":
                            $scope.startDate = helperService.getMonthStartDate();
                            break;
                        case "years":
                            $scope.startDate = helperService.getYearStartDate();
                            break;
                        default:
                            $scope.startDate = helperService.getWeekStartDate();
                            break;
                    }
                    rotaService.notify(null, "calendar-updated-event");
                };

                $scope.print = function() {
                    var wrappedDp = angular.element("#dp");

                    _.forEach(wrappedDp[0].dp.events.list, function(event) {
                        var str = event.text.split("<br />");
                        event.text = str[0] + "\n" + str[1] + "\n" + str[2] + "\n" + str[3] + "\n" + str[4];
                    });

                    wrappedDp[0].dp.export().print();

                    _.forEach(wrappedDp[0].dp.events.list, function (event) {
                        var str = event.text.split("\n");
                        event.text = str[0] + "<br />" + str[1] + "<br />" + str[2] + "<br />" + str[3] + "<br />" + str[4];
                    });
                };
            }
        };
    }
]);