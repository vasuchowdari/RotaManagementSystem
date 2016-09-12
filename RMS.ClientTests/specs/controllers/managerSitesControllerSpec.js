/// <reference path="~/dependencies/jasmine.js" />
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
/// <reference path="../../../RMS.WebAPI/public/js/controllers/manager/managerSitesController.js" />
/// <reference path="../../../RMS.WebAPI/public/js/controllers/api-call/sitePersonnelLookupService.js" />

describe("Controller: Manager Area - Sites Controller", function() {
    var scope, $state;
    var helperService, managerAreaService, siteService, sitePersonnelLookupService;
    var blockUI;

    beforeEach(function() {
        var mHelperService = {};
        var mManagerAreaService = {};
        var mSiteService = {};
        var mSitePersonnelLookupService = {};

        module("ui.router");
        module("blockUI");
        module("controllers");
        module("services", function($provide) {
            $provide.value("helperService", mHelperService);
            $provide.value("managerAreaService", mManagerAreaService);
            $provide.value("siteService", mSiteService);
            $provide.value("sitePersonnelLookupService", mSitePersonnelLookupService);
        });

        inject(function ($q) {

            mSiteService.getSitesForCompany = function(companyId) {
                var deferred = $q.defer();

                if (!companyId) {
                    deferred.reject(this.sitesRejectObj);
                } else {
                    deferred.resolve(this.sitesResponseObj);
                }

                return deferred.promise;
            };

            mSiteService.sitesResponseObj = {
                data: {},
                status: 200,
                statusText: "Success"
            };

            mSiteService.sitesRejectObj = {
                data: {},
                status: 400,
                statusText: "Bad Request"
            };

            mManagerAreaService.companyStore = {};

            mManagerAreaService.managerStore = {};

            mManagerAreaService.subscribe = function() {};

            mManagerAreaService.accessFilterSiteStore = [];

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

            mSitePersonnelLookupService.getForEmployee = function(employeeId) {
                var deferred = $q.defer();

                if (!employeeId) {
                    deferred.reject(this.lookupsRejectObj);
                } else {
                    deferred.resolve(this.lookupsResponseObj);
                }

                return deferred.promise;
            };

            mSitePersonnelLookupService.lookupsResponseObj = {
                data: {},
                status: 200,
                statusText: "Success"
            };

            mSitePersonnelLookupService.lookupsRejectObj = {
                data: {},
                status: 400,
                statusText: "Bad Request"
            };
        });
    });

    beforeEach(inject(function ($controller, $rootScope, _$state_, _helperService_,
        _managerAreaService_, _siteService_, _sitePersonnelLookupService_, _blockUI_) {
        scope = $rootScope.$new();
        $state = _$state_;
        helperService = _helperService_;
        managerAreaService = _managerAreaService_;
        siteService = _siteService_;
        sitePersonnelLookupService = _sitePersonnelLookupService_;
        blockUI = _blockUI_;

        $controller("managerSitesCtrl", {
            $scope: scope,
            $state: $state,
            helperService: helperService,
            managerAreaService: managerAreaService,
            siteService: siteService,
            sitePersonnelLookupService: sitePersonnelLookupService,
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

    it("should not call the helperService.notAuth function if scope.authentication.isAuth is true.", function () {
        // Arrange
        spyOn(helperService, "notAuth");
        scope.authentication = helperService.blockAndAuth(scope, blockUI);

        // Act
        scope.init();

        // Assert
        expect(helperService.notAuth).not.toHaveBeenCalled();
    });

    it("should call blockUI.stop function if scope.authentication.isAuth is true.", function () {
        // Arrange
        spyOn(blockUI, "stop");
        scope.authentication = helperService.blockAndAuth(scope, blockUI);

        // Act
        scope.init();

        // Assert
        expect(blockUI.stop).toHaveBeenCalled();
    });

    it("should not call blockUI.stop function if scope.authentication.isAuth is false.", function () {
        // Arrange
        spyOn(blockUI, "stop");
        scope.authentication = helperService.blockAndAuth(scope, blockUI);
        scope.authentication.isAuth = false;

        // Act
        scope.init();

        // Assert
        expect(blockUI.stop).not.toHaveBeenCalled();
    });

    it("should call scope.getSites if scope.authentication.isAuth is true.", function () {
        // Arrange
        spyOn(scope, "getSites");

        // Act
        scope.init();

        // Assert
        expect(scope.getSites).toHaveBeenCalled();
    });

    it("should not call scope.getSites if scope.authentication.isAuth is false.", function () {
        // Arrange
        spyOn(scope, "getSites");
        scope.authentication = helperService.blockAndAuth(scope, blockUI);
        scope.authentication.isAuth = false;

        // Act
        scope.init();

        // Assert
        expect(scope.getSites).not.toHaveBeenCalled();
    });

    it("should call blockUI.stop function on a reject response from siteService.getSitesForCompany.", function() {
        // Arrange
        spyOn(blockUI, "stop");
        managerAreaService.companyStore.Id = null;

        // Act
        scope.getSites();
        scope.$apply();

        // Assert
        expect(blockUI.stop).toHaveBeenCalled();
    });

    it("should call scope.error function on a reject response from siteService.getSitesForCompany.", function () {
        // Arrange
        spyOn(scope, "error");
        managerAreaService.companyStore.Id = null;

        // Act
        scope.getSites();
        scope.$apply();

        // Assert
        expect(scope.error).toHaveBeenCalled();
    });

    it("should not call scope.error function on a successful response from siteService.getSitesForCompany.", function () {
        // Arrange
        managerAreaService.companyStore.Id = 1;
        managerAreaService.managerStore.Id = 1;
        spyOn(scope, "error");


        // Act
        scope.getSites();
        scope.$apply();

        // Assert
        expect(scope.error).not.toHaveBeenCalled();
    });

    it("should call blockUI.stop on a successful response from siteService.getSitesForCompany.", function() {
        // Arrange
        spyOn(blockUI, "stop");
        managerAreaService.companyStore.Id = 1;

        // Act
        scope.getSites();
        scope.$apply();

        // Assert
        expect(blockUI.stop).toHaveBeenCalled();
    });

    it("should call scope.getSiteAccessRights if managerAreaService.accessFilterSiteStore length is greater than zero.", function() {
        // Arrange
        managerAreaService.companyStore.Id = 1;
        managerAreaService.managerStore.Id = 1;

        spyOn(scope, "getSiteAccessRights");

        // Act
        scope.getSites();
        scope.$apply();

        // Assert
        expect(scope.getSiteAccessRights).toHaveBeenCalled();
    });

    it("should call blockUI.stop function on reject response from sitePersonnelLookupService.", function() {
        // Arrange
        spyOn(blockUI, "stop");

        // Act
        scope.getSiteAccessRights();
        scope.$apply();

        // Assert
        expect(blockUI.stop).toHaveBeenCalled();
    });

    it("should call scope.error function on reject response from sitePersonnelLookupService.", function() {
        // Arrange
        spyOn(scope, "error");

        // Act
        scope.getSiteAccessRights();
        scope.$apply();

        // Assert
        expect(scope.error).toHaveBeenCalled();
    });

    it("should not call blockUI.stop function on successful response from sitePersonnelLookupService.", function() {
        // Arrange
        spyOn(blockUI, "stop");
        scope.filterSitesStoreForManager = function() {};

        // Act
        scope.getSiteAccessRights(1);
        scope.$apply();

        // Assert
        expect(blockUI.stop).not.toHaveBeenCalled();
    });

    it("should not call scope.error on successful response from sitePersonnelLookupService.", function() {
        // Arrange
        spyOn(scope, "error");
        scope.filterSitesStoreForManager = function () { };

        // Act
        scope.getSiteAccessRights(1);
        scope.$apply();

        // Assert
        expect(scope.error).not.toHaveBeenCalled();
    });

    it("should call scope.filterSitesStoreForManager on successful response from sitePersonnelLookupService.", function() {
        // Arrange
        spyOn(scope, "filterSitesStoreForManager");

        // Act
        scope.getSiteAccessRights(1);
        scope.$apply();

        // Assert
        expect(scope.filterSitesStoreForManager).toHaveBeenCalled();
    });
});