//职工控制器
'use strict';
 
app.controller('EmployeeCtrl', ['$scope', '$uibModal', '$state', 'employeeService', function ($scope, $uibModal, $state,employeeService) {
    $scope.deleteDisabled = true;

    employeeService.getEmployeeById($scope.employeeId).then(function (data, status) {
        $scope.employee = data;

        $scope.deleteDisabled = false;
        $scope.open = function (size) {
            var modalInstance = $uibModal.open({
                templateUrl: 'deleteEmployees.html',
                controller: 'DeleteEmployeeModalCtrl',
                size: size
            });

            modalInstance.result.then(function (selectedItem) {

                //删除职工
                employeeService.deleteEmployeeByIds($scope.employee.Id).then(function (data, status) {

                }, function (message) { alert(message); }).finally(function () {
                    $state.go('app.personnel.employee.list');
                });

            }, function () {
            });
        };

    }, function (message, status) {

        $state.go('app.personnel.employee.list');
        $scope.error = message;
        $scope.deleteDisabled = true;
    }).finally(function () {
        
    });
}]);

app.controller('DeleteEmployeeModalCtrl', function ($scope, $uibModalInstance) {

    $scope.title = "消息";
    $scope.content = "是否删除当前职工？";
    $scope.okText = "确定";
    $scope.cancelText = "取消";


    $scope.ok = function () {       
        $uibModalInstance.close();
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss();
    };
});