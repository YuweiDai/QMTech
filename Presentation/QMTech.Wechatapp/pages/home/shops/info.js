Page({
    data: {
        shop: {
            name: "测试商家一",
            noticement: "哈哈哈哈哈哈",
            categories: [
                {
                    name: "类别一",
                    products: [
                        {
                            id: 0,
                            name: "商品1",
                            pic: "http://www.atool.org/placeholder.png?size=300x150&bg=9BCE3B&fg=fff",
                            price: 2,
                            good: 88,
                            sales: 100,
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
                {
                    name: "类别二",
                    products: [
                        {
                            id: 0,
                            name: "商品1",
                            pic: "http://www.atool.org/placeholder.png?size=300x150&bg=9BCE3B&fg=fff",
                            price: 2,
                            good: 88,
                            sales: 100,
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
                {
                    name: "类别三",
                    products: [
                        {
                            id: 0,
                            name: "商品1",
                            pic: "http://www.atool.org/placeholder.png?size=300x150&bg=9BCE3B&fg=fff",
                            price: 2,
                            good: 88,
                            sales: 100,
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
                }
            ]
        }
    },
    onLoad: function (options) {
        this.setData({
            id: options.id
        })
    },

    bindViewTap:function(e)
    {
        console.log("open noticement")
    }
})
