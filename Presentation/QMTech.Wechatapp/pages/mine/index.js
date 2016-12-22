//index.js
//获取应用实例
var app = getApp()
Page({
  data: {
    userInfo: {
      avatarUrl:'',      
    },
  },
  //事件处理函数
  bindViewTap: function() {
   console.log("taped me");
  },
  onLoad: function () {
  if(app.globalData.userInfo){
       this.setData({
        userInfo:app.globalData.userInfo
      })
  }
  },
  navToAddress: function (e) {
      wx.navigateTo({ url: "address/index" });
  }
})