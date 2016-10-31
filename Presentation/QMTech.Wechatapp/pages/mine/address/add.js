var util=require("../../../utils/util.js")
Page({
    data: {
        items: [{ text: "先生", checked: true, value: 0}, { text: "女士", checked: false, value: 0 }]
    },

    onLoad: function () {
        var _this = this;
        wx.getSystemInfo({
            success: function (res) {              
                _this.setData({
                    windowHeight: res.windowHeight + 55,
                    bodyHeight: res.windowHeight - ((res.windowWidth / 750) * 80 + 1) * 2 + 55
                });
            }
        });        
    },

    navToMap: function (e) {
        wx.navigateTo({ url: 'map' });
    },
    itemTap:function(e)
    {
        this.setData({
            isManaging: !this.data.isManaging,
            manageTxt: this.data.manageTxt == "管理" ? "完成" : "管理"
        });
        console.log(e);
    },
    saveAddress: function (e) {

        if (e.detail.value.name == "" || e.detail.value.name == null) {
            wx.showModal({
                title: '提示',
                content: '收货人姓名不能为空',
                showCancel: false
            });

            return;
        }

        if (!util.validatemobile(e.detail.value.mobile)) {
            wx.showModal({
                title: '提示',
                content: '请输入正确的手机号码',
                showCancel: false
            });

            return;
        }

        if (e.detail.value.name == "" || e.detail.value.name == null) {
            wx.showModal({
                title: '提示',
                content: '收货人地址不能为空',
                showCancel: false
            });

            return;
        }

        console.log('form发生了submit事件，携带数据为：', e.detail.value)
    }
});