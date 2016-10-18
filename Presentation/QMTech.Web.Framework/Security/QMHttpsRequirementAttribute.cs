using QMTech.Core;
using QMTech.Core.Domain;
using QMTech.Core.Domain.Security;
using QMTech.Core.Infrastructure;
using System;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace QMTech.Web.Framework.Security
{
    /// <summary>
    /// 暂时未实现
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class QMHttpsRequirementAttribute : FilterAttribute, IAuthorizationFilter
    {
        public QMHttpsRequirementAttribute(SslRequirement sslRequirement)
        {
            this.SslRequirement = sslRequirement;
        }

        public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, 
            Func<Task<HttpResponseMessage>> continuation)
        {
            if (actionContext == null)
                throw new ArgumentNullException("filterContext");

            // only redirect for GET requests, 
            // otherwise the browser might not propagate the verb and request body correctly.
            if (!String.Equals(actionContext.Request.Method.Method, "GET", StringComparison.OrdinalIgnoreCase))
                return Task.FromResult<HttpResponseMessage>(new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest));
           
            var securitySettings = EngineContext.Current.Resolve<SecuritySettings>();
            if (securitySettings.ForceSslForAllPages)
                //all pages are forced to be SSL no matter of the specified value
                this.SslRequirement = SslRequirement.Yes;

            switch (this.SslRequirement)
            {
                case SslRequirement.Yes:
                    {
                        var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                        var currentConnectionSecured = webHelper.IsCurrentConnectionSecured();
                        if (!currentConnectionSecured)
                        {

                            var systemInformationSettings = EngineContext.Current.Resolve<SystemInformationSettings>();  //原本设置存储在每个Store中
                            if (systemInformationSettings.SslEnabled)
                            {
                                //redirect to HTTPS version of page
                                //string url = "https://" + filterContext.HttpContext.Request.Url.Host + filterContext.HttpContext.Request.RawUrl;
                                string url = webHelper.GetThisPageUrl(true, true);

                                //301 (permanent) redirection
                                //filterContext.Result = new RedirectResult(url, true);
                            }
                        }
                    }
                    break;
                case SslRequirement.No:
                    {
                        var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                        var currentConnectionSecured = webHelper.IsCurrentConnectionSecured();
                        if (currentConnectionSecured)
                        {
                            //redirect to HTTP version of page
                            //string url = "http://" + filterContext.HttpContext.Request.Url.Host + filterContext.HttpContext.Request.RawUrl;
                            string url = webHelper.GetThisPageUrl(true, false);
                            //301 (permanent) redirection
                            //filterContext.Result = new RedirectResult(url, true);
                        }
                    }
                    break;
                case SslRequirement.NoMatter:
                    {
                        //do nothing
                    }
                    break;
                default:
                    throw new QMTechException("Not supported SslProtected parameter");
            }
            throw new NotImplementedException();
        }

        public SslRequirement SslRequirement { get; set; }
    }
}
