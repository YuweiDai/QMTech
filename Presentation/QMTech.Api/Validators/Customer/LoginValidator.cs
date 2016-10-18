using QMTech.Api.Models.Customer;
using QMTech.Core.Domain.Customers;
using QMTech.Web.Framework.Validators;
using FluentValidation;

namespace QMTech.Api.Validators.Customer
{
    public class LoginValidator:BaseQMValidator<LoginModel>
    {
        public LoginValidator(CustomerSettings customerSettings)
        {
            if(!customerSettings.UsernamesEnabled)
            {
                RuleFor(x => x.Account).NotEmpty().WithMessage("登录账户不能为空");
            }
        }
    }
}