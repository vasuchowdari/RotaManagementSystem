"use strict";

var app = angular.module("directives");
app.directive("dateField", function($filter) {
    return {
        require: "^ngModel",
        restrict: "A",
        link: function (scope, elm, attrs, ctrl) {
            ctrl.$parsers.push(function (data) {
                var date = Date.parse(data);

                ctrl.$setValidity('date', date != null);
                return date == null ? undefined : date;
            });
            ctrl.$formatters.push(function (data) {
                return $filter('date')(data.value, 'yyyy-MM-dd');
            });
        }
    }
})