"use strict";

var app = angular.module("services");
app.factory("reportService", [
    "$http",
    function($http) {
        var reportServiceFactory = {};

        var _returnNewDailyActivityReportParamsModel = function() {
            var model = {
                startDate: null,
                endDate: null,
                siteName: null,
                subSiteName: null,
                firstname: null,
                lastname: null,
                roleName: null
            }

            return model;
        };

        var _returnNewMonthlyPayrollReportParamsModel = function() {
            var model = {
                startDate: null,
                endDate: null
            }

            return model;
        };

        var _runDailyActivityReport = function(paramsModel) {
            return $http.post("api/report/dailyactivity", paramsModel).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _runMonthlyPayrollReport = function(paramsModel) {
            return $http.post("api/report/payroll/monthly", paramsModel).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        reportServiceFactory.returnNewDailyActivityReportParamsModel = _returnNewDailyActivityReportParamsModel;
        reportServiceFactory.returnNewMonthlyPayrollReportParamsModel = _returnNewMonthlyPayrollReportParamsModel;

        reportServiceFactory.runDailyActivityReport = _runDailyActivityReport;
        reportServiceFactory.runMonthlyPayrollReport = _runMonthlyPayrollReport;

        return reportServiceFactory;
    }
]);