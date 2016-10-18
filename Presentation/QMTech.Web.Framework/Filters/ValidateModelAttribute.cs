using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace QMTech.Web.Framework.Filters
{
    /// <summary>
    /// 模型验证过滤器
    /// </summary>
    public class ValidateModelAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ModelState.IsValid == false)
            {
                System.Text.StringBuilder sbErrors = new System.Text.StringBuilder();
                foreach (var item in actionContext.ModelState.Values)
                {
                    if (item.Errors.Count > 0)
                    {
                        for (int i = item.Errors.Count - 1; i >= 0; i--)
                        {
                            sbErrors.Append(item.Errors[i].ErrorMessage);
                            sbErrors.Append("<br/>");
                        }
                    }
                }


                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest, sbErrors.ToString());
            }
        }
    }
}
