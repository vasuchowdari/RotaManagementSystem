/// <reference path="~/dependencies/jasmine.js" />
/// <reference path="~/dependencies/angular.min.js" />
/// <reference path="~/dependencies/angular-mocks.js" />
/// <reference path="~/dependencies/angular-ui-router.js" />
/// <reference path="~/dependencies/sinon.js" />
/// <reference path="~/dependencies/chai.js" />
/// <reference path="~/dependencies/lodash.js" />
/// <reference path="~/dependencies/angular-block-ui.js" />

/// <reference path="../../../RMS.WebAPI/public/js/app.js" />
/// <reference path="../../../RMS.WebAPI/public/js/services.js" />
/// <reference path="../../../RMS.WebAPI/public/js/controllers.js" />
/// <reference path="../../../RMS.WebAPI/public/js/services/authService.js" />
/// <reference path="../../../RMS.WebAPI/public/js/services/helperService.js" />
/// <reference path="../../../RMS.WebAPI/public/js/controllers/admin/adminAppController.js" />

describe("Controller: Admin Area - App Controller", function() {
    var scope, $state;
    var authService, helperService, blockUI;

    beforeEach(function() {
        var mockAuthService = {};
        var mockHelperService = {};

        module("ui.router");
        module("blockUI");
        module("controllers");
        module("services", function($provide) {
            $provide.value("authService", mockAuthService);
            $provide.value("helperService", mockHelperService);
        });

        inject(function ($q) {
            mockAuthService.authentication = {
                isAuth: true,
                login: "JohnW",
                sysAccessRole: "Admin",
                userId: 1
            };

            mockHelperService.blockAndAuth = function(scp, bUI) {
                var authentication = {
                    isAuth: true,
                    login: "JohnW",
                    sysAccessRole: "Admin",
                    userId: 1
                }

                scp.authentication = authentication;

                return scp.authentication;
            };

            mockHelperService.notAuth = function () {

            };

            mockHelperService.setDashState = function () {
                var stateStr = null;

                switch (mockAuthService.authentication.sysAccessRole) {
                    case "Admin":
                        stateStr = "admin.dash";
                        break;
                    case "Manager":
                        stateStr = "manager.dash";
                        break;
                    default:
                        stateStr = "user.dash";
                        break;

                }

                return stateStr;
            };
        });
    });

    beforeEach(inject(function ($controller, $rootScope, _$state_, _helperService_, _blockUI_) {
        scope = $rootScope.$new();
        $state = _$state_;
        helperService = _helperService_;
        blockUI = _blockUI_;

        $controller("adminAppCtrl", {
            $scope: scope,
            $state: $state,
            helperService: helperService,
            blockUI: blockUI
        });

        scope.$digest();
    }));

    it("should call the helperService.notAuth function if scope.authentication.isAuth is false.", function() {
        // Arrange
        spyOn(helperService, "notAuth");

        scope.authentication = helperService.blockAndAuth(scope, blockUI);
        scope.authentication.isAuth = false;

        // Act
        scope.init();

        // Assert
        expect(helperService.notAuth).toHaveBeenCalled();
    });

    it("should not call the helperService.notAuth function if scope.authentication.isAuth is true.", function () {
        // Arrange
        spyOn(helperService, "notAuth");

        scope.authentication = helperService.blockAndAuth(scope, blockUI);

        // Act
        scope.init();

        // Assert
        expect(helperService.notAuth).not.toHaveBeenCalled();
    });

    it("should not call state.go if state.current.name is not admin.", function () {
        // Arrange
        spyOn($state, "go");
        $state.$current.name = "manager";

        // Act
        scope.init();

        // Assert
        expect($state.go).not.toHaveBeenCalled();
    });

    it("should call state.go if state.current.name is admin.", function () {
        // Arrange
        spyOn($state, "go");
        $state.$current.name = "admin";

        // Act
        scope.init();

        // Assert
        expect($state.go).toHaveBeenCalled();
    });

    it("should call state.go with the correct state.", function () {
        // Arrange
        spyOn($state, "go");
        $state.$current.name = "admin";
        scope.authentication = helperService.blockAndAuth(scope, blockUI);


        // Act
        scope.init();

        // Assert
        expect($state.go).toHaveBeenCalledWith("admin.dash");
    });
});