using FluentValidation;

namespace QMTech.Web.Framework.Validators
{
    public abstract class BaseQMValidator<T> : AbstractValidator<T> where T : class
    {
        protected BaseQMValidator()
        {
            PostInitialize();
        }

        /// <summary>
        /// Developers can override this method in custom partial classes
        /// in order to add some custom initialization code to constructors
        /// </summary>
        protected virtual void PostInitialize()
        {

        }
    }
}
