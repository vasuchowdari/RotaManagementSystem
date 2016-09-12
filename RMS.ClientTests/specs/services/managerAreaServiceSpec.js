/// <reference path="~/dependencies/jasmine.js" />
/// <reference path="~/dependencies/angular.min.js" />
/// <reference path="~/dependencies/angular-mocks.js" />
/// <reference path="~/dependencies/lodash.js" />
/// <reference path="~/dependencies/sinon.js" />
/// <reference path="~/dependencies/angular-local-storage.min.js" />

/// <reference path="../../../RMS.WebAPI/public/js/app.js" />
/// <reference path="../../../RMS.WebAPI/public/js/services.js" />
/// <reference path="../../../RMS.WebAPI/public/js/services/manager-area/managerAreaService.js" />

describe("Service: Manager Area Service", function() {
    var mRootScope, mLocalStorageService;
    var managerAreaServiceObj;

    beforeEach(function() {
        module("LocalStorageModule");
        module("services");
    });

    beforeEach(inject(function ($rootScope, managerAreaService, localStorageService) {
        mRootScope = $rootScope;
        managerAreaServiceObj = managerAreaService;
        mLocalStorageService = localStorageService;
    }));

    // _notify
    it("should call $rootScope.emit when managerAreaService.notify is called.", function() {
        // Arrange
        spyOn(mRootScope, "$emit");

        // Act
        managerAreaServiceObj.notify(1, "test-event");

        // Assert
        expect(mRootScope.$emit).toHaveBeenCalled();
        expect(mRootScope.$emit).toHaveBeenCalledWith("test-event", 1);
    });

    // _subscribe
    it("should call scope.$on when managerAreaService.subscribe is called.", function() {
        // Arrange
        spyOn(mRootScope, "$on");

        // Act
        managerAreaServiceObj.subscribe(mRootScope, function() {

        }, "test-event");

        // Assert
        expect(mRootScope.$on).toHaveBeenCalled();
    });


    // _companyStore
    it("should reutrn the companyStore object when managerAreaService.companyStore is referenced.", function() {
        // Arrange
        var expected = {};

        // Act
        var result = managerAreaServiceObj.companyStore;

        // Assert
        expect(result).toEqual(expected);
    });

    it("should persist information passed to the companyStore.", function() {
        // Arrange
        var expected = { name: "TestName" };
        var companyResponseData = { name: "TestName" };

        // Act
        managerAreaServiceObj.companyStore = companyResponseData;
        var companyStore = managerAreaServiceObj.companyStore;


        // Assert
        expect(companyStore).toEqual(expected);
    });


    // _managerStore
    it("should return the managerStore object when managerAreaService.managerStore is referenced.", function() {
        // Arrange
        var expected = {};

        // Act
        var result = managerAreaServiceObj.managerStore;

        // Assert
        expect(result).toEqual(expected);
    });

    it("should persist information passed to the managerStore", function() {
        // Arrange
        var expected = { employeeId: 1, companyId: 12 };
        var employeeResponseData = { employeeId: 1, companyId: 12 };

        // Act
        managerAreaServiceObj.managerStore = employeeResponseData;
        var managerStore = managerAreaServiceObj.managerStore;

        // Assert
        expect(managerStore).toEqual(expected);
    });


    // _sitesStore
    it("should return the sitesStore object collection when managerAreaService.sitesStore is referenced.", function() {
        // Arrange
        var expected = [];

        // Act
        var result = managerAreaServiceObj.sitesStore;

        // Assert
        expect(result).toEqual(expected);
    });

    it("should persist information passed to the sites store.", function() {
        // Arrange
        var expected = [
            { id: 1, name: "Test Site A" },
            { id: 2, name: "Test Site B" }
        ];

        var sitesResponseData = [
            { id: 1, name: "Test Site A" },
            { id: 2, name: "Test Site B" }
        ];

        // Act
        managerAreaServiceObj.sitesStore = sitesResponseData;
        var sitesStore = managerAreaServiceObj.sitesStore;

        // Assert
        expect(sitesStore).toEqual(expected);
    });


    // _siteStore
    it("should return the siteStore object when managerAreaService.siteStore is referenced.", function() {
        // Arrange
        var expected = {};

        // Act
        var result = managerAreaServiceObj.siteStore;

        // Assert
        expect(result).toEqual(expected);
    });

    it("should persist information passed to the site store.", function() {
        // Arrange
        var expected = { id: 1, name: "Test Site C" };
        var siteResponseData = { id: 1, name: "Test Site C" };

        // Act
        managerAreaServiceObj.sitesStore = siteResponseData;
        var siteStore = managerAreaServiceObj.sitesStore;

        // Assert
        expect(siteStore).toEqual(expected);
    });


    // _accessFilteredSiteStore
    it("should return the accessFilteredSiteStore object when managerAreaService.accessFilterSiteStore is referenced.", function() {
        // Arrange
        var expected = [];

        // Act 
        var result = managerAreaServiceObj.accessFilterSiteStore;

        // Assert
        expect(result).toEqual(expected);
    });

    it("should persist information passed to the access filter site store.", function() {
        // Arrange
        var expected = [
            { id: 1, name: "Test Site D" },
            { id: 2, name: "Test Site E" }
        ];

        var siteResponseData = [
            { id: 1, name: "Test Site D" },
            { id: 2, name: "Test Site E" }
        ];


        // Act
        managerAreaServiceObj.accessFilterSiteStore = siteResponseData;
        var filteredSiteStore = managerAreaServiceObj.accessFilterSiteStore;

        // Assert
        expect(filteredSiteStore).toEqual(expected);
    });


    // _accessRightsStore
    it("should return the accessRightsStore object when managerAreaService.accessRightsStore is referenced.", function () {
        // Arrange
        var expected = [];

        // Act 
        var result = managerAreaServiceObj.accessRightsStore;

        // Assert
        expect(result).toEqual(expected);
    });

    it("should persist information passed to the access rights store.", function () {
        // Arrange
        var expected = [
            { id: 1, name: "Test Site D" },
            { id: 2, name: "Test Site E" }
        ];

        var siteResponseData = [
            { id: 1, name: "Test Site D" },
            { id: 2, name: "Test Site E" }
        ];


        // Act
        managerAreaServiceObj.accessRightsStore = siteResponseData;
        var accessRightsStore = managerAreaServiceObj.accessRightsStore;

        // Assert
        expect(accessRightsStore).toEqual(expected);
    });


    // _siteCalendarsStore
    it("should return the siteCalendarsStore object when managerAreaService.siteCalendarsStore is referenced.", function() {
        // Arrange
        var expected = [];

        // Act
        var result = managerAreaServiceObj.siteCalendarsStore;

        // Assert
        expect(result).toEqual(expected);
    });

    it("should persist information passed to the site calendars store.", function() {
        // Arrange
        var expected = [
            { id: 1, name: "Test Calendar A" },
            { id: 2, name: "Test Calendar B" }
        ];

        var calendarResponseData = [
            { id: 1, name: "Test Calendar A" },
            { id: 2, name: "Test Calendar B" }
        ];

        // Act
        managerAreaServiceObj.siteCalendarsStore = calendarResponseData;
        var siteCalendarsStore = managerAreaServiceObj.siteCalendarsStore;

        // Assert
        expect(siteCalendarsStore).toEqual(expected);
    });


    // _siteCalendarStore
    it("should return the siteCalendarStore object when managerAreaService.siteCalendarStore is referenced.", function () {
        // Arrange
        var expected = {};

        // Act
        var result = managerAreaServiceObj.siteCalendarStore;

        // Assert
        expect(result).toEqual(expected);
    });

    it("should persist information passed to the site calendar store.", function () {
        // Arrange
        var expected = { id: 1, name: "Test Calendar A" };

        var calendarResponseData = { id: 1, name: "Test Calendar A" };

        // Act
        managerAreaServiceObj.siteCalendarStore = calendarResponseData;
        var siteCalendarStore = managerAreaServiceObj.siteCalendarStore;

        // Assert
        expect(siteCalendarStore).toEqual(expected);
    });


    // _sitePeriodsStore
    it("should return the sitePeriodsStore object when managerAreaService.sitePeriodsStore is referenced.", function () {
        // Arrange
        var expected = [];

        // Act
        var result = managerAreaServiceObj.sitePeriodsStore;

        // Assert
        expect(result).toEqual(expected);
    });

    it("should persist information passed to the site calendars store.", function () {
        // Arrange
        var expected = [
            { id: 1, name: "Test Period A" },
            { id: 2, name: "Test Period B" }
        ];

        var periodResponseData = [
            { id: 1, name: "Test Period A" },
            { id: 2, name: "Test Period B" }
        ];

        // Act
        managerAreaServiceObj.sitePeriodsStore = periodResponseData;
        var sitePeriodsStore = managerAreaServiceObj.sitePeriodsStore;

        // Assert
        expect(sitePeriodsStore).toEqual(expected);
    });


    // _sitePeriodStore
    it("should return the sitePeriodStore object when managerAreaService.sitePeriodStore is referenced.", function () {
        // Arrange
        var expected = {};

        // Act
        var result = managerAreaServiceObj.sitePeriodStore;

        // Assert
        expect(result).toEqual(expected);
    });

    it("should persist information passed to the site period store.", function () {
        // Arrange
        var expected = { id: 1, name: "Test Period A" };

        var periodResponseData = { id: 1, name: "Test Period A" };

        // Act
        managerAreaServiceObj.sitePeriodStore = periodResponseData;
        var sitePeriodStore = managerAreaServiceObj.sitePeriodStore;

        // Assert
        expect(sitePeriodStore).toEqual(expected);
    });


    // _siteEmployeesStore
    it("should return the siteEmployeesStore object when managerAreaService.siteEmployeesStore is referenced.", function () {
        // Arrange
        var expected = [];

        // Act
        var result = managerAreaServiceObj.siteEmployeesStore;

        // Assert
        expect(result).toEqual(expected);
    });

    it("should persist information passed to the site employees store.", function () {
        // Arrange
        var expected = [
            { id: 1, name: "Joe Bloggs" },
            { id: 2, name: "John Smith" },
            { id: 3, name: "Jane Doe" },
            { id: 4, name: "Hannah Barbera" }
        ];

        var employeesResponseData = [
            { id: 1, name: "Joe Bloggs" },
            { id: 2, name: "John Smith" },
            { id: 3, name: "Jane Doe" },
            { id: 4, name: "Hannah Barbera" }
        ];

        // Act
        managerAreaServiceObj.siteEmployeesStore = employeesResponseData;
        var siteEmployeesStore = managerAreaServiceObj.siteEmployeesStore;

        // Assert
        expect(siteEmployeesStore).toEqual(expected);
    });


    // _getPeriodsStore
    it("should reutrn a DayPilot config object when managerAreaService.schedulerWeekScaleConfig is called.", function() {
        // Arrange
        var expected = {
            scale: "Day",
            days: 7,
            startDate: "2016-02-08",
            cellWidth: 191,
            eventHeight: 50,
            allowEventOverlap: true,
            eventStackingLineHeight: 25,
            useEventBoxes: "Never",
            timeHeaders: [
                { groupBy: "Month", format: "MMMM yyyy" },
                { groupBy: "Day", format: "d" }
            ],
            resources: undefined
        };

        // Act
        var result = managerAreaServiceObj.schedulerWeekScaleConfig();

        // Assert
        expect(result).toEqual(expected);
    });


    // _returnNewResourceRequirementModel
    it("should return a new ResourceRequirementModel object when referenced.", function() {
        // Arrange
        var expected = {
            employeeId: null,
            resource: {},
            shiftTemplate: [],
            shift: {}
        };

        // Act
        var result = managerAreaServiceObj.returnNewResourceRequirementModel();

        // Assert
        expect(result).toEqual(expected);
    });
});