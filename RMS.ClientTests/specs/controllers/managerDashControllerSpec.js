﻿/// <reference path="~/dependencies/jasmine.js" />
/// <reference path="~/dependencies/angular.min.js" />
/// <reference path="~/dependencies/angular-mocks.js" />
/// <reference path="~/dependencies/angular-ui-router.js" />
/// <reference path="~/dependencies/sinon.js" />
/// <reference path="~/dependencies/lodash.js" />
/// <reference path="~/dependencies/angular-block-ui.js" />

/// <reference path="../../../RMS.WebAPI/public/js/app.js" />
/// <reference path="../../../RMS.WebAPI/public/js/services.js" />
/// <reference path="../../../RMS.WebAPI/public/js/controllers.js" />
/// <reference path="../../../RMS.WebAPI/public/js/services/helperService.js" />
/// <reference path="../../../RMS.WebAPI/public/js/services/manager-area/managerAreaService.js" />
/// <reference path="../../../RMS.WebAPI/public/js/controllers/manager/managerDashController.js" />

describe("Controller: Manager Area - Dashboard Controller", function () {
    var scope, $state;
    var helperService, blockUI;

    beforeEach(function() {
        var mHelperService = {};

        module("ui.router");
        module("blockUI");
        module("controllers");
        module("services", function($provide) {
            $provide.value("helperService", mHelperService);
        });

        inject(function($q) {
            mHelperService.blockAndAuth = function (scp, bUI) {
                var authentication = {
                    isAuth: true,
                    login: "JohnW",
                    sysAccessRole: "Manager",
                    userId: 1
                }

                scp.authentication = authentication;

                return scp.authentication;
            };

            mHelperService.notAuth = function () { };
        });
    });

    beforeEach(inject(function($controller, $rootScope, _$state_, _helperService_, _blockUI_) {
        scope = $rootScope.$new();
        $state = _$state_;
        helperService = _helperService_;
        blockUI = _blockUI_;

        $controller("managerDashCtrl", {
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

    it("should not call the helperService.notAuth function if scope.authentication.isAuth is true.", function() {
        // Arrange
        spyOn(helperService, "notAuth");
        scope.authentication = helperService.blockAndAuth(scope, blockUI);

        // Act
        scope.init();

        // Assert
        expect(helperService.notAuth).not.toHaveBeenCalled();
    });

    it("should call blockUI.stop function if if scope.authentication.isAuth is true.", function() {
        // Arrange
        spyOn(blockUI, "stop");
        scope.authentication = helperService.blockAndAuth(scope, blockUI);

        // Act
        scope.init();

        // Assert
        expect(blockUI.stop).toHaveBeenCalled();
    });

    it("should not call blockUI.stop function if if scope.authentication.isAuth is false.", function () {
        // Arrange
        spyOn(blockUI, "stop");
        scope.authentication = helperService.blockAndAuth(scope, blockUI);
        scope.authentication.isAuth = false;

        // Act
        scope.init();

        // Assert
        expect(blockUI.stop).not.toHaveBeenCalled();
    });
});