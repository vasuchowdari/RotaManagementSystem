"use strict";

var app = angular.module("services");
app.factory("adminActionStatusService", [
    "helperService",
    "adminAreaService",
    function (helperService, adminAreaService) {
        var adminActionStatusServiceFactory = {};

        // Shift Templates
        var _handleShiftTemplateCreateError = function(errorMsg) {
            helperService.toasterError("Shift Template Create Error", errorMsg);
        };

        var _handleShiftTemplateCreateSuccess = function() {
            helperService.toasterSuccess("Create Success", "Shift Template was successfully created.");
        };

        var _handleShiftTemplateUpdateError = function(errorMsg) {
            helperService.toasterError("Shift Template Update Error", errorMsg);
        };

        var _handleShiftTemplateUpdateSuccess = function() {
            helperService.toasterSuccess("Update Success", "Shift Template was successfully updated.");
        };

        var _validateShiftTemplateSearchCriteriaForm = function(shiftTemplateSearchFormObj) {
            var toasterHeaderStr = "Shift Template Search";

            if (!shiftTemplateSearchFormObj.templateResource.$modelValue) {
                helperService.toasterWarning(toasterHeaderStr,
                    "You need to select a Role from the dropdown box.");
                return [false, 0];
            }

            if (!shiftTemplateSearchFormObj.templateSite.$modelValue) {
                helperService.toasterWarning(toasterHeaderStr,
                    "You need to select a Location from the dropdown box.");
                return [false, 1];
            }

            return [true];
        };

        var _validateShiftTemplateUpdateForm = function(shiftTemplateUpdateFormObj) {
            var toasterHeaderStr = "Shift Template Update";

            if (!shiftTemplateUpdateFormObj.templateName.$modelValue) {
                helperService.toasterWarning(toasterHeaderStr,
                    "You need to supply a Name for the Shift Template.");

                return [false, 0];
            }

            if (!shiftTemplateUpdateFormObj.templateResource.$modelValue) {
                helperService.toasterWarning(toasterHeaderStr,
                    "You need to select a Role from the dropdown box.");

                return [false, 1];
            }

            if (!shiftTemplateUpdateFormObj.templateShiftRate.$modelValue) {
                helperService.toasterWarning(toasterHeaderStr,
                    "You need to supply a Rate for the Shift Template.");

                return [false, 2];
            }

            if (!shiftTemplateUpdateFormObj.templateUnpaidBreakDuration.$modelValue) {
                helperService.toasterWarning(toasterHeaderStr,
                    "You need to supply an Unpaid Break Duration for the Shift Template.");

                return [false, 3];
            }

            return [true];
        };

        var _adminShiftTemplateSearchWarningEnum = {
            role: 0,
            location: 1
        };

        var _adminShiftTemplateWarningEnum = {
            name: 0,
            role: 1,
            shiftRate: 2,
            unpaidBreak: 3
        };


        // Leave Requests (and Training)
        var _handleLeaveRequestCreateError = function(errorMsg) {
            helperService.toasterError("Staff Calendar Update Error", errorMsg);
        };

        var _handleLeaveRequestCreateSuccess = function() {
            helperService.toasterSuccess("Create Success", "Staff Calendar was successfully updated.");
        };

        var _handleLeaveRequestDeleteError = function(errorMsg) {
            helperService.toasterError("Staff Calendar Update Error", errorMsg);
        };

        var _handleLeaveRequestDeleteSuccess = function() {
            helperService.toasterSuccess("Delete Success", "Staff Calendar was successfully updated.");
        };

        var _handleLeaveRequestUpdateError = function(errorMsg) {
            helperService.toasterError("Staff Calendar Update Error", errorMsg);
        };

        var _handleLeaveRequestUpdateSuccess = function() {
            helperService.toasterSuccess("Update Success", "Staff Calendar was successfully updated.");
        };

        var _validateLeaveRequestObjectForm = function(calendarLeaveRequestFormObj) {
            var toasterHeaderStr = "Staff Calendar Update";

            if (!calendarLeaveRequestFormObj.adminLeaveType.$modelValue) {
                helperService.toasterWarning(toasterHeaderStr,
                    "Please select a Leave Type.");
                return false;
            }

            if (moment(calendarLeaveRequestFormObj.endDateTime.$modelValue).isBefore(calendarLeaveRequestFormObj.startDateTime.$modelValue)) {
                helperService.toasterWarning(toasterHeaderStr,
                    "End Date / Time is before Start Date / Time.");
                return false;
            }

            if (calendarLeaveRequestFormObj.hours.$modelValue < 0) {
                helperService.toasterWarning(toasterHeaderStr,
                    "Please enter an Hours value equal to at least zero (0).");
                return false;
            }

            if (calendarLeaveRequestFormObj.minutes.$modelValue < 0) {
                helperService.toasterWarning(toasterHeaderStr,
                    "Please enter a Minutes value equal to at least zero (0).");
                return false;
            }

            return true;
        };


        // Resources (Roles)
        var _handleResourceCreateError = function(errorMsg) {
            helperService.toasterError("Create Error",
                "There was a problem creating the Role: " + errorMsg);
        };

        var _handleResourceCreateSuccess = function() {
            helperService.toasterSuccess("Create Success",
                "The Role was successfully created.");
        }

        var _handleResourceUpdateError = function(errorMsg) {
            helperService.toasterError("Update Error",
                    "There was a problem updating the Role: " + errorMsg);
        };

        var _handleResourceUpdateSuccess = function() {
            helperService.toasterSuccess("Update Success",
                "The Role was successfully updated.");
        }

        var _validateResourceObjectForm = function(resourceFormObj) {
            var toasterHeaderStr = "Role Update";

            if (!resourceFormObj.resourceName.$modelValue) {
                helperService.toasterWarning(toasterHeaderStr,
                    "You need to supply a Name for the Role.");
                return [false, 0];
            }

            if (!resourceFormObj.resourceBaseRate.$modelValue) {
                helperService.toasterWarning(toasterHeaderStr,
                    "You need to supply a Base Rate for the Role.");
                return [false, 1];
            }

            return [true];
        };

        var _adminRoleValidationEnumWarning = {
            name: 0,
            baseRate: 1
        };


        // System Config
        var _handleSystemConfigUpdateError = function(errorMsg) {
            helperService.toasterError("Update Error",
                    "There was a problem updating the System Config: " + errorMsg);
        };

        var _handleSystemConfigUdateSuccess = function() {
            helperService.toasterSuccess("Update Success",
                "The System Config was successfully updated.");
        };

        var _validateSystemConfigFormObject = function(udscFormObj) {
            var toasterHeaderStr = "System Config Update";

            if (!udscFormObj.sysConfigMinLookback.$modelValue) {
                helperService.toasterWarning(toasterHeaderStr,
                    "You need to supply a Minimum Look Back for the System Config.");
                return false;
            }

            if (!udscFormObj.sysConfigPostStartValidThreshold.$modelValue) {
                helperService.toasterWarning(toasterHeaderStr,
                    "You need to supply a Shift Start Threshold for the System Config.");
                return false;
            }

            if (!udscFormObj.sysConfigPreStartEarlyInThreshold.$modelValue) {
                helperService.toasterWarning(toasterHeaderStr,
                    "You need to supply an Early In Threshold for the System Config.");
                return false;
            }

            if (!udscFormObj.sysConfigPostEndValidThreshold.$modelValue) {
                helperService.toasterWarning(toasterHeaderStr,
                    "You need to supply a Late Out Threshold for the System Config.");
                return false;
            }

            if (!udscFormObj.sysConfigPayrollStartDayOfMonth.$modelValue) {
                helperService.toasterWarning(toasterHeaderStr,
                    "You need to supply a Payroll Start Day of Month for the System Config.");
                return false;
            }

            if (!udscFormObj.sysConfigNicFactor.$modelValue) {
                helperService.toasterWarning(toasterHeaderStr,
                    "You need to supply an NIC Factor for the System Config.");
                return false;
            }

            if (!udscFormObj.sysConfigAccruedHolidayFactor.$modelValue) {
                helperService.toasterWarning(toasterHeaderStr,
                    "You need to supply an Accrued Holiday Factor for the System Config.");
                return false;
            }

            return true;
        };


        // Users
        var _handleUserUpdateError = function(errorMsg) {
            if (adminAreaService.userUpdateStatusObject.userUpdateFailure) {
                helperService.toasterError("Update Error",
                    "There was a problem updating the User part of the data you submitted: " + errorMsg);

                adminAreaService.userUpdateStatusObject.userUpdateFailure = false;
            }

            if (adminAreaService.userUpdateStatusObject.employeeUpdateFailure) {
                helperService.toasterError("Update Error",
                    "There was a problem updating the Employee part of the data you submitted: " + errorMsg);

                adminAreaService.userUpdateStatusObject.employeeUpdateFailure = false;
            }

            if (adminAreaService.userUpdateStatusObject.contractUpdateFailure) {
                helperService.toasterError("Update Error",
                    "There was a problem updating the Contract part of the data you submitted: " + errorMsg);

                adminAreaService.userUpdateStatusObject.contractUpdateFailure = false;
            }

            if (adminAreaService.userUpdateStatusObject.siteAccessUpdateFailure) {
                helperService.toasterError("Update Error",
                    "There was a problem updating the Location Access records: " + errorMsg);

                adminAreaService.userUpdateStatusObject.siteAccessUpdateFailure = false;
            }

            if (adminAreaService.userUpdateStatusObject.siteAccessCreateFailure) {
                helperService.toasterError("Update Error",
                    "There was a problem creating Location Access records: " + errorMsg);

                adminAreaService.userUpdateStatusObject.siteAccessCreateFailure = false;
            }
        };

        var _handleUserUpdateSuccess = function() {
            var successMsg = "The User was successfully updated.";

            // just user updated
            if (adminAreaService.userUpdateStatusObject.userUpdateSuccessful) {
                helperService.toasterSuccess("Update Success", successMsg);
                adminAreaService.userUpdateStatusObject.userUpdateSuccessful = false;
            }

            // just employee
            if (adminAreaService.userUpdateStatusObject.employeeUpdateSuccessful) {
                helperService.toasterSuccess("Update Success", successMsg);
                adminAreaService.userUpdateStatusObject.employeeUpdateSuccessful = false;
            }

            // just contract
            if (adminAreaService.userUpdateStatusObject.contractUpdateSuccessful) {
                helperService.toasterSuccess("Update Success", successMsg);
                adminAreaService.userUpdateStatusObject.contractUpdateSuccessful = false;
            }

            // just site access update
            if (adminAreaService.userUpdateStatusObject.siteAccessUpdateSuccessful) {
                helperService.toasterSuccess("Update Success", successMsg);
                adminAreaService.userUpdateStatusObject.siteAccessUpdateSuccessful = false;
            }

            // just site access create
            if (adminAreaService.userUpdateStatusObject.siteAccessCreateSuccessful) {
                helperService.toasterSuccess("Update Success", successMsg);
                adminAreaService.userUpdateStatusObject.siteAccessCreateSuccessful = false;
            }
        };

        var _validateUserObjectForm = function(userFormObj, scope) {
            var userToasterHeaderStr = "RMS User Details Section Warning";
            var employeeToasterHeaderStr = "Employee Details Section Warning";
            var contractToasterHeaderStr = "Contract Detail Section Warning";
            var accessSectionHeaderStr = "Location Access Section Error";

            // user section
            if (!userFormObj.userLogin.$modelValue || userFormObj.userLogin.$modelValue === "") {
                helperService.toasterWarning(userToasterHeaderStr,
                    "You need to supply a Login for the User.");
                return [false, adminActionStatusServiceFactory.USER_VAL_ENUM.login];
            }

            if (!userFormObj.userEmail.$modelValue || userFormObj.userEmail.$modelValue === "") {
                helperService.toasterWarning(userToasterHeaderStr,
                    "You need to supply an Email Address for the User.");
                return [false, adminActionStatusServiceFactory.USER_VAL_ENUM.email];
            }

            if (!userFormObj.userFirstname.$modelValue || userFormObj.userFirstname.$modelValue === "") {
                helperService.toasterWarning(userToasterHeaderStr,
                    "You need to supply a First Name for the User.");
                return [false, adminActionStatusServiceFactory.USER_VAL_ENUM.firstname];
            }

            if (!userFormObj.userLastname.$modelValue || userFormObj.userLastname.$modelValue === "") {
                helperService.toasterWarning(userToasterHeaderStr,
                    "You need to supply a Last Name for the User.");
                return [false, adminActionStatusServiceFactory.USER_VAL_ENUM.lastname];
            }

            if (!userFormObj.userAccess.$modelValue) {
                helperService.toasterWarning(userToasterHeaderStr,
                    "You need to select an RMS Access Level for the User.");
                return [false, adminActionStatusServiceFactory.USER_VAL_ENUM.sysAccess];
            }

            if (!userFormObj.userExtTimeSysId.$modelValue || userFormObj.userExtTimeSysId.$modelValue === "") {
                helperService.toasterWarning(userToasterHeaderStr,
                    "You need to supply a ZK Time ID for the User.");
                return [false, adminActionStatusServiceFactory.USER_VAL_ENUM.extTimeSysId];
            }

            if (!userFormObj.userPayrollRef.$modelValue || userFormObj.userPayrollRef.$modelValue === "") {
                helperService.toasterWarning(userToasterHeaderStr,
                    "You need to supply a Payroll Reference number for the User.");
                return [false, adminActionStatusServiceFactory.USER_VAL_ENUM.payroll];
            }


            // employee section
            if (!userFormObj.employeeCompany.$modelValue) {
                helperService.toasterWarning(employeeToasterHeaderStr,
                    "You need to select a Company for the User.");
                return [false, adminActionStatusServiceFactory.USER_VAL_ENUM.company];
            }

            if (!userFormObj.employeeType.$modelValue) {
                helperService.toasterWarning(employeeToasterHeaderStr,
                    "You need to select an Employee Type for the User.");
                return [false, adminActionStatusServiceFactory.USER_VAL_ENUM.employeeType];
            }

            if (!userFormObj.employeeBaseSite.$modelValue) {
                helperService.toasterWarning(employeeToasterHeaderStr,
                    "You need to select a Base Location for the User.");
                return [false, adminActionStatusServiceFactory.USER_VAL_ENUM.baseSite];
            }

            if (!userFormObj.employeeAddress.$modelValue || userFormObj.employeeAddress.$modelValue === "") {
                helperService.toasterWarning(employeeToasterHeaderStr,
                    "You need to supply an Address for the User.");
                return [false, adminActionStatusServiceFactory.USER_VAL_ENUM.address];
            }

            if (!userFormObj.employeeCity.$modelValue || userFormObj.employeeCity.$modelValue === "") {
                helperService.toasterWarning(employeeToasterHeaderStr,
                    "You need to supply an Town or City for the User.");
                return [false, adminActionStatusServiceFactory.USER_VAL_ENUM.city];
            }

            if (!userFormObj.employeePostcode.$modelValue || userFormObj.employeePostcode.$modelValue === "") {
                helperService.toasterWarning(employeeToasterHeaderStr,
                    "You need to supply a Postcode for the User.");
                return [false, adminActionStatusServiceFactory.USER_VAL_ENUM.postcode];
            }

            if (!userFormObj.employeeTelephone.$modelValue || userFormObj.employeeTelephone.$modelValue === "") {
                helperService.toasterWarning(employeeToasterHeaderStr,
                    "You need to supply a Telephone number for the User.");
                return [false, adminActionStatusServiceFactory.USER_VAL_ENUM.telephone];
            }

            if (!userFormObj.employeeMobile.$modelValue || userFormObj.employeeMobile.$modelValue === "") {
                helperService.toasterWarning(employeeToasterHeaderStr,
                    "You need to supply a Mobile number for the User.");
                return [false, adminActionStatusServiceFactory.USER_VAL_ENUM.mobile];
            }

            // contract section
            if (!userFormObj.contractResource.$modelValue || userFormObj.contractResource.$modelValue === "") {
                helperService.toasterWarning(contractToasterHeaderStr,
                    "You need to supply a Role for the User.");
                return [false, adminActionStatusServiceFactory.USER_VAL_ENUM.role];
            }

            if (!userFormObj.contractWeeklyHours.$modelValue || userFormObj.contractWeeklyHours.$modelValue === "") {
                helperService.toasterWarning(contractToasterHeaderStr,
                    "You need to supply a number of Weekly Hours for the User.");
                return [false, adminActionStatusServiceFactory.USER_VAL_ENUM.weeklyHours];
            }

            // location access section
            var psycareIsChecked = false;
            _.forEach(scope.psycare, function(site) {
                if (site.isChecked) {
                    psycareIsChecked = true;
                }
            });

            if (!scope.baldock.isChecked && !psycareIsChecked) {
                helperService.toasterWarning(accessSectionHeaderStr,
                    "You need to allocate Location Access for the User.");
                return [false, adminActionStatusServiceFactory.USER_VAL_ENUM.locationAccess];
            }

            return [true];
        };

        var _adminUserValidationWarningEnum = {
            login: 0,
            email: 1,
            firstname: 2,
            lastname: 3,
            sysAccess: 4,
            extTimeSysId: 5,
            payroll: 6,
            company: 7,
            employeeType: 8,
            baseSite: 9,
            address: 10,
            city: 11,
            postcode: 12,
            telephone: 13,
            mobile: 14,
            role: 15,
            weeklyHours: 16,
            locationAccess: 17
        };

        
        // Functions
        adminActionStatusServiceFactory.handleShiftTemplateCreateError = _handleShiftTemplateCreateError;
        adminActionStatusServiceFactory.handleShiftTemplateCreateSuccess = _handleShiftTemplateCreateSuccess;
        adminActionStatusServiceFactory.handleShiftTemplateUpdateError = _handleShiftTemplateUpdateError;
        adminActionStatusServiceFactory.handleShiftTemplateUpdateSuccess = _handleShiftTemplateUpdateSuccess;

        adminActionStatusServiceFactory.handleLeaveRequestCreateError = _handleLeaveRequestCreateError;
        adminActionStatusServiceFactory.handleLeaveRequestCreateSuccess = _handleLeaveRequestCreateSuccess;
        adminActionStatusServiceFactory.handleLeaveRequestDeleteError = _handleLeaveRequestDeleteError;
        adminActionStatusServiceFactory.handleLeaveRequestDeleteSuccess = _handleLeaveRequestDeleteSuccess;
        adminActionStatusServiceFactory.handleLeaveRequestUpdateError = _handleLeaveRequestUpdateError;
        adminActionStatusServiceFactory.handleLeaveRequestUpdateSuccess = _handleLeaveRequestUpdateSuccess;
        
        adminActionStatusServiceFactory.handleResourceCreateError = _handleResourceCreateError;
        adminActionStatusServiceFactory.handleResourceCreateSuccess = _handleResourceCreateSuccess;
        adminActionStatusServiceFactory.handleResourceUpdateError = _handleResourceUpdateError;
        adminActionStatusServiceFactory.handleResourceUpdateSuccess = _handleResourceUpdateSuccess;

        adminActionStatusServiceFactory.handleSystemConfigUpdateError = _handleSystemConfigUpdateError;
        adminActionStatusServiceFactory.handleSystemConfigUdateSuccess = _handleSystemConfigUdateSuccess;
        adminActionStatusServiceFactory.handleUserUpdateError = _handleUserUpdateError;
        adminActionStatusServiceFactory.handleUserUpdateSuccess = _handleUserUpdateSuccess;

        adminActionStatusServiceFactory.validateLeaveRequestObjectForm = _validateLeaveRequestObjectForm;
        adminActionStatusServiceFactory.validateResourceObjectForm = _validateResourceObjectForm;
        adminActionStatusServiceFactory.validateShiftTemplateSearchCriteriaForm = _validateShiftTemplateSearchCriteriaForm;
        adminActionStatusServiceFactory.validateShiftTemplateUpdateForm = _validateShiftTemplateUpdateForm;
        adminActionStatusServiceFactory.validateSystemConfigFormObject = _validateSystemConfigFormObject;
        adminActionStatusServiceFactory.validateUserObjectForm = _validateUserObjectForm;

        // Enums
        adminActionStatusServiceFactory.ROLE_VAL_ENUM = _adminRoleValidationEnumWarning;
        adminActionStatusServiceFactory.SHIFT_TEMPLATE_SEARCH_VAL_ENUM = _adminShiftTemplateSearchWarningEnum;
        adminActionStatusServiceFactory.SHIFT_TEMPLATE_VAL_ENUM = _adminShiftTemplateWarningEnum;
        adminActionStatusServiceFactory.USER_VAL_ENUM = _adminUserValidationWarningEnum;

        return adminActionStatusServiceFactory;
    }
]);