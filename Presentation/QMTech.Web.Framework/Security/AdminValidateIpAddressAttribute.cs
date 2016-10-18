using QMTech.Core;
using QMTech.Core.Domain.Security;
using QMTech.Core.Infrastructure;
using System;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Net.Http;

namespace QMTech.Web.Framework.Security
{
    public class AdminValidateIpAddressAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext == null)
                return;

            var request = actionContext.Request;
            if (request == null)
                return;

            bool ok = false;
            var ipAddresses = EngineContext.Current.Resolve<SecuritySettings>().AdminAreaAllowedIpAddresses;
            if (ipAddresses != null && ipAddresses.Count > 0)
            {
                var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                foreach (string ip in ipAddresses)
                    if (ip.Equals(webHelper.GetCurrentIpAddress(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        ok = true;
                        break;
                    }
            }
            else
            {
                //no restrictions
                ok = true;
            }

            if (!ok)
            {
                actionContext.Response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.Forbidden };
                actionContext.Response.Content = new StringContent("不在预定义的IP集合范围", System.Text.Encoding.Unicode);
            }
        }
    }
}
