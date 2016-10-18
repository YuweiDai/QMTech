'use strict';

angular.module('app').config(function ($httpProvider) {
	$httpProvider.interceptors.push('authInterceptorService');
}).run(['authService', function (authService) {
	authService.fillAuthData();
}]);