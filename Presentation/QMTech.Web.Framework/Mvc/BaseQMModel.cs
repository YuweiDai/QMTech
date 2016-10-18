using System.Collections.Generic;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace QMTech.Web.Framework.Mvc
{
    [ModelBinder(typeof(QMModelBinder))]
    public partial class BaseQMModel
    {
        public BaseQMModel()
        {
            this.CustomProperties = new Dictionary<string, object>();
            PostInitialize();
        }

        public virtual void BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
        }

        /// <summary>
        /// Developers can override this method in custom partial classes
        /// in order to add some custom initialization code to constructors
        /// </summary>
        protected virtual void PostInitialize()
        {

        }

        /// <summary>
        /// Use this property to store any custom value for your models. 
        /// </summary>
        public Dictionary<string, object> CustomProperties { get; set; }
    }

    /// <summary>
    /// Base QMCommerce entity model
    /// </summary>
    public partial class BaseQMEntityModel : BaseQMModel
    {
        public virtual int Id { get; set; }
    }
}
