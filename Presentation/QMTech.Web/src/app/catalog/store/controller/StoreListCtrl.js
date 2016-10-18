//商家列表控制器
'use strict';
 
app.controller('StoreListCtrl', ['$rootScope', '$uibModal', '$state', '$scope', 'StoreService',
    function ($rootScope, $uibModal, $state, $scope, storeService) {
    var error = $scope.error = "";
    var response = $scope.response = null;

    //自定义参数集合
    $scope.params = { showHidden: false, pageSize: 15 };
        //选中的行
    $scope.selectedItem = [];

    //#region table config

    //ajax获取数据
    $scope.ajax = {
        url: $rootScope.apiUrl + 'Admin/Catalog/Stores',
        method: "Get"
    };
 
    //列集合
    $scope.columns = [{
        type:'link',
        title: "名称",
        name: "name",
        orderable: true,
        click: function (storeId) {
            $state.go("app.catalog.store.detail.overview", { id: storeId });
        },
    }, {
        title: "类别",
        width: 250,
        name: "storeCategory",
        orderable: true
    }, {
        title: "地址",
        width: 200,
        name: "address",
        orderable: true
    }, {
        title: "联系方式",
        width: 150,
        name: "tel",
        orderable: true,
        alignCenter: true
    },
 {
     type: 'bool',
     title: "自营",
     width: 100,
     name: "selfSupport",
     orderable: true,
     alignCenter: true
 }, {
     type: 'bool',
     title: "营业中",
     width: 100,
     name: "opend",
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
        edit: function (storeId) {
            $state.go("app.catalog.store.edit", { id: storeId });
        },
        del: function (rows) {
            var idStr = "";
            var content = "";

            //遍历生成idStr
            if (rows.length == 1) {
                idStr = rows[0].Id;
                content = "是否删除商家《" + rows[0].Name + "》？";
            }
            else if (rows.length > 1) {
                content = "是否删除当前选中的商家？";

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

                //删除商家
                storeService.deleteStoreByIds(idStr).then(function (data, status) { }, function (message) { alert(message); }).finally(function () {
                    $state.reload();
                });

            }, function () { });
        }
    };

    //工具扩展
    $scope.extButtons = [
        {
            caption: ' <span class="text">加载未发布的商家</span><span class="text-active">移除未发布的商家</span>',
            checked: true,
            toggle: "btn-warning",
            click: function () {
                $scope.reset = true;
                $scope.params.showHidden = !$scope.params.showHidden;
            }
        }
    ];

//删除选中的
    $scope.deleteSelectedStores = function (rows) {

        var modalInstance = $uibModal.open({
            templateUrl: 'dialog.html',
            controller: 'ModalDialogCtrl',
            resolve: {
                title: function () { return "提示"; },
                content: function () { return "是否删除选中的商家？"; }
            }
        });

        modalInstance.result.then(function () {

            var idString = "";
            angular.forEach(rows, function (item, i) {
                if (item.selected) {
                    idString += item.id;

                    if (i < rows.length - 1) idString += ',';
                }
            });

            storeService.deleteStoreByIds(idString).then(function () {
           
            }, function (msg, status) {
 
            });
        }, function () {
            alert("刷新页面");
            $state.reload();
        });


    };

    
    $scope.reset = true;

    //#endregion    
}]);