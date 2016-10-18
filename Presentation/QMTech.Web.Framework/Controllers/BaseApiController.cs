using System.Web.Http;

namespace QMTech.Web.Framework.Controllers
{
    [StoreIpAddress]
    [CustomerLastActivity]
    public abstract class BaseApiController : ApiController
    {

        #region 原来MVC的实现
        //protected void LogException(Exception exc)
        //{
        //    var workContext = EngineContext.Current.Resolve<IWorkContext>();
        //    var logger = EngineContext.Current.Resolve<ILogger>();

        //    var customer = workContext.CurrentUser;
        //    logger.Error(exc.Message, exc, customer);
        //}

        ///// <summary>
        ///// Display success notification
        ///// </summary>
        ///// <param name="message">Message</param>
        ///// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        //protected virtual void SuccessNotification(string message, bool persistForTheNextRequest = true)
        //{
        //    AddNotification(NotifyType.Success, message, persistForTheNextRequest);
        //}

        ///// <summary>
        ///// Display error notification
        ///// </summary>
        ///// <param name="message">Message</param>
        ///// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        //protected virtual void ErrorNotification(string message, bool persistForTheNextRequest = true)
        //{
        //    AddNotification(NotifyType.Error, message, persistForTheNextRequest);
        //}

        ///// <summary>
        ///// Display error notification
        ///// </summary>
        ///// <param name="exception">Exception</param>
        ///// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        ///// <param name="logException">A value indicating whether exception should be logged</param>
        //protected virtual void ErrorNotification(Exception exception, bool persistForTheNextRequest = true, bool logException = true)
        //{
        //    if (logException)
        //        LogException(exception);
        //    AddNotification(NotifyType.Error, exception.Message, persistForTheNextRequest);
        //}
        ///// <summary>
        ///// Display notification
        ///// </summary>
        ///// <param name="type">Notification type</param>
        ///// <param name="message">Message</param>
        ///// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        //protected virtual void AddNotification(NotifyType type, string message, bool persistForTheNextRequest)
        //{
        //    string dataKey = string.Format("qm.notifications.{0}", type);
        //    //if (persistForTheNextRequest)
        //    //{
        //    //    if (TempData[dataKey] == null)
        //    //        TempData[dataKey] = new List<string>();
        //    //    ((List<string>)TempData[dataKey]).Add(message);
        //    //}
        //    //else
        //    //{
        //    //    if (ViewData[dataKey] == null)
        //    //        ViewData[dataKey] = new List<string>();
        //    //    ((List<string>)ViewData[dataKey]).Add(message);
        //    //}
        //}

        ///// <summary>
        ///// 遍历获取模型错误
        ///// </summary>
        ///// <returns></returns>
        ////protected virtual string GetModelErrorMessage()
        ////{
        ////    System.Text.StringBuilder message = new System.Text.StringBuilder();

        ////    //获取所有错误的Key 
        ////    //获取每一个key对应的ModelStateDictionary
        ////    foreach (var key in ModelState.Keys)
        ////    {
        ////        var errors = ModelState[key].Errors.ToList();
        ////        //将错误描述添加到sb中
        ////        foreach (var error in errors)
        ////        {                   
        ////            message.Append(error.ErrorMessage);
        ////        }
        ////    }

        ////    //foreach (var error in ModelState.Values)
        ////    //{
        ////    //    if (!string.IsNullOrEmpty(error.))
        ////    //        message.Append(string.Format("字段 {0} ，错误 {1};\n", error, ModelState[error]));
        ////    //    else
        ////    //        message.Append(ModelState[error]);
        ////    //}

        ////    return message.ToString();
        ////}

        /////// <summary>
        /////// 因模型验证错误的返回错误提示
        /////// </summary>
        /////// <returns></returns>
        ////protected virtual IHttpActionResult ModelStateBadrequest(string erroMsg = "")
        ////{
        ////    var error = GetModelErrorMessage();
        ////    return BadRequest(string.IsNullOrEmpty(erroMsg) ? error : erroMsg + "\n" + error);
        ////} 
        #endregion

    }
}
