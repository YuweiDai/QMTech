'use strict';

/*
author:Yuwei Dai
date:2016.1.12
desc:包装Jquery.DataTables组件
*/

app.directive('mytable', ['uiLoad', '$timeout', function (uiLoad, $timeout) {

    return {
        restrict: "A",
        link: function (scope, element, attrs) {
 
            setTimeout(function () {
                var options = eval(scope.options);

                var table = $(element).DataTable(options);

                scope.table = table;

                table.on('draw.dt', function () {
                    table.column(1, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                        cell.innerHTML = i + 1;
                    });
                }).draw();
            }, 10);
        }
    }

}]);