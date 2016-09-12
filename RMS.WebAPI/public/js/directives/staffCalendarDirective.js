"use strict";

var app = angular.module("directives");
app.directive("staffCalendar", [
    "leaveRequestService",
    "adminAreaService",
    "helperService",
    "configService",
    "blockUI",
    function (leaveRequestService, adminAreaService, helperService, configService, blockUI) {
        function link(scope, elem, attrs) {
            // Declarations
            scope.events = [];
            scope.mappedEvents = [];
            scope.startDate = helperService.getMonthStartDate();

            scope.config = {
                startDate: scope.startDate
            };

            

            function init() {

            };

            init();
        }

        return {
            restrict: "E",
            replace: "true",
            templateUrl: configService.path + "/public/views/directive-tmpls/staff-calendar.html",
            scope: {
                employeeId: "=employeeId",
            },
            link: link,
            controller: function ($scope) {
                //var stepSize = 1;

                //$scope.previousMonth = function() {
                //    $scope.startDate = moment($scope.startDate).add(-stepSize, "months").format("YYYY-MM-DD");
                //    //$scope.endDate = moment($scope.endDate).add(-stepSize, "months").format("YYYY-MM-DD");
                //    adminAreaService.notify(null, "leave-calendar-updated-event");
                //};

                //$scope.nextMonth = function () {
                //    $scope.startDate = moment($scope.startDate).add(stepSize, "months").format("YYYY-MM-DD");
                //    //$scope.endDate = moment($scope.endDate).add(stepSize, "months").format("YYYY-MM-DD");
                //    adminAreaService.notify(null, "leave-calendar-updated-event");
                //};
            }
        };
    }
]);