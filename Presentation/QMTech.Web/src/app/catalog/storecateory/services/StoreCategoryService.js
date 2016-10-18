"use strict";

//类别服务
app.service("StoreCategoryService", ['$rootScope', '$http', '$q', function ($rootScope, $http, $q) {

    //获取目录树
    this.loadTreeData = function (showHidden) {
        var deferred = $q.defer();
        var url = $rootScope.apiUrl + "Admin/catalog/storeCategory/treeview";
        if (showHidden)
            url += "?showHidden=true";

        $http.get(url).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };

    //增加类别
    this.createCategory = function (category) {
        var deferred = $q.defer();

        $http.post($rootScope.apiUrl + 'Admin/catalog/storeCategory', category).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };

    //删除分类
    this.deleteStoreCategory = function (category_id) {
        var deferred = $q.defer();

        $http.delete($rootScope.apiUrl + "Admin/catalog/storeCategory/"+category_id).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };

    //更新类别
    this.updateCategory = function (category) {
        var deferred = $q.defer();

        $http.put($rootScope.apiUrl + "Admin/catalog/storeCategory/" + category.Id, category).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };


    //获取下拉列表集合
    this.loadSelectListItems = function (showHidden) {
        var deferred = $q.defer();
        var url = $rootScope.apiUrl + "Admin/catalog/storeCategory/selectlist";
        if (showHidden)
            url += "?showHidden=true";

        $http.get(url).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    }

}]);
