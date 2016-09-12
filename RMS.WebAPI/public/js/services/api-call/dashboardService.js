"use strict";

var app = angular.module("services");
app.factory("dashboardService", [
    "$http",
    function ($http) {
        var dashboardServiceFactory = {};

        var _renderDashboard = function(authentication) {

            var myVar = "dashboard";

            switch (authentication.sysAccess) {
                case "Admin":
                    myVar += ".admin";
                    break;
                case "Manager":
                    myVar += ".manager";
                    break;
                case "User":
                    myVar += ".user";
                    break;
            default:
            }

            return myVar;
        };

        dashboardServiceFactory.renderDashboard = _renderDashboard;

        return dashboardServiceFactory;
    }
]);