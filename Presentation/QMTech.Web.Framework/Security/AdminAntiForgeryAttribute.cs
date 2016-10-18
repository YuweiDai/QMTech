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
    /// 未实现
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class AdminAntiForgeryAttribute : FilterAttribute, IAuthorizationFilter
    {
        private readonly bool _ignore;

        /// <summary>
        /// Anti-fogery security attribute
        /// </summary>
        /// <param name="ignore">Pass false in order to ignore this security validation</param>
        public AdminAntiForgeryAttribute(bool ignore = false)
        {
            this._ignore = ignore;
        }

        public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            throw new NotImplementedException();

            //if (filterContext == null)
            //    throw new ArgumentNullException("filterContext");

            //if (_ignore)
            //    return;

            ////don't apply filter to child methods
            //if (filterContext.IsChildAction)
            //    return;

            ////only POST requests
            //if (!String.Equals(filterContext.HttpContext.Request.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase))
            //    return;

            //var securitySettings = EngineContext.Current.Resolve<SecuritySettings>();
            //if (!securitySettings.EnableXsrfProtectionForAdminArea)
            //    return;

            //var validator = new ValidateAntiForgeryTokenAttribute();
            //validator.OnAuthorization(filterContext);
        }
    }
}
