using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace QMTech.Web.Framework.Mvc
{
    /// <summary>
    /// 貌似用不上
    /// </summary>
    public class QMModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            if (bindingContext == null)
            {
                throw new ArgumentNullException("bindingContext");
            }

            try
            {
                var model = Activator.CreateInstance(bindingContext.ModelType);
                var properties = bindingContext.ModelType.GetProperties();
                foreach (var property in properties)
                {
                    var value = bindingContext.ValueProvider.GetValue(property.Name);
                    if (value == null) continue;
                    property.SetValue(model, value.RawValue);
                }

                bindingContext.Model = model;
                if (model is BaseQMModel)
                {
                    ((BaseQMModel)model).BindModel(actionContext, bindingContext);
                }

                return true;
            }
            catch
            {
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
                actionContext.Response.Content = new StringContent("数据邦定错误");
                return false;
            }

        }
    }
}
