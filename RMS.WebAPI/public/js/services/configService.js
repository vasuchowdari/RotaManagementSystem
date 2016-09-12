"use strict";

var app = angular.module("services");
app.factory("configService", [
    function() {
        var configServiceFactory = {};

        //var _path = "http://80.229.27.248:8093/RMSV2Dev";
        var _path = "http://80.229.27.248:8093/RMSV2FinDev";
        //var _path = "";

        configServiceFactory.path = _path;

        return configServiceFactory;
    }
]);