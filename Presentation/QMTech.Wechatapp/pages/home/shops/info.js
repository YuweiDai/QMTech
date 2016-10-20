//Page({
//    data: {
//        shop: {
//            name: "测试商家一",
//            noticement: "哈哈哈哈哈哈",
//            categories: [
//                {
//                    name: "类别一",
//                    products: [
//                        {
//                            id: 0,
//                            name: "商品1",
//                            pic: "http://www.atool.org/placeholder.png?size=300x150&bg=9BCE3B&fg=fff",
//                            price: 2,
//                            good: 88,
//                            sales: 100,
//                        },
//                        {
//                            id: 0,
//                            name: "商品2",
//                            pic: "http://www.atool.org/placeholder.png?size=300x150&bg=9BCE3B&fg=fff",
//                            price: 2,
//                            good: 88,
//                            sales: 100,
//                        },
//                        {
//                            id: 0,
//                            name: "商品3",
//                            pic: "http://www.atool.org/placeholder.png?size=300x150&bg=9BCE3B&fg=fff",
//                            price: 2,
//                            good: 88,
//                            sales: 100,
//                        },
//                        {
//                            id: 0,
//                            name: "商品4",
//                            pic: "http://www.atool.org/placeholder.png?size=300x150&bg=9BCE3B&fg=fff",
//                            price: 2,
//                            good: 88,
//                            sales: 100,
//                        }

//                    ]
//                },
//                {
//                    name: "类别二",
//                    products: [
//                        {
//                            id: 0,
//                            name: "商品1",
//                            pic: "http://www.atool.org/placeholder.png?size=300x150&bg=9BCE3B&fg=fff",
//                            price: 2,
//                            good: 88,
//                            sales: 100,
//                        },
//                        {
//                            id: 0,
//                            name: "商品2",
//                            pic: "http://www.atool.org/placeholder.png?size=300x150&bg=9BCE3B&fg=fff",
//                            price: 2,
//                            good: 88,
//                            sales: 100,
//                        },
//                        {
//                            id: 0,
//                            name: "商品3",
//                            pic: "http://www.atool.org/placeholder.png?size=300x150&bg=9BCE3B&fg=fff",
//                            price: 2,
//                            good: 88,
//                            sales: 100,
//                        },
//                        {
//                            id: 0,
//                            name: "商品4",
//                            pic: "http://www.atool.org/placeholder.png?size=300x150&bg=9BCE3B&fg=fff",
//                            price: 2,
//                            good: 88,
//                            sales: 100,
//                        }

//                    ]
//                },
//                {
//                    name: "类别三",
//                    products: [
//                        {
//                            id: 0,
//                            name: "商品1",
//                            pic: "http://www.atool.org/placeholder.png?size=300x150&bg=9BCE3B&fg=fff",
//                            price: 2,
//                            good: 88,
//                            sales: 100,
//                        },
//                        {
//                            id: 0,
//                            name: "商品2",
//                            pic: "http://www.atool.org/placeholder.png?size=300x150&bg=9BCE3B&fg=fff",
//                            price: 2,
//                            good: 88,
//                            sales: 100,
//                        },
//                        {
//                            id: 0,
//                            name: "商品3",
//                            pic: "http://www.atool.org/placeholder.png?size=300x150&bg=9BCE3B&fg=fff",
//                            price: 2,
//                            good: 88,
//                            sales: 100,
//                        },
//                        {
//                            id: 0,
//                            name: "商品4",
//                            pic: "http://www.atool.org/placeholder.png?size=300x150&bg=9BCE3B&fg=fff",
//                            price: 2,
//                            good: 88,
//                            sales: 100,
//                        }

//                    ]
//                }
//            ]
//        }
//    },
//    onLoad: function (options) {
//        this.setData({
//            id: options.id
//        })
//    },

//    bindViewTap:function(e)
//    {
//        console.log("open noticement")
//    }
//})
var app = getApp()
Page({
    data: {
        shop: {
            name: "测试商家一",
            noticement: "哈哈哈哈哈哈",
        },
        curNavMenu: 1,
        curNavMenuIndex:0,
        navMenus: [
			{
			    id: 1,
			    name: '点菜'
			},
			{
			    id: 2,
			    name: '评价'
			},
			{
			    id: 3,
			    name: '商家'
			},
        ],

        hidden: false,
        curNav: 1,
        curIndex: 0,
        cart: [],
        cartTotal: 0,
        navList: [
			{
			    id: 1,
			    name: '热销菜品'
			},
			{
			    id: 2,
			    name: '热菜'
			},
			{
			    id: 3,
			    name: '凉菜'
			},
			{
			    id: 4,
			    name: '套餐'
			}
        ],
        dishesList: [
			[
				{
				    name: "红烧肉",
				    price: 38,
				    num: 1,
				    id: 1
				},
				{
				    name: "宫保鸡丁",
				    price: 58,
				    num: 1,
				    id: 29
				},
				{
				    name: "水煮鱼",
				    price: 88,
				    num: 1,
				    id: 2
				}
			],
			[
				{
				    name: "小炒日本豆腐",
				    price: 18,
				    num: 1,
				    id: 3
				},
				{
				    name: "烤鱼",
				    price: 58,
				    num: 1,
				    id: 4
				}
			],
			[
				{
				    name: "大拌菜",
				    price: 18,
				    num: 1,
				    id: 5
				},
				{
				    name: "川北凉粉",
				    price: 8,
				    num: 1,
				    id: 6
				}
			],
			[]
        ],
        dishes: []
    },
    loadingChange () {
        setTimeout(() => {
            this.setData({
                hidden: true
            })
        }, 2000)
    },
    selectNavMenu (event) {
        let id = event.target.dataset.id,
			index = parseInt(event.target.dataset.index);
        self = this;
        this.setData({
            curNavMenu: id,
            curNavMenuIndex: index
        })
    },
    selectNav (event) {
        let id = event.target.dataset.id,
			index = parseInt(event.target.dataset.index);
        self = this;
        this.setData({
            curNav: id,
            curIndex: index
        })
    },
    // 选择菜品
    selectDish (event) {
        let dish = event.currentTarget.dataset.dish;
        let flag = true;
        let cart = this.data.cart;

        if (cart.length > 0) {
            cart.forEach(function (item, index) {
                if (item == dish) {
                    cart.splice(index, 1);
                    flag = false;
                }
            })
        }
        if (flag) cart.push(dish);
        this.setData({
            cartTotal: cart.length
        })
        this.setStatus(dish)
    },
    setStatus (dishId) {
        let dishes = this.data.dishesList;
        for (let dish of dishes) {
            dish.forEach((item) => {
                if (item.id == dishId) {
                    item.status = !item.status || false
                }
            })
        }

        this.setData({
            dishesList: this.data.dishesList
        })
    },
    onLoad () {
        this.loadingChange()
    }
})