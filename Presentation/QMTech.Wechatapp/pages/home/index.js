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
        shops: [
            {
                name: "测试商家一",
                star: 5,
                sales: 55,
                pic: ""
            }
        ]
    },
    changeIndicatorDots: function (e) {
        this.setData({
            indicatorDots: !this.data.indicatorDots
        })
    },
    changeVertical: function (e) {
        this.setData({
            vertical: !this.data.vertical
        })
    },
    changeAutoplay: function (e) {
        this.setData({
            autoplay: !this.data.autoplay
        })
    },
    intervalChange: function (e) {
        this.setData({
            interval: e.detail.value
        })
    },
    durationChange: function (e) {
        this.setData({
            duration: e.detail.value
        })
    }
})
