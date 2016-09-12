"use strict";

var app = angular.module("directives");
app.directive("availableStaff", [
    "configService",
    function(configService) {

        function link(scope, elem, attrs) {

        }

        return {
            restrict: "E",
            replace: "true",
            templateUrl: configService.path + "/public/views/directive-tmpls/available-staff.html",
            scope: {
                staff: "=staff"
            },
            link: link
        };
    }
]);