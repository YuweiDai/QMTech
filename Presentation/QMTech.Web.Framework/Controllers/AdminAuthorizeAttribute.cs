using QMTech.Core;
using QMTech.Core.Infrastructure;
using QMTech.Services.Customers;
using System;
using System.Net;
using System.Net.Http;

namespace QMTech.Web.Framework.Controllers
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class AdminAuthorizeAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        private readonly bool _dontValidate;


        public AdminAuthorizeAttribute()
            : this(false)
        {
        }

        public AdminAuthorizeAttribute(bool dontValidate)
        {
            this._dontValidate = dontValidate;
        }

        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var webContext = EngineContext.Current.Resolve<IWorkContext>();
            var customer = webContext.CurrentUser;
            if (customer == null || !customer.IsRegistered())
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
 
        }
 

        //public override void OnResultExecuting(ResultExecutingContext filterContext)
        //{

        //}

        //private void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        //{
        //    filterContext.Result = new HttpUnauthorizedResult();
        //}

        //private IEnumerable<AdminAuthorizeAttribute> GetAdminAuthorizeAttributes(ActionDescriptor descriptor)
        //{
        //    return descriptor.GetCustomAttributes(typeof(AdminAuthorizeAttribute), true)
        //        .Concat(descriptor.ControllerDescriptor.GetCustomAttributes(typeof(AdminAuthorizeAttribute), true))
        //        .OfType<AdminAuthorizeAttribute>();
        //}

        //private bool IsAdminPageRequested(AuthorizationContext filterContext)
        //{
        //    var adminAttributes = GetAdminAuthorizeAttributes(filterContext.ActionDescriptor);
        //    if (adminAttributes != null && adminAttributes.Any())
        //        return true;
        //    return false;
        //}

        //public void OnAuthorization(AuthorizationContext filterContext)
        //{
        //    if (_dontValidate)
        //        return;

        //    if (filterContext == null)
        //        throw new ArgumentNullException("filterContext");

        //    if (OutputCacheAttribute.IsChildActionCacheActive(filterContext))
        //        throw new InvalidOperationException("You cannot use [AdminAuthorize] attribute when a child action cache is active");





        //    //if (IsAdminPageRequested(filterContext))
        //    //{
        //    //    if (!this.HasAdminAccess(filterContext))
        //    //        this.HandleUnauthorizedRequest(filterContext);
        //    //}
        //}

        //public virtual bool HasAdminAccess(AuthorizationContext filterContext)
        //{
        //    var permissionService = EngineContext.Current.Resolve<IPermissionService>();
        //    bool result = permissionService.Authorize(StandardPermissionProvider.AccessAdminPanel);
        //    return result;
        //}
    }
}
