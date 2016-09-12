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
/// <reference path="../../../RMS.WebAPI/public/js/services/api-call/employeeService.js" />
/// <reference path="../../../RMS.WebAPI/public/js/services/api-call/companyService.js" />
/// <reference path="../../../RMS.WebAPI/public/js/controllers/manager/managerAppController.js" />

describe("Controller: Manager Area - App Controller", function() {
    var scope, $state;
    var helperService, managerAreaService;
    var employeeService, companyService, localStorageService, blockUI;
    var companyObject;

    beforeEach(function() {
        var mockAuthService = {};
        var mockHelperService = {};
        var mManagerAreaService = {};
        var mockEmployeeService = {};
        var mockCompanyService = {};
        var mCompanyObject = {};
        var mockLocalStorageService = {};

        module("ui.router");
        module("blockUI");
        module("controllers");
        module("services", function ($provide) {
            $provide.value("authService", mockAuthService);
            $provide.value("helperService", mockHelperService);
            $provide.value("managerAreaService", mManagerAreaService);
            $provide.value("employeeService", mockEmployeeService);
            $provide.value("companyService", mockCompanyService);
            $provide.value("companyObject", mCompanyObject);
            $provide.value("localStorageService", mockLocalStorageService);
        });

        inject(function($q) {
            mockAuthService.authentication = {
                isAuth: true,
                login: "JohnW",
                sysAccessRole: "Manager",
                userId: 1
            };

            mockHelperService.blockAndAuth = function(scp, bUI) {
                var authentication = {
                    isAuth: true,
                    login: "JohnW",
                    sysAccessRole: "Manager",
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

            mockCompanyService.getById = function(companyId) {
                var deferred = $q.defer();

                if (!companyId) {
                    deferred.reject(this.compRejectObj);
                } else {
                    deferred.resolve(this.compResponseObj);
                }

                return deferred.promise;
            };

            mockEmployeeService.getByUserId = function(userId) {
                var deferred = $q.defer();

                if (!userId) {
                    deferred.reject(this.empRejectObj);
                } else {
                    deferred.resolve(this.empResponseObj);
                }

                return deferred.promise;
            }

            mockEmployeeService.empResponseObj = {
                data: { Id: 1, CompanyId: 1 },
                status: 200,
                statusText: "Success"
            };

            mockEmployeeService.empRejectObj = {
                data: {},
                status: 400,
                statusText: "Bad Request"
            };

            mockCompanyService.compResponseObj = {
                data: { Name: "Nouvita", CompanyId: 1 },
                status: 200,
                statusText: "Success"
            };

            mockCompanyService.compRejectObj = {
                data: {},
                status: 400,
                statusText: "Bad Request"
            };

            mManagerAreaService.notify = function() {};
        });
    });

    beforeEach(inject(function ($controller, $rootScope, _$state_, _helperService_,
        _managerAreaService_, _employeeService_, _companyObject_, _blockUI_) {
        scope = $rootScope.$new();
        $state = _$state_;
        helperService = _helperService_;
        managerAreaService = _managerAreaService_;
        employeeService = _employeeService_;
        companyObject = _companyObject_;
        blockUI = _blockUI_;

        $controller("managerAppCtrl", {
            $scope: scope,
            $state: $state,
            helperService: helperService,
            managerAreaService: managerAreaService,
            employeeService: employeeService,
            companyObject: companyObject,
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

    it("should not call state.go if state.current.name is not manager.", function () {
        // Arrange
        spyOn($state, "go");
        $state.$current.name = "admin";

        // Act
        scope.init();

        // Assert
        expect($state.go).not.toHaveBeenCalled();
    });

    it("should call state.go if state.current.name is manager.", function () {
        // Arrange
        spyOn($state, "go");
        $state.$current.name = "manager";

        // Act
        scope.init();

        // Assert
        expect($state.go).toHaveBeenCalled();
    });

    it("should call state.go with the correct state.", function () {
        // Arrange
        spyOn($state, "go");
        $state.$current.name = "manager";
        scope.authentication = helperService.blockAndAuth(scope, blockUI);


        // Act
        scope.init();

        // Assert
        expect($state.go).toHaveBeenCalledWith("manager.dash");
    });

    it("should call the blockUI.stop function when scope.init is called.", function() {
        // Arrange
        spyOn(blockUI, "stop");
        scope.getEmployee = function () {};

        // Act
        scope.init();

        // Assert
        expect(blockUI.stop).toHaveBeenCalled();
    });

});