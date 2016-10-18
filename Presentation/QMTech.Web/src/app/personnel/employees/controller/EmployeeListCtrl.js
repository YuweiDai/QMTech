//职工列表控制器
'use strict';
 
app.controller('EmployeeListCtrl', ['$rootScope', '$uibModal', '$state', '$scope', 'employeeService', function ($rootScope, $uibModal, $state, $scope, employeeService) {

    //ajax获取数据
    $scope.ajax = {
        url: $rootScope.apiUrl + 'employees',
        method: "Get"
    };

    //列集合
    $scope.columns = [{
        title: "工号",
        width: 100,
        name: "Number",
        orderable: true,
        dir: "asc"
    }, {
        title: "姓名",
        name: "Name",
        orderable: true,
        state: 'app.personnel.employee.detail'
    }, {
        title: "部门",
        width: 200,
        name: "DepartmentName",
        orderable: true
    }, {
        title: "手机",
        width: 200,
        name: "Mobile",
        orderable: true
    }, {
        title: "状态",
        width: 150,
        name: "StatusDes",
        orderable: true
    }];

    $scope.editState = "app.personnel.employee.edit";

    //删除表格行
    $scope.deleteRows = function (rows) {

        var idStr = "";
        var content = "";


        //遍历生成idStr
        if (rows.length == 1) {
            idStr = rows[0].Id;
            content = "是否删除职工《" + rows[0].Name + "》？";
        }
        else if (rows.length > 1) {
            content = "是否删除当前选中的职工？";

            angular.forEach(rows, function (value, index) {
                if (value.selected) idStr += value.Id + '_';
            });

        }

        var modalInstance = $uibModal.open({
            templateUrl: 'deleteRow.html',
            controller: 'DeleteEmployeeModalCtrl',
            resolve: {
                items: function () {
                    return $scope.items;
                },
                content: function () {
                    return content;
                },
            }
        });

        modalInstance.result.then(function (selectedItem) {

            //删除职工
            employeeService.deleteEmployeeByIds(idStr).then(function (data, status) { }, function (message) { alert(message); }).finally(function () {
                $state.reload();
            });

        }, function () { });
    };

}]);


app.controller('DeleteEmployeeModalCtrl', function ($scope, $uibModalInstance, content) {

    $scope.title = "消息";
    $scope.content = content;
    $scope.okText = "确定";
    $scope.cancelText = "取消";


    $scope.ok = function () {
        $uibModalInstance.close();
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss();
    };
});