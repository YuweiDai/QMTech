using QMTech.Web.Framework.Controllers;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace QMTech.Api.Controllers
{
    [RoutePrefix("ExternalAuth")]
    public class ExternalAuthController : BaseApiController
    {
        /// <summary>
        /// 微信小程序登录
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("Wechat")]
        public IHttpActionResult Wechat(string code)
        {
            var authUrl = ConfigurationManager.AppSettings["wechat_authUrl"];
            var appid = ConfigurationManager.AppSettings["wechat_appid"];
            var secret = ConfigurationManager.AppSettings["wechat_secret"];

            string url = string.Format("{0}?appid={1}&secret={2}&js_code={3}&grant_type=authorization_code", authUrl, appid, secret, code);

            HttpClient client = new HttpClient();
           
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync(url).Result;  // Blocking call（阻塞调用）! 

            var result = "";

            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsStringAsync().Result;

                JavaScriptSerializer Serializer = new JavaScriptSerializer();
                var items = Serializer.DeserializeObject(result);
                return Json(items);
            }
            else
            {
                return Json(result);
            }

    

            //HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            //request.Method = "GET";

            //var result = request.GetResponse() as HttpWebResponse;

            return Ok();
        }
    }
}
