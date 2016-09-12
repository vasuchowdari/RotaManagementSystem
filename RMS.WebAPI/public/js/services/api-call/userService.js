"use strict";

var app = angular.module("services");
app.factory("userService", [
    "$http",
    function($http) {
        var userServiceFactory = {};

        var _checkEmailExists = function(emailStr) {
            return $http.get("api/account/email/check/", { Email: emailStr }).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _getAll = function() {
            return $http.get("api/user/getall").then(function(response) {
                return response.data;
            }).catch(function(error) {
                return error;
            });
        };

        var _getAllActive = function() {
            return $http.get("api/user/getallactive").then(function (response) {
                return response.data;
            }).catch(function(error) {
                return error;
            });
        };

        var _getById = function(userId) {
            return $http.get("api/user/" + userId).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _getUsernameAndId = function() {
            return $http.get("api/user/getusernameandid").then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _getCurrentProfile = function() {
            return $http.get("api/user/profile").then(function(response) {
                return response.data;
            }).catch(function(error) {
                return error;
            });
        };

        var _passwordReset = function(passwordResetModel) {
            return $http.post("api/account/passwordreset", passwordResetModel).then(function (response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _registerUser = function(viewModel) {
            return $http.post("api/account/register", viewModel).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _update = function(userModel) {
            return $http.post("api/user/update", userModel).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        var _updateUserPassword = function(userModel) {
            return $http.post("api/account/passwordupdate", userModel).then(function(response) {
                return response;
            }).catch(function(error) {
                return error;
            });
        };

        userServiceFactory.checkEmailExists = _checkEmailExists;
        userServiceFactory.getAll = _getAll;
        userServiceFactory.getAllActive = _getAllActive;
        userServiceFactory.getCurrentProfile = _getCurrentProfile;
        userServiceFactory.getById = _getById;
        userServiceFactory.getUsernameAndId = _getUsernameAndId;
        userServiceFactory.passwordReset = _passwordReset;
        userServiceFactory.registerUser = _registerUser;
        userServiceFactory.update = _update;
        userServiceFactory.updateUserPassword = _updateUserPassword;

        return userServiceFactory;
    }
]);