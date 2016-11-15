var common = require('utils/util.js');

//app.js
App({
  onLaunch: function () {
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
       wx.login({
      success: function(res) {
          if (res.code) {

              wx.getUserInfo({
                  success: function (res1) {
                      console.log(res1);
                  }
              })

            //发起网络请求
            var url = 'https://api.weixin.qq.com/sns/jscode2session?appid=wxf1fe2ec4f67ca88d&secret=89d07bfc96a63e8f4e56bbeeb76bc829&js_code=' + res.code + '&grant_type=authorization_code';

            wx.request({
                url: 'https://api.weixin.qq.com/sns/jscode2session',
                method:"Get",
                data: {
                        appid:"wxf1fe2ec4f67ca88d",
                        secret:"89d07bfc96a63e8f4e56bbeeb76bc829",
                        code:res.code,
                        grant_type:"authorization_code"
                },
                header: {
                    'Content-Type': 'application/json'
                },
                success: function (res) {
                    console.log(res.data)
                },
                fail: function (a) {
                    console.log(a);
                }
            });

          //wx.request({
          //    url: url,

          //  data: {
          //    //appid:"wxf1fe2ec4f67ca88d",
          //    //secret:"89d07bfc96a63e8f4e56bbeeb76bc829",
          //    code:res.code,
          //    //grant_type:"authorization_code"
          //  },
          //  success: function (r) {
          //      console.log(r);
          //  },
          //  fail: function (a) {
          //      console.log(a);
          //  }
          //});
        } else {
          console.log('获取用户登录态失败！' + res.errMsg)
        }
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
