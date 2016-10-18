"use strict";

app.controller("StoreProductCategoryCtrl", ['$scope', '$timeout', '$rootScope', '$state', 'w5cValidator', '$uibModal',  'ProductCategoryService',
function ($scope,$timeout, $rootScope, $state,w5cValidator, $uibModal,productCategoryService) {
    var newCategory = null;                   //新增的类别
    $scope.processing = false;    
    $scope.selectedCategory = null;      //当前选中的类别
    $scope.showHidden = true;
    $scope.nameDefaultValue = "";       //原始name值
    $scope.productCategories = [];
    $scope.initial_selection = null;         //初始选中值
    $scope.operationSuccess = false;

    //刷新
    $scope.refresh = function (isProcess) {
        $scope.processing = true;

        //加载商品分类
        productCategoryService.getProductCategories($scope.storeId, $scope.showHidden).then(function (data) {
            $scope.productCategories = data;

            //表示这个是操作完成
            if (isProcess) {
                $scope.operationSuccess = true;
                $timeout(function () { $scope.operationSuccess = false; }, 500);
            }

            return $scope.initialSelection();
        }, function (message) {
            alert("error:" + message);
        }).finally(function () {
            $scope.processing = false;
        });
    };

    //选中类别
    $scope.selectCategory = function (category) {
        if ($scope.selectedCategory != null)
        {
            angular.forEach($scope.productCategories, function (v, i) {
                v.selected = false;
            });
        }

        //表单重置
        $scope.selectedCategory = null;

        //延迟，原因不明，不做延迟不会触发事件验证
        $timeout(function () {
            category.selected = true;
            $scope.selectedCategory = angular.copy(category);
            //默认值
            $scope.nameDefaultValue = category.id == undefined ? "" : category.name;
        }, 1);       
    };

    //新增类别
    $scope.addCategory = function () {
        if (newCategory != null) {
            var modalInstance = $uibModal.open({
                templateUrl: 'dialog.html',
                controller: 'ModalDialogCtrl',
                resolve: {
                    title: function () { return "提示"; },
                    content: function () { return "必须保存当前类别 《" + newCategory.name + "》后，才能继续添加类别"; }
                }
            });

            modalInstance.result.then(function () { }, function () { });

            return;
        }
        else {
            newCategory = {
                name: "新增类别",
                displayOrder: $scope.productCategories.length,
                description: "",
                published: true,
                storeId: $scope.storeId
            };
           
            $scope.productCategories.push(newCategory);
            $scope.selectCategory(newCategory);
        }

    };

    //保存
    $scope.saveCategory = function () {

        if ($scope.selectedCategory != null)
        {
            $scope.processing = true;

            if($scope.selectedCategory.id==undefined)
            {
                //#region 新增
                productCategoryService.createProductCategory($scope.storeId, $scope.selectedCategory).then(function (data, message) {
                    newCategory = null;
                    $scope.selectedCategory = data;
                    $scope.initial_selection = data.id;              
                    $scope.refresh(true);
                }, function (msg, status) {
                    $scope.error = msg;
                }).finally(function () {
                    $scope.processing = false;
                });
                //#endregion
            }
            else
            {
                //#region 更新

                productCategoryService.updateProductCategory( $scope.selectedCategory).then(function (data, message) {
                    $scope.selectedCategory = data;
                    $scope.initial_selection = data.id;
                    $scope.refresh(true);
                }, function (msg, status) {
                    $scope.error = msg;
                }).finally(function () {
                    $scope.processing = false;
                });

                //#endregion
            }
        }
     
    };

    //删除类别
    $scope.deleteCategory = function (category,index) {
        if(category.id==undefined)
        {
            $scope.productCategories.splice(index, 1);
            newCategory = null;
            $scope.initialSelection();
        }
        else
        {
            var modalInstance = $uibModal.open({
                templateUrl: 'dialog.html',
                controller: 'ModalDialogCtrl',
                resolve: {
                    title: function () { return "提示"; },
                    content: function () { return "是否删除类别 《" + category.name + "》？"; }
                }
            });

            modalInstance.result.then(function () {

                productCategoryService.deleteProductCategory( category.id).then(function () {
                    $scope.initial_selection = null;
                    $scope.refresh(true);
                }, function (msg, status) {
                    $scope.error = msg;
                });
            }, function () { });

            return;
        }

    };

    //初始化选择类别
    $scope.initialSelection = function () {
        if ($scope.initial_selection == null)
        {
            if ($scope.productCategories.length > 0) $scope.selectCategory($scope.productCategories[0]);
        }
    
        else
        {
            angular.forEach($scope.productCategories, function (v, i) {
              
                if (v.id == $scope.initial_selection)
                    $scope.selectCategory(v);

            });
        }
    };

    //#region 验证配置

    w5cValidator.setRules({
        cname: {
            required: "商品分类名不能为空",
            w5cuniquecheck: "输入商品分类名已经存在，请重新输入"
        }
    });

    //每个表单的配置，如果不设置，默认和全局配置相同
     $scope.validation = {
        options: {
            blurTrig: true
        }
    };

    //#endregion

    return $scope.refresh(false);
}]);