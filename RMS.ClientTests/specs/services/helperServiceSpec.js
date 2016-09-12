/// <reference path="~/dependencies/jasmine.js" />
/// <reference path="~/dependencies/angular.min.js" />
/// <reference path="~/dependencies/angular-mocks.js" />
/// <reference path="~/dependencies/angular-ui-router.js" />
/// <reference path="~/dependencies/angular-local-storage.min.js" />
/// <reference path="~/dependencies/angular-block-ui.js" />
/// <reference path="~/dependencies/toaster.min.js" />
/// <reference path="~/dependencies/lodash.js" />
/// <reference path="~/dependencies/sinon.js" />
/// <reference path="../../../RMS.WebAPI/public/js/app.js" />
/// <reference path="../../../RMS.WebAPI/public/js/services.js" />
/// <reference path="../../../RMS.WebAPI/public/js/services/authService.js" />
/// <reference path="../../../RMS.WebAPI/public/js/services/helperService.js" />
describe("Service: Helper Service", function() {
    var mState;
    var mAuthService, mLocalStorageService;
    var helperServiceObj;
    var mBlockUI, mToaster;

    beforeEach(function() {

        module(function($provide) {
            $provide.service("authService", function() {
                this.authentication = jasmine.createSpy("authentication");
            });
        });

        module("ui.router");
        module("blockUI");
        module("toaster");
        module("LocalStorageModule");
        module("services");
    });

    beforeEach(inject(function($state, localStorageService, authService, blockUI, toaster, helperService) {
        mState = $state;
        mLocalStorageService = localStorageService;
        mAuthService = authService;
        mBlockUI = blockUI;
        mToaster = toaster;
        helperServiceObj = helperService;
    }));

    // _auth
    it("should set the scope.authentication object when helperService.auth is called.", function() {
        // Arrange
        var expected = true;
        var scope = {
            authentication: {}
        };

        mAuthService.authentication = {
            isAuth: true
        }; // Act
        helperServiceObj.auth(scope);

        // Assert
        expect(scope.authentication).toBeDefined();
        expect(scope.authentication.isAuth).toBe(expected);
    });

    it("should set the scope.authentication object to a new instance when helperService.auth is called and the Auth Service object is not set.", function() {
        // Arrange
        var expected = false;
        var scope = {
            authentication: {}
        };

        // Act
        helperServiceObj.auth(scope);

        // Assert
        expect(scope.authentication).toBeDefined();
        expect(scope.authentication.isAuth).toBe(expected);
    });


    // _blockAndAuth
    it("should start the blockUI service when helperService.blockAndAuth is called.", function() {
        // Arrange
        var scope = {};
        spyOn(mBlockUI, "start");

        // Act
        helperServiceObj.blockAndAuth(scope, mBlockUI);

        // Assert
        expect(mBlockUI.start).toHaveBeenCalled();
    });

    it("should set the scope.authentication object to a new instance when helperService.blockAndAuth is called and the Auth Service object is not set.", function() {
        // Arrange
        var expected = false;
        var scope = {
            authentication: {}
        };

        // Act
        helperServiceObj.blockAndAuth(scope, mBlockUI);

        // Assert
        expect(scope.authentication).toBeDefined();
        expect(scope.authentication.isAuth).toBe(expected);
    });

    it("should set the scope.authentication object when helperService.blockAndAuth is called.", function() {
        // Arrange
        var expected = true;
        var scope = {
            authentication: {}
        };

        mAuthService.authentication = {
            isAuth: true
        }; // Act
        helperServiceObj.blockAndAuth(scope, mBlockUI);

        // Assert
        expect(scope.authentication).toBeDefined();
        expect(scope.authentication.isAuth).toBe(expected);
    });


    //  _uncheckAll
    it("should set the IsChecked property of passed scope objects to false.", function() {
        // Arrange
        var expected = false;
        var scope = {};
        var checkableObjects = [];

        for (var i = 0; i < 10; i++) {
            var obj = {
                IsChecked: false
            };

            checkableObjects.push(obj);
        }

        scope.checkableObjects = checkableObjects;

        // Act
        helperServiceObj.uncheckAll(scope.checkableObjects);

        // Assert
        _.forEach(scope.checkableObjects, function(object) {
            expect(object.IsChecked).toBe(expected);
        });
    });


    // _checkAll
    it("should set the IsChecked property of passed scope objects to true.", function() {
        // Arrange
        var expected = true;
        var scope = {};
        var checkableObjects = [];

        for (var i = 0; i < 10; i++) {
            var obj = {
                IsChecked: false
            };

            checkableObjects.push(obj);
        }

        scope.checkableObjects = checkableObjects;

        // Act
        helperServiceObj.checkAll(scope.checkableObjects);

        // Assert
        _.forEach(scope.checkableObjects, function(object) {
            expect(object.IsChecked).toBe(expected);
        });
    });


    // _makeCheckable
    it("should add and set the IsChecked property to passed scope objects.", function() {
        // Arrange
        var expected = false;
        var scope = {};
        scope.checkableObjs = [];

        for (var i = 0; i < 9; i++) {
            var obj = {};
            scope.checkableObjs.push(obj);
        }

        // Act
        helperServiceObj.makeCheckable(scope.checkableObjs);

        // Assert
        _.forEach(scope.checkableObjs, function(checkableObj) {
            expect(checkableObj.IsChecked).toBeDefined();
            expect(checkableObj.IsChecked).toBe(expected);
        });
    });


    // _notAuth
    it("should set the state when helperService.notAuth is called.", function() {
        // Arrange
        var expected = "login";
        spyOn(mState, "go");

        // Act
        helperServiceObj.notAuth();

        // Assert
        expect(mState.go).toHaveBeenCalledWith(expected);
    });

    it("should stop the blockUI service when helperService.notAuth is called.", function() {
        // Arrange
        spyOn(mBlockUI, "stop");
        spyOn(mState, "go");

        // Act
        helperServiceObj.notAuth();

        // Assert
        expect(mBlockUI.stop).toHaveBeenCalled();
    });


    // _setDashState
    it("should return the state string for Admin system access role when helperService.setDashState is called.", function() {
        // Arrange
        var expected = "admin.dash";
        mAuthService.authentication.sysAccess = "Admin";

        // Act
        var result = helperServiceObj.setDashState();

        // Assert
        expect(result).toBe(expected);
    });

    it("should return the state string for Manager system access role when helperService.setDashState is called.", function() {
        // Arrange
        var expected = "manager.dash";
        mAuthService.authentication.sysAccess = "Manager";

        // Act
        var result = helperServiceObj.setDashState();

        // Assert
        expect(result).toBe(expected);
    });

    it("should return the state string for User system access role when helperService.setDashState is called.", function() {
        // Arrange
        var expected = "user.dash";
        mAuthService.authentication.sysAccess = "User";

        // Act
        var result = helperServiceObj.setDashState();

        // Assert
        expect(result).toBe(expected);
    });


    // _setLoginState
    it("should return the state string for Admin system access role when helperService.setLoginState is called.", function() {
        // Arrange
        var expected = "admin";
        mAuthService.authentication.sysAccess = "Admin";

        // Act
        var result = helperServiceObj.setLoginState();

        // Assert
        expect(result).toBe(expected);
    });

    it("should return the state string for Manager system access role when helperService.setLoginState is called.", function() {
        // Arrange
        var expected = "manager";
        mAuthService.authentication.sysAccess = "Manager";

        // Act
        var result = helperServiceObj.setLoginState();

        // Assert
        expect(result).toBe(expected);
    });

    it("should return the state string for User system access role when helperService.setLoginState is called.", function() {
        // Arrange
        var expected = "user";
        mAuthService.authentication.sysAccess = "User";

        // Act
        var result = helperServiceObj.setLoginState();

        // Assert
        expect(result).toBe(expected);
    });


    // _statusHandler
    it("should stop the blockUI service when the response status is 200.", function() {
        // Arrange
        var status = 200;
        var scope = {
            success: function() {}
        };

        spyOn(mBlockUI, "stop");

        // Act
        helperServiceObj.statusHandler(scope, status);

        // Assert
        expect(mBlockUI.stop).toHaveBeenCalled();
    });

    it("should call scope.success when response status is 200.", function() {
        // Arrange
        var status = 200;
        var scope = {
            success: function() {}
        };

        spyOn(scope, "success");

        // Act
        helperServiceObj.statusHandler(scope, status);

        // Assert
        expect(scope.success).toHaveBeenCalled();
    });

    it("should stop the blockUI service when the response status is not 200.", function() {
        // Arrange
        var status = 404;
        var scope = {
            error: function() {}
        };

        spyOn(mBlockUI, "stop");

        // Act
        helperServiceObj.statusHandler(scope, status);

        // Assert
        expect(mBlockUI.stop).toHaveBeenCalled();
    });

    it("should call scope.error when response status is not 200.", function() {
        // Arrange
        var status = 400;
        var scope = {
            error: function() {}
        };

        spyOn(scope, "error");

        // Act
        helperServiceObj.statusHandler(scope, status);

        // Assert
        expect(scope.error).toHaveBeenCalled();
    });


    // _toasterError
    it("should call the toaster.pop function with type error when helperService.toasterError is called.", function() {
        // Arrange
        var expectedPopType = "error";
        var expectedPopHeader = "Error";
        var expectedPopMessage = "Your action was unsuccessful";
        spyOn(mToaster, "pop");

        // Act
        helperServiceObj.toasterError();

        // Assert
        expect(mToaster.pop).toHaveBeenCalled();
        expect(mToaster.pop).toHaveBeenCalledWith(expectedPopType, expectedPopHeader, expectedPopMessage);
    });

    it("should call the toaster.pop function with type error and passed header and message strings when helperService.toasterError is called.", function() {
        // Arrange
        var expectedPopType = "error";
        var expectedPopHeader = "ERROR!";
        var expectedPopMessage = "Hello World";
        var passedHeaderStr = "ERROR!";
        var passedMessageStr = "Hello World";

        spyOn(mToaster, "pop");

        // Act
        helperServiceObj.toasterError(passedHeaderStr, passedMessageStr);

        // Assert
        expect(mToaster.pop).toHaveBeenCalled();
        expect(mToaster.pop).toHaveBeenCalledWith(expectedPopType, expectedPopHeader, expectedPopMessage);
    });


    // _toasterInfo
    it("should call the toaster.pop function with type info when helperService.toasterInfo is called.", function() {
        // Arrange
        var expectedPopType = "info";
        var expectedPopHeader = "FYI";
        var expectedPopMessage = "This is important";
        spyOn(mToaster, "pop");

        // Act
        helperServiceObj.toasterInfo();

        // Assert
        expect(mToaster.pop).toHaveBeenCalled();
        expect(mToaster.pop).toHaveBeenCalledWith(expectedPopType, expectedPopHeader, expectedPopMessage);
    });

    it("should call the toaster.pop function with type info and passed header and message strings when helperService.toasterInfo is called.", function() {
        // Arrange
        var expectedPopType = "info";
        var expectedPopHeader = "OI OI!";
        var expectedPopMessage = "Check this";
        var passedHeaderStr = "OI OI!";
        var passedMessageStr = "Check this";

        spyOn(mToaster, "pop");

        // Act
        helperServiceObj.toasterInfo(passedHeaderStr, passedMessageStr);

        // Assert
        expect(mToaster.pop).toHaveBeenCalled();
        expect(mToaster.pop).toHaveBeenCalledWith(expectedPopType, expectedPopHeader, expectedPopMessage);
    });


    // _toasterSuccess
    it("should call the toaster.pop function with type success when helperService.toasterSuccess is called.", function() {
        // Arrange
        spyOn(mToaster, "pop");

        // Act
        helperServiceObj.toasterSuccess();

        // Assert
        expect(mToaster.pop).toHaveBeenCalled();
    });


    // _toasterWarning
    it("should call the toaster.pop function with type warning when helperService.toasterWarning is called.", function() {
        // Arrange
        var expectedPopType = "warning";
        var expectedPopHeader = "Warning";
        var expectedPopMessage = "Careful now!";
        spyOn(mToaster, "pop");

        // Act
        helperServiceObj.toasterWarning();

        // Assert
        expect(mToaster.pop).toHaveBeenCalled();
        expect(mToaster.pop).toHaveBeenCalledWith(expectedPopType, expectedPopHeader, expectedPopMessage);
    });

    it("should call the toaster.pop function with type warning and passed header and message strings when helperService.toasterWarning is called.", function() {
        // Arrange
        var expectedPopType = "warning";
        var expectedPopHeader = "Look out!";
        var expectedPopMessage = "Check this";
        var passedHeaderStr = "Look out!";
        var passedMessageStr = "Check this";

        spyOn(mToaster, "pop");

        // Act
        helperServiceObj.toasterWarning(passedHeaderStr, passedMessageStr);

        // Assert
        expect(mToaster.pop).toHaveBeenCalled();
        expect(mToaster.pop).toHaveBeenCalledWith(expectedPopType, expectedPopHeader, expectedPopMessage);
    });


    // _toggleShowHide
    it("should return showHide as true when false.", function() {
        // Arrange
        var expected = true;
        var flag = false;

        // Act
        var result = helperServiceObj.toggleShowHide(flag);

        // Assert
        expect(result).toEqual(expected);
    });

    it("should return showHide as false when true.", function() {
        // Arrange
        var expected = false;
        var flag = true;

        // Act
        var result = helperServiceObj.toggleShowHide(flag);

        // Assert
        expect(result).toEqual(expected);
    });


    // _toggleButtonDisable
    it("should return buttonDisable as true when false", function() {
        // Arrange
        var expected = true;
        var flag = false;

        // Act
        var result = helperServiceObj.toggleButtonDisable(flag);

        // Assert
        expect(result).toEqual(expected);
    });

    it("should return buttonDisable as false when true", function() {
        // Arrange
        var expected = false;
        var flag = true;

        // Act
        var result = helperServiceObj.toggleButtonDisable(flag);

        // Assert
        expect(result).toEqual(expected);
    });


    // _shiftDateTimeFormatter
    it("should return a correctly formatted DateTime string from Shift dates and ShiftTemplate times.", function() {
        // Arrange
        var shiftStartDate = "2016-02-06T00:00:00";
        var shiftTemplateStartTime = "2016-01-01T06:00:00";

        var expectedStartDateTime = "2016-02-06T06:00:00";

        // Act
        var result = helperServiceObj.shiftDateTimeFormatter(shiftStartDate, shiftTemplateStartTime);

        // Assert
        expect(result).toEqual(expectedStartDateTime);
    });


    // _shiftTimeFormatter
    it("should return a correctly formatted Time string from Shift.StartTime.", function() {
        // Arrange
        var expected = "15:30";
        var dateTimeStr = "2016-02-28T15:30:00";


        // Act
        var result = helperServiceObj.shiftTimeFormatter(dateTimeStr);

        // Assert
        expect(result).toEqual(expected);
    });


    // _rateFormatter
    it("should return a correctly formatted Rate string when passed a one digit decimal to 1 dp.", function() {
        // Arrange
        var expected = "9.50";
        var rateStr = 9.5;

        // Act
        var result = helperServiceObj.rateFormatter(rateStr);

        // Assert
        expect(result).toEqual(expected);
    });

    it("should return a correctly formatted Rate string when passed a one digit integer", function() {
        // Arrange
        var expected = "8.00";
        var rateStr = 8;

        // Act
        var result = helperServiceObj.rateFormatter(rateStr);

        // Assert
        expect(result).toEqual(expected);
    });

    it("should return a correctly formatted Rate string when passed a two digit decimal to 1 dp.", function () {
        // Arrange
        var expected = "10.50";
        var rateStr = 10.5;

        // Act
        var result = helperServiceObj.rateFormatter(rateStr);

        // Assert
        expect(result).toEqual(expected);
    });

    it("should return a correctly formatted Rate string when passed a one digit integer", function () {
        // Arrange
        var expected = "10.00";
        var rateStr = 10;

        // Act
        var result = helperServiceObj.rateFormatter(rateStr);

        // Assert
        expect(result).toEqual(expected);
    });
});