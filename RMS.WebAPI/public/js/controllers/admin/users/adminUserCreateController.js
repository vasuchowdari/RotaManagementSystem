"use strict";

var app = angular.module("controllers");
app.controller("adminUserCreateCtrl", [
    "$scope",
    "$state",
    "helperService",
    "adminAreaService",
    "adminActionStatusService",
    "blockUI",
    function($scope, $state, helperService, adminAreaService, adminActionStatusService, blockUI) {
        // Block And Auth
        helperService.blockAndAuth($scope, blockUI);


        // Event Subscription


        // Declarations
        $scope.baldock = [];
        $scope.baseSubLocationDropdownDisabled = true;
        $scope.companies = [];
        $scope.employeeTypes = [];
        var formObj = null;
        $scope.hideAddressWarning = true;
        $scope.hideBaseSiteWarning = true;
        $scope.hideCityWarning = true;
        $scope.hideCompanyWarning = true;
        $scope.hideEmailWarning = true;
        $scope.hideEmployeeTypeWarning = true;
        $scope.hideExtTimeSysIdWarning = true;
        $scope.hideFirstnameWarning = true;
        $scope.hideLastnameWarning = true;
        $scope.hideLoginWarning = true;
        $scope.hideMobileWarning = true;
        $scope.hidePayrollRefWarning = true;
        $scope.hidePostcodeWarning = true;
        $scope.hideRoleWarning = true;
        $scope.hideSysAccessWarning = true;
        $scope.hideTelephoneWarning = true;
        $scope.hideWeeklyHoursWarning = true;
        $scope.mvm = adminAreaService.returnNewUserViewModel();
        $scope.openContractDetails = false;
        $scope.openEmployeeDetails = false;
        $scope.openLocationAccess = false;
        $scope.openUserDetails = true;
        $scope.psycare = [];
        $scope.resources = [];
        $scope.sites = [];
        $scope.subsites = [];
        $scope.systemAccessRoles = [];
        var USER_INVALID = adminActionStatusService.USER_VAL_ENUM;


        // Functions
        $scope.clearBaldockChildren = function() {
            adminAreaService.clearBaldockChildren($scope.baldock);
        };

        $scope.getSystemAccessRoles = function() {
            adminAreaService.getSystemAccessRoles().then(function(res) {
                $scope.systemAccessRoles = res;
                blockUI.stop();
            }).catch(function(error) {
                helperService.toasterError(null, error);
                blockUI.stop();
            });
        };

        $scope.hydrateBaldock = function() {
            adminAreaService.hydrateBaldock($scope);
        };

        $scope.hydratePsycare = function() {
            adminAreaService.hydratePsycare($scope);
        };

        $scope.removeAddressWarning = function() {
            if (!$scope.hideAddressWarning) {
                $scope.hideAddressWarning = true;
            }
        };

        $scope.removeBaseSiteWarning = function() {
            if (!$scope.hideBaseSiteWarning) {
                $scope.hideBaseSiteWarning = true;
            }
        };

        $scope.removeCityWarning = function() {
            if (!$scope.hideCityWarning) {
                $scope.hideCityWarning = true;
            }
        };

        $scope.removeCompanyWarning = function() {
            if (!$scope.hideCompanyWarning) {
                $scope.hideCompanyWarning = true;
            }
        };

        $scope.removeEmailWarning = function() {
            if (!$scope.hideEmailWarning) {
                $scope.hideEmailWarning = true;
            }
        };

        $scope.removeEmployeeTypeWarning = function() {
            if (!$scope.hideEmployeeTypeWarning) {
                $scope.hideEmployeeTypeWarning = true;
            }
        };

        $scope.removeExtTimeSysIdWarning = function() {
            if (!$scope.hideExtTimeSysIdWarning) {
                $scope.hideExtTimeSysIdWarning = true;
            }
        };

        $scope.removeFirstnameWarning = function() {
            if (!$scope.hideFirstnameWarning) {
                $scope.hideFirstnameWarning = true;
            }
        };

        $scope.removeLastnameWarning = function() {
            if (!$scope.hideLastnameWarning) {
                $scope.hideLastnameWarning = true;
            }
        };

        $scope.removeLoginWarning = function() {
            if (!$scope.hideLoginWarning) {
                $scope.hideLoginWarning = true;
            }
        };

        $scope.removeMobileWarning = function() {
            if (!$scope.hideMobileWarning) {
                $scope.hideMobileWarning = true;
            }
        };

        $scope.removePayrollRefWarning = function() {
            if (!$scope.hidePayrollRefWarning) {
                $scope.hidePayrollRefWarning = true;
            }
        };

        $scope.removePostcodeWarning = function() {
            if (!$scope.hidePostcodeWarning) {
                $scope.hidePostcodeWarning = true;
            }
        };

        $scope.removeRoleWarning = function() {
            if (!$scope.hideRoleWarning) {
                $scope.hideRoleWarning = true;
            }
        };

        $scope.removeSysAccessWarning = function() {
            if (!$scope.hideSysAccessWarning) {
                $scope.hideSysAccessWarning = true;
            }
        };

        $scope.removeTelephoneWarning = function() {
            if (!$scope.hideTelephoneWarning) {
                $scope.hideTelephoneWarning = true;
            }
        };

        $scope.removeWeeklyHoursWarning = function() {
            if (!$scope.hideWeeklyHoursWarning) {
                $scope.hideWeeklyHoursWarning = true;
            }
        };

        $scope.selectBaldockParentSite = function() {
            adminAreaService.selectBaldockParent($scope.baldock);
        };

        $scope.submitUserCreateForm = function() {
            var self = this;
            formObj = angular.copy(self.userCreateForm);

            var valObj = adminActionStatusService.validateUserObjectForm(formObj, $scope);

            if (!valObj[0]) {
                if (valObj[1] === USER_INVALID.login) {
                    $scope.hideLoginWarning = false;
                    $scope.openUserDetails = true;
                }

                if (valObj[1] === USER_INVALID.email) {
                    $scope.hideEmailWarning = false;
                    $scope.openUserDetails = true;
                }

                if (valObj[1] === USER_INVALID.firstname) {
                    $scope.hideFirstnameWarning = false;
                    $scope.openUserDetails = true;
                }

                if (valObj[1] === USER_INVALID.lastname) {
                    $scope.hideLastnameWarning = false;
                    $scope.openUserDetails = true;
                }

                if (valObj[1] === USER_INVALID.sysAccess) {
                    $scope.hideSysAccessWarning = false;
                    $scope.openUserDetails = true;
                }

                if (valObj[1] === USER_INVALID.extTimeSysId) {
                    $scope.hideExtTimeSysIdWarning = false;
                    $scope.openUserDetails = true;
                }

                if (valObj[1] === USER_INVALID.payroll) {
                    $scope.hidePayrollRefWarning = false;
                    $scope.openUserDetails = true;
                }

                if (valObj[1] === USER_INVALID.company) {
                    $scope.hideCompanyWarning = false;
                    $scope.openEmployeeDetails = true;
                }

                if (valObj[1] === USER_INVALID.employeeType) {
                    $scope.hideEmployeeTypeWarning = false;
                    $scope.openEmployeeDetails = true;
                }

                if (valObj[1] === USER_INVALID.baseSite) {
                    $scope.hideBaseSiteWarning = false;
                    $scope.openEmployeeDetails = true;
                }

                if (valObj[1] === USER_INVALID.address) {
                    $scope.hideAddressWarning = false;
                    $scope.openEmployeeDetails = true;
                }

                if (valObj[1] === USER_INVALID.city) {
                    $scope.hideCityWarning = false;
                    $scope.openEmployeeDetails = true;
                }

                if (valObj[1] === USER_INVALID.postcode) {
                    $scope.hidePostcodeWarning = false;
                    $scope.openEmployeeDetails = true;
                }

                if (valObj[1] === USER_INVALID.telephone) {
                    $scope.hideTelephoneWarning = false;
                    $scope.openEmployeeDetails = true;
                }

                if (valObj[1] === USER_INVALID.mobile) {
                    $scope.hideMobileWarning = false;
                    $scope.openEmployeeDetails = true;
                }

                if (valObj[1] === USER_INVALID.role) {
                    $scope.hideRoleWarning = false;
                    $scope.openContractDetails = true;
                }

                if (valObj[1] === USER_INVALID.weeklyHours) {
                    $scope.hideWeeklyHoursWarning = false;
                    $scope.openContractDetails = true;
                }

                if (valObj[1] === USER_INVALID.locationAccess) {
                    $scope.openLocationAccess = true;
                }

                return;
            }

            $scope.updateUserSitePermissions();

            adminAreaService.createUser($scope.mvm).then(function(res) {
                helperService.statusHandler($scope, res.status, null, res.data, "admin.users.create");
            }).catch(function(error) {
                helperService.toasterError(null, error);
                blockUI.stop();
            });
        };

        $scope.toggleBaseSubLocationDropdown = function() {
            if (!$scope.baseSubLocationDropdownDisabled) {
                $scope.baseSubLocationDropdownDisabled = true;
                $scope.subsites = [];
            } else {
                $scope.baseSubLocationDropdownDisabled = false;
                var tempSite = _.head(_.filter($scope.sites, { "Id": 2 }));
                $scope.subsites = tempSite.SubSiteModels;
            }
        };

        $scope.updateUserSitePermissions = function() {
            if ($scope.baldock.isChecked) {
                $scope.mvm.siteAccessModels.push({
                    IsActive: true,
                    EmployeeId: $scope.mvm.employeeModel.Id,
                    SiteId: $scope.baldock.Id,
                    SubSiteId: null
                });
            }

            _.forEach($scope.baldock.SubSiteModels, function(subsite) {
                if (subsite.isChecked) {
                    $scope.mvm.siteAccessModels.push({
                        IsActive: true,
                        EmployeeId: $scope.mvm.employeeModel.Id,
                        SiteId: $scope.baldock.Id,
                        SubSiteId: subsite.Id
                    });
                }
            });

            _.forEach($scope.psycare, function(site) {
                if (site.isChecked) {
                    $scope.mvm.siteAccessModels.push({
                        IsActive: true,
                        EmployeeId: $scope.mvm.employeeModel.Id,
                        SiteId: site.Id,
                        SubSiteId: null
                    });
                }
            });
        };


        // Init
        $scope.init = function() {
            if (!$scope.authentication.isAuth) {
                helperService.notAuth();
            }

            $scope.getSystemAccessRoles();

            $scope.employeeTypes = helperService.getLocalStorageObject("employeeTypesStore");
            $scope.companies = helperService.getLocalStorageObject("companiesStore");
            $scope.resources = helperService.getLocalStorageObject("resourcesStore");
            $scope.sites = helperService.getLocalStorageObject("sitesStore");
            var tempSite = _.head(_.filter($scope.sites, { "Id": 2 }));
            $scope.subsites = tempSite.SubSiteModels;

            $scope.hydrateBaldock();
            $scope.hydratePsycare();

            $scope.mvm.userModel.IsActive = true;
            $scope.mvm.employeeModel.IsActive = true;
            $scope.mvm.contractModel.IsActive = true;

            blockUI.stop();
        };

        $scope.init();
    }
]);