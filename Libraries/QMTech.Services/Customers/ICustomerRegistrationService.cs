using QMTech.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMTech.Services.Customers
{
    public interface ICustomerRegistrationService
    {
        /// <summary>
        /// 验证用户名和密码
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<Customer> ValidateCustomerAsync(string account, string password);

        /// <summary>
        /// 用户名可以是手机、邮件以及用户名
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        CustomerLoginResults ValidateCustomer(string account, string password);

        /// <summary>
        /// 用户注册请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        CustomerRegistrationResult RegisterCustomer(CustomerRegistrationRequest request);

    }
}
