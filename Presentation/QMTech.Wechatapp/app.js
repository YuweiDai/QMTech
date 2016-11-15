var common = require('utils/util.js');

//app.js
App({
    onLaunch: function () {

        wx.login({
            success: function(res) {
                if (res.code) {
                    //发起网络请求
                    wx.request({
                        url: 'https://test.com/onLogin',
                        data: {
                            code: res.code
                        }
                    })
                } else {
                    console.log('获取用户登录态失败！' + res.errMsg)
                }
            };

    //调用API从本地缓存中获取数据
    var logs = wx.getStorageSync('logs') || []
    logs.unshift(Date.now())
    wx.setStorageSync('logs', logs);

    common.getCurrentLocation();

    this.getUserInfo();
  },

  getUserInfo:function(cb){
    var that = this;
    if(this.globalData.userInfo){
      typeof cb == "function" && cb(this.globalData.userInfo)
    }else{
      //调用登录接口
      wx.login({
        success: function () {
          wx.getUserInfo({
            success: function (res) {
              that.globalData.userInfo = res.userInfo;
              typeof cb == "function" && cb(that.globalData.userInfo)
            }
          })
        }
      });
    }
  },
  globalData:{
      userInfo: null,
      test:0,
      currentAddress: {
          lat: 0,
          lng: 0,
          around: [
              {
                  name: "",
                  lat: 0,
                  lng: 0
              }
          ]
      }
  }
})
