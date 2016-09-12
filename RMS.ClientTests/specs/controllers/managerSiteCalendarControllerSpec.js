/// <reference path="~/dependencies/jasmine.js" />
/// <reference path="~/dependencies/angular.min.js" />
/// <reference path="~/dependencies/angular-mocks.js" />
/// <reference path="~/dependencies/angular-ui-router.js" />
/// <reference path="~/dependencies/sinon.js" />
/// <reference path="~/dependencies/lodash.js" />
/// <reference path="~/dependencies/angular-block-ui.js" />
/// <reference path="~/dependencies/angular-local-storage.min.js" />

/// <reference path="../../../RMS.WebAPI/public/js/app.js" />
/// <reference path="../../../RMS.WebAPI/public/js/services.js" />
/// <reference path="../../../RMS.WebAPI/public/js/controllers.js" />
/// <reference path="../../../RMS.WebAPI/public/js/services/helperService.js" />
/// <reference path="../../../RMS.WebAPI/public/js/services/manager-area/managerAreaService.js" />
/// <reference path="../../../RMS.WebAPI/public/js/controllers/manager/managerSiteCalendarController.js" />

describe("Controller: Manager Area - Site Calendar Controller", function() {
    var scope, $state, $stateParams;
    var helperService, managerAreaService;
    var blockUI, localStorageService;

    beforeEach(function() {
        var mHelperService = {};
        var mManagerAreaService = {};
        var mLocalStorageService = {};

        module("ui.router");
        module("blockUI");
        module("controllers");
        module("services", function ($provide) {
            $provide.value("helperService", mHelperService);
            $provide.value("managerAreaService", mManagerAreaService);
            $provide.value("localStorageService", mLocalStorageService);
        });

        inject(function ($q) {
            mHelperService.notAuth = function () { };

            mHelperService.blockAndAuth = function (scp, bUI) {
                var authentication = {
                    isAuth: true,
                    login: "JohnW",
                    sysAccessRole: "Manager",
                    userId: 1
                }

                scp.authentication = authentication;

                return scp.authentication;
            }

            mManagerAreaService.siteCalendarsStore = [
                {
                    Id: 1,
                    Name: "Test Calendar A",
                    CalendarPeriodModels: [
                        { Id: 1, Name: "Test Caledndar Period A" }
                    ]
                }
            ];

            mLocalStorageService.set = function () { console.log("LS SET CALLED"); };
            mLocalStorageService.get = function () { };
        });
    });

    beforeEach(inject(function ($controller, $rootScope, _$state_, _$stateParams_, _helperService_,
        _managerAreaService_, _localStorageService_, _blockUI_) {
        scope = $rootScope.$new();
        $state = _$state_;
        $stateParams = _$stateParams_;
        helperService = _helperService_;
        managerAreaService = _managerAreaService_;
        localStorageService = _localStorageService_;
        blockUI = _blockUI_;

        $stateParams = { calendarId: 1 };

        $controller("managerSiteCalendarCtrl", {
            $scope: scope,
            $state: $state,
            $stateParams: $stateParams,
            helperService: helperService,
            managerAreaService: managerAreaService,
            localStorageService: localStorageService,
            blockUI: blockUI
        });

        scope.$digest();
    }));

    it("should call the helperService.notAuth function if scope.authentication.isAuth is false.", function () {
        // Arrange
        spyOn(helperService, "notAuth");
        scope.authentication = helperService.blockAndAuth(scope, blockUI);
        scope.authentication.isAuth = false;


        // Act
        scope.init();

        // Assert
        expect(helperService.notAuth).toHaveBeenCalled();
    });
});