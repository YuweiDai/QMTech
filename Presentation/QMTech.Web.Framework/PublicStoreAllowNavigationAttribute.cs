using QMTech.Core.Infrastructure;
using QMTech.Services.Security;
using System;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;

namespace QMTech.Web.Framework
{
    /// <summary>
    /// 未实现
    /// </summary>
    public class PublicStoreAllowNavigationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);

            if (actionContext == null)
                return;

            var request = actionContext.Request;
            if (request == null)
                return;

            string actionName = actionContext.ActionDescriptor.ActionName;
            if (String.IsNullOrEmpty(actionName))
                return;

            string controllerName = actionContext.ControllerContext.Controller.ToString();
            if (String.IsNullOrEmpty(controllerName))
                return;

            //暂时不用权限认证
            //var permissionService = EngineContext.Current.Resolve<IPermissionService>();
            //var publicStoreAllowNavigation = permissionService.Authorize(StandardPermissionProvider.PublicStoreAllowNavigation);      
            //if (publicStoreAllowNavigation)
            //    return;

            if (//ensure it's not the Login page
                !(controllerName.Equals("Nop.Web.Controllers.CustomerController", StringComparison.InvariantCultureIgnoreCase) && actionName.Equals("Login", StringComparison.InvariantCultureIgnoreCase)) &&
                //ensure it's not the Logout page
                !(controllerName.Equals("Nop.Web.Controllers.CustomerController", StringComparison.InvariantCultureIgnoreCase) && actionName.Equals("Logout", StringComparison.InvariantCultureIgnoreCase)) &&
                //ensure it's not the Register page
                !(controllerName.Equals("Nop.Web.Controllers.CustomerController", StringComparison.InvariantCultureIgnoreCase) && actionName.Equals("Register", StringComparison.InvariantCultureIgnoreCase)) &&
                !(controllerName.Equals("Nop.Web.Controllers.CustomerController", StringComparison.InvariantCultureIgnoreCase) && actionName.Equals("RegisterResult", StringComparison.InvariantCultureIgnoreCase)) &&
                //ensure it's not the Password recovery page
                !(controllerName.Equals("Nop.Web.Controllers.CustomerController", StringComparison.InvariantCultureIgnoreCase) && actionName.Equals("PasswordRecovery", StringComparison.InvariantCultureIgnoreCase)) &&
                !(controllerName.Equals("Nop.Web.Controllers.CustomerController", StringComparison.InvariantCultureIgnoreCase) && actionName.Equals("PasswordRecoveryConfirm", StringComparison.InvariantCultureIgnoreCase)) &&
                //ensure it's not the Account activation page
                !(controllerName.Equals("Nop.Web.Controllers.CustomerController", StringComparison.InvariantCultureIgnoreCase) && actionName.Equals("AccountActivation", StringComparison.InvariantCultureIgnoreCase)) &&
                //ensure it's not the Register page
                !(controllerName.Equals("Nop.Web.Controllers.CustomerController", StringComparison.InvariantCultureIgnoreCase) && actionName.Equals("CheckUsernameAvailability", StringComparison.InvariantCultureIgnoreCase)) &&
                //ensure it's not the GetStatesByCountryId ajax method (can be used during registration)
                !(controllerName.Equals("Nop.Web.Controllers.CountryController", StringComparison.InvariantCultureIgnoreCase) && actionName.Equals("GetStatesByCountryId", StringComparison.InvariantCultureIgnoreCase)) &&
                //ensure it's not the method (AJAX) for accepting EU Cookie law
                !(controllerName.Equals("Nop.Web.Controllers.CommonController", StringComparison.InvariantCultureIgnoreCase) && actionName.Equals("EuCookieLawAccept", StringComparison.InvariantCultureIgnoreCase)))
            {
                //var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                //var loginPageUrl = webHelper.GetStoreLocation() + "login";
                //var loginPageUrl = new UrlHelper(actionContext.RequestContext).RouteUrl("login");
                //actionContext.Result = new RedirectResult(loginPageUrl);
                //actionContext.Result = new HttpUnauthorizedResult();

                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }
        }
    }
}
