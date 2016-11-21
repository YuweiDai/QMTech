using QMTech.Core;
using QMTech.Core.Infrastructure;
using QMTech.Services.Customers;
using System;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;

namespace QMTech.Web.Framework.Controllers
{
    public class CustomerLastActivityAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext == null || actionContext.Request == null || actionContext.Request.Method.Method== null)
                return;

            //only GET requests
            if (!String.Equals(actionContext.Request.Method.Method, "GET", StringComparison.OrdinalIgnoreCase))
                return;

            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            var customer = workContext.CurrentCustomer;

            //update last activity date
            if (customer.LastActivityDate.AddMinutes(1.0) < DateTime.Now)
            {
                var customerService = EngineContext.Current.Resolve<ICustomerService>();
                customer.LastActivityDate = DateTime.Now;
                customerService.UpdateCustomer(customer);
            }
        }
    }
}