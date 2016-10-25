
//index.js
//获取应用实例
var app = getApp()
Page({
    data: {
        selectedMenu: "全部订单",
        nav_pills_menus: ["全部订单", "待评价"],
        orders: [
            {
                id: 0,
                shop: {
                    id: 0,
                    pic: "http://atool.org/placeholder.png",
                    name: "肯德基宅急送（衢州香溢店）",
                },
                status: "订单完成",
                products: [
                    {
                        name: "香醇奶茶（冰）",
                        count: 1
                    },
                    {
                        name: "手撕猪肉卷土豆泥餐",
                        count: 1
                    },
                    {
                        name: "新奥尔良烤鸡腿堡",
                        count: 1
                    }
                ],
                productsCount: 3,
                fee: 51
            },
            {
                id: 0,
                shop: {
                    id: 0,
                    pic: "http://atool.org/placeholder.png",
                    name: "肯德基宅急送（衢州香溢店）",
                },
                status: "订单完成",
                products: [
                    {
                        name: "香醇奶茶（冰）",
                        count: 1
                    },
                    {
                        name: "手撕猪肉卷土豆泥餐",
                        count: 1
                    },
                    {
                        name: "新奥尔良烤鸡腿堡",
                        count: 1
                    }
                ],
                productsCount:4,
                fee: 51
            },
            {
                id: 0,
                shop: {
                    id: 0,
                    pic: "http://atool.org/placeholder.png",
                    name: "肯德基宅急送（衢州香溢店）",
                },
                status: "订单完成",
                products: [
                    {
                        name: "香醇奶茶（冰）",
                        count: 1
                    },
                    {
                        name: "手撕猪肉卷土豆泥餐",
                        count: 1
                    },
                    {
                        name: "新奥尔良烤鸡腿堡",
                        count: 1
                    }
                ],
                productsCount: 3,
                fee: 51
            },
            {
                id: 0,
                shop: {
                    id: 0,
                    pic: "http://atool.org/placeholder.png",
                    name: "肯德基宅急送（衢州香溢店）",
                },
                status: "订单完成",
                products: [
                    {
                        name: "香醇奶茶（冰）",
                        count: 1
                    },
                    {
                        name: "手撕猪肉卷土豆泥餐",
                        count: 1
                    },
                    {
                        name: "新奥尔良烤鸡腿堡",
                        count: 1
                    }
                ],
                productsCount: 4,
                fee: 51
            }
        ]
    },
    selectItem: function (e) {
        this.setData({
            selectedMenu: e.currentTarget.dataset.item
        })
    },
})
