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
/// <reference path="../../../RMS.WebAPI/public/js/controllers/resetController.js" />

describe("Controller: User Password Reset Controller", function() {
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
            mockHelperService.toasterError = function () {

            };

            mockHelperService.toasterSuccess = function () {

            };

            mockHelperService.toggleButtonDisable = function (flag) {
                if (!flag) {
                    return true;
                }

                return false;
            };

            mockHelperService.statusHandler = function(scp, responseStatus) {

            };

            mockUserService.passwordReset = function (resetModel) {
                var deferred = $q.defer();

                if (resetModel.Email === "wisejp@hotmail.com") {
                    deferred.resolve(this.responseObj);
                } else {
                    deferred.reject(this.rejectObj);
                }

                return deferred.promise;
            }

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
        });
    });

    beforeEach(inject(function($controller, $rootScope, _$state_, _helperService_, _userService_, _blockUI_) {
        scope = $rootScope.$new();
        $state = _$state_;
        helperService = _helperService_;
        userService = _userService_;
        blockUI = _blockUI_;

        $controller("resetCtrl", {
            $scope: scope,
            $state: $state,
            helperService: helperService,
            userService: userService,
            blockUI: blockUI
        });

        scope.$digest();
    }));

    it("should call the helperService.toasterError function when scope.error is called.", function() {
        // Arrange
        spyOn(helperService, "toasterError");

        // Act
        scope.error();

        // Assert
        expect(helperService.toasterError).toHaveBeenCalled();
    });

    it("should call the helperService.toasterSuccess function when scope.success is called.", function () {
        // Arrange
        spyOn(helperService, "toasterSuccess");

        // Act
        scope.success();

        // Assert
        expect(helperService.toasterSuccess).toHaveBeenCalled();
    });

    it("should set the reset button as enabled on successful password reset.", function() {
        // Arrange
        var expected = false;
        scope.resetBtnDisabled = false;
        scope.passwordResetModel = {
            Email: "wisejp@hotmail.com"
        };

        // Act
        scope.passwordReset();
        scope.$apply();

        // Assert
        expect(scope.resetBtnDisabled).toBe(expected);
    });

    it("should call the helperService.statusHandler on successful password reset.", function() {
        // Arrange
        spyOn(helperService, "statusHandler");

        scope.passwordResetModel = {
            Email: "wisejp@hotmail.com"
        };

        // Act
        scope.passwordReset();
        scope.$apply();

        // Assert
        expect(helperService.statusHandler).toHaveBeenCalledWith(scope, 200);
    });

    it("should set the reset button as enabled on rejected password reset.", function() {
        // Arrange
        var expected = false;
        scope.resetBtnDisabled = false;
        scope.passwordResetModel = {
            Email: "wisejp@gmail.com"
        };

        // Act
        scope.passwordReset();
        scope.$apply();

        // Assert
        expect(scope.resetBtnDisabled).toBe(expected);
    });

    it("should call scope.error on rejected password reset.", function() {
        // Arrange
        spyOn(scope, "error");

        scope.passwordResetModel = {
            Email: "wisejp@gmail.com"
        };


        // Act
        scope.passwordReset();
        scope.$apply();

        // Assert
        expect(scope.error).toHaveBeenCalled();
    });
});