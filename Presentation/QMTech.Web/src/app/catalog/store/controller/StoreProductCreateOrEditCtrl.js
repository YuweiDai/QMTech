"use strict";

app.controller("StoreProductCreateOrEditCtrl", ['$scope', '$rootScope', '$state','toaster', 'moment', 'w5cValidator', 'ProductCategoryService','ProductService', 'TagService',
    function ($scope, $rootScope, $state,toaster, moment, w5cValidator, productCategoryService, productService, tagService, $window) {

        $scope.errorMsg = null;
        $scope.productProcessing = true;    //正在请求product数据
        $scope.categoryProcessing = true;     //正在请求category数据
        $scope.submitProcessing = false;    //正在提交表单
        $scope.processingStatus =false;
        //设置页面处理状态
        $scope.updateProcessingStatus = function () {
            $scope.processingStatus = $scope.productProcessing || $scope.categoryProcessing || $scope.submitProcessing;

            //设置选中的类别
            if ($scope.productId > 0 && !$scope.categoryProcessing) {
                angular.forEach($scope.categories, function (item, index) {
                    if (item.Value == $scope.product.productCategoryId)
                        $scope.selectedCategory = item;
                });
            }
        };

        $scope.categories = [];
        $scope.nameDefaultValue = "";       //原始name值
        $scope.selectedCategory = null;     //商品类别
        $scope.product = {
            name: "",
            storeId:$scope.storeId,
            productCategoryId: 0,
            groupedProduct: false,
            shortDescription: "",
            fullDescription: "",
            hasValidDate:false,
            availableDateRange: "",
            isNew: true,
            published: false,
            ProductPictures: [],
            buyDisabled: false,
            productCost:1,
            price: 1,
            oldPrice: 1,
            specialPrice: 0,
            hasSpecialPrice:false,
            specialDateRange: ""
        };

        //初始化处理状态
        $scope.updateProcessingStatus();

        //加载商品
        if ($scope.productId > 0) {
            productService.getProductById($scope.storeId, $scope.productId)
                .then(function (data) {
                    $scope.product = data;

                    //设置默认值
                    $scope.nameDefaultValue = $scope.product.name;
                }, function (message) {
                    $scope.errorMsg = message;
                }).finally(function () {
                    $scope.productProcessing = false;
                    $scope.updateProcessingStatus();
                });
        }
        else {
            $scope.productProcessing = false;
            $scope.updateProcessingStatus();
        }

        //商品对象监听，保持
        $scope.$watch("product", function (newValue, oldValue) {
            if (newValue.oldPrice == "")
                $scope.product.oldPrice = 1;

            if (newValue.price == "")
                $scope.product.price = 1;

            if (newValue.specialPrice == "")
                $scope.product.specialPrice = 0;

        }, true);
        
        //加载商品分类
        productCategoryService.getProductCategories($scope.storeId, false, true).then(function (data) {
            $scope.categories = data;
        }, function (message) {
            alert("类别加载错误：" + message);
        }).finally(function () {
            $scope.categoryProcessing = false;
            $scope.updateProcessingStatus();
        });

        //#region 验证

        //验证规则
        w5cValidator.setRules({
            pname: {
                required: "商品名称不能为空",
                w5cuniquecheck: "输入商品名已经存在，请重新输入"
            },
            pcategory: {
                required: "商品类别不能为空"
            },
            pShortDes: {
                required: "简单描述不能为空",
                minlength: "简单描述长度不能小于{minlength}",
                maxlength: "简单描述长度不能大于{maxlength}"

            },
            pOldPrice: {
                required: "成本价不能为空"
            },
            pPrice: {
                required: "价格不能为空",
                customizer:"价格不能低于成本价"
            },          
            pSpecialPrice: {
                required: "特价不能为空",
                customizer: "特价不能低于价格"
            },
            pMetaTitle:
            {
                minlength: "SEO标题长度不能小于{minlength}",
                maxlength: "SEO标题长度不能大于{maxlength}"
            },
            pMetaKeyword:
            {
                minlength: "SEO关键字长度不能小于{minlength}",
                maxlength: "SEO关键字长度不能大于{maxlength}"
            },
            pMetaDescriptions:
            {
                minlength: "SEO描述长度不能小于{minlength}",
                maxlength: "SEO描述长度不能大于{maxlength}"
            }
        });

        //每个表单的配置，如果不设置，默认和全局配置相同
        var validation = $scope.validation = {
            options: {
                blurTrig: true
            }
        };

        //价格比较
        validation.priceCompare = function (price1, price2, enableEqual) {
            if (enableEqual) return price1 < price2;
            else return price1 < price2;
        };

        //时间对比
        //validation.compare = function () {
        //    var time1 = moment(store.StartTime, "HH:mm");
        //    var time2 = moment(store.EndTime, "HH:mm");

        //    return time1 <= time2;
        //};
        
        
        //保存实体
        validation.saveEntity = function ($event, continueEditing) {
            $scope.submitProcessing = true;
            $scope.updateProcessingStatus();

            if (!$scope.product.hasValidDate) $scope.product.availableDateRange = "";
            if (!$scope.product.hasSpecialPrice) $scope.product.specialDateRange = "";

            if ($scope.product.id != undefined && $scope.product.id != null) {
                //alert("更新商品");

                productService.updateProduct($scope.product).then(function (data) {
                    $scope.errorMsg = "";

                    toaster.pop('success', '','保存成功');

                    if (continueEditing) $state.reload();
                    else $state.go("app.catalog.store.detail.product.list");

                }, function (message) {

                    toaster.pop('error', '', '保存失败');

                    $scope.errorMsg = message;
                }).finally(function () {
                    $scope.submitProcessing = false;
                    $scope.updateProcessingStatus();
                });
            }
            else {
                //新建商品                               
                productService.createProduct($scope.storeId, $scope.product).then(function (data) {
                    $scope.errorMsg = "";
                    toaster.pop('success', '', '保存失败');
                    if (continueEditing) $state.go("app.catalog.store.detail.product.edit", { id: data.StoreId,productId:data.id });
                    else $state.go("app.catalog.store.detail.product.list");

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

        //#region 标签
        $scope.product.productTags = [];
        $scope.tags = [];

        $scope.tagTransform = function (newTag) {
            console.log("now is taggings");
            var exsit = false;

            angular.forEach($scope.tags, function (v, i) {
                if (v.name.indexOf(newTag) > -1) {
                    exsit = true;
                }
            });

            if (exsit) return false;

            var item = {
                name: newTag                
            };

            return item;
        };
         
        $scope.refreshProductTags = function (search) {

            console.log("now is searching");
            tagService.queryProudctTags(search).then(function (data) {
                $scope.tags = data;
            }, function (message) { });
               
        };
 
        //#endregion

        //#region 日期配置

        $scope.AvailableDateRangeOptions = {
            "showDropdowns": true,
            "showWeekNumbers": true,
            "autoApply": true,
            "ranges": $scope.app.dateRangeConfig.predefinedRnages(),
            "locale": $scope.app.dateRangeConfig.local,
            "linkedCalendars": false,
            "alwaysShowCalendars": true,         
            "opens": "center",
            "drops": "up"
        };

        var local = angular.copy($scope.app.dateRangeConfig.local);

        local.format = "YYYY年MM月DD日 HH:mm";

        $scope.specialDateRangeOptions = {
            "showDropdowns": true,
            "showWeekNumbers": true,
            "autoApply": true,
            "timePicker": true,
            "timePicker24Hour": true,
            "timePickerIncrement": 10,
            "ranges": $scope.app.dateRangeConfig.predefinedRnages(),
            "locale": local,
            "linkedCalendars": false,
            "alwaysShowCalendars": true,
            "opens": "center",
            "drops": "up"
        };

        //#endregion

        //#region 文件上传模块         
        var uploader = null;

        //上传初始化
        $scope.uploadInit = function () {
           

            //删除图片
            $scope.removeImg = function (img, index) {
                $scope.product.ProductPictures.splice(index, 1);

                if ($scope.product.ProductPictures.length > 0)
                    $scope.selectPicutre($scope.product.ProductPictures[0]);
                else $scope.selectedImg = null;
            };

            //删除所有图片
            $scope.removeAllImgs = function () {

                $scope.product.ProductPictures = [];
                $scope.product.selectedImg = null;
            };

            //product.selectPicture(img)
            $scope.selectPicutre = function (img) {
                if ($scope.product.selectedImg != undefined && $scope.product.selectedImg != null) {
                    $scope.product.selectedImg.selected = false;
                }

                img.selected = true;

                $scope.product.selectedImg = img;
            };

            if ($scope.product.ProductPictures.length > 0)
                $scope.selectPicutre($scope.product.ProductPictures[0]);
            

            setTimeout(function () {
                if (uploader == null) {
                    // 实例化

                    uploader = WebUploader.create({
                        // swf文件路径
                        swf: $rootScope.baseUrl + 'vender/libs/webuploader/Uploader.swf',
                        // 文件接收服务端。
                        server: $rootScope.apiUrl + 'Media/pictures/Upload?size=500',
                        auto: true,
                        // 选择文件的按钮。可选。
                        // 内部根据当前运行是创建，可能是input元素，也可能是flash.
                        pick: '#filePicker',
                        // 不压缩image, 默认如果是jpeg，文件上传前会压缩一把再上传！
                        resize: false,
                        accept: {
                            title: 'Images',
                            extensions: 'gif,jpg,jpeg,bmp,png',
                            mimeTypes: 'image/*'
                        }
                    });

                    // 当有文件添加进来的时候
                    uploader.on('fileQueued', function (file) {

                        var img = {
                            fileId: file.id,
                            Title: file.name.replace("." + file.ext, ""),
                            Alt: "",
                            displayOrder: $scope.product.ProductPictures.length,
                            percentage: 0,
                            uploaded: false,
                            ProductId: $scope.product.id
                        };
                        $scope.product.ProductPictures.push(img);
                        $scope.selectPicutre(img);

                        // 创建缩略图
                        // 如果为非图片文件，可以不用调用此方法。
                        // thumbnailWidth x thumbnailHeight 为 100 x 100
                        uploader.makeThumb(file, function (error, src) {
                            if (error) {
                                img.Src = $rootScope.baseUrl + "img/webUploader/notpreviewed.png";
                            }
                            else {
                                img.Src = src;
                            }
                            //非ng环境下，需要手动触发
                            $scope.$apply();
                        }, 200, 200);
                    });
                    //文件上传进度
                    uploader.on('uploadProgress', function (file, percentage) {
                        angular.forEach($scope.product.ProductPictures, function (i, v) {

                            if(v.id==file.id)
                            {
                                v.percentage = percentage;
                                console.log(percentage);

                                $scope.$apply();
                                return;
                            }

                        });

                       
                    });
                    //文件上传结果处理
                    uploader.on('uploadSuccess', function (file, response) {
                    
                        angular.forEach($scope.product.ProductPictures, function (v, i) {

                            if(v.fileId==file.id)
                            {
                                v.percentage = 100;
                                v.uploaded=true;
                                v.error=false;
                                v.status="上传成功";
                                v.PictureId = response[0].id;
                                return;
                            }

                        });

                    });

                    uploader.on('uploadError', function (file, reason) {


                        angular.forEach($scope.product.ProductPictures, function (i, v) {

                            if (v.fileId == file.id)
                            {
                                v.percentage = 100;
                                v.uploaded=true;
                                v.error=true;
                                v.status="上传失败";
                                alert("上传失败：" + reason);
                                return;
                            }

                        });
                    });
                    uploader.on('uploadComplete', function (file) {

                        //非ng环境下，需要手动触发
                        $scope.$apply();
                    });


                }
            });
        };



 


        ////#endregion
    }]);


//#region 批量上传 修改了部分

//$scope.webUploader = {
//    placeHolder_elementInvisible: false,
//    statusBar_elementInvisible: false,
//    filePicker2_elementInvisible: false,

//    statusBar_visible: false,  //状态条可见
//    queue_visible: false,      //队列可见
//    progress_visible: false,    //进度条可见

//    fileError: "",  //错误信息

//    upload_text: "",  //上传按钮文本
//    previewText: "",  //缩略图文本
//    percentages: [],



//    // 可能有pedding, ready, uploading, confirm, done.
//    state: 'pedding',
//};

//#region 参数

// WebUploader实例
//var uploader = null;

//var webUploaderUtli = {
//    // 添加的文件数量
//    fileCount: 0,

//    // 添加的文件总大小
//    fileSize: 0,

//    // 优化retina, 在retina下这个值是2

//    // 缩略图大小
//    thumbnailWidth: 110 * window.devicePixelRatio || 1,
//    thumbnailHeight: 110 * window.devicePixelRatio || 1,

//    // 所有文件的进度信息，key为file id
//    percentages: {},

//    // 判断浏览器是否支持图片的base64
//    isSupportBase64: function () {
//        var data = new Image();
//        var support = true;
//        data.onload = data.onerror = function () {
//            if (this.width != 1 || this.height != 1) {
//                support = false;
//            }
//        }
//        data.src = "data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///ywAAAAAAQABAAACAUwAOw==";
//        return support;
//    },

//    // 检测是否已经安装flash，检测flash的版本
//    flashVersion: function () {
//        var version;

//        try {
//            version = navigator.plugins['Shockwave Flash'];
//            version = version.description;
//        } catch (ex) {
//            try {
//                version = new ActiveXObject('ShockwaveFlash.ShockwaveFlash')
//                        .GetVariable('$version');
//            } catch (ex2) {
//                version = '0.0';
//            }
//        }
//        version = version.match(/\d+/g);
//        return parseFloat(version[0] + '.' + version[1], 10);
//    },

//    supportTransition: function () {
//        var s = document.createElement('p').style,
//            r = 'transition' in s ||
//                    'WebkitTransition' in s ||
//                    'MozTransition' in s ||
//                    'msTransition' in s ||
//                    'OTransition' in s;
//        s = null;
//        return r;
//    }
//};

//#endregion
//uploader = WebUploader.create({
//    pick: {
//        id: '#filePicker',
//        label: '点击选择图片'
//    },
//    formData: {
//        uid: 123
//    },
//    dnd: '#dndArea',
//    paste: '#uploader',
//    swf: $rootScope.baseUrl + 'vender/libs/webuploader/Uploader.swf',
//    chunked: false,
//    chunkSize: 512 * 1024,
//    server: $rootScope.apiUrl + 'Media/pictures/Upload?size=300',
//    // runtimeOrder: 'flash',

//    accept: {
//        title: 'Images',
//        extensions: 'gif,jpg,jpeg,bmp,png',
//        mimeTypes: 'image/*'
//    },

//    // 禁掉全局的拖拽功能。这样不会出现图片拖进页面的时候，把图片打开。
//    disableGlobalDnd: true,
//    fileNumLimit: 300,
//    fileSizeLimit: 200 * 1024 * 1024,    // 200 M
//    fileSingleSizeLimit: 50 * 1024 * 1024    // 50 M
//});

//// 拖拽时不接受 js, txt 文件。
//uploader.on('dndAccept', function (items) {
//    var denied = false,
//        len = items.length,
//        i = 0,
//        // 修改js类型
//        unAllowed = 'text/plain;application/javascript ';

//    for (; i < len; i++) {
//        // 如果在列表里面
//        if (~unAllowed.indexOf(items[i].type)) {
//            denied = true;
//            break;
//        }
//    }

//    return !denied;
//});

//uploader.on('dialogOpen', function () {
//    console.log('here');
//});

//// uploader.on('filesQueued', function() {
////     uploader.sort(function( a, b ) {
////         if ( a.name < b.name )
////           return -1;
////         if ( a.name > b.name )
////           return 1;
////         return 0;
////     });
//// });

//// 添加“添加文件”的按钮，
//uploader.addButton({
//    id: '#filePicker2',
//    label: '继续添加'
//});

//uploader.on('ready', function () {
//    window.uploader = uploader;
//});


//// 当有文件添加进来时执行，负责view的创建
//function addFile(file) {

//    $scope.product.ImgFiles.push(file);
//    //var $li = $('<li id="' + file.id + '">' +
//    //        '<p class="title">' + file.name + '</p>' +
//    //        '<p class="imgWrap"></p>' +
//    //        '<p class="progress"><span></span></p>' +
//    //        '</li>'),

//    //    $btns = $('<div class="file-panel">' +
//    //        '<span class="cancel">删除</span>' +
//    //        '<span class="rotateRight">向右旋转</span>' +
//    //        '<span class="rotateLeft">向左旋转</span></div>').appendTo($li),
//    //    $prgress = $li.find('p.progress span'),
//    //    $wrap = $li.find('p.imgWrap'),
//    //    $info = $('<p class="error"></p>'),

//    var showError = function (code) {
//        switch (code) {
//            case 'exceed_size':
//                $scope.webUploader.fileError = '文件大小超出';
//                break;

//            case 'interrupt':
//                $scope.webUploader.fileError = '上传暂停';
//                break;

//            default:
//                $scope.webUploader.fileError = '上传失败，请重试';
//                break;
//        }
//    };

//    if (file.getStatus() === 'invalid') {
//        showError(file.statusText);
//    } else {
//        // @todo lazyload
//        $scope.webUploader.previewText = "预览中";

//        uploader.makeThumb(file, function (error, src) {

//            if (error) {
//                $scope.webUploader.previewText = "不能预览";
//                return;
//            }

//            if (webUploaderUtli.isSupportBase64) {
//                file.src = src;

//                //img = $('<img src="' + src + '">');
//                //$wrap.empty().append(img);
//            }
//            //#region 暂时不考虑不支持base64的情况
//            //else {
//            //    $.ajax('../../server/preview.php', {
//            //        method: 'POST',
//            //        data: src,
//            //        dataType: 'json'
//            //    }).done(function (response) {
//            //        if (response.result) {
//            //            img = $('<img src="' + response.result + '">');
//            //            $wrap.empty().append(img);

//            //            $scope.webUploader.previewText = "";
//            //        } else {
//            //            $scope.webUploader.previewText = "预览出错";
//            //        }
//            //    });                                  
//            //}
//            //#endregion 暂时不考虑不支持base64的情况
//        }, webUploaderUtli.thumbnailWidth, webUploaderUtli.thumbnailHeight);

//        $scope.webUploader.percentages[file.id] = [file.size, 0];
//        file.rotation = 0;


//    }

//    file.on('statuschange', function (cur, prev) {
//        if (prev === 'progress') {
//            $prgress.hide().width(0);
//        } else if (prev === 'queued') {
//            $li.off('mouseenter mouseleave');
//            $btns.remove();
//        }

//        // 成功
//        if (cur === 'error' || cur === 'invalid') {
//            console.log(file.statusText);
//            showError(file.statusText);
//            percentages[file.id][1] = 1;
//        } else if (cur === 'interrupt') {
//            showError('interrupt');
//        } else if (cur === 'queued') {
//            $info.remove();
//            $prgress.css('display', 'block');
//            percentages[file.id][1] = 0;
//        } else if (cur === 'progress') {
//            $info.remove();
//            $prgress.css('display', 'block');
//        } else if (cur === 'complete') {
//            $prgress.hide().width(0);
//            $li.append('<span class="success"></span>');
//        }

//        $li.removeClass('state-' + prev).addClass('state-' + cur);
//    });

//    $li.on('mouseenter', function () {
//        $btns.stop().animate({ height: 30 });
//    });

//    $li.on('mouseleave', function () {
//        $btns.stop().animate({ height: 0 });
//    });

//    $btns.on('click', 'span', function () {
//        var index = $(this).index(),
//            deg;

//        switch (index) {
//            case 0:
//                uploader.removeFile(file);
//                return;

//            case 1:
//                file.rotation += 90;
//                break;

//            case 2:
//                file.rotation -= 90;
//                break;
//        }

//        if (supportTransition) {
//            deg = 'rotate(' + file.rotation + 'deg)';
//            $wrap.css({
//                '-webkit-transform': deg,
//                '-mos-transform': deg,
//                '-o-transform': deg,
//                'transform': deg
//            });
//        } else {
//            $wrap.css('filter', 'progid:DXImageTransform.Microsoft.BasicImage(rotation=' + (~~((file.rotation / 90) % 4 + 4) % 4) + ')');
//            // use jquery animate to rotation
//            // $({
//            //     rotation: rotation
//            // }).animate({
//            //     rotation: file.rotation
//            // }, {
//            //     easing: 'linear',
//            //     step: function( now ) {
//            //         now = now * Math.PI / 180;

//            //         var cos = Math.cos( now ),
//            //             sin = Math.sin( now );

//            //         $wrap.css( 'filter', "progid:DXImageTransform.Microsoft.Matrix(M11=" + cos + ",M12=" + (-sin) + ",M21=" + sin + ",M22=" + cos + ",SizingMethod='auto expand')");
//            //     }
//            // });
//        }


//    });

//    $li.appendTo($queue);
//}




//// 负责view的销毁
//function removeFile(file) {
//    var $li = $('#' + file.id);

//    delete percentages[file.id];
//    updateTotalProgress();
//    $li.off().find('.file-panel').off().end().remove();
//}

//function updateTotalProgress() {
//    var loaded = 0,
//        total = 0,
//        spans = $progress.children(),
//        percent;

//    $.each(percentages, function (k, v) {
//        total += v[0];
//        loaded += v[0] * v[1];
//    });

//    percent = total ? loaded / total : 0;


//    spans.eq(0).text(Math.round(percent * 100) + '%');
//    spans.eq(1).css('width', Math.round(percent * 100) + '%');
//    updateStatus();
//}

//function updateStatus() {
//    var text = '', stats;

//    if ($scope.webUploader.state === 'ready') {
//        text = '选中' + webUploaderUtli.fileCount + '张图片，共' +
//                WebUploader.formatSize(webUploaderUtli.fileSize) + '。';
//    } else if (state === 'confirm') {
//        stats = uploader.getStats();
//        if (stats.uploadFailNum) {
//            text = '已成功上传' + stats.successNum + '张照片至XX相册，' +
//                stats.uploadFailNum + '张照片上传失败，<a class="retry" href="#">重新上传</a>失败图片或<a class="ignore" href="#">忽略</a>'
//        }

//    } else {
//        stats = uploader.getStats();
//        text = '共' + webUploaderUtli.fileCount + '张（' +
//                WebUploader.formatSize(webUploaderUtli.fileSize) +
//                '），已上传' + stats.successNum + '张';

//        if (stats.uploadFailNum) {
//            text += '，失败' + stats.uploadFailNum + '张';
//        }
//    }

//    $info.html(text);
//}

//function setState(val) {
//    var file, stats;

//    if (val === state) {
//        return;
//    }

//    $scope.webUploader.state = val;
//    //state = val;

//    switch ($scope.webUploader.state) {
//        case 'pedding':
//            $scope.webUploader.placeHolder_elementInvisible = false;
//            $scope.webUploader.queue_visible = false;
//            $scope.webUploader.statusBar_elementInvisible = true;

//            uploader.refresh();
//            break;

//        case 'ready':
//            $scope.webUploader.placeHolder_elementInvisible = true;
//            $scope.webUploader.filePicker2_elementInvisible = false;
//            $scope.webUploader.queue_visible = true;
//            $scope.webUploader.statusBar_elementInvisible = false;

//            uploader.refresh();
//            break;

//        case 'uploading':
//            $scope.webUploader.filePicker2_elementInvisible = false;
//            $scope.webUploader.progress_visible = true;
//            $scope.webUploader.upload_text = "暂停上传";

//            break;

//        case 'paused':
//            $scope.webUploader.progress_visible = true;
//            $scope.webUploader.upload_text = "继续上传";
//            break;

//        case 'confirm':
//            $scope.webUploader.progress_visible = false;
//            $scope.webUploader.filePicker2_elementInvisible = false;
//            $scope.webUploader.upload_text = "开始上传";

//            stats = uploader.getStats();
//            if (stats.successNum && !stats.uploadFailNum) {
//                setState('finish');
//                return;
//            }
//            break;
//        case 'finish':
//            stats = uploader.getStats();
//            if (stats.successNum) {
//                alert('上传成功');
//            } else {
//                // 没有成功的图片，重设
//                $scope.webUploader.state = 'done';
//                location.reload();
//            }
//            break;
//    }

//    updateStatus();
//}

//uploader.onUploadProgress = function (file, percentage) {
//    var $li = $('#' + file.id),
//        $percent = $li.find('.progress span');

//    $percent.css('width', percentage * 100 + '%');
//    percentages[file.id][1] = percentage;
//    updateTotalProgress();
//};

//uploader.onFileQueued = function (file) {
//    webUploaderUtli.fileCount++;
//    webUploaderUtli.fileSize += file.size;

//    if (webUploaderUtli.fileCount === 1) {
//        $scope.webUploader.placeHolder_elementInvisible = true;
//        $scope.webUploader.statusBar_visible = true;
//    }

//    addFile(file);
//    setState('ready');
//    updateTotalProgress();

//    $scope.$apply();
//};

//uploader.onFileDequeued = function (file) {
//    webUploaderUtli.fileCount--;
//    webUploaderUtli.fileSize -= file.size;

//    if (!webUploaderUtli.fileCount) {
//        setState('pedding');
//    }

//    removeFile(file);
//    updateTotalProgress();

//};

//uploader.on('all', function (type) {
//    var stats;
//    switch (type) {
//        case 'uploadFinished':
//            setState('confirm');
//            break;

//        case 'startUpload':
//            setState('uploading');
//            break;

//        case 'stopUpload':
//            setState('paused');
//            break;

//    }
//});

//uploader.onError = function (code) {
//    alert('Eroor: ' + code);
//};


////// 当有文件添加进来的时候
////uploader.on('fileQueued', function (file) {


////    // 创建缩略图
////    // 如果为非图片文件，可以不用调用此方法。
////    // thumbnailWidth x thumbnailHeight 为 100 x 100
////    uploader.makeThumb(file, function (error, src) {

////        if (error) {
////            return;
////        }
////        else {

////            var img = {
////                name: file.name,
////                src: src,
////                Title: "",
////                Alt: "",
////                displayOrder: $scope.product.ImgFiles.length - 1
////            };
////            $scope.product.ImgFiles.push(img);

////            $scope.selectPicutre(img);

////            //非ng环境下，需要手动触发
////            $scope.$apply();

////        }
////    }, 200, 200);
////});

//////文件上传进度
////uploader.on('uploadProgress', function (file, percentage) {

////});

//////文件上传结果处理
////uploader.on('uploadSuccess', function (file) {

////});

////uploader.on('uploadError', function (file) {

////});

////uploader.on('uploadComplete', function (file) {

////    //非ng环境下，需要手动触发
////    $scope.$apply();
////});
//#endregion