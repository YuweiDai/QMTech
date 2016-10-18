'use strict';

/* Config for the router */
angular.module('app').run(['$rootScope', '$state', '$stateParams', function ($rootScope, $state, $stateParams) {
    //只有实例对象和常量能够被注入到 运行(run )块当中。这样做的目的是为了防止在应用程序运行期间，增加，修改了系统配置。
    $rootScope.$state = $state;
    $rootScope.$stateParams = $stateParams;
}]).config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
    $urlRouterProvider
        .otherwise('/');

    $stateProvider
             .state('home', {
                 url: '/',
                 templateUrl: 'views/home.html'
             })
             .state('category', {
                 url: '/category',
                 templateUrl: 'views/category.html'
             })
           .state('account', {
               abstract: true,
               url: '/account',
               templateUrl: 'views/account/index.html'
           }).state('account.login', {
               url: '/login',
               templateUrl: 'views/account/login.html',
               resolve: {
                   deps: ['$ocLazyLoad',
                     function ($ocLazyLoad) {
                         return $ocLazyLoad.load('js/account/LoginController.js');
                     }]
               }
               //#region old 采用自定义service 实现
               //resolve: {
               //    deps: ['uiLoad',
               //      function (uiLoad) {
               //          return uiLoad.load(['js/account/LoginController.js']);
               //      }]
               //}
               //#endregion
           }).state('account.register', {
               url: '/register',
               templateUrl: 'views/account/register.html',
               resolve: {
                   deps: ['$ocLazyLoad',
                     function ($ocLazyLoad) {
                         return $ocLazyLoad.load('js/account/RegisterController.js');
                     }]
               }
           }).state('account.register.forbidden', {
               url: '/register/forbiden',
               templateUrl: 'views/account/register.forbidden.html',
           }).state('account.register.emailvalidation', {
               url: '/register/emailvalidation',
               templateUrl: 'views/account/register.emailvalidation.html',
           }).state('account.register.adminapproval', {
               url: '/register/adminapproval',
               templateUrl: 'views/account/register.adminapproval.html',
           });
}]);