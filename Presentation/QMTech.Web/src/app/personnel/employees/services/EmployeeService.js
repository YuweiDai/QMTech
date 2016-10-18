"use strict";

//员工服务
app.service("employeeService", ['$rootScope', '$http', '$q', function ($rootScope, $http, $q) {


    //创建员工
    this.createEmployee = function (employee) {       
        var deferred = $q.defer();

        $http.post($rootScope.apiUrl + 'employees', employee).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };

    //更新员工
    this.updateEmployee = function (employee) {
        var deferred = $q.defer();

        $http.put($rootScope.apiUrl + 'employees/' + employee.Id,employee).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };

    //获取员工
    this.getEmployeeById = function (employee_id ){
        var deferred = $q.defer();

        $http.get($rootScope.apiUrl + 'employees/' + employee_id).success(function (data, status) {
            deferred.resolve(data, status);
        }).error(function (data, status) {
            deferred.reject("error");
        });

        return deferred.promise;
    };

    //批量删除员工
    this.deleteEmployeeByIds = function (employee_ids) {
        var deferred = $q.defer();

        $http.delete($rootScope.apiUrl + 'employees/' + employee_ids).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };

}]);
 