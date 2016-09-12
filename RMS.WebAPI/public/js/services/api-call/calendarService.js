"use strict";

var app = angular.module("services");
app.factory("calendarService", [
    "$http",
    function ($http) {
        var calendarServiceFactory = {};

        var _getAll = function () {
            return $http.get("api/calendar/getall").then(function (response) {
                return response;
            }, function (err) {
                return err;
            });
        };

        var _getById = function (id) {
            return $http.get("api/calendar/" + id).then(function (response) {
                return response;
            }, function (err) {
                return err;
            });
        };

        var _getForSite = function (siteId) {
            return $http.get("api/calendar/site/" + siteId).then(function (response) {
                return response;
            }, function (err) {
                return err.data.message;
            });
        };

        var _getForSubSite = function (subSiteId) {
            return $http.get("api/calendar/subsite/" + subSiteId).then(function (response) {
                return response;
            }, function (err) {
                return err.data.message;
            });
        };

        var _create = function (calendarModel) {
            return $http.post("api/calendar/create", calendarModel).then(function (response) {
                return response;
            }, function (err) {
                return err;
            });
        };

        var _update = function (calendarModel) {
            return $http.put("api/calendar/update", calendarModel).then(function (response) {
                return response;
            }, function (err) {
                return err;
            });
        };

        calendarServiceFactory.getAll = _getAll;
        calendarServiceFactory.getById = _getById;
        calendarServiceFactory.getForSite = _getForSite;
        calendarServiceFactory.getForSubSite = _getForSubSite;
        calendarServiceFactory.create = _create;
        calendarServiceFactory.update = _update;

        return calendarServiceFactory;
    }
]);