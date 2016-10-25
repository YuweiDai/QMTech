// Register a Page.
Page({
    data: {
        shops: [
    {
        id: 1,
        name: "测试商家一",
        star: 5,
        sales: 55,
        pic: "http://www.atool.org/placeholder.png?size=300x150&bg=9BCE3B&fg=fff",
        deliverFee: 10,
        timeCost: 40,
        dist: 2.0,
        open: true
    },
    {
        id: 2,
        name: "测试商家二",
        star: 5,
        sales: 55,
        pic: "http://www.atool.org/placeholder.png?size=300x150&bg=9BCE3B&fg=fff",
        deliverFee: 10,
        timeCost: 40,
        dist: 2.0,
        open: true
    },
    {
        id: 3,
        name: "测试商家二",
        star: 5,
        sales: 55,
        pic: "http://www.atool.org/placeholder.png?size=300x150&bg=9BCE3B&fg=fff",
        deliverFee: 10,
        timeCost: 40,
        dist: 2.0,
        open: true
    }
        ],
        title:"测试"
    },
    onLoad: function () {
        this.setData({
            title: "餐饮"
        })
    },
    navToSearch: function (e) {
        wx.navigateTo({ url: "../search" });
    },
    navToStore: function (e) {
        wx.navigateTo({ url: 'info?id=' + e.currentTarget.id });
    },
})