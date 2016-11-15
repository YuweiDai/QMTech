Page({
    data: {
        isManaging: false,
        manageTxt:"管理",
        windowHeight: 0,
        bodyHeight: 0,
        addresses: [
            {
                id: 0,
                address_name: "开园小区",
                name: "答复",
                male: true,
                mobile: "13876541234"
            },
            {
                id: 1,
                address_name: "开园小区",
                name: "答复",
                male: true,
                mobile: "13876541234"
            },
            {
                id: 3,
                address_name: "开园小区",
                name: "答复",
                male: true,
                mobile: "13876541234"
            },
      
        ],
        currentAddressId:0
    },
    onPullDownRefresh: function () {
        console.log('onPullDownRefresh', new Date())
    },
    stopPullDownRefresh: function () {
        wx.stopPullDownRefresh({
            complete: function (res) {
                console.log(res, new Date())
            }
        })
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
    naveToAdd: function (e) {
        wx.navigateTo({ url: 'add' });
    },

    navToEdit: function (e) {
        wx.navigateTo({ url: 'search' });
    },
    startManage: function () {
        this.setData({
            isManaging: !this.data.isManaging,
            manageTxt: this.data.manageTxt == "管理" ? "完成" : "管理"
        });
    },
    deleteAdd:function(e)
    {
        wx.showModal({
            title: '提示',
            content: '确认删除此送货地址？',
            showCancel:true,
            success: function (res) {
                if (res.confirm) {
                    //console.log(e.currentTarget.dataset.id);                
                }
            }
        })
    },
    editAdd:function(e)
    {

    },
    itemTap:function(e)
    {
        this.setData({
            isManaging: !this.data.isManaging,
            manageTxt: this.data.manageTxt == "管理" ? "完成" : "管理"
        });
        console.log(e);
    }
});