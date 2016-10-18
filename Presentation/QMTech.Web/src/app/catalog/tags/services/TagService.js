"use strict";

//类别服务
app.service("TagService", ['$rootScope', '$http', '$q', function ($rootScope, $http, $q) {

    //获取当前商品可选的标签
    this.queryProudctTags = function (search) {
        var deferred = $q.defer();
        var url = $rootScope.apiUrl + "admin/catalog/ProductTags";
        var params = { search: search };

        $http.get(url, { params: params }).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };


}]);