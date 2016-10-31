Page({
    data: {
        lat: 28.8,
        lng:118.8,
        //markers: [{
        //    latitude: 23.099994,
        //    longitude: 113.324520,
        //    name: 'T.I.T 创意园',
        //    desc: '我现在的位置'
        //}],
        //covers: [{
        //    latitude: 23.099794,
        //    longitude: 113.324520,
        //    iconPath: '../images/car.png',
        //    rotate: 10
        //}, {
        //    latitude: 23.099298,
        //    longitude: 113.324129,
        //    iconPath: '../images/car.png',
        //    rotate: 90
        //}],
        bodyHeight: 0,
    },
    onLoad: function () {
        var _this = this;
        wx.getSystemInfo({
            success: function (res) {
                var h = (res.windowHeight - ((res.windowWidth / 750) * 90 + 1)) / 2;
                _this.setData({
                    bodyHeight: h
                });
            }
        });

        wx.getLocation({
            type: 'gcj02',
            success: function (res) {
                _this.setData({
                    lat: res.latitude,
                    lng: res.longitude
                });
                //var latitude = res.latitude
                //var longitude = res.longitude
                //var speed = res.speed
                //var accuracy = res.accuracy
            }
        })
    },
});