/// <reference path="~/dependencies/jasmine.js" />
/// <reference path="~/dependencies/angular.min.js" />
/// <reference path="~/dependencies/angular-mocks.js" />
/// <reference path="~/dependencies/angular-ui-router.js" />
/// <reference path="~/dependencies/sinon.js" />
/// <reference path="~/dependencies/chai.js" />
/// <reference path="~/dependencies/lodash.js" />
/// <reference path="~/dependencies/toaster.min.js" />
/// <reference path="~/dependencies/angular-block-ui.js" />

/// <reference path="../../../RMS.WebAPI/public/js/app.js" />
/// <reference path="../../../RMS.WebAPI/public/js/services.js" />
/// <reference path="../../../RMS.WebAPI/public/js/controllers.js" />
/// <reference path="../../../RMS.WebAPI/public/js/services/helperService.js" />
/// <reference path="../../../RMS.WebAPI/public/js/services/userService.js" />
/// <reference path="../../../RMS.WebAPI/public/js/controllers/profileController.js" />

describe("Controller: User Profile Controller", function() {
    var assert = chai.assert;

    var scope, $state;
    var helperService, userService, blockUI;

    beforeEach(function() {
        var mockHelperService = {};
        var mockUserService = {};

        module("ui.router");
        module("blockUI");
        module("controllers");
        module("services", function($provide) {
            $provide.value("helperService", mockHelperService);
            $provide.value("userService", mockUserService);
        });

        inject(function($q) {
            mockUserService.getById = function(userId) {
                var deferred = $q.defer();

                if (userId === 1) {
                    deferred.resolve(this.responseObj);
                } else {
                    deferred.reject(this.rejectObj);
                }

                return deferred.promise;
            };

            mockUserService.updateUserPassword = function(user) {
                var deferred = $q.defer();

                if (user.Password === "password") {
                    deferred.resolve(this.responseObj);
                } else {
                    deferred.reject(this.rejectObj);
                }

                return deferred.promise;
            };

            mockUserService.userData = {

            };

            mockUserService.responseObj = {
                data: {},
                status: 200,
                statusText: "Success"
            };

            mockUserService.rejectObj = {
                data: {},
                status: 400,
                statusText: "Bad Request"
            };

            mockHelperService.notAuth = function() {

            };

            mockHelperService.blockAndAuth = function(scp, bUI) {
                var authentication = {
                    isAuth: true,
                    login: "JohnW",
                    sysAccessRole: "Admin", 
                    userId: 2
                }

                scp.authentication = authentication;

                return scp.authentication;
            }

            mockHelperService.toggleButtonDisable = function (flag) {
                if (!flag) {
                    return true;
                }

                return false;
            };

            mockHelperService.toasterError = function() {

            };

            mockHelperService.toasterSuccess = function() {

            };

            mockHelperService.statusHandler = function(scp, responseStatus) {

            };
        });
    });

    beforeEach(inject(function($controller, $rootScope, _$state_, _helperService_, _userService_, _blockUI_) {
        scope = $rootScope.$new();
        $state = _$state_;
        helperService = _helperService_;
        userService = _userService_;
        blockUI = _blockUI_;

        $controller("profileCtrl", {
            $scope: scope,
            $state: $state,
            helperService: helperService,
            userService: userService,
            blockUI: blockUI
        });

        scope.$digest();
    }));

    it("should call the helperService notAuth function if not authenticated", function() {
        // Arrange
        spyOn(helperService, "notAuth");

        scope.authentication = helperService.blockAndAuth(scope, blockUI);
        scope.authentication.isAuth = false;

        // Act
        scope.init();

        // Assert
        expect(helperService.notAuth).toHaveBeenCalled();
    });

    it("should call the getUser function if authenticated", function() {
        // Arrange
        spyOn(scope, "getUser");

        // Act
        scope.init();

        // Assert
        expect(scope.getUser).toHaveBeenCalled();
    });

    it("should call the helperService toasterError function when scope.error is called", function() {
        // Arrange
        spyOn(helperService, "toasterError");

        // Act
        scope.error();

        // Assert
        expect(helperService.toasterError).toHaveBeenCalled();
    });

    it("should call the helperService toasterSuccess function when scope.success is called", function() {
        // Arrange
        spyOn(helperService, "toasterSuccess");

        // Act
        scope.success();

        // Assert
        expect(helperService.toasterSuccess).toHaveBeenCalled();
    });

    it("should call scope.error on userService.getById promise reject", function() {
        // Arrange
        spyOn(scope, "error");

        // Act
        // no need to init, as controller does anyway
        scope.getUser();
        scope.$apply();

        // Assert
        expect(scope.error).toHaveBeenCalled();
    });

    it("should set the submit button as enabled on failed password reset attempt", function() {
        // Arrange
        var expected = false;

        scope.newPassword = "pasword";
        scope.confirmNewPassword = "password";
        scope.user = {
            Password: "drowssap"
        };
        scope.submitBtnDisabled = false;

        // Act
        scope.updatePassword();

        // Assert
        expect(scope.submitBtnDisabled).toBe(expected);
    });

    it("should set the submit button as enabled on successful password reset attempt", function() {
        // Arrange
        var expected = false;

        scope.newPassword = "password";
        scope.confirmNewPassword = "password";
        scope.user = {
            Password: "drowssap"
        };
        scope.submitBtnDisabled = false;

        // Act
        scope.updatePassword();
        scope.$apply();

        // Assert
        expect(scope.submitBtnDisabled).toBe(expected);
    });

    it("should call the helperService.statusHandler function on successful password reset attempt", function() {
        // Arrange
        spyOn(helperService, "statusHandler");

        scope.newPassword = "password";
        scope.confirmNewPassword = "password";
        scope.user = { Password: "" };

        // Act
        scope.updatePassword();
        scope.$apply();

        // Assert
        expect(helperService.statusHandler).toHaveBeenCalled();
    });

    it("should call scope.error on userService.updateUserPassword promise reject", function() {
        // Arrange
        spyOn(scope, "error");

        scope.newPassword = "";
        scope.confirmNewPassword = "";
        scope.user = { Password: "" };

        // Act
        scope.updatePassword();
        scope.$apply();

        // Asert
        expect(scope.error).toHaveBeenCalled();
    });

    it("should call the helperService toasterError function when scope.updatePassword is called", function() {
        spyOn(helperService, "toasterError");

        scope.newPassword = "";
        scope.confirmNewPassword = "";
        scope.user = { Password: "" };

        // Act
        scope.updatePassword();
        scope.$apply();

        // Asert
        expect(helperService.toasterError).toHaveBeenCalled();
    });
});