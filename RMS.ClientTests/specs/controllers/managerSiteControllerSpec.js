/// <reference path="~/dependencies/jasmine.js" />
/// <reference path="~/dependencies/angular.min.js" />
/// <reference path="~/dependencies/angular-mocks.js" />
/// <reference path="~/dependencies/angular-ui-router.js" />
/// <reference path="~/dependencies/sinon.js" />
/// <reference path="~/dependencies/lodash.js" />
/// <reference path="~/dependencies/angular-block-ui.js" />
/// <reference path="~/dependencies/angular-local-storage.js" />
/// <reference path="../../../RMS.WebAPI/public/js/app.js" />
/// <reference path="../../../RMS.WebAPI/public/js/services.js" />
/// <reference path="../../../RMS.WebAPI/public/js/controllers.js" />
/// <reference path="../../../RMS.WebAPI/public/js/services/helperService.js" />
/// <reference path="../../../RMS.WebAPI/public/js/services/manager-area/managerAreaService.js" />
/// <reference path="../../../RMS.WebAPI/public/js/controllers/manager/managerSiteController.js" />
describe("Controller: Manager Area - Site Controller", function() {
    var scope, $state, $stateParams;
    var helperService, managerAreaService, calendarService;
    var blockUI, localStorageService;

    beforeEach(function() {
        var mHelperService = {};
        var mManagerAreaService = {};
        var mCalendarService = {};
        var mLocalStorageService = {};

        module("ui.router");
        module("blockUI");
        //module("LocalStorageModule", function($provide) {
        //    $provide.value("localStorageService", mLocalStorageService);
        //});
        module("controllers");
        module("services", function($provide) {
            $provide.value("helperService", mHelperService);
            $provide.value("managerAreaService", mManagerAreaService);
            $provide.value("calendarService", mCalendarService);
            $provide.value("localStorageService", mLocalStorageService);
        });


        inject(function($q) {
            mCalendarService.getForSite = function(siteId) {
                var deferred = $q.defer();

                if (!siteId) {
                    deferred.resolve(this.siteResponseObj);
                } else {
                    deferred.reject(this.siteRejectObj);
                }

                return deferred.promise;
            };

            mCalendarService.siteResponseObj = {
                data: {},
                status: 200,
                statusText: "Success"
            };

            mCalendarService.siteRejectObj = {
                data: {},
                status: 400,
                statusText: "Bad Request"
            };

            mHelperService.notAuth = function() {};

            mHelperService.blockAndAuth = function(scp, bUI) {
                var authentication = {
                    isAuth: true,
                    login: "JohnW",
                    sysAccessRole: "Manager",
                    userId: 1
                };
                scp.authentication = authentication;

                return scp.authentication;
            };
            mManagerAreaService.siteStore = {};
            mManagerAreaService.sitesStore = [
                {
                    Id: 1,
                    Name: "Test Site A",
                    SubSiteModels: [
                        { Id: 1, Name: "Test Sub Site A" }
                    ]
                }
            ];

            mLocalStorageService.set = function() {};

            mLocalStorageService.get = function(str) {
                var sitesStore = [
                    {
                        Id: 1,
                        Name: "Test Site A",
                        SubSiteModels: [
                            { Id: 1, Name: "Test SubSite A" }
                        ]
                    }
                ];

                return sitesStore;
            };
        });


    });

    beforeEach(inject(function($controller, $rootScope, _$state_, _$stateParams_, _helperService_,
        _managerAreaService_, _localStorageService_, _calendarService_, _blockUI_) {
        scope = $rootScope.$new();
        $state = _$state_;
        $stateParams = _$stateParams_;
        helperService = _helperService_;
        managerAreaService = _managerAreaService_;
        localStorageService = _localStorageService_;
        calendarService = _calendarService_;
        blockUI = _blockUI_;

        $stateParams = { siteId: 1 };

        $controller("managerSiteCtrl", {
            $scope: scope,
            $state: $state,
            $stateParams: $stateParams,
            helperService: helperService,
            managerAreaService: managerAreaService,
            localStorageService: localStorageService,
            calendarService: calendarService,
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

    it("should call blockUI.stop function if scope.authentication.isAuth is true.", function() {
        // Arrange
        spyOn(blockUI, "stop");
        scope.authentication = helperService.blockAndAuth(scope, blockUI);

        // Act
        scope.init();

        // Assert
        expect(blockUI.stop).toHaveBeenCalled();
    });

    it("should not call blockUI.stop function if scope.authentication.isAuth is false.", function() {
        // Arrange
        spyOn(blockUI, "stop");
        scope.authentication = helperService.blockAndAuth(scope, blockUI);
        scope.authentication.isAuth = false;

        // Act
        scope.init();

        // Assert
        expect(blockUI.stop).not.toHaveBeenCalled();
    });

    // TODO: LocalStorage bugs
    //it("should call localStorageService.get if managerAreaService.sitesStore.length equals zero.", function() {
    //    // Arrange
    //    var expected = "sitesStore";
    //    spyOn(localStorageService, "get");

    //    managerAreaService.sitesStore = [];
    //    scope.authentication = helperService.blockAndAuth(scope, blockUI);

    //    // Act
    //    scope.init();

    //    // Assert
    //    expect(localStorageService.get).toHaveBeenCalledWith(expected);
    //});
});