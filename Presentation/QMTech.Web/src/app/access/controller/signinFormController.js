'use strict';

angular.module("app").controller('signinFormController', ['$scope', '$rootScope', '$location', '$http', '$state', 'w5cValidator', 'toaster', 'authService', function ($scope, $rootScope, $location, $http, $state, w5cValidator, toaster, authService) {

    $scope.processing = false;
    $scope.user = {};
    $scope.authError = null;

    //验证规则
    w5cValidator.setRules({
        account: {
            required: "登录账户不能为空"
        },
        password: {
            required: "密码不能为空"
        }
    });

    //每个表单的配置，如果不设置，默认和全局配置相同
    var validation = $scope.validation = {
        options: {
            blurTrig: true
        }
    };

    //保存实体
    validation.saveEntity = function ($event, continueEditing) {
        $scope.processing = true;

        var loginData = {
            account: $scope.user.account,
            password: $scope.user.password,
            isAdmin: true
        };

        authService.login(loginData).then(function (data) {
            toaster.pop('success', '', '登录成功');
            $scope.authError = "";
            // 获取url参数  
            var backUrl = $location.search().backUrl;
            if (backUrl != null && backUrl != undefined && backUrl != "") {
                window.location.href = backUrl;
            }
            else {
                $state.go('app.dashboard');
            }
        }, function (err) {
            $scope.authError = err.error_description;
            toaster.pop('error', '', '登录失败：' + err.error_description);
        }).finally(function () {
            $scope.processing = false;
        });

       // $http.post(loginUrl, {
       //     account: $scope.user.account,
       //     password: $scope.user.password
       // })
       //.then(function (response) {

               
       //}, function (response) {
       //    $scope.authError = '登录失败：' + response.data.message;
       //    toaster.pop('error', '登录失败', response.data.message, 3000);
       //})
       //.finally(function () {
       //    $scope.processing = false;
       //});
    };
}]);