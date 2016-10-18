/// <reference path="../vendor/libs/webuploader/webuploader.js" />
/// <reference path="../vendor/modules/angular-amap/angular-amap.min.js" />
/// <reference path="../vendor/modules/angular-amap/angular-amap.js" />
/// <reference path="../vendor/modules/angular-amap/angular-amap.js" />
'use strict';

/* Config for the router */
angular.module('app').run(['$rootScope', '$state', '$stateParams', function ($rootScope, $state, $stateParams) {
    //只有实例对象和常量能够被注入到 运行(run )块当中。这样做的目的是为了防止在应用程序运行期间，增加，修改了系统配置。
    $rootScope.$state = $state;
    $rootScope.$stateParams = $stateParams;
}]).config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
    $urlRouterProvider.otherwise('/app/dashboard');

    $stateProvider
            .state('app', {
                abstract: true,
                url: '/app',
                templateUrl: 'app/views/app.html'
            })
              .state('app.dashboard', {
                  url: '/dashboard',
                  templateUrl: 'app/views/app_dashboard.html',
                  resolve: {
                      //deps: ['$ocLazyLoad',
                      //  function ($ocLazyLoad) {
                      //      return $ocLazyLoad.load(['js/controllers/chart.js']);
                      //  }]
                  }
              })
                // fullCalendar
              .state('app.calendar', {
                  url: '/calendar',
                  templateUrl: 'app/calendar/calendar.html',
                  // use resolve to load other dependences
                  resolve: {
                      deps: ['$ocLazyLoad', 'uiLoad',
                        function ($ocLazyLoad, uiLoad) {
                            return uiLoad.load(
                              ['vendor/jquery/fullcalendar/fullcalendar.css',
                                'vendor/jquery/fullcalendar/theme.css',
                                //'vendor/jquery/jquery-ui-1.10.3.custom.min.js',
                                'vendor/libs/moment.min.js',
                                'vendor/jquery/fullcalendar/fullcalendar.min.js',
                                'app/calendar/calendar.js']
                            ).then(
                              function () {
                                  return $ocLazyLoad.load('ui.calendar');
                              }
                            )
                        }]
                  }
              })

        //#region 目录管理路由配置

        .state('app.catalog', {
            url: '/catalog',
            template: '<div ui-view></div>'
        })
        .state('app.catalog.storecategory', {
            url: '/storecategories',
            templateUrl: 'app/catalog/storecateory/views/list.html',
            resolve: {
                deps: ['$ocLazyLoad',
                  function ($ocLazyLoad) {
                      return $ocLazyLoad.load(['angularBootstrapNavTree', 'w5c.validator']).then(
                          function () {
                              return $ocLazyLoad.load(['app/common/controller/ModalDialogCtrl.js', 'app/catalog/storecateory/Services/StoreCategoryService.js',
                                                                      'app/catalog/storecateory/Controller/storeCategoryListCtrl.js']);
                          }
                      );
                  }
                ]
            }
        })
        .state('app.catalog.store', {
            abstract: true,
            url: '',
            template: '<div ui-view></div>',
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load(['app/common/controller/ModalDialogCtrl.js']);
                }]
            }
        })
        .state('app.catalog.store.list', {
            url: '/stores',
            templateUrl: 'app/catalog/store/views/list.html',
            resolve: {
                deps: ['$ocLazyLoad',
                  function ($ocLazyLoad) {
                      return $ocLazyLoad.load(['app/catalog/store/Services/StoreService.js', 'app/catalog/store/Controller/StoreListCtrl.js'])
                  }
                ]
            }
        })
        .state('app.catalog.store.create', {
            url: '/stores/create',
            templateUrl: 'app/catalog/store/views/create.html',
            resolve: {
                deps: ['$ocLazyLoad', 'uiLoad',
                  function ($ocLazyLoad, uiLoad) {
                      return uiLoad.load([
                          'http://webapi.amap.com/maps?v=1.3&key=1e4ff737e5e9e2f63cc3333316b38a7c'
                      ]).then(function () {
                          return $ocLazyLoad.load(['toaster', 'ngImgCrop', 'w5c.validator']).then(function () {
                              return $ocLazyLoad.load(['app/catalog/storecateory/Services/StoreCategoryService.js',
                                  'app/catalog/store/Services/StoreService.js', 'app/catalog/store/Controller/StoreCreateOrEditCtrl.js']);
                          });
                      });
                  }
                ]
            }
        })
        .state('app.catalog.store.edit', {
            url: '/stores/:id/edit',
            templateUrl: 'app/catalog/store/views/create.html',
            controller: function ($scope, $stateParams) {
                $scope.storeId = $stateParams.id;
            },
            resolve: {
                deps: ['$ocLazyLoad', 'uiLoad',
                  function ($ocLazyLoad, uiLoad) {
                      return uiLoad.load([
                          'http://webapi.amap.com/maps?v=1.3&key=1e4ff737e5e9e2f63cc3333316b38a7c'
                      ]).then(function () {
                          return $ocLazyLoad.load(['toaster', 'ngImgCrop', 'w5c.validator']).then(function () {
                              return $ocLazyLoad.load(['app/catalog/storecateory/Services/StoreCategoryService.js',
                                  'app/catalog/store/Services/StoreService.js', 'app/catalog/store/Controller/StoreCreateOrEditCtrl.js']);
                          });
                      });
                  }
                ]
            }
        })
        .state('app.catalog.store.detail', {
            url: '/stores/:id',
            templateUrl: 'app/catalog/store/views/detail.html',
            controller: function ($scope, $stateParams) {
                $scope.storeId = $stateParams.id;
            },
            resolve: {
                deps: ['$ocLazyLoad', 'uiLoad',
                  function ($ocLazyLoad, uiLoad) {
                      return $ocLazyLoad.load(['app/catalog/store/Services/StoreService.js', 'app/catalog/store/Controller/StoreCtrl.js']);
                  }
                ]
            }
        })
        .state('app.catalog.store.detail.overview', {
            url: '/overview',
            templateUrl: 'app/catalog/store/views/detail_overview.html'
        })
        .state('app.catalog.store.detail.info', {
            url: '/info',
            templateUrl: 'app/catalog/store/views/detail_info.html',
            resolve: {
                deps: ['$ocLazyLoad', 'uiLoad',
                  function ($ocLazyLoad, uiLoad) {
                      return uiLoad.load([
                    'http://webapi.amap.com/maps?v=1.3&key=1e4ff737e5e9e2f63cc3333316b38a7c'
                      ]).then(function () {
                          return $ocLazyLoad.load(['app/catalog/store/Controller/StoreMapCtrl.js']);
                      });
                  }
                ]
            }
        })
        .state('app.catalog.store.detail.category', {
            url: '/categories',
            templateUrl: 'app/catalog/store/views/detail_category.html',
            resolve: {
                deps: ['$ocLazyLoad',
                  function ($ocLazyLoad) {
                      return $ocLazyLoad.load(['w5c.validator']).then(function () {
                          return $ocLazyLoad.load(['app/catalog/store/Services/ProductCategoryService.js', 'app/catalog/store/Controller/StoreProductCategoryCtrl.js']); //
                      });

                  }
                ]
            }
        })
        .state('app.catalog.store.detail.product', {
            abstract: true,
            url: '',
            template: '<div ui-view></div>'
        })
        .state('app.catalog.store.detail.product.list', {
            url: '/products',
            templateUrl: 'app/catalog/store/views/detail_product_list.html',
            resolve: {
                deps: ['$ocLazyLoad',
                  function ($ocLazyLoad) {
                      return $ocLazyLoad.load(['app/catalog/store/Services/ProductService.js', 'app/catalog/store/Controller/StoreProductListCtrl.js'])
                  }
                ]
            }
        })
        .state('app.catalog.store.detail.product.create', {
            url: '/products/create',
            templateUrl: 'app/catalog/store/views/detail_product_create.html',
            resolve: {
                deps: ['$ocLazyLoad', 'uiLoad', function ($ocLazyLoad, uiLoad) {

                    return uiLoad.load(['vendor/libs/webuploader/webuploader.js', 'vendor/libs/webuploader/webuploader.css']).then(function () {
                        return $ocLazyLoad.load(['toaster', 'ngKeditor', 'ui.select', 'w5c.validator']).then(function () {
                            return $ocLazyLoad.load([
                                'vendor/modules/ngSticky/sticky.min.js',
                                'app/catalog/tags/Services/tagService.js',
                                'app/catalog/store/Services/ProductCategoryService.js',
                                'app/catalog/store/Services/ProductService.js',
                                'app/catalog/store/Controller/StoreProductCreateOrEditCtrl.js']); //
                        });
                    });


                }]
            }
        })
        .state('app.catalog.store.detail.product.edit', {
            url: '/products/:productId/edit',
            templateUrl: 'app/catalog/store/views/detail_product_create.html',
            controller: function ($scope, $stateParams) {
                $scope.productId = $stateParams.productId;
            },
            resolve: {
                deps: ['$ocLazyLoad', 'uiLoad', function ($ocLazyLoad, uiLoad) {

                    return uiLoad.load(['vendor/libs/webuploader/webuploader.js', 'vendor/libs/webuploader/webuploader.css']).then(function () {
                        return $ocLazyLoad.load(['toaster', 'ngKeditor', 'ui.select', 'w5c.validator']).then(function () {
                            return $ocLazyLoad.load([
                                'vendor/modules/ngSticky/sticky.min.js',
                                'app/catalog/tags/Services/tagService.js',
                                'app/catalog/store/Services/ProductCategoryService.js',
                                'app/catalog/store/Services/ProductService.js',
                                'app/catalog/store/Controller/StoreProductCreateOrEditCtrl.js']);
                        });
                    });
                }]
            }
        })
        .state('app.catalog.store.detail.product.detail', {
            url: '/products/:productId',
            templateUrl: 'app/catalog/store/views/detail_product_detail.html',
            controller: function ($scope, $stateParams) {
                $scope.productId = $stateParams.productId;
            },
            resolve: {
                deps: ['$ocLazyLoad', 'uiLoad', function ($ocLazyLoad, uiLoad) {
                    return $ocLazyLoad.load(['app/catalog/store/Services/ProductService.js', 'app/catalog/store/Controller/StoreProductCtrl.js'])
                }]
            }
        })
        .state('app.catalog.tags', {
            url: '/catalog/tags',
            template: '标签>'
        })
        .state('app.catalog.settings', {
            url: '/catalog/settings',
            template: '设置'
        })

        //#endregion

        .state('access', {
            abstract: true,
            url: '/access',
            template: '<div ui-view class="fade-in-right-big smooth"></div>'
        })
        .state('access.signin', {
            url: '/signin',
            templateUrl: 'app/access/views/signin.html',
            resolve: {
                deps: ['$ocLazyLoad',
                function ($ocLazyLoad) {
                    return $ocLazyLoad.load(['toaster', 'w5c.validator']).then(function () {
                        return $ocLazyLoad.load('app/access/controller/signinFormController.js');
                    });
                }]
            }
        });
}]);