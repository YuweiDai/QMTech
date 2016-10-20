Page({
    data: {
        background: ['http://img.waimai.baidu.com/pb/0e0d97764c447207ed3c08834089b7c96e@w_750,q_90',
         'http://img.waimai.baidu.com/pb/4225d918b283ebb1060548bb1875e040d0',
          '../../image/banner3.jpg'],
        indicatorDots: true,
        vertical: false,
        autoplay: true,
        interval: 3000,
        duration: 1200,
         
        top_lines: ['儿童身高, 体重发育标准! 快来看看0...', '儿童身高, 体重发育标准! 快来看看1...', ],
        selectedMenu: "附近商户",
        shops: [
            {
                id:1,
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
                id:2,
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
                id:3,
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

        products: [
            {
                id: 0,
                name: "商品1",
                pic: "http://www.atool.org/placeholder.png?size=300x150&bg=9BCE3B&fg=fff",
                price: 2,
                good: 88,
                sales:100,
            },
            {
                id: 0,
                name: "商品2",
                pic: "http://www.atool.org/placeholder.png?size=300x150&bg=9BCE3B&fg=fff",
                price: 2,
                good: 88,
                sales: 100,
            },
            {
                id: 0,
                name: "商品3",
                pic: "http://www.atool.org/placeholder.png?size=300x150&bg=9BCE3B&fg=fff",
                price: 2,
                good: 88,
                sales: 100,
            },
            {
                id: 0,
                name: "商品4",
                pic: "http://www.atool.org/placeholder.png?size=300x150&bg=9BCE3B&fg=fff",
                price: 2,
                good: 88,
                sales: 100,
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
    }

})
