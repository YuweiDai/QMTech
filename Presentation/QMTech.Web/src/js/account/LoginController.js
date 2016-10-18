'use strict';

angular.module("app").controller('LoginController', ['$scope', '$http', '$state', function ($scope, $http, $state) {

    $scope.user = {};
    $scope.authError = null;
    $scope.login = function () {
        // Try to create
        $http.post('api/customer/login', {
                email: $scope.user.email,
                password: $scope.user.password
        })
        .then(function (response) {
			console.log(response);
            //if (response.status == 200) {
            //    window.Location = "/Admin";
           //} else {
           //     $scope.authError = response;
          //  }
        }, function (x) {
            $scope.authError = 'Server Error';
            //123123
        });
    };

}]);