"use strict";

var appp = angular.module("services");
app.factory("timeAdjustFormService", [
    "$http",
    function($http) {
        var timeAdjustmentFormServiceFactory = {};

        var _getAllUnapproved = function() {
            return $http.get("api/timeadjustmentform/getallunapproved").then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _getTimeAdjustmentFormsForIds = function(staffIds) {
            return $http.post("api/timeadjustmentform/getforids", staffIds).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        }

        var _returnNewTimeAdjustmentUpdateFormModel = function() {
            var tafUpdateModel = {
                timeAdjustmentFormId: null,
                shiftProfileId: null,
                reason: null,
                notes: null,
                isManagerApproved: null,
                isAdminApproved: null,
                actualStartDateTime: null,
                actualEndDateTime: null,
                zktStartDateTime: null,
                zktEndDateTime: null
            };

            return tafUpdateModel;
        };

        var _create = function(timeAdjustmentFormModel) {
            return $http.post("api/timeadjustmentform/create", timeAdjustmentFormModel).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _updateFormAndShiftProfile = function(tafUpdateModel) {
            return $http.post("api/timeadjustmentform/updateformandshiftprofile", tafUpdateModel).then(function (response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        timeAdjustmentFormServiceFactory.getAllUnapproved = _getAllUnapproved;
        timeAdjustmentFormServiceFactory.getTimeAdjustmentFormsForIds = _getTimeAdjustmentFormsForIds;
        timeAdjustmentFormServiceFactory.returnNewTimeAdjustmentUpdateFormModel = _returnNewTimeAdjustmentUpdateFormModel;
        timeAdjustmentFormServiceFactory.create = _create;
        timeAdjustmentFormServiceFactory.updateFormAndShiftProfile = _updateFormAndShiftProfile;

        return timeAdjustmentFormServiceFactory;
    }
]);