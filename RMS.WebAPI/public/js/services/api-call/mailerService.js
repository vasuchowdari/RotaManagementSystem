"use strict";

var app = angular.module("services");
app.factory("mailerService", [
    "$http",
    function($http) {
        var mailerServiceFactory = {};

        var _sendLeaveRequestEmail = function(leaveRequestFormModel) {
            return $http.post("api/mailer/leaverequest", leaveRequestFormModel).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _sendTimeAdjustmentEmail = function(timeAdjustmentFormModel) {
            return $http.post("api/mailer/timeadjustment", timeAdjustmentFormModel).then(function (response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        mailerServiceFactory.sendLeaveRequestEmail = _sendLeaveRequestEmail;
        mailerServiceFactory.sendTimeAdjustmentEmail = _sendTimeAdjustmentEmail;

        return mailerServiceFactory;
    }
]);