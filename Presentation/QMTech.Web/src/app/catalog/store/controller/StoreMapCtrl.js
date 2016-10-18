"use strict";

app.controller("StoreMapCtrl", ['$scope', '$rootScope', '$state',
    function ($scope, $rootScope, $state) {

        var map = $scope.map = {
            instance: null,
            plugin: {
                geocoder: null,
                placeSearch: null
            }
        };
        var marker = $scope.marker = null;


        //高德地图初始化
        $scope.initMap = function () {
            map.instance = new AMap.Map('mapDiv', {
                zoom: 14,
                center: [118.859472, 28.970085]
            });

            //添加插件
            AMap.plugin(['AMap.ToolBar', 'AMap.Scale'], function () {
                var toolBar = new AMap.ToolBar();
                var scale = new AMap.Scale();
                map.instance.addControl(toolBar);
                map.instance.addControl(scale);
            });



            AMap.plugin('AMap.Geocoder', function () {
                map.plugin.geocoder = new AMap.Geocoder({
                    city: "0570"//城市，默认：“全国”
                });
            });

            AMap.service(["AMap.PlaceSearch"], function () {
                map.plugin.placeSearch = new AMap.PlaceSearch({ //构造地点查询类
                    pageSize: 5,
                    pageIndex: 1,
                    city: "0570", //城市
                    map: map//,
                    //panel: "panel"
                });
            });
        };

        $scope.setMarker = function (lon, lat) {
            //添加
            marker = new AMap.Marker({
                position: [lon, lat],
                map: map.instance
            });

            map.instance.setCenter(marker.getPosition());
        };

        $scope.initMap();

        if ($scope.store.Lon != undefined && $scope.store.Lat != undefined)
            $scope.setMarker($scope.store.Lon, $scope.store.Lat);
        else
            $scope.$watch("store", function (oldValue, newValue) {
                if ($scope.store.Lon != undefined && $scope.store.Lat != undefined)
                    $scope.setMarker($scope.store.Lon, $scope.store.Lat);
            }, true);

    }]);
