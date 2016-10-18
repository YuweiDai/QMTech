'use strict';

//HTTP 拦截器 用于HTTP请求时注入token
app.factory('authInterceptorService', ['$q', '$injector', '$location', '$localStorage',  function ($q, $injector, $location, $localStorage) {

    var authInterceptorServiceFactory = {};

    var _request = function (config) {

        config.headers = config.headers || {};

        var authData = $localStorage.authorizationData;
        //var authData = $localStorage.get('authorizationData');
        if (authData) {
            config.headers.Authorization = 'Bearer ' + authData.token;
        }

        return config;
    }

    var _responseError = function (rejection) {
        if (rejection.status === 401) {
            var authService = $injector.get('authService');
            var $state = $injector.get('$state');

            var authData = $localStorage.authorizationData;
            //var authData = $localStorage.get('authorizationData');

            if (authData) {
                if (authData.useRefreshTokens) {
                    $location.path('/refresh');
                    return $q.reject(rejection);
                }
            }
            authService.logOut();
            //$location.path('/login');
            $state.go("access.signin");
        }
        return $q.reject(rejection);
    }

    authInterceptorServiceFactory.request = _request;
    authInterceptorServiceFactory.responseError = _responseError;

    return authInterceptorServiceFactory;
}]);