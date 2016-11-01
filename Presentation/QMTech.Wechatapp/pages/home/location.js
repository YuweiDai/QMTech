var app = getApp()
Page({
    data: {

    },
    //事件处理函数
    bindViewTap: function () {
        console.log("taped me");
    },
    onLoad: function () {
        console.log(this);
    },
    navToAddress: function (e) {
        wx.navigateTo({ url: "address" });
    }
})
