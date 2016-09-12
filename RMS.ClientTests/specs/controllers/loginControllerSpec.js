/// <reference path="~/dependencies/jasmine.js" />
/// <reference path="~/dependencies/angular.min.js" />
/// <reference path="~/dependencies/angular-mocks.js" />
/// <reference path="~/dependencies/angular-ui-router.js" />
/// <reference path="~/dependencies/sinon.js" />
/// <reference path="~/dependencies/chai.js" />
/// <reference path="~/dependencies/lodash.js" />
/// <reference path="~/dependencies/toaster.min.js" />

/// <reference path="../../../RMS.WebAPI/public/js/app.js" />
/// <reference path="../../../RMS.WebAPI/public/js/services.js" />
/// <reference path="../../../RMS.WebAPI/public/js/controllers.js" />
/// <reference path="../../../RMS.WebAPI/public/js/services/authService.js" />
/// <reference path="../../../RMS.WebAPI/public/js/services/helperService.js" />
/// <reference path="../../../RMS.WebAPI/public/js/controllers/loginController.js" />

describe("Controller: Login Controller", function() {
    var assert = chai.assert;

    var scope, $state, httpBackend;
    var authService, helperService, toaster;

    beforeEach(function() {
        var mockAuthService = {};
        var mockHelperService = {};

        module("ui.router");
        module("toaster");
        module("controllers");
        module("services", function($provide) {
            $provide.value("authService", mockAuthService);
            $provide.value("helperService", mockHelperService);
        });

        inject(function($q) {
            mockAuthService.login = function(loginData) {
                var defer = $q.defer();
                
                if (loginData.password === "A_dr0w554pZ") {
                    defer.resolve(this.responseObj);
                } else {
                    defer.reject(this.rejectObj);
                }

                return defer.promise;
            };

            mockAuthService.authentication = {
                isAuth: true,
                login: "JohnW",
                sysAccess: "Admin",
                userId: 1
            };

            mockAuthService.responseObj = {
                data: {},
                status: 200,
                statusText: "Success"
            };

            mockAuthService.rejectObj = {
                data: {},
                status: 400,
                statusText: "Bad Request"
            };

            mockHelperService.toggleButtonDisable = function(flag) {
                if (!flag) {
                    return true;
                }

                return false;
            };

            mockHelperService.setLoginState = function() {
                var stateStr = null;
                
                switch (authService.authentication.sysAccess) {
                    case "Admin":
                        stateStr = "admin";
                        break;
                    case "Manager":
                        stateStr = "manager";
                        break;
                    default:
                        stateStr = "user";
                        break;

                }

                return stateStr;
            };

            mockHelperService.toasterError = function(headerStr, messageStr) {
                var toasterHeader = headerStr || "Error";
                var toasterMessage = messageStr || "Your action was unsuccessful";
            };
        });
    });

    beforeEach(inject(function ($injector) {
        httpBackend = $injector.get("$httpBackend");
    }));

    beforeEach(inject(function($controller, $rootScope, _$state_, _authService_, _helperService_, _toaster_) {
        scope = $rootScope.$new();
        $state = _$state_;
        authService = _authService_;
        helperService = _helperService_;
        toaster = _toaster_;

        $controller("loginCtrl", {
            $scope: scope,
            $state: $state,
            authService: authService,
            helperService: helperService,
            toaster: toaster
        });

        scope.$digest();
    }));

    // make sure no expectations were missed in your tests.
    // (e.g. expectGET or expectPOST)
    afterEach(function () {
        httpBackend.verifyNoOutstandingExpectation();
        httpBackend.verifyNoOutstandingRequest();
    });

    it("should set the login button as disabled on login attempt.", function() {
        // Arrange
        var expected = true;
        var stateSpy = sinon.stub($state, "go");

        scope.loginBtnDisabled = false;
        scope.loginData = {
            login: "JohnW",
            password: "A_dr0w554pZ"
        };

        // Act
        scope.login();

        // Assert
        expect(scope.loginBtnDisabled).toBe(expected);
    });

    it("should set the login button as enabled on failed login attempt.", function() {
        // Arrange
        var expected = false;

        scope.loginBtnDisabled = false;
        scope.loginData = {
            login: "JohnW",
            password: "password"
        };


        // Act
        scope.login();
        scope.$apply();

        // Assert
        expect(scope.loginBtnDisabled).toBe(expected);
    });

    it("should call a toaster when scope.loginError is called.", function() {
        // Arrange
        var headerStr = "Login Error";
        var messageStr = "There was an error logging you in.";

        spyOn(helperService, "toasterError");

        // Act
        scope.loginError();

        // Assert
        expect(helperService.toasterError).toHaveBeenCalledWith(headerStr, messageStr);
    });

    it("should call scope.loginError when a failed login attempt occurs.", function() {
        // Arrange
        spyOn(scope, "loginError");

        // Act
        scope.login();
        scope.$apply();

        // Assert
        expect(scope.loginError).toHaveBeenCalled();
    });
});