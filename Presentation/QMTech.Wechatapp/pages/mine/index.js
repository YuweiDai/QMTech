//index.js
//获取应用实例
var app = getApp()
Page({
  data: {
    motto: '登录',
    userInfo: {},
    appInfo:{
      logoUrl:'../../image/user.png',
      title:'登录'
    }
  },
  //事件处理函数
  bindViewTap: function() {
   console.log("taped me");
  },
  onLoad: function () {
    console.log('onLoad')
    var that = this
    console.log(this.data)
    that.setData({
        appInfo:this.data.appInfo
    })
  	//调用应用实例的方法获取全局数据
    // app.getUserInfo(function(userInfo){
    //   //更新数据
    //   that.setData({
    //     userInfo:userInfo
    //   })
    //   that.update()
    // })
  },
  navToAddress: function (e) {
      wx.navigateTo({ url: "address/index" });
  }
})
