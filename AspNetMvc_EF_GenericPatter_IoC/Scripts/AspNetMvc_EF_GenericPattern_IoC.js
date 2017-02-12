var AspNetMvc_EF_GenericPattern_IoC = angular.module('AspNetMvc_EF_GenericPattern_IoC', ['ngRoute']);

var configFunction = function ($routeProvider) {
    $routeProvider.
        when('/Index', {
            templateUrl: 'People/Index'
        })
        .when('/Details/:id', {
            templateUrl: function (params) { return '/People1/Details?id=' + params.id; }
        })
        .when('/Create', {
            templateUrl: 'People/Create'
        })
        .when('/Delete', {
            templateUrl: 'People/Delete'
        });
}

configFunction.$inject = ['$routeProvider'];

AspNetMvc_EF_GenericPattern_IoC.config(configFunction);