using System;
using QMTech.Core.Domain.Customers;
using System.Net;

namespace QMTech.Services.Authentication.External
{
    public class WechatAppAuthorizer : IWechatAppAuthorizer
    {
        public Customer GetCusteomer(string jsCode)
        {
            HttpWebRequest webRequest = WebRequest.Create("https://api.weixin.qq.com/sns/jscode2session?appid=APPID&secret=SECRET&js_code=JSCODE&grant_type=authorization_code") as HttpWebRequest;
            webRequest.Method = "GET";
            HttpWebResponse response = webRequest.GetResponse() as HttpWebResponse;

            //判断获取的
        }
    }
}
