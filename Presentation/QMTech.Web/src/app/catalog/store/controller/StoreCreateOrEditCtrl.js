"use strict";

app.controller("StoreCreateOrEditCtrl", ['$scope', '$rootScope', '$state', 'moment', 'toaster','w5cValidator', '$uibModal', 'StoreService', 'StoreCategoryService',
    function ($scope, $rootScope, $state, moment,toaster, w5cValidator, $uibModal, storeService, storeCategoryService) {

        $scope.errorMsg = null;
        $scope.storeProcessing = true;    //正在请求product数据
        $scope.categoryProcessing = true;     //正在请求category数据
        $scope.submitProcessing = false;    //正在提交表单
        $scope.processingStatus = false;
        //设置页面处理状态
        $scope.updateProcessingStatus = function () {
            $scope.processingStatus = $scope.storeProcessing || $scope.categoryProcessing || $scope.submitProcessing;

            //设置选中的类别
            if ($scope.storeId > 0 && !$scope.categoryProcessing && !$scope.storeProcessing) {
                angular.forEach($scope.categories, function (item, index) {
                    if (item.Value == $scope.store.storeCategoryId)
                        $scope.selectedCategory = item; 
                });
            }
        };

        $scope.categories = [];
        $scope.selectedCategory = null;
        $scope.sourceImg = null;   //上传图片         
        //实体
        $scope.store = {
            name: "",
            storeCategoryId: 0,
            address: "",
            logo: "",
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
            lon: 118.859472,
            lat: 28.970085
        };

        var marker = null;
        var map = {
            instance: null,
            plugin: {
                geocoder: null,
                placeSearch: null
            }
        };

        //初始化处理状态
        $scope.updateProcessingStatus();

        //#region 验证

        //验证规则
        w5cValidator.setRules({
            sname: {
                required: "商家名称不能为空",
                w5cuniquecheck: "输入商家名已经存在，请重新输入"
            },
            scategory: {
                required: "商家类别不能为空"
            },
            sLogo: {
                required: "LOGO图片不能为空"
            },
            stel: {
                required: "联系方式不能为空",
                pattern: "手机号码格式不正确"
            },
            saddress: {
                required: "地址不能为空不能为空",
            },
            sStartTime: {
                required: "运营开始时间不能为空",
                pattern: "不是一个正确的24小时制时间格式",
                minlength: "不是一个正确的24小时制时间格式",
                maxlength: "不是一个正确的24小时制时间格式"
            },
            sEndTime: {
                required: "运营结束时间不能为空",
                pattern: "不是一个正确的24小时制时间格式",
                customizer: "结束时间必须大于开始时间",
                minlength: "不是一个正确的24小时制时间格式",
                maxlength: "不是一个正确的24小时制时间格式"
            },
            spersontel: {
                required: "运营人联系方式不能为空",
                pattern: "手机号码格式不正确"
            },
        });

        //每个表单的配置，如果不设置，默认和全局配置相同
        var validation = $scope.validation = {
            options: {
                blurTrig: true
            }
        };

        //保存实体
        validation.saveEntity = function ($event, continueEditing) {
            $scope.submitProcessing = true;
            $scope.updateProcessingStatus();

            if ($scope.store.Id != undefined && $scope.store.Id != null) {
                
                storeService.updateStore($scope.store).then(function (data) {
                    $scope.errorMsg = "";

                    toaster.pop('success', '', '保存成功');

                    if (continueEditing) $state.reload();
                    else $state.go("app.catalog.store.list");
                }, function (message) {
                    toaster.pop('error', '', '保存失败');

                    $scope.errorMsg = message;
                })
                .finally(function () {
                    $scope.submitProcessing = false;
                    $scope.updateProcessingStatus();
                });
            }
            else {
            
                //新建商家
                storeService.createStore($scope.store).then(function (data) {
                    $scope.errorMsg = "";
                    toaster.pop('success', '', '保存成功');

                    if (continueEditing) $state.go("app.catalog.store.edit", { id: data.Id });
                    else $state.go("app.catalog.store.list");

                }, function (message) {
                    toaster.pop('error', '', '保存失败');
                    $scope.errorMsg = message;
                }).finally(function () {
                    $scope.submitProcessing = false;
                    $scope.updateProcessingStatus();
                });
            }
        };


        //#endregion

        //获取store
        if ($scope.storeId >0) {
            storeService.getStoreById($scope.storeId).then(function (data) {
                $scope.store = data;
                //设置默认值
                $scope.nameDefaultValue = $scope.store.name;

                //设置地图对象
                var lnglat =new AMap.LngLat($scope.store.lon, $scope.store.lat);
                marker.setPosition(lnglat);
                map.instance.setCenter(marker.getPosition());

            }, function (message) {

                $scope.errorMsg = message;
            }).finally(function () {
                $scope.storeProcessing = false;
                $scope.updateProcessingStatus();
            });
        }
        else
        {
            $scope.storeProcessing = false;
            $scope.updateProcessingStatus();
        }
 
        //加载所有类别
        storeCategoryService.loadSelectListItems().then(function (data) {
            $scope.categories = data;
        }, function (message) { $scope.errorMsg = message }).finally(function () {
            $scope.categoryProcessing = false;
            $scope.updateProcessingStatus();
        });
        //类型选择
        $scope.categorySelected = function () {
            $scope.store.storeCategoryId = $scope.selectedCategory.Value;
        };

        //#region 地图模块

        //高德地图初始化
        $scope.initMap = function () {
            map.instance = new AMap.Map('mapDiv', {
                zoom: 14,
                center: [118.859472, 28.970085]
            });

            //添加
            marker = new AMap.Marker({
                position: [118.859472, 28.970085],
                map: map.instance
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

            var clickEventListener = map.instance.on('click', function (e) {


                if (map.instance.getZoom() <= 16) {
                    var modalInstance = $uibModal.open({
                        templateUrl: 'dialog.html',
                        controller: 'ModalDialogCtrl',
                        resolve: {
                            title: function () { return "提示"; },
                            content: function () { return "当前地图级别过低，请放到到街区级别"; }
                        }
                    });

                    modalInstance.result.then(function () { }, function () { });
                }
                else {
                    marker.setPosition(e.lnglat);

                    $scope.store.lon = e.lnglat.lng;
                    $scope.store.lat = e.lnglat.lat;

                    console.log($scope.store);
                }
            });
        };

        //地址变化
        $scope.addressChange = function () {
            map.plugin.geocoder.getLocation($scope.store.address, function (status, result) {
                if (status == 'complete' && result.geocodes.length) {
                    marker.setPosition(result.geocodes[0].location);
                    map.instance.setCenter(marker.getPosition());

                    $scope.store.lon = result.geocodes[0].location.lng;
                    $scope.store.lat = result.geocodes[0].location.lat;
                } else {
                
                }
            });
        };

        //#endregion

        //#region logo图片生成

        //选择图片
        var handleFileSelect = function (evt) {
            var file = evt.currentTarget.files[0];
            //格式判断
            if (!(file.type == "image/png" || file.type == "image/jpeg" || file.type == "image/bmp"))
                alert("上传文件必须为jpg bmp或者png图片格式");
            else if (file.size > 1024 * 1024)
                alert("图片不能大于1M");
            else {
                var reader = new FileReader();
                console.log(reader);
                reader.onload = function (evt) {
                    $scope.$apply(function ($scope) {
                        $scope.sourceImg = evt.target.result;
                    });
                };
                reader.readAsDataURL(file);
            }
        };
        angular.element(document.querySelector('#fileInput')).on('change', handleFileSelect);

        //#endregion

        return $scope.initMap();
    }]);