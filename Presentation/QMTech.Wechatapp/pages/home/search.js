Page({
    data: {        
        tags: [
            {
                id: 1,
                name: "三顾冒菜",
            },
            {
                id: 2,
                name: "辣有道",
            },
            {
                id: 3,
                name: "麻辣烫",
            },
            {
                id: 3,
                name: "星巴克",
            },
            {
                id: 3,
                name: "肯德基",
            },
            {
                id: 3,
                name: "连马小厨房",
            },
            {
                id: 3,
                name: "东南亚特产",
            },
            {
                id: 3,
                name: "果果鲜花",
            }
        ],
        historylist: [
            {
                id: 1,
                name: "三顾冒菜",
            },
            {
                id: 2,
                name: "辣有道",
            },
            {
                id: 3,
                name: "麻辣烫",
            },
            {
                id: 3,
                name: "星巴克",
            },
            {
                id: 3,
                name: "肯德基",
            },
            {
                id: 3,
                name: "连马小厨房",
            },
            {
                id: 3,
                name: "东南亚特产",
            },
            {
                id: 3,
                name: "果果鲜花",
            }
        ]
    },
    selectShops: function (e) {
        this.setData({
            selectedMenu: "附近商户"
        })
    },
    selectProducts: function (e) {
        this.setData({
            selectedMenu: "大家都在吃"
        })
    },

    naveToStore: function (e) {
        wx.navigateTo({ url: 'shops/info?id=' + e.currentTarget.id });
    },

    navToSearch: function (e) {
        wx.navigateTo({ url: 'search' });
    }
})
