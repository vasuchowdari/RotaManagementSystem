"use strict";

var app = angular.module("rmsApp", [
    "ui.router",
    "ui.bootstrap",
    "ui.router.tabs",
    'ui.bootstrap.datetimepicker',
    "daypilot",
    "scrollable-table",
    "services",
    "controllers",
    "directives",
    "LocalStorageModule",
    "ngDialog",
    "toaster",
    "blockUI",
    "ngAnimate"
]);

app.config(function($stateProvider, $urlRouterProvider) {
        
    //var path = "http://80.229.27.248:8093/RMSV2Dev"; // <-- for live server deployment
    var path = "http://80.229.27.248:8093/RMSV2FinDev";  // <-- finance test site
    //var path = "";  // <-- for local dev deployment

    // default to
    $urlRouterProvider.otherwise("/login");

    $stateProvider

        // Public
        .state("login", {
            url: "/login",
            templateUrl: path + "/public/views/public-tmpls/login.html",
            controller: "loginCtrl"
        })
        .state("reset", {
            url: "/reset",
            templateUrl: path + "/public/views/public-tmpls/password-reset.html",
            controller: "resetCtrl"
        })
        .state("profile", {
            url: "/profile",
            templateUrl: path + "/public/views/common-tmpls/profile.html",
            controller: "profileCtrl"
        })


        // User
        .state("user", {
            url: "/user",
            views: {
                "": {
                    templateUrl: path + "/public/views/user-tmpls/app-container.html",
                    controller: "userAppCtrl"
                },
                "userNav@user": {
                    templateUrl: path + "/public/views/user-tmpls/user-nav.html",
                    controller: "userNavCtrl"
                }
            }
        })
        .state("user.dash", {
            parent: "user",
            url: "/dash",
            templateUrl: path + "/public/views/user-tmpls/user-dash.html",
            controller: "userDashCtrl"
        })
        .state("user.sites", {
            parent: "user",
            url: "/sites",
            templateUrl: path + "/public/views/user-tmpls/user-sites.html",
            controller: "userSitesCtrl"
        })
        .state("user.rotas", {
            parent: "user",
            url: "/rotas",
            templateUrl: path + "/public/views/user-tmpls/user-rotas.html",
            controller: "userRotasCtrl"
        })
        .state("user.leave", {
            parent: "user",
            url: "/leave",
            templateUrl: path + "/public/views/user-tmpls/user-leave.html",
            controller: "userLeaveCtrl"
        })
        .state("user.adjustment", {
            parent: "user",
            url: "/adjustment",
            params: { shiftData: null },
            templateUrl: path + "/public/views/user-tmpls/user-adjustment.html",
            controller: "userAdjustmentCtrl"
        })


        // Manager
        .state("manager", {
            url: "/manager",
            views: {
                "": {
                    templateUrl: path + "/public/views/manager-tmpls/app-container.html",
                    controller: "managerAppCtrl",
                    resolve: {
                        companyObject: function(authService, companyService, employeeService) {
                            return employeeService.getByUserId(authService.authentication.userId).then(function(response) {
                                var employee = response.data;
                                var companyId = response.data.CompanyId;

                                return companyService.getById(companyId).then(function(response) {
                                    var company = response.data;

                                    return employeeService.getAllByCompanyId(companyId).then(function(response) {
                                        var employees = response.data;
                                        return [company, employee, employees];

                                    }).catch(function(error) {});
                                }).catch(function(error) {});
                            }).catch(function(error) {});
                        }
                    }
                },
                "managerNav@manager": {
                    templateUrl: path + "/public/views/manager-tmpls/manager-nav.html",
                    controller: "managerNavCtrl"
                }
            }
        })
        .state("manager.dash", {
            parent: "manager",
            url: "/dash",
            templateUrl: path + "/public/views/manager-tmpls/manager-dash.html",
            controller: "managerDashCtrl"
        })
        .state("manager.sites", {
            parent: "manager",
            url: "/sites",
            templateUrl: path + "/public/views/manager-tmpls/manager-sites.html",
            controller: "managerSitesCtrl"
        })
        .state("manager.site", {
            parent: "manager.sites",
            url: "/site/:siteId",
            templateUrl: path + "/public/views/manager-tmpls/manager-site.html",
            controller: "managerSiteCtrl"
        })
        .state("manager.site.calendar", {
            parent: "manager.site",
            url: "/calendar/:calendarId",
            templateUrl: path + "/public/views/manager-tmpls/manager-site-calendar.html",
            controller: "managerSiteCalendarCtrl"
        })
        .state("manager.site.subsite", {
            parent: "manager.sites",
            url: "/subsite/:subsiteId",
            templateUrl: path + "/public/views/manager-tmpls/manager-subsite.html",
            controller: "managerSubSiteCtrl"
        })
        .state("manager.site.subsite.calendar", {
            parent: "manager.site.subsite",
            url: "/calendar/:subsiteCalendarId",
            templateUrl: path + "/public/views/manager-tmpls/manager-subsite-calendar.html",
            controller: "managerSubSiteCalendarCtrl"
        })
        .state("manager.adjustment", {
            parent: "manager",
            url: "/adjustment",
            params: { timeAdjustmentFormData: null },
            templateUrl: path + "/public/views/manager-tmpls/manager-adjustment.html",
            controller: "managerAdjustmentCtrl"
        })


        // Admin
        .state("admin", {
            url: "/admin",
            views: {
                "": {
                    templateUrl: path + "/public/views/admin-tmpls/app-container.html",
                    controller: "adminAppCtrl"
                },
                "adminNav@admin": {
                    templateUrl: path + "/public/views/admin-tmpls/admin-nav.html",
                    controller: "adminNavCtrl"
                }
            }
        })
        .state("admin.dash", {
            parent: "admin",
            url: "/dash",
            templateUrl: path + "/public/views/admin-tmpls/admin-dash.html",
            controller: "adminDashCtrl"
        })
        .state("admin.adjustment", {
            parent: "admin",
            url: "/adjustment",
            params: { shiftProfileData: null },
            templateUrl: path + "/public/views/admin-tmpls/admin-shift-adjustment.html",
            controller: "adminShiftAdjustmentCtrl"
        })
        .state("admin.users", {
            parent: "admin",
            url: "/users",
            templateUrl: path + "/public/views/admin-tmpls/user-tmpls/admin-users-dash.html",
            controller: "adminUsersDashCtrl"
        })
        .state("admin.users.detail", {
            parent: "admin.users",
            url: "/detail/:userId",
            templateUrl: path + "/public/views/admin-tmpls/user-tmpls/admin-user-details.html",
            controller: "adminUserDetailsCtrl"
        })
        .state("admin.users.create", {
            parent: "admin.users",
            url: "/create",
            templateUrl: path + "/public/views/admin-tmpls/user-tmpls/admin-new-user.html",
            controller: "adminUserCreateCtrl"
        })
        .state("admin.shifts", {
            parent: "admin",
            url: "/shifts",
            templateUrl: path + "/public/views/admin-tmpls/shift-tmpls/admin-shifts-dash.html",
            controller: "adminShiftsDashCtrl"
        })
        .state("admin.shifts.templates", {
            parent: "admin",
            url: "/shifts/templates",
            templateUrl: path + "/public/views/admin-tmpls/shift-tmpls/admin-shift-template-search.html",
            controller: "adminShiftTemplateSearchCtrl"
        })
        .state("admin.shifts.templates.detail", {
            parent: "admin.shifts.templates",
            url: "/detail",
            params: { shiftTemplateData: null },
            templateUrl: path + "/public/views/admin-tmpls/shift-tmpls/admin-shift-template-details.html",
            controller: "adminShiftTemplateDetailsCtrl"
        })
        .state("admin.rotas", {
            url: "/rotas",
            templateUrl: path + "/public/views/admin-tmpls/rota-tmpls/admin-rotas-dash.html",
            controller: "adminRotasDashCtrl"
        })
        .state("admin.rotas.detail", {
            parent: "admin.rotas",
            url: "/detail",
            params: { rotaSearchParamsData: null },
            templateUrl: path + "/public/views/admin-tmpls/rota-tmpls/admin-rota-details.html",
            controller: "adminRotaDetailsCtrl"
        })
        .state("admin.payroll", {
            url: "/payroll",
            templateUrl: path + "/public/views/admin-tmpls/payroll-tmpls/payroll-dash.html",
            controller: "adminPayrollDashCtrl"
        })
        .state("admin.roles", {
            url: "/roles",
            templateUrl: path + "/public/views/admin-tmpls/role-tmpls/admin-roles-dash.html",
            controller: "adminRolesDashCtrl"
        })
        .state("admin.roles.detail", {
            parent: "admin.roles",
            url: "/detail/:resourceId",
            templateUrl: path + "/public/views/admin-tmpls/role-tmpls/admin-role-details.html",
            controller: "adminRoleDetailsCtrl"
        })
        .state("admin.roles.create", {
            parent: "admin.roles",
            url: "/create",
            templateUrl: path + "/public/views/admin-tmpls/role-tmpls/admin-role-create.html",
            controller: "adminRoleCreateCtrl"
        })
        .state("admin.config", {
            url: "/sysconfig",
            templateUrl: path + "/public/views/admin-tmpls/sysconfig-tmpls/admin-system-config-dash.html",
            controller: "adminSystemConfigDashCtrl"
        });
});

app.run([
    "authService",
    function(authService) {
        authService.fillAuthData();
    }
]);