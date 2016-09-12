/// <reference path="~/dependencies/jasmine.js" />
/// <reference path="~/dependencies/angular.min.js" />
/// <reference path="~/dependencies/angular-ui-router.js" />
/// <reference path="~/dependencies/angular-mocks.js" />
/// <reference path="~/dependencies/sinon.js" />
/// <reference path="~/dependencies/chai.js" />
/// <reference path="~/dependencies/lodash.js" />

/// <reference path="../../../RMS.WebAPI/public/js/app.js" />
/// <reference path="../../../RMS.WebAPI/public/js/services.js" />
/// <reference path="../../../RMS.WebAPI/public/js/controllers.js" />
/// <reference path="../../../RMS.WebAPI/public/js/services/authService.js" />
/// <reference path="../../../RMS.WebAPI/public/js/controllers/indexController.js" />

describe("Controller: Index Controller", function() {
    var scope, $state, authService;
    var assert = chai.assert;

    beforeEach(function () {
        var mockAuthService = {};

        module("ui.router");
        module("controllers");
        module("services", function ($provide) {
            $provide.value("authService", mockAuthService);
        });


        inject(function () {
            // bare bones mock implementation of
            // auth service logout method called in controller
            mockAuthService.logOut = function () { };
        });
    });

    beforeEach(inject(function ($controller, $rootScope, _$state_, _authService_) {
        scope = $rootScope.$new();
        $state = _$state_;
        authService = _authService_;

        $controller("indexCtrl", {
            $scope: scope,
            $state: $state,
            authService: authService
        });

        scope.$digest();
    }));

    it("should set state to login on logout", function() {
        // Arrange
        var state = "login";
        var stateSpy = sinon.stub($state, "go");

        // Act
        scope.logOut();
        
        // Assert
        expect(stateSpy.calledOnce).toBe(true);
        assert(stateSpy.withArgs(state));
    });
});