using QMTech.Core;
using QMTech.Core.Infrastructure;
using QMTech.Services.Logging;
using QMTech.Web.Framework.UI;
using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http.Filters;

namespace QMTech.Web.Framework.Filters
{
    public class APIExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private class ExcepitonModel
        {
            public int Code { get; set; }

            public string Message { get; set; }
        }

        private HttpResponseMessage GetResponse(int code, string message)
        {
            var resultModel = new ExcepitonModel() { Code = code, Message = message };

            return new HttpResponseMessage()
            {
                Content = new ObjectContent<ExcepitonModel>(
                    resultModel,
                    new JsonMediaTypeFormatter(),
                    "application/json"
                    )
            };
        }

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var code = -1;
            var message = "请求失败!";

            if (actionExecutedContext.Exception is QMTechException)
            {
                message = actionExecutedContext.Exception.Message;
            }
            else
            {
                code = -2;
                message = actionExecutedContext.Exception.Message;
            }

            if (actionExecutedContext.Response == null)
            {
                actionExecutedContext.Response = GetResponse(code, message);
            }

            LogException(actionExecutedContext.Exception);

            base.OnException(actionExecutedContext);
        }      

        protected void LogException(Exception exc)
        {
            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            var logger = EngineContext.Current.Resolve<ILogger>();

            var customer = workContext.CurrentUser;
            logger.Error(exc.Message, exc, customer);
        }

        /// <summary>
        /// Display success notification
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        protected virtual void SuccessNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Success, message, persistForTheNextRequest);
        }

        /// <summary>
        /// Display error notification
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        protected virtual void ErrorNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Error, message, persistForTheNextRequest);
        }

        /// <summary>
        /// Display error notification
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        /// <param name="logException">A value indicating whether exception should be logged</param>
        protected virtual void ErrorNotification(Exception exception, bool persistForTheNextRequest = true, bool logException = true)
        {
            if (logException)
                LogException(exception);
            AddNotification(NotifyType.Error, exception.Message, persistForTheNextRequest);
        }
        /// <summary>
        /// Display notification
        /// </summary>
        /// <param name="type">Notification type</param>
        /// <param name="message">Message</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        protected virtual void AddNotification(NotifyType type, string message, bool persistForTheNextRequest)
        {
            string dataKey = string.Format("qm.notifications.{0}", type);
            //if (persistForTheNextRequest)
            //{
            //    if (TempData[dataKey] == null)
            //        TempData[dataKey] = new List<string>();
            //    ((List<string>)TempData[dataKey]).Add(message);
            //}
            //else
            //{
            //    if (ViewData[dataKey] == null)
            //        ViewData[dataKey] = new List<string>();
            //    ((List<string>)ViewData[dataKey]).Add(message);
            //}
        }
    }
}
