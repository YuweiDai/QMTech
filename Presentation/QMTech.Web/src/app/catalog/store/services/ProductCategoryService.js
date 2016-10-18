"use strict";

//商品类别服务
app.service("ProductCategoryService", ['$rootScope', '$http', '$q', function ($rootScope, $http, $q) {
    this.baseUrl = $rootScope.apiUrl + "Admin/Catalog/Stores/";

    //获取商家商品类别
    this.getProductCategories = function (storeId, showHidden, selectList) {
        var deferred = $q.defer();
        var url = this.baseUrl + storeId + "/ProductCategories?showHidden=" +
        showHidden;
        if (selectList)
            url = url + '&selectelist=' + selectList;

        $http.get(url).success(function (data, status) {
            deferred.resolve(data, status);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };

    //增加类别
    this.createProductCategory = function (storeId, category) {
        var deferred = $q.defer();
        var url = this.baseUrl + storeId + "/ProductCategories";
        $http.post(url, category).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };

    //删除分类
    this.deleteProductCategory = function (category_id) {
        var deferred = $q.defer();
        var url = this.baseUrl +  "ProductCategories/" + category_id;
        $http.delete(url).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };

    //更新类别
    this.updateProductCategory = function (category) {
        var deferred = $q.defer();
        var url = this.baseUrl + category.StoreId + "/ProductCategories/" + category.Id;
        $http.put(url, category).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };

}]);
