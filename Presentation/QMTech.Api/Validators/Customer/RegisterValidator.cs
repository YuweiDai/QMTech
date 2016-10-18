using FluentValidation;
using QMTech.Api.Models.Customer;
using QMTech.Core.Domain.Customers;
using QMTech.Services.Customers;
using QMTech.Web.Framework.Validators;

namespace QMTech.Api.Validators.Customer
{
    public class RegisterValidator : BaseQMValidator<RegisterModel>
    {
        private readonly ICustomerService _customerService = null;

        public RegisterValidator(ICustomerService customerService, CustomerSettings customerSettings)
        {
            _customerService = customerService;

            //RuleFor(x => x.Email).NotEmpty().WithMessage("邮件地址不能为空");
            RuleFor(x => x.Email).EmailAddress().WithMessage("电子邮件格式错误");

            RuleFor(x => x.Mobile).NotEmpty().WithMessage("手机号码不能为空");
            RuleFor(x => x.Mobile).Must(BeUniqueName).WithMessage("手机号码 {0} 已存在", x => x.Mobile);

            RuleFor(x => x.Password).NotEmpty().WithMessage("密码不能为空");
            RuleFor(x => x.Password).Length(customerSettings.PasswordMinLength, 999).WithMessage(string.Format("密码最小长度必须为 {0}", customerSettings.PasswordMinLength));
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("确认密码不能为空");
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("两次密码不一致");


        }


        /// <summary>
        /// 名称唯一性验证
        /// </summary>
        /// <param name="registerModel"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        private bool BeUniqueName(RegisterModel registerModel, string mobile)
        {
            return _customerService.GetCustomerByMobileNumber(mobile) == null;
        }
    }
}