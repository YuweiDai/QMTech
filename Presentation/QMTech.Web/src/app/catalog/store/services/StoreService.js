"use strict";

//商店服务
app.service("StoreService", ['$rootScope', '$http', '$q', function ($rootScope, $http, $q) {

    //获取商家列表
    this.getAllStores = function (params) {
        var deferred = $q.defer();

        var req = {
            method: "get",
            url: $rootScope.apiUrl + 'Admin/Catalog/Stores' + '?_=' + new Date().getTime(),
            params: params
        };

        $http(req).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };

    //创建商店
    this.createStore = function (store) {       
        var deferred = $q.defer();

        $http.post($rootScope.apiUrl + 'Admin/catalog/stores', store).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };

    //更新商店
    this.updateStore = function (store) {
        var deferred = $q.defer();

        $http.put($rootScope.apiUrl + 'Admin/catalog/stores/' + store.Id,store).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };

    //获取商店
    this.getStoreById = function (store_id ){
        var deferred = $q.defer();

        $http.get($rootScope.apiUrl + 'Admin/catalog/stores/' + store_id).success(function (data, status) {
            deferred.resolve(data, status);
        }).error(function (data, status) {
            deferred.reject("error");
        });

        return deferred.promise;
    };

    //删除商家
    this.deleteStore = function (storeId) {
        var deferred = $q.defer();

        $http.delete($rootScope.apiUrl + 'Admin/catalog/stores/' + storeId).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };

    //批量删除商店
    this.deleteStoreByIds = function (store_ids) {
        var deferred = $q.defer();

        $http.delete($rootScope.apiUrl + 'Admin/catalog/stores/' + store_ids).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };
}]);
 