'use strict';

/* Controllers */

angular.module('app').controller('AppCtrl', ['$scope', '$localStorage', '$window', '$rootScope', function ($scope, $localStorage, $window, $rootScope) {
    // add 'ie' classes to html
    var isIE = !!navigator.userAgent.match(/MSIE/i);
    isIE && angular.element($window.document.body).addClass('ie');
    isSmartDevice($window) && angular.element($window.document.body).addClass('smart');

    $rootScope.apiUrl = 'http://localhost/QM/api/'
    $rootScope.baseUrl = 'http://localhost/QM/';
    //config
    $scope.app = {
        name: 'Angular',
        version: '1.0.0',
        // for chart colors
        color: {
            primary: '#7266ba',
            info: '#23b7e5',
            success: '#27c24c',
            warning: '#fad733',
            danger: '#f05050',
            light: '#e8eff0',
            dark: '#3a3f51',
            black: '#1c2b36'
        },
        settings: {
            themeID: 1,
            navbarHeaderColor: 'bg-black',
            navbarCollapseColor: 'bg-white-only',
            asideColor: 'bg-black',
            headerFixed: true,
            asideFixed: false,
            asideFolded: false,
            asideDock: false,
            container: false
        },   
        dateRangeConfig: {
            local: {
                "format": "YYYY年MM月DD日",
                "separator": " - ",
                "applyLabel": "应用",
                "cancelLabel": "取消",
                "fromLabel": "开始",
                "toLabel": "结束",
                "customRangeLabel": "自定义",
                "daysOfWeek": [
                    "日",
                    "一",
                    "二",
                    "三",
                    "四",
                    "五",
                    "六"
                ],
                "monthNames": [
                    "一月",
                    "二月",
                    "三月",
                    "四月",
                    "五月",
                    "六月",
                    "七月",
                    "八月",
                    "九月",
                    "十月",
                    "十一月",
                    "十二月"
                ],
                "firstDay": 1
            },
            predefinedRnages: function () {
                var range = {
                    '今天': [moment(), moment()],
                    '昨天': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                    '上周': [moment().subtract(6, 'days'), moment()],
                    '上30天': [moment().subtract(29, 'days'), moment()],
                    '本月': [moment().startOf('month'), moment().endOf('month')],
                    '下月': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
                };

                return range;
            }
        }
    };

    //保存设置到Local storage
    if(angular.isDefined($localStorage.settings))
    {
        $scope.app.settings = $localStorage.settings;
    }
    else
    {
        $localStorage.settings = $scope.app.settings;
    }

    $scope.$watch('app.settings', function () {

        //保存到local storage
        $localStorage.settings = $scope.app.settings;
    }, true);

    function isSmartDevice($window) {
        // Adapted from http://www.detectmobilebrowsers.com
        var ua = $window['navigator']['userAgent'] || $window['navigator']['vendor'] || $window['opera'];
        // Checks for iOs, Android, Blackberry, Opera Mini, and Windows mobile devices
        return (/iPhone|iPod|iPad|Silk|Android|BlackBerry|Opera Mini|IEMobile/).test(ua);
    }
}]);

angular.module('app').constant('ngAuthSettings', {
    apiServiceBaseUri: 'http://localhost/QM/',
    clientId: 'administration',
});