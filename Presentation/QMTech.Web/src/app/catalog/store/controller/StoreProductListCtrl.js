"use strict";
//商品商品列表
app.controller("StoreProductListCtrl", ['$rootScope', '$uibModal', '$state', '$scope', 'ProductService',
    function ($rootScope, $uibModal, $state, $scope, productService) {
        var error = $scope.error = "";
        var response = $scope.response = null;

        //自定义参数集合
        $scope.params = { showHidden: false };

        //#region table config

        //ajax获取数据
        $scope.ajax = {
            url: $rootScope.apiUrl + 'Admin/Catalog/Stores/' + $scope.storeId + '/Products',
            method: "Get"
        };

        //列集合
        $scope.columns = [{
            type: 'link',
            title: "名称",
            name: "name",
            orderable: true,
            click: function (productId) {
                $state.go("app.catalog.store.detail.product.detail", { id: $scope.storeId, productId: productId });
            },
        }, {
            title: "类别",
            width: 250,
            name: "productCategory",
            orderable: true
        }, {
            title: "价格",
            width: 100,
            name: "price",
            orderable: true
        }, 
     {
         type: 'bool',
         title: "发布",
         width: 100,
         name: "published",
         orderable: true,
         alignCenter: true
     }, {
         type: 'bool',
         title: "新上市",
         width: 100,
         name: "isNew",
         alignCenter: true
     },
        {
            title: "显示次序",
            width: 100,
            name: "displayOrder",
            orderable: true,
            alignCenter: true,
            dir: "asc"
        },
        ];

        //编辑删除功能
        $scope.tableEidtAndDelete = {
            edit: function (productId) {
                $state.go("app.catalog.store.detail.product.edit", { id: $scope.storeId, productId: productId });
            },
            del: function (rows) {
                var idStr = "";
                var content = "";

                //遍历生成idStr
                if (rows.length == 1) {
                    idStr = rows[0].Id;
                    content = "是否删除商品《" + rows[0].Name + "》？";
                }
                else if (rows.length > 1) {
                    content = "是否删除当前选中的商品？";

                    angular.forEach(rows, function (value, index) {
                        if (value.selected) idStr += value.Id + '_';
                    });

                }

                var modalInstance = $uibModal.open({
                    templateUrl: 'deleteRow.html',
                    controller: 'ModalDialogCtrl',
                    resolve: {
                        title: function () { return "提示"; },
                        content: function () { return content; }
                    }
                });

                modalInstance.result.then(function () {

                    //删除商品
                    productService.deleteProductByIds($scope.storeId,idStr).then(function (data, status) { }, function (message) { alert(message); }).finally(function () {
                        $state.reload();
                    });

                }, function () { });
            }
        };

        //工具扩展
        $scope.extButtons = [
            {
                caption: ' <span class="text">加载未发布的商品</span><span class="text-active">移除未发布的商品</span>',
                checked: true,
                toggle: "btn-warning",
                click: function () {
                    $scope.reset = true;
                    $scope.params.showHidden = !$scope.params.showHidden;
                }
            }
        ];


        $scope.reset = true;

        //#endregion    
    }]);