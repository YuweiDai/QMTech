function formatTime(date) {
  var year = date.getFullYear()
  var month = date.getMonth() + 1
  var day = date.getDate()

  var hour = date.getHours()
  var minute = date.getMinutes()
  var second = date.getSeconds();


  return [year, month, day].map(formatNumber).join('/') + ' ' + [hour, minute, second].map(formatNumber).join(':')
}

function formatNumber(n) {
  n = n.toString()
  return n[1] ? n : '0' + n
}

function validatemobile(mobile) {
    if (mobile.length == 0) {      
        return false;
    }
    if (mobile.length != 11) {
        return false;
    }

    var myreg = /^(((13[0-9]{1})|(15[0-9]{1})|(18[0-9]{1}))+\d{8})$/;
    if (!myreg.test(mobile)) { 
        return false;
    }

    return true;
}

function getCurrentLocation() {
    //获取当前位置
    wx.getLocation({
        type: 'gcj02',
        success: function (res) {
            var app = getApp()
            // Get the global data and change it.
            app.globalData.test = 1;
        },
        fail: function (res) {

        }
    });
}

module.exports = {
    formatTime: formatTime,
    getCurrentLocation: getCurrentLocation,
    validatemobile: validatemobile
}
