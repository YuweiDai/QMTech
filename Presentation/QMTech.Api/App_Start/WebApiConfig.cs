using QMTech.Web.Framework.WebAPI;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace QMTech.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务

            // Web API 路由
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
               name: "DefaultApi",
               routeTemplate: "{controller}/{id}",
               defaults: new { id = RouteParameter.Optional }
           );

            //config.Routes.MapHttpRoute("DefaultAreaApi", "{area}/{controller}/{id}", new { id = RouteParameter.Optional });

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            //修改控制器路由查找
            //config.Services.Replace(typeof(IHttpControllerSelector), new AreaHttpControllerSelector(GlobalConfiguration.Configuration));
        }
    }
}
