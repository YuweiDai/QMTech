var common = require('utils/util.js');

//app.js
App({
  onLaunch: function () {
    //调用API从本地缓存中获取数据
    var logs = wx.getStorageSync('logs') || []
    logs.unshift(Date.now())
    wx.setStorageSync('logs', logs);

    
    //调用应用实例的方法获取全局数据
    this.getUserInfo(function(userInfo){
      //更新数据
      this.setData({
        userInfo:userInfo
      })
    })
  },
  getUserInfo:function(cb){
    var that = this
    if(this.globalData.userInfo){
      typeof cb == "function" && cb(this.globalData.userInfo)
    }else{
      //调用登录接口
      wx.login({
        success: function (login_res) {
          //后台登录
          var url=that.globalData.apiUrl+'ExternalAuth/Wechat';
          //发起网络请求       
          wx.request({
              url:url,
              method:"GET",
              data: {
                code: login_res.code ,     
              },
              header: {
                  'content-type': 'application/json'
              },
              success: function (res) {
                  console.log(res.data)
              },
              fail: function (a) {
                  console.log(a);
              }
          });

          //获取用户信息
          // wx.getUserInfo({
          //   success: function (res) {
          //     that.globalData.userInfo = res.userInfo
          //     typeof cb == "function" && cb(that.globalData.userInfo)
          //   }
          // })
        }
      })
    }
  },
  globalData:{
    apiUrl:"https://localhost/qm/api/",
    userInfo:null
  }
})

//app.js
// App({
//     onLaunch: function () {

//         wx.login({
//             success: function (res) {
//                 if (res.code) {
//                     //发起网络请求
//                     wx.request({
//                         url: 'https://test.com/onLogin',
//                         data: {
//                             code: res.code
//                         }
//                     })
//                 } else {
//                     console.log('获取用户登录态失败！' + res.errMsg)
//                 }
//             }
//         });

//         //调用API从本地缓存中获取数据
//         var logs = wx.getStorageSync('logs') || []
//         logs.unshift(Date.now())
//         wx.setStorageSync('logs', logs);

//         common.getCurrentLocation();

//         this.getUserInfo();
//     },

//     getUserInfo: function (cb) {
//         var that = this;
//         if (this.globalData.userInfo) {
//             typeof cb == "function" && cb(this.globalData.userInfo)
//         } else {
//             wx.login({
//                 success: function (res) {
//                     if (res.code) {

//                         //wx.getUserInfo({
//                         //    success: function (res1) {
//                         //        console.log(res1);
//                         //    }
//                         //});

//                     
//                     } else {
//                         console.log('获取用户登录态失败！' + res.errMsg)
//                     }
//                 }
//             });
//         }
//     },
//     globalData: {
//         userInfo: null,
//         test: 0,
//         currentAddress: {
//             lat: 0,
//             lng: 0,
//             around: [
//                 {
//                     name: "",
//                     lat: 0,
//                     lng: 0
//                 }
//             ]
//         }
//     }
// })
