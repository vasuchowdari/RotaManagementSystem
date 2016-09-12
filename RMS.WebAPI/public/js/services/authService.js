"use strict";

var app = angular.module("services");
app.factory("authService", [
    "$http",
    "$q",
    "localStorageService",
    function ($http, $q, localStorageService) {

        var authServiceFactory = {};

        var _authentication = {
            isAuth: false,
            login: "",
            sysAccess: "",
            userId: null
        };

        var _saveRegistration = function(registration) {

            _logOut();

            return $http.post("api/account/register", registration).then(function(response) {
                return response;
            });
        };

        var _login = function(loginData) {

            var deferred = $q.defer();

            $http.post("api/account/login", loginData).then(function (response) {
                var sysAccess = response.data.SystemAccessRoleDto.Name;
                var userId = response.data.Id;

                localStorageService.set("authorizationData", { login: loginData.login, sysAccess: sysAccess, userId: userId });

                _authentication.isAuth = true;
                _authentication.login = loginData.login;
                _authentication.sysAccess = sysAccess;
                _authentication.userId = userId;

                deferred.resolve(response);
            }, function(error) {
                _logOut();
                deferred.reject(error);
            });

            return deferred.promise;
        };

        var _logOut = function() {
             //localStorageService.remove("authorizationData");
            localStorageService.clearAll();

            _authentication.isAuth = false;
            _authentication.login = "";
            _authentication.sysAccess = "";
            _authentication.userId = null;
        };

        var _fillAuthData = function() {

            var authData = localStorageService.get("authorizationData");

            if (authData) {
                _authentication.isAuth = true;
                _authentication.login = authData.login;
                _authentication.sysAccess = authData.sysAccess;
                _authentication.userId = authData.userId;
            }
        };

        authServiceFactory.saveRegistration = _saveRegistration;
        authServiceFactory.login = _login;
        authServiceFactory.logOut = _logOut;
        authServiceFactory.fillAuthData = _fillAuthData;
        authServiceFactory.authentication = _authentication;

        return authServiceFactory;
    }
]);