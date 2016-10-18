"use strict";

angular.module("app").controller("EmployeeCreateOrEditCtrl", ['$scope', '$rootScope', '$state', '$http', 'employeeService', 'departmentService', function ($scope, $rootScope, $state, $http, employeeService, departmentService) {

    $scope.processing = false;
    $scope.depts = [];

    $scope.employee = {
        Name: "",
        BirthDay:new Date(),
        IdCard: "",
        Mobile: "",
        DepartmentId: 0,
        EntryDate: new Date(),
        LeaveDate: new Date(),
        Actived: true
    };

  

    var url = $rootScope.apiUrl + "department/getall";

    departmentService.getDepartments().then(function (data,status) {
        $scope.depts = data;

        //如果是编辑页面，获取employee对象
        if ($scope.employeeId != undefined) {
            employeeService.getEmployeeById($scope.employeeId).then(function (data, status) {
                $scope.employee = data;

                //设置选中的部门
                angular.forEach( $scope.depts,function (key,value) {

                    if (key.Id == $scope.employee.DepartmentId) {
                        $scope.depts.selected = key;
                        return false;
                    }
                }); 

                if ($scope.employee.LeaveDate == "") $scope.employee.LeaveDate = new Date();

            }, function (message) {
                $scope.error = message;

            }).finally(function () {

            });
        }

    }, function () { });

    $scope.setDepartment = function ($item, $model) {
        $scope.employee.DepartmentId = $item.Id;
    };

    //#region 日期相关

    var dateOptions = {
        formatYear: 'yyyy',
        startingDay: 1,
        class: 'datepicker', 
        showWeeks: true
    };

    $scope.birthDay = {
        opened: false,
        dateOptions: dateOptions
    };

    $scope.entryDate = {
        opened: false,
        dateOptions: dateOptions
    };

    $scope.leaveDate = {
        opened: false,
        dateOptions: dateOptions
    };

    $scope.open = function ($event, item) {
        $event.preventDefault();
        $event.stopPropagation();

        //关闭其他的日期选择器
        $scope.birthDay.opened = false;
        $scope.entryDate.opened = false;
        $scope.leaveDate.opened = false;

        $scope[item].opened = true;
    };
  
    $scope.format = 'yyyy-MM-dd';

    //入职日期大于出生日期
    $scope.moreThenBirth = function (value,valueIsEnter) {       
        var v = new Date(value);
        valueIsEnter = valueIsEnter == "true";

        var b =valueIsEnter ? new Date($scope.employee.BirthDay) : v;
        var e = valueIsEnter ? v : new Date($scope.employee.EntryDate);

        return b < e;       
    };

    $scope.lessThenLeave = function (value, valueIsEnter) {
        var v = new Date(value);
        valueIsEnter = valueIsEnter == "true";

        var e = valueIsEnter ? v : new Date($scope.employee.EntryDate);
        var l = valueIsEnter ? new Date($scope.employee.LeaveDate) : v;

        if ($scope.employee.Actived) return true;
        else return e < l;
    };

    //#endregion

    //表单提交
    $scope.processForm = function () {
        $scope.processing = true;

        //如果职工为在职状态，离职日期清空
        if ($scope.employee.Actived) $scope.employee.LeaveDate = "";

        if ($scope.employeeId == undefined) {         

            employeeService.createEmployee($scope.employee).then(function (data, status) {
                $state.go('app.personnel.employee.list');
                $scope.error = "";
            }, function (message) {
                $scope.error = message;
            }).finally(function () {
                $scope.processing = false;
            });
        }
        else {
            employeeService.updateEmployee($scope.employee).then(function (data, status) {
                //$state.go('app.personnel.employee.detail', { id: $scope.employeeId });
                $scope.employee = data;

                $scope.error = "";
            }, function (message) {
                $scope.error = message;
            }).finally(function () {
                $scope.processing = false;
            });
        }



    }; 
}]);
 