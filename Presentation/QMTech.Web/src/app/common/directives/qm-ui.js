'use strict';

/*
author:Yuwei Dai
date:2016.1.21
desc:自定义UI组件
*/
angular.module('qm.ui', ['qm.ui.tables']);
//表格组件
angular.module("qm.ui.tables", []).constant('tableConfig', {
    //每行自动生成 checkbox，
    enableSelected: true,
    enableSearch: true,
    page: {
        length: 10,
        start: 0,
        index: 0
    },
    pageSizeOptions: [10, 15, 20]
}).controller('TableController', ['$http', '$scope', '$attrs', 'tableConfig', function ($http, $scope, $attrs, tableConfig) {
      
    if ($scope.tableConfig == null) $scope.tableConfig = tableConfig;
    //页面总数
    $scope.tableConfig.page.total = 0;
    $scope.tableConfig.page.allRecords = 0;
    //页面集合
    $scope.tableConfig.page.list = [];

    //重置参数
    $scope.reset = false;

    //排序
    $scope.order = {};
    $scope.processing = false;
    $scope.searchValue = "";
    $scope.rows = [];


    //设置参数
    $scope.setParams = function (reset) {
        if ($scope.reset) reset = true;
        if (reset) $scope.tableConfig.page.index = 0;

        $scope.tableConfig.page.start = $scope.tableConfig.page.index * $scope.tableConfig.page.length;

        var sortConditions = "";

        angular.forEach($scope.columns, function (key, value) {
            //排序集合对象
            if (key.dir) {
                sortConditions += key.name + "," + key.dir + ";";
            }
        });

        var paramss = angular.copy($scope.params);
        angular.extend(paramss, { sort: sortConditions, pageSize: $scope.tableConfig.page.length, pageIndex: $scope.tableConfig.page.index });

        if ($scope.searchValue != undefined && $scope.searchValue != null && $scope.searchValue != "")
            paramss.query = $scope.searchValue;
        else
            paramss.query = null;

        $scope.params = paramss;
    };

    $scope.setParams();

    //翻页
    //pageIndex 从0开始
    $scope.flipPage = function ($pageIndex) {
        if ($pageIndex < 0 || $pageIndex >= $scope.tableConfig.page.total) return;
        $scope.tableConfig.page.index = $pageIndex;
        $scope.setParams();
    };

    //设置列排序
    $scope.setOrder = function (column, reset) {

        //如果当前列处于排序中，直接在升序 降序中切换。反之，遍历取消当前排序的列，当前列默认先升序
        if (column.dir == "asc" || column.dir == "desc") {
            column.dir = column.dir == "asc" ? "desc" : "asc";
        }
        else {
            angular.forEach($scope.columns, function (key, value) {
                if (key.dir == "asc" || key.dir == "desc")
                    key.dir = ""

                if (column.name == key.name) key.dir = "asc";
            });
        }
        $scope.setParams(reset);
    };

    //#region 行选择

    //选中行数目
    $scope.selectedRowCount = 0;
    //选中所有
    $scope.selectAll = false;

    //批量设置行选中
    $scope.setRowSelectiton = function (selected) {
        $scope.selectAll = selected;
        angular.forEach($scope.rows, function (item, value) {
            item.selected = selected;
        });

        $scope.selectedRowCount = selected ? $scope.rows.length : 0;
    };

    //选中切换
    $scope.switchSelection = function () {
        var count = 0;
        angular.forEach($scope.rows, function (item, value) {
            item.selected = !item.selected;
            if (item.selected) count++;
        });
        $scope.selectedRowCount = count;
    };

    //监测每行选中状态变化
    $scope.selectedRowChange = function (row) {
        if (row.selected)
            $scope.selectedRowCount++;
        else
            $scope.selectedRowCount--;
    };

    //监测选中数目
    $scope.$watch('selectedRowCount', function (newValue, oldValue) {
        if ($scope.rows.length == 0) return;
        $scope.selectAll = $scope.selectedRowCount == $scope.rows.length;
    });

    //#endregion

    //监测参数变化，深度监测
    $scope.$watch("params", function (newValue, oldValue) {
        $scope.processData();
    }, true);

    //请求数据
    $scope.processData = function () {

        $scope.processing = true;

        //利用js 闭包，记录当前查询的值
        var q = $scope.searchValue;

        var req = {
            method: $scope.ajax.method,
            url: $scope.ajax.url + '?_=' + new Date().getTime(),
            params: $scope.params
        };

        $http(req).success(function (response, status) {
            $scope.processing = false;

            //如果当前查询的值不等于临时记录的值，则返回
            if (q != $scope.searchValue) return;

            $scope.tableConfig.page.total = Math.ceil(response.paging.total / tableConfig.page.length);
            $scope.tableConfig.page.allRecords = response.paging.total;

            //计算页面集合         
            var i = -2;
            $scope.tableConfig.page.list = [];

            //最多显示5页
            while ($scope.tableConfig.page.list.length < 5 && $scope.tableConfig.page.list.length < $scope.tableConfig.page.total) {

                var p = $scope.tableConfig.page.index + i;
                if (p > 0) {
                    $scope.tableConfig.page.list.push(p);
                }

                i++;
            }
            $scope.response = response;
            $scope.rows = response.data;
            $scope.selectedRowCount = 0;
        }).error(function (response, status) {
            $scope.processing = false;
        });

        $scope.reset = false;
    };

    //console.log($scope.extButtons);

}]).directive('qmtable', function () {
    return {
        restrict: 'AE',
        scope: {
            params: "=",
            response: "=",
            columns: "=",
            ajax: "=",
            tableConfig: "=",
            extButtons: "=",
            tableEidtAndDelete: "=",
            reset: "=",  //强制重置参数
        },
        replace: true,
        templateUrl: 'app/common/directives/qm-ui/table.html',
        controller: "TableController"
    };
});