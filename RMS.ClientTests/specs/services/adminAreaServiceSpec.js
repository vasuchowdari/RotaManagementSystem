/// <reference path="~/dependencies/jasmine.js" />
/// <reference path="~/dependencies/angular.min.js" />
/// <reference path="~/dependencies/angular-mocks.js" />
/// <reference path="~/dependencies/lodash.js" />
/// <reference path="~/dependencies/sinon.js" />

/// <reference path="../../../RMS.WebAPI/public/js/app.js" />
/// <reference path="../../../RMS.WebAPI/public/js/services.js" />
/// <reference path="../../../RMS.WebAPI/public/js/services/admin-area/adminAreaService.js" />

describe("Service: Admin Area Service", function() {
    var mRootScope;
    var adminAreaServiceObj;

    beforeEach(function() {
        module("services");
    });

    beforeEach(inject(function ($rootScope, adminAreaService) {
        mRootScope = $rootScope;
        adminAreaServiceObj = adminAreaService;
    }));

    it("should return an empty array object.", function() {
        // Arrange
        var expected = [];

        // Act
        var result = adminAreaServiceObj.returnEmptyArray();

        // Assert
        expect(result).toEqual(expected);
    });

    it("should return an area tab object when adminAreaService.setupAreaTabs is called.", function() {
        // Arrange
        var expected = [
            {
                heading: "Details",
                route: "admin.users.details"
            },
            {
                heading: "Contract",
                route: "admin.users.contract"
            },
            {
                heading: "Access",
                route: "admin.users.access"
            },
            {
                heading: "Calendar",
                route: "admin.users.leave"
            }
        ];

        // Act
        var result = adminAreaServiceObj.setupAreaTabs();

        // Assert
        expect(result).toEqual(expected);
    });

    it("should return an initialised Master View Model when adminAreaService.masterViewModel", function() {
        // Arrange
        var expected = {
            userModel: {
                Id: null,
                IsActive: true,
                Login: null,
                Password: null,
                Email: null,
                Firstname: null,
                Lastname: null,
                SystemAccessRoleId: 3,
                IsAccountLocked: false
            },
            employeeModel: {
                Id: null,
                IsActive: true,
                Address: null,
                City: null,
                Postcode: null,
                Telephone: null,
                Mobile: null,
                GenderId: 0,
                CompanyId: 1,
                EmployeeTypeId: 1,
                UserId: null
            },
            agencyWorkerModel: {
                Id: null,
                IsActive: true,
                Address: null,
                City: null,
                Postcode: null,
                Telephone: null,
                Mobile: null,
                GenderId: 0,
                AgencyId: 1,
                UserId: null
            },
            contractModel: {
                Id: null,
                IsActive: true,
                ResourceId: 1,
                EmployeeId: null,
                AgencyWorkerId: null,
                WeeklyHours: null,
                BaseRateOverride: null,
                OvertimeModifierOverride: null
            },
            siteAccessModels: []
        };

        // Act
        

        // Assert
        expect(adminAreaServiceObj.masterViewModel).toEqual(expected);
    });
});