"use strict";

//商品类别
app.service("ProductService", ["$rootScope", "$http", "$q", function ($rootScope,$http,$q) {
    this.baseUrl = $rootScope.apiUrl + "Admin/Catalog/Stores/";

    //获取商品列表
    this.getAllProducts = function (params) {
        var deferred = $q.defer();

        var req = {
            method: "get",
            url: this.baseUrl + storeId + "/Products?_=" + new Date().getTime(),
            params: params
        };

        $http(req).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };

    //获取商品
    this.getProductById = function (storeId, productId) {
        var deferred = $q.defer();
        var url = this.baseUrl + storeId + "/Products/"+productId;
        $http.get(url).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message, status);
        });

        return deferred.promise;
    };

    //增加商品
    this.createProduct = function (storeId, product) {
        var deferred = $q.defer();
        var url = this.baseUrl + storeId + "/Products";
        $http.post(url, product).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };

    //删除商品
    this.deleteProduct = function (storeId,product_id) {
        var deferred = $q.defer();
        var url = this.baseUrl + storeId + "/Products/" + product_id;
        $http.delete(url).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };

    //更新商品
    this.updateProduct = function (storeId,product) {
        var deferred = $q.defer();
        var url = this.baseUrl + storeId + "/Products/" + product.Id;
        $http.put(url, product).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };

    this.deleteProductByIds = function (storeId,product_ids) {
        var deferred = $q.defer();

        $http.delete(this.baseUrl + storeId + '/Products/' + product_ids).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };


}]);