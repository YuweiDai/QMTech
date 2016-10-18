"use strict";

app.controller("StoreCtrl", ['$scope', '$rootScope', '$uibModal', '$state', 'StoreService',
    function ($scope, $rootScope,$uibModal, $state, storeService) {


    var store = $scope.store = {
        id: 0,
        name: "",
        CategoryId: 0,
        Address: "",
        Logo: "",
        displayOrder: 0,
        published: true,
        allOpened: false,
        startTime: null,
        endTime: null,
        selfSupport: false,
        person: "",
        personTel: "",
        salesNumber: "",
        description: "",
        metaTitle: "",
        metaKeywords: "",
        metaDescription: "",
        lat: 0,
        lon: 0
    };

    if ($state.current.name == "app.catalog.store.detail")
        $state.go("app.catalog.store.detail.overview");


    if ($scope.storeId != undefined) {
        storeService.getStoreById($scope.storeId).then(function (data) {
            store = $scope.store = data;
        }, function (message) {

            $state.go("app.catalog.store.list");
        });
    }
    else
        $state.go("app.catalog.store.list");


    $scope.deleteStore = function () {

        var modalInstance = $uibModal.open({
            templateUrl: 'dialog.html',
            controller: 'ModalDialogCtrl',
            resolve: {
                title: function () { return "提示"; },
                content: function () { return "是否删除商家？"; }
            }
        });

        modalInstance.result.then(function () {
            storeService.deleteStore($scope.storeId).then(function () {
                $state.go("app.catalog.store.list");
            }, function (msg, status) {
                $scope.errorMsg = msg;
            });
        }, function () {
            $state.reload();
        });

    };

}]);