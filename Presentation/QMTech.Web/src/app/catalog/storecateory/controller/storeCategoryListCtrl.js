//部门列表控制器
'use strict'

app.controller('storeCategoryListCtrl', ['$scope', '$timeout','w5cValidator', '$uibModal', 'StoreCategoryService',
    function ($scope, $timeout, w5cValidator,$uibModal, storeCategoryService) {
        $scope.error = "";
        $scope.loadUnPublised = false;    //是否加载未发布的
        $scope.my_tree = {};   //目录树对象
        $scope.my_data = [];   //目录树数据
        $scope.selectedCategory = null;
        $scope.nameDefaultValue = "";   //原始name值
        $scope.initial_selection = null;

        //刷新
        $scope.refresh = function (clearInitialSelection) {
            if (clearInitialSelection) $scope.initial_selection = null;
            $scope.selectedCategory = null;
            $scope.doing_async = true;
            $scope.my_data = [];
            //清空错误
            $scope.error = "";

            //加载目录
            storeCategoryService.loadTreeData($scope.loadUnPublised).then(function (data, status) {
                if ($scope.initial_selection == null)
                    $scope.initial_selection = data.length > 0 ? data[0].label : null;
                $scope.my_data = data;
                $scope.doing_async = false;

                //延迟选中
                $timeout(function () { $scope.my_tree.select_branch_by_label($scope.initial_selection); }, 1);
            }, function (msg, status) {
                $scope.error = msg;
            });
        };

        //目录树节点选中事件
        $scope.my_tree_handler = function (branch) {
          
            //清空错误
            $scope.error = "";

            //表单重置
            $scope.selectedCategory = null;
                                                   
            $timeout(function () {
                $scope.selectedCategory = angular.copy(branch.data);
                $scope.nameDefaultValue = branch.data.id == undefined ? "" : $scope.selectedCategory.name;
                if (branch.children.length > 0) {
                    $scope.my_tree.expand_branch();
                }
            }, 1);

        };

        //增加新节点
        $scope.adding_a_branch = function () {
            var b_new_node = null;

            var nextDisplayOrder = 0;
            var b = $scope.my_tree.get_selected_branch();

            if (b == null) {
                //#region 根节点

                b_new_node = $scope.my_tree.add_branch(null, {
                    label: '新增类别',
                    data: {
                        name: '新增类别',
                        displayOrder: nextDisplayOrder,
                        description: "描述",
                        parentId: 0,
                        published: true
                    }
                });

                //#endregio
            }
            else {
                if (b.data.id == undefined) {
                    var modalInstance = $uibModal.open({
                        templateUrl: 'dialog.html',
                        controller: 'ModalDialogCtrl',
                        resolve: {
                            title: function () { return "提示"; },
                            content: function () { return "必须保存当前类别 《" + b.label + "》后，才能在此节点下增加节点"; }
                        }
                    });

                    modalInstance.result.then(function (selectedItem) { }, function () { });

                    return;
                }

                var children = $scope.my_tree.get_children(b);
                if (children.length > 0)
                    nextDisplayOrder = children[children.length - 1].data.displayOrder + 1;

                b_new_node = $scope.my_tree.add_branch(b, {
                    label: '新增类别',
                    data: {
                        name: '新增类别',
                        displayOrder: nextDisplayOrder,
                        description: "描述",
                        parentId: b.data.id,
                        published: true,
                        storeId: 0
                    }
                });
            }



            //选中
            $scope.my_tree.select_branch(b_new_node);

            return b_new_node;
        };

        //删除节点
        $scope.removing_a_branch = function (size) {
            var b = $scope.my_tree.get_selected_branch();

            var modalInstance = $uibModal.open({
                templateUrl: 'dialog.html',
                controller: 'ModalDialogCtrl',
                size: size,
                resolve: {
                    title: function () { return "提示"; },
                    content: function () { return "是否删除类别 《" + b.label + "》？"; }
                }
            });

            modalInstance.result.then(function (selectedItem) {

                if (b.data.id != undefined) {
                    storeCategoryService.deleteStoreCategory(b.data.id).then(function () {

                        //获取父节点
                        var p = $scope.my_tree.get_parent_branch(b);
                        if (p != null) $scope.initial_selection = p.label;
                        else $scope.my_tree.select_first_branch();
                        $scope.refresh();
                    }, function (msg, status) {
                        $scope.error = msg;
                    });
                }
                else {

                    b.deleted = true;
                    var p = $scope.my_tree.get_parent_branch(b);
                    if (p != null) $scope.my_tree.select_branch(p);
                    else {
                        var firtst = $scope.my_tree.get_first_branch();
                        if (firtst != null) $scope.initial_selection = firtst.label;
                        else $scope.initial_selection = "";
                    }
                }
            }, function () { });
        };

        //删除选中
        $scope.clearSelection = function () {
            $scope.my_tree.select_branch(null);
            $scope.selectedCategory = null;
        };

        //保存类别
        $scope.saveCategory = function () {

            var b = $scope.my_tree.get_selected_branch();
            if (b == null) {
            }
            else
            {
                $scope.init = b;



                if (b.data.id == undefined) {
                    //#region 新增类别
                    storeCategoryService.createCategory($scope.selectedCategory).then(function (data, message) {
                        if (data.published || $scope.loadUnPublised) {
                            $scope.initial_selection = data.name + (data.published ? "" : "（未发布）");
                        }
                        else {
                            //获取父节点
                            var p = $scope.my_tree.get_parent_branch(b);
                            $scope.initial_selection = p.label;
                        }

                        //刷新
                        $scope.refresh();
                    }, function (msg, status) {
                        $scope.error = msg;
                    }).finally(function () { });

                    //#endregion
                }
                else {
                    //#region 更新
                    storeCategoryService.updateCategory($scope.selectedCategory).then(function (data, message) {
                        if (data.published || $scope.loadUnPublised) {
                            $scope.initial_selection = data.Name + (data.published ? "" : "（未发布）");
                        }
                        else {
                            //获取父节点
                            var p = $scope.my_tree.get_parent_branch(b);
                            if (p != null) $scope.initial_selection = p.label;
                            else {
                                //获取第一个节点
                                var first = $scope.my_tree.get_first_branch();
                                if (first != null) $scope.initial_selection = first.label;
                                else $scope.initial_selection = "";
                            }
                        }

                        //刷新
                        $scope.refresh();
                    }, function (msg, status) {
                        $scope.error = msg;
                    }).finally(function () {

                    });
                    //#endregion
                }
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
        var vm = $scope.vm = {             
            validateOptions: {
                blurTrig: true
            }
        };
       
        //#endregion

        //清空错误
        $scope.clearError = function (index) {        
            $scope.error = "";
        };

        //初始化操作
        return $scope.refresh();

    }]);