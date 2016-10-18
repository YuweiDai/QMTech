using QMTech.Core.Domain;
using QMTech.Core.Infrastructure;
using System;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;

namespace QMTech.Web.Framework
{
    /// <summary>
    /// 为实现
    /// </summary>
    public class StoreClosedAttribute : ActionFilterAttribute
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

            var storeInformationSettings = EngineContext.Current.Resolve<SystemInformationSettings>();
            if (!storeInformationSettings.StoreClosed)
                return;

            ////<controller, action>
            //var allowedPages = new List<Tuple<string, string>>();
            ////login page
            //allowedPages.Add(new Tuple<string, string>("Nop.Web.Controllers.CustomerController", "Login"));
            ////logout page
            //allowedPages.Add(new Tuple<string, string>("Nop.Web.Controllers.CustomerController", "Logout"));
            ////store closed page
            //allowedPages.Add(new Tuple<string, string>("Nop.Web.Controllers.CommonController", "EuCookieLawAccept"));
            ////the method (AJAX) for accepting EU Cookie law
            //allowedPages.Add(new Tuple<string, string>("Nop.Web.Controllers.CommonController", "StoreClosed"));
            ////the change language page (request)
            //allowedPages.Add(new Tuple<string, string>("Nop.Web.Controllers.CommonController", "SetLanguage"));
            ////contact us page
            //allowedPages.Add(new Tuple<string, string>("Nop.Web.Controllers.CommonController", "ContactUs"));
            //allowedPages.Add(new Tuple<string, string>("Nop.Web.Controllers.CommonController", "ContactUsSend"));
            //var isPageAllowed = allowedPages.Any(
            //    x => controllerName.Equals(x.Item1, StringComparison.InvariantCultureIgnoreCase) &&
            //         actionName.Equals(x.Item2, StringComparison.InvariantCultureIgnoreCase));
            //if (isPageAllowed)
            //    return;

            ////topics accessible when a store is closed
            //if (controllerName.Equals("Nop.Web.Controllers.TopicController", StringComparison.InvariantCultureIgnoreCase) &&
            //    actionName.Equals("TopicDetails", StringComparison.InvariantCultureIgnoreCase))
            //{
            //    //var topicService = EngineContext.Current.Resolve<ITopicService>();
            //    //var storeContext = EngineContext.Current.Resolve<IStoreContext>();
            //    //var allowedTopicIds = topicService.GetAllTopics(storeContext.CurrentStore.Id)
            //    //    .Where(t => t.AccessibleWhenStoreClosed)
            //    //    .Select(t => t.Id)
            //    //    .ToList();
            //    //var requestedTopicId = actionContext.RouteData.Values["topicId"] as int?;
            //    //if (requestedTopicId.HasValue && allowedTopicIds.Contains(requestedTopicId.Value))
            //    //    return;
            //}

            ////allow admin access
            //if (storeInformationSettings.StoreClosedAllowForAdmins &&
            //    EngineContext.Current.Resolve<IWorkContext>().CurrentUser.IsAdmin())
            //    return;


            //var storeClosedUrl = new UrlHelper(actionContext.RequestContext).RouteUrl("StoreClosed");
            //actionContext.Result = new RedirectResult(storeClosedUrl);
        }
    }
}
