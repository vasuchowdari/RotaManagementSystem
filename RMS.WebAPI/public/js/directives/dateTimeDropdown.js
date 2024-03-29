﻿"use strict";

var app = angular.module("directives");
app.directive("dateTimeDropdown", function($window) {
    return {
        require: "^ngModel",
        restrict: "A",
        link: function (scope, elm, attrs, ctrl) {
            var moment = $window.moment;
            var dateFormat = attrs.moMediumDate;
            attrs.$observe('dateTimeDropdown', function(newValue) {
                if (dateFormat == newValue || !ctrl.$modelValue) return;
                dateFormat = newValue;
                ctrl.$modelValue = new Date(ctrl.$setViewValue);
            });

            ctrl.$formatters.unshift(function(modelValue) {
                scope = scope;
                if (!dateFormat || !modelValue) return "";
                var retVal = moment(modelValue).format(dateFormat);
                return retVal;
            });

            ctrl.$parsers.unshift(function(viewValue) {
                scope = scope;
                var date = moment(viewValue, dateFormat);
                return (date && date.isValid() && date.year() > 1950) ? date.toDate() : "";
            });
        }
    }
})