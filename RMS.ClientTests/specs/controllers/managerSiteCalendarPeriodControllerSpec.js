/// <reference path="~/dependencies/jasmine.js" />
/// <reference path="~/dependencies/angular.min.js" />
/// <reference path="~/dependencies/angular-mocks.js" />
/// <reference path="~/dependencies/angular-ui-router.js" />
/// <reference path="~/dependencies/sinon.js" />
/// <reference path="~/dependencies/lodash.js" />
/// <reference path="~/dependencies/angular-block-ui.js" />
/// <reference path="~/dependencies/angular-local-storage.min.js" />
/// <reference path="~/dependencies/daypilot-all.min.js" />
/// <reference path="../../../RMS.WebAPI/public/js/app.js" />
/// <reference path="../../../RMS.WebAPI/public/js/services.js" />
/// <reference path="../../../RMS.WebAPI/public/js/controllers.js" />
/// <reference path="../../../RMS.WebAPI/public/js/services/helperService.js" />
/// <reference path="../../../RMS.WebAPI/public/js/services/manager-area/managerAreaService.js" />
/// <reference path="../../../RMS.WebAPI/public/js/controllers/manager/managerSiteCalendarPeriodController.js" />
describe("Controller: Manager Area - Site Calendar Period Controller", function() {
    var scope, $state, $stateParams;
    var helperService, managerAreaService, resourceService, employeeService;
    var sitePersonnelLookupService, calResRqService, shiftPatternService;
    var shiftTemplateService, shiftPersonnelService;
    var localStorageService, blockUI;

    beforeEach(function() {
        var mHelperService = {};
        var mManagerAreaService = {};
        var mResourceService = {};
        var mEmployeeService = {};
        var mSitePersonnelLookupService = {};
        var mCalPeriodResRqService = {};
        var mShiftPatternService = {};
        var mShiftTemplateService = {};
        var mShiftPersonnelService = {};
        var mLocalStorageService = {};


        module("ui.router");
        module("blockUI");
        module("controllers");
        module("services", function($provide) {
            $provide.value("helperService", mHelperService);
            $provide.value("managerAreaService", mManagerAreaService);

            $provide.value("resourceService", mResourceService);
            $provide.value("employeeService", mEmployeeService);
            $provide.value("sitePersonnelLookupService", mSitePersonnelLookupService);
            $provide.value("calResRqService", mCalPeriodResRqService);
            $provide.value("shiftPatternService", mShiftPatternService);
            $provide.value("shiftTemplateService", mShiftTemplateService);
            $provide.value("shiftPersonnelService", mShiftPersonnelService);
        });
        module("LocalStorageModule", function($provide) {
            $provide.value("localStorageService", mLocalStorageService);
        });

        inject(function($q) {
            mManagerAreaService.subscribe = function() {};
            mManagerAreaService.notify = function() {};
            mManagerAreaService.getSitePeriodsStore = function() {
                var periods = [];

                return periods;
            };
            mManagerAreaService.schedulerWeekScaleConfig = function() {
                var config = { resources: [] };
                return config;
            };
            mManagerAreaService.companyStore = { Id: 1 };
            mManagerAreaService.siteStore = { Id: 1 };
            mManagerAreaService.returnNewResourceRequirementModel = function() {
                var resRqModel = {
                    employeeId: null
                };

                return resRqModel;
            }


            mLocalStorageService.set = function() {};
            mLocalStorageService.get = function() {};

            mHelperService.shiftTimeFormatter = function (timeStr) { return "06:00"; };
            mHelperService.rateFormatter = function (rateStr) { return null; };
            mHelperService.shiftDateTimeFormatter = function(dateStr, timeStr) {};
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

            mResourceService.getAll = function(flag) {
                var deferred = $q.defer();

                if (flag) {
                    deferred.reject(this.getAllRejectObj);
                } else {
                    deferred.resolve(this.getAllResponseObj);
                }

                return deferred.promise;
            };
            mResourceService.getAllResponseObj = {
                data: [
                    { Id: 1, Name: "RMN" },
                    { Id: 2, Name: "HCA" }
                ],
                status: 200,
                statusText: "Success"
            };
            mResourceService.getAllRejectObj = {
                data: [],
                status: 400,
                statusText: "Bad Request"
            };

            mEmployeeService.getByCompanyId = function(companyId) {
                var deferred = $q.defer();

                if (!companyId) {
                    deferred.reject(this.getForCompanyIdRejectObj);
                } else {
                    deferred.resolve(this.getForCompanyIdResponseObj);
                }

                return deferred.promise;
            };
            mEmployeeService.getForCompanyIdRejectObj = {
                data: [],
                status: 400,
                statusText: "Bad Request"
            };
            mEmployeeService.getForCompanyIdResponseObj = {
                data: [
                    {
                        Id: 1,
                        UserModel: {
                            Firstname: "",
                            Lastname: ""
                        },
                        ContractModels: [
                            { Id: 1, ResourceId: 1 }
                        ]
                    },
                    {
                        Id: 2,
                        UserModel: {
                            Firstname: "",
                            Lastname: ""
                        },
                        ContractModels: [
                            { Id: 2, ResourceId: 1 }
                        ]
                    },
                    {
                        Id: 3,
                        UserModel: {
                            Firstname: "",
                            Lastname: ""
                        },
                        ContractModels: [
                            { Id: 3, ResourceId: 2 }
                        ]
                    },
                    {
                        Id: 4,
                        UserModel: {
                            Firstname: "",
                            Lastname: ""
                        },
                        ContractModels: [
                            { Id: 4, ResourceId: 2 }
                        ]
                    }
                ],
                status: 200,
                statusText: "Success"
            };

            mSitePersonnelLookupService.getForSite = function(siteId) {
                var deferred = $q.defer();

                deferred.resolve(this.getForSiteResponseObj);

                return deferred.promise;
            };
            mSitePersonnelLookupService.getForSiteResponseObj = {
                data: [
                    { Id: 1, SiteId: 1, EmployeeId: 1 },
                    { Id: 1, SiteId: 1, EmployeeId: 2 },
                    { Id: 1, SiteId: 1, EmployeeId: 3 },
                    { Id: 1, SiteId: 1, EmployeeId: 4 }
                ],
                status: 200,
                statusText: "Success"
            };

            mShiftTemplateService.getForSite = function(siteId) {
                var deferred = $q.defer();

                if (!siteId) {
                    deferred.reject(this.getForSiteRejectObj);
                } else {
                    deferred.resolve(this.getForSiteResponseObj);
                }

                return deferred.promise;
            };
            mShiftTemplateService.getForSiteRejectObj = {
                data: [],
                status: 400,
                statusText: "Bad Request"
            };
            mShiftTemplateService.getForSiteResponseObj = {
                data: [
                    {
                        Id: 1,
                        StartTime: "06:00",
                        EndTime: "06:00",
                        ResourceId: 1
                    },
                    {
                        Id: 2,
                        StartTime: "06:00",
                        EndTime: "15:30",
                        ResourceId: 2
                    }
                ],
                status: 200,
                statusText: "Success"
            };

            mShiftPatternService.getForCalendarPeriodResRq = function(resRqId) {
                var deferred = $q.defer();

                if (!resRqId) {
                    deferred.reject(this.getForCalendaPeriodResRqRejectObj);
                } else {
                    deferred.response(this.getForCalendaPeriodResRqResponseObj);
                }

                return deferred.promise;
            };
            mShiftPatternService.getForCalendaPeriodResRqRejectObj = {
                data: [],
                status: 400,
                statusText: "Bad Request"
            };
            mShiftPatternService.getForCalendaPeriodResRqResponseObj = {
                data: [
                    {
                        Id: 1,
                        CalendarPeriodResourceRequirementId: 1,
                        ShiftModels: []
                    },
                    {
                        Id: 2,
                        CalendarPeriodResourceRequirementId: 2,
                        ShiftModels: []
                    }
                ],
                status: 200,
                statusText: "Success"
            };

            mShiftPersonnelService.getForShift = function(shiftModelId) {
                var deferred = $q.defer();

                if (!shiftModelId) {
                    deferred.reject(this.getForShiftRejectObj);
                } else {
                    deferred.resolve(this.getForShiftResponseObj);
                }

                return deferred.promise;
            };
            mShiftPersonnelService.getForShiftResponseObj = {
                data: {
                    Id: 1,
                    SiteId: 1,
                    EmployeeId: 1
                },
                status: 200,
                statusText: "Success"
            };
            mShiftPersonnelService.getForShiftRejectObj = {
                data: [],
                status: 400,
                statusText: "Bad Request"
            };

            mCalPeriodResRqService.getForPeriod = function (periodId) {
                var deferred = $q.defer();

                if (!periodId) {
                    deferred.reject(this.getForPeriodRejectObj);
                } else {
                    deferred.resolve(this.getForPeriodResponseObj);
                }

                return deferred.promise;
            };
            mCalPeriodResRqService.getForPeriodResponseObj = {
                data: [],
                status: 200,
                statusText: "Success"
            };
            mCalPeriodResRqService.getForPeriodRejectObj = {
                data: [],
                status: 400,
                statusText: "Bad Request"
            };
        });
    });

    beforeEach(inject(function($controller, $rootScope, _$state_, _$stateParams_, _helperService_,
        _managerAreaService_, _resourceService_, _employeeService_, _sitePersonnelLookupService_,
        _calPeriodResRqService_, _shiftPatternService_, _shiftTemplateService_, _shiftPersonnelService_,
        _localStorageService_, _blockUI_) {
        scope = $rootScope.$new();
        $state = _$state_;
        $stateParams = _$stateParams_;
        helperService = _helperService_;
        managerAreaService = _managerAreaService_;
        resourceService = _resourceService_;
        employeeService = _employeeService_;
        sitePersonnelLookupService = _sitePersonnelLookupService_;
        calResRqService = _calPeriodResRqService_;
        shiftPatternService = _shiftPatternService_;
        shiftTemplateService = _shiftTemplateService_;
        shiftPersonnelService = _shiftPersonnelService_;
        localStorageService = _localStorageService_;
        blockUI = _blockUI_;

        $stateParams = { periodId: 1 };

        $controller("managerSiteCalendarPeriodCtrl", {
            $scope: scope,
            $state: $state,
            $stateParams: $stateParams,
            helperService: helperService,
            managerAreaService: managerAreaService,
            resourceService: resourceService,
            employeeService: employeeService,
            sitePersonnelLookupService: sitePersonnelLookupService,
            calResRqService: calResRqService,
            shiftPatternService: shiftPatternService,
            shiftTemplateService: shiftTemplateService,
            shiftPersonnelService: shiftPersonnelService,
            localStorageService: localStorageService,
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

    it("should call scope.getPeriods if scope.authentication.isAuth is true.", function() {
        // Arrange
        spyOn(scope, "getPeriods");
        scope.authentication = helperService.blockAndAuth(scope, blockUI);

        // Act
        scope.init();

        // Assert
        expect(scope.getPeriods).toHaveBeenCalled();
    });

    it("should call scope.getResources if scope.authentication.isAuth is true.", function() {
        // Arrange
        spyOn(scope, "getPeriods");
        scope.authentication = helperService.blockAndAuth(scope, blockUI);

        // Act
        scope.init();

        // Assert
        expect(scope.getPeriods).toHaveBeenCalled();
    });

    // TODO: Uses LS in code snippet. Look at how you solved this, John.
    it("should return an object from managerArea.getSitePeriodsStore when called.", function() {
        // Arrange
        var expected = [];
        scope.periods = [];

        // Act
        scope.getPeriods();
        scope.$apply();

        // Assert
        expect(scope.periods).toEqual(expected);
    });

    it("should set scope.shiftTemplates to response data when shiftTemplateService.getForSite is called.", function() {
        // Arrange
        var expected = [
            {
                Id: 1,
                StartTime: "06:00",
                EndTime: "06:00",
                ResourceId: 1,
                ShiftRate: null
            },
            {
                Id: 2,
                StartTime: "06:00",
                EndTime: "06:00",
                ResourceId: 2,
                ShiftRate: null
            }
        ];

        // Act
        scope.getShiftTemplates(1);

        // Assert
        expect(scope.shiftTemplates).toEqual(expected);
    });

    it("should set scope.filteredTemplates to only contain templates with the specified ResourceId.", function() {
        // Arrange
        var expected = [
            {
                Id: 1,
                StartTime: "06:00",
                EndTime: "06:00",
                ResourceId: 1,
                ShiftRate: null
            }
        ];

        // Act
        scope.getShiftTemplates(1);

        // Assert
        expect(scope.filteredTemplates).toEqual(expected);
    });

    it("should set scope.selectedTemplate to template with Id of passed scope.selectedShiftTemplateId.", function() {
        // Arrange
        var expected = {
            Id: 1,
            StartTime: "06:00",
            EndTime: "06:00",
            ResourceId: 1,
            ShiftRate: null
        };
        scope.selectedShiftTemplateId = 1;

        // Act
        scope.getShiftTemplates(1);

        // Assert
        expect(scope.selectedTemplate).toEqual(expected);
    });

    it("should call blockUI.stop function on reject response from shiftTemplateService.getForSite.", function() {
        // Arrange
        spyOn(blockUI, "stop");

        // Act
        scope.getShiftTemplates();
        scope.$apply();

        // Assert
        expect(blockUI.stop).toHaveBeenCalled();
    });

    it("should not call blockUI.stop function on success response from shiftTemplateService.getForSite.", function () {
        // Arrange
        spyOn(blockUI, "stop");

        // Act
        scope.getShiftTemplates(1);
        scope.$apply();

        // Assert
        expect(blockUI.stop).not.toHaveBeenCalled();
    });

    it("should call scope.error function on reject response from shiftTemplateService.getForSite.", function () {
        // Arrange
        spyOn(scope, "error");

        // Act
        scope.getShiftTemplates();
        scope.$apply();

        // Assert
        expect(scope.error).toHaveBeenCalled();
    });

    it("should not call scope.error function on success response from shiftTemplateService.getForSite.", function () {
        // Arrange
        spyOn(scope, "error");

        // Act
        scope.getShiftTemplates(1);
        scope.$apply();

        // Assert
        expect(scope.error).not.toHaveBeenCalled();
    });

    it("should not call blockUI.stop function on success response from resourceService.getAll.", function () {
        // Arrange
        spyOn(blockUI, "stop");

        // Act
        scope.getResources();
        scope.$apply();

        // Assert
        expect(blockUI.stop).not.toHaveBeenCalled();
    });

    it("should not call scope.error function on success response from resourceService.getAll.", function () {
        // Arrange
        spyOn(scope, "error");

        // Act
        scope.getResources();
        scope.$apply();

        // Assert
        expect(scope.error).not.toHaveBeenCalled();
    });

    it("should call blockUI.stop function on reject response from employeeService.getByCompanyId.", function () {
        // Arrange
        spyOn(blockUI, "stop");
        managerAreaService.companyStore.Id = null;

        // Act
        scope.getEmployees();
        scope.$apply();

        // Assert
        expect(blockUI.stop).toHaveBeenCalled();
    });

    it("should not call blockUI.stop function on success response from employeeService.getByCompanyId.", function () {
        // Arrange
        spyOn(blockUI, "stop");

        // Act
        scope.getEmployees();
        scope.$apply();

        // Assert
        expect(blockUI.stop).not.toHaveBeenCalled();
    });

    it("should call scope.error function on reject response from employeeService.getByCompanyId.", function () {
        // Arrange
        spyOn(scope, "error");
        managerAreaService.companyStore.Id = null;

        // Act
        scope.getEmployees();
        scope.$apply();

        // Assert
        expect(scope.error).toHaveBeenCalled();
    });

    it("should not call scope.error function on success response from employeeService.getByCompanyId.", function () {
        // Arrange
        spyOn(scope, "error");

        // Act
        scope.getEmployees();
        scope.$apply();

        // Assert
        expect(scope.error).not.toHaveBeenCalled();
    });

    it("should call blockUI.stop function on reject response from calResRqService.getForPeriod.", function () {
        // Arrange
        spyOn(blockUI, "stop");
        $stateParams.periodId = null;

        // Act
        scope.getCalPeriodResRqs();
        scope.$apply();

        // Assert
        expect(blockUI.stop).toHaveBeenCalled();
    });

    it("should not call blockUI.stop function on success response from calResRqService.getForPeriod.", function () {
        // Arrange
        spyOn(blockUI, "stop");

        // Act
        scope.getCalPeriodResRqs();
        scope.$apply();

        // Assert
        expect(blockUI.stop).not.toHaveBeenCalled();
    });

    it("should call scope.error function on reject response from calResRqService.getForPeriod.", function () {
        // Arrange
        spyOn(scope, "error");
        $stateParams.periodId = null;

        // Act
        scope.getCalPeriodResRqs();
        scope.$apply();

        // Assert
        expect(scope.error).toHaveBeenCalled();
    });

    it("should not call scope.error function on success response from from calResRqService.getForPeriod.", function () {
        // Arrange
        spyOn(scope, "error");

        // Act
        scope.getCalPeriodResRqs();
        scope.$apply();

        // Assert
        expect(scope.error).not.toHaveBeenCalled();
    });

    it("should call blockUI.stop function on reject response from shiftPatternService.getForCalendarPeriodResRq.", function () {
        // Arrange
        spyOn(blockUI, "stop");
        scope.calPeriodResRqs = [
            { Id: null }
        ];

        // Act
        scope.getShiftPatterns();
        scope.$apply();

        // Assert
        expect(blockUI.stop).toHaveBeenCalled();
    });

    it("should not call blockUI.stop function on success response from shiftPatternService.getForCalendarPeriodResRq.", function () {
        // Arrange
        spyOn(blockUI, "stop");

        // Act
        scope.getShiftPatterns();
        scope.$apply();

        // Assert
        expect(blockUI.stop).not.toHaveBeenCalled();
    });

    it("should call scope.error function on reject response from shiftPatternService.getForCalendarPeriodResRq.", function () {
        // Arrange
        spyOn(scope, "error");
        scope.calPeriodResRqs = [
            { Id: null }
        ];

        // Act
        scope.getShiftPatterns();
        scope.$apply();

        // Assert
        expect(scope.error).toHaveBeenCalled();
    });

    it("should not call scope.error function on success response from from shiftPatternService.getForCalendarPeriodResRq.", function () {
        // Arrange
        spyOn(scope, "error");

        // Act
        scope.getShiftPatterns();
        scope.$apply();

        // Assert
        expect(scope.error).not.toHaveBeenCalled();
    });

    it("should call blockUI.stop function on reject response from shiftPersonnelService.getForShift.", function () {
        // Arrange
        spyOn(blockUI, "stop");
        var shiftPattern = {
            Id: 1,
            ShiftModels: [
                { Id: null }
            ]
        };

        // Act
        scope.mapShifts(shiftPattern);
        scope.$apply();

        // Assert
        expect(blockUI.stop).toHaveBeenCalled();
    });

    it("should not call blockUI.stop function on success response from shiftPersonnelService.getForShift.", function () {
        // Arrange
        spyOn(blockUI, "stop");

        scope.resources = [
            { Id: 1, Name: "RMN" },
            { Id: 2, Name: "HCA" }
        ];

        var shiftPattern = {
            Id: 1,
            ShiftModels: [
                {
                    Id: 1,
                    ShiftTemplateModel: {
                        StartTime: "",
                        EndTime: ""
                    }
                }
            ]
        };

        // Act
        scope.mapShifts(shiftPattern, 1);
        scope.$apply();

        // Assert
        expect(blockUI.stop).not.toHaveBeenCalled();
    });

    it("should call scope.error function on reject response from shiftPersonnelService.getForShift.", function () {
        // Arrange
        spyOn(scope, "error");
        var shiftPattern = {
            Id: 1,
            ShiftModels: [
                { Id: null }
            ]
        };

        // Act
        scope.mapShifts(shiftPattern);
        scope.$apply();

        // Assert
        expect(scope.error).toHaveBeenCalled();
    });

    it("should not call scope.error function on success response from from shiftPersonnelService.getForShift.", function () {
        // Arrange
        spyOn(scope, "error");
        scope.resources = [
            { Id: 1, Name: "RMN" },
            { Id: 2, Name: "HCA" }
        ];

        var shiftPattern = {
            Id: 1,
            ShiftModels: [
                {
                    Id: 1,
                    ShiftTemplateModel: {
                        StartTime: "",
                        EndTime: ""
                    }
                }
            ]
        };

        // Act
        scope.mapShifts(shiftPattern, 1);
        scope.$apply();

        // Assert
        expect(scope.error).not.toHaveBeenCalled();
    });

    it("should swap out scope.filteredTemplates when scope.resourceChange is called.", function() {
        // Arrange
        scope.filteredTemplates = [
            {
                Id: 3,
                Name: "RMN Morning",
                StartTime: "06:00",
                EndTime: "14:30",
                ResourceId: 1
            },
            {
                Id: 4,
                Name: "RMN Day",
                StartTime: "14:00",
                EndTime: "22:30",
                ResourceId: 1
            }
        ];

        var expected = [
            {
                Id: 1,
                Name: "HCA Morning",
                StartTime: "06:00",
                EndTime: "14:30",
                ResourceId: 2
            },
            {
                Id: 1,
                Name: "HCA Day",
                StartTime: "14:00",
                EndTime: "22:30",
                ResourceId: 2
            }
        ];

        scope.resources = [
            { Id: 1, Name: "RMN" },
            { Id: 2, Name: "HCA" }
        ];

        scope.selectedResourceId = 2;
        scope.resourceRequestModel = {
            resource: [],
            shiftTemplate: {}
        };

        scope.shiftTemplates = [
            {
                Id: 1,
                Name: "HCA Morning",
                StartTime: "06:00",
                EndTime: "14:30",
                ResourceId: 2
            },
            {
                Id: 1,
                Name: "HCA Day",
                StartTime: "14:00",
                EndTime: "22:30",
                ResourceId: 2
            }
        ];


        // Act
        scope.resourceChange();

        // Assert
        expect(scope.filteredTemplates).toEqual(expected);
    });

    it("should swap out scope.selectedTemplate when scope.resourceChange is called.", function() {
        // Arrange
        scope.filteredTemplates = [
            {
                Id: 3,
                Name: "RMN Morning",
                StartTime: "06:00",
                EndTime: "14:30",
                ResourceId: 1
            },
            {
                Id: 4,
                Name: "RMN Day",
                StartTime: "14:00",
                EndTime: "22:30",
                ResourceId: 1
            }
        ];

        var expected = {
            Id: 1,
            Name: "HCA Morning",
            StartTime: "06:00",
            EndTime: "14:30",
            ResourceId: 2
        };

        scope.selectedTemplate = {
            Id: 4,
            Name: "RMN Day",
            StartTime: "14:00",
            EndTime: "22:30",
            ResourceId: 1
        };

        scope.resources = [
            { Id: 1, Name: "RMN" },
            { Id: 2, Name: "HCA" }
        ];

        scope.selectedResourceId = 2;
        scope.resourceRequestModel = {
            resource: [],
            shiftTemplate: {}
        };

        scope.shiftTemplates = [
            {
                Id: 1,
                Name: "HCA Morning",
                StartTime: "06:00",
                EndTime: "14:30",
                ResourceId: 2
            },
            {
                Id: 1,
                Name: "HCA Day",
                StartTime: "14:00",
                EndTime: "22:30",
                ResourceId: 2
            }
        ];


        // Act
        scope.resourceChange();

        // Assert
        expect(scope.selectedTemplate).toEqual(expected);
    });

    it("should swap out scope.resourceFilteredEmployees when scope.resourceChange is called.", function() {
        // Arrange
        scope.filteredTemplates = [{Id: 3, Name: "RMN Morning", StartTime: "06:00", EndTime: "14:30", ResourceId: 1}];

        scope.selectedTemplate = {Id: 4, Name: "RMN Day", StartTime: "14:00", EndTime: "22:30", ResourceId: 1};

        scope.shiftTemplates = [{Id: 1, Name: "HCA Morning", StartTime: "06:00", EndTime: "14:30", ResourceId: 2}];

        scope.resources = [
            { Id: 1, Name: "RMN" },
            { Id: 2, Name: "HCA" }
        ];

        scope.selectedResourceId = 2;
        scope.resourceRequestModel = {
            resource: [],
            shiftTemplate: {}
        };

        scope.siteFilteredEmployees = [
            {
                Id: 1,
                Name: "John Smith",
                UserModel: { Firstname: "", Lastname: ""},
                ContractModels: [{ Id: 2, ResourceId: 1 }]
            },
            {
                Id: 2,
                Name: "Bill Jones",
                UserModel: { Firstname: "", Lastname: ""},
                ContractModels: [{ Id: 1, ResourceId: 2 }]
            }
        ];

        var expected = [
            {
                Id: 2,
                Name: "Bill Jones",
                UserModel: {
                    Firstname: "",
                    Lastname: ""
                },
                ContractModels: [
                    {
                        Id: 1,
                        ResourceId: 2
                    }
                ]
            }
        ];

        // Act
        scope.resourceChange();

        // Assert
        expect(scope.resourceFilteredEmployees).toEqual(expected);
    });

    it("should swap out scope.selectedTemplate when scope.templateChange is called.", function() {
        // Arrange
        scope.selectedShiftTemplateId = 1;
        scope.filteredTemplates = [
            {
                Id: 1, Name: "HCA Morning", StartTime: "06:00", EndTime: "14:30", ResourceId: 2
            },
            {
                Id: 2, Name: "HCA Day", StartTime: "14:00", EndTime: "22:30", ResourceId: 2
            }
        ];

        var expected = {
            Id: 1, Name: "HCA Morning", StartTime: "06:00", EndTime: "14:30", ResourceId: 2
        };


        // Act
        scope.templateChange();

        // Assert
        expect(scope.selectedTemplate).toEqual(expected);
    });
});