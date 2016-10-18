"use strict";

//部门服务
app.service("departmentService", ['$rootScope', '$http', '$q', function ($rootScope, $http, $q) {

    //获取部门
    this.getDepartments = function (employee_id) {
        var deferred = $q.defer();

        $http.get($rootScope.apiUrl + "department/getall").success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };

}]);
