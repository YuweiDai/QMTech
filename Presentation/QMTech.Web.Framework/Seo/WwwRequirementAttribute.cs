using QMTech.Core;
using QMTech.Core.Domain.Seo;
using QMTech.Core.Infrastructure;
using System;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace QMTech.Web.Framework.Seo
{
    /// <summary>
    /// 未实现
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class WwwRequirementAttribute : FilterAttribute, IAuthorizationFilter
    {
        public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            throw new NotImplementedException();
            //if (actionContext == null)
            //    throw new ArgumentNullException("filterContext");


            //// only redirect for GET requests, 
            //// otherwise the browser might not propagate the verb and request body correctly.
            //if (!String.Equals(actionContext.Request.Method.Method, "GET", StringComparison.OrdinalIgnoreCase))
            //    return;

            ////ignore this rule for localhost
            //if (actionContext.HttpContext.Request.IsLocal)
            //    return;

            //var seoSettings = EngineContext.Current.Resolve<SeoSettings>();

            //switch (seoSettings.WwwRequirement)
            //{
            //    case WwwRequirement.WithWww:
            //        {
            //            var webHelper = EngineContext.Current.Resolve<IWebHelper>();
            //            string url = webHelper.GetThisPageUrl(true);
            //            var currentConnectionSecured = webHelper.IsCurrentConnectionSecured();
            //            if (currentConnectionSecured)
            //            {
            //                bool startsWith3W = url.StartsWith("https://www.", StringComparison.OrdinalIgnoreCase);
            //                if (!startsWith3W)
            //                {
            //                    url = url.Replace("https://", "https://www.");

            //                    //301 (permanent) redirection
            //                    filterContext.Result = new RedirectResult(url, true);
            //                }
            //            }
            //            else
            //            {
            //                bool startsWith3W = url.StartsWith("http://www.", StringComparison.OrdinalIgnoreCase);
            //                if (!startsWith3W)
            //                {
            //                    url = url.Replace("http://", "http://www.");

            //                    //301 (permanent) redirection
            //                    filterContext.Result = new RedirectResult(url, true);
            //                }
            //            }
            //        }
            //        break;
            //    case WwwRequirement.WithoutWww:
            //        {
            //            var webHelper = EngineContext.Current.Resolve<IWebHelper>();
            //            string url = webHelper.GetThisPageUrl(true);
            //            var currentConnectionSecured = webHelper.IsCurrentConnectionSecured();
            //            if (currentConnectionSecured)
            //            {
            //                bool startsWith3W = url.StartsWith("https://www.", StringComparison.OrdinalIgnoreCase);
            //                if (startsWith3W)
            //                {
            //                    url = url.Replace("https://www.", "https://");

            //                    //301 (permanent) redirection
            //                    filterContext.Result = new RedirectResult(url, true);
            //                }
            //            }
            //            else
            //            {
            //                bool startsWith3W = url.StartsWith("http://www.", StringComparison.OrdinalIgnoreCase);
            //                if (startsWith3W)
            //                {
            //                    url = url.Replace("http://www.", "http://");

            //                    //301 (permanent) redirection
            //                    filterContext.Result = new RedirectResult(url, true);
            //                }
            //            }
            //        }
            //        break;
            //    case WwwRequirement.NoMatter:
            //        {
            //            //do nothing
            //        }
            //        break;
            //    default:
            //        throw new QMTechException("Not supported WwwRequirement parameter");
            //}
        }
    }
}