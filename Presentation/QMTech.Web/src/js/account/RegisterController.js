'use strict';

//register controller
angular.module("app").controller('RegisterController', ['$scope', '$http', '$state', function ($scope, $http, $state) {

    $scope.user = {};
    $scope.authError = null;
    $scope.register = function () {
        // Try to create
        $http.post('api/customer/register', {
                username: $scope.user.name,
                email: $scope.user.email,
                password: $scope.user.password
        })
        .then(function (response) {
            if (response.status == 200) {
                window.Location = "/Admin";
            } else {
                $scope.authError = response;
            }
        }, function (x) {
            $scope.authError = 'Server Error';
        });
    };

}]);
