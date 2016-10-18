using QMTech.Core;
using QMTech.Core.Domain.Customers;
using QMTech.Services.Security;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QMTech.Services.Customers
{
    public class CustomerRegistrationService : ICustomerRegistrationService
    {
        private readonly ICustomerService _customerService;
        private readonly IEncryptionService _encryptionService;

        private readonly RewardPointsSettings _rewardPointsSettings;
        private readonly CustomerSettings _customerSettings;

        public CustomerRegistrationService(ICustomerService customerService, IEncryptionService encryptionService,
             RewardPointsSettings rewardPointsSettings,
        CustomerSettings customerSettings)
        {
            _customerService = customerService;
            _encryptionService = encryptionService;
            _rewardPointsSettings = rewardPointsSettings;
            _customerSettings = customerSettings;
        }

        /// <summary>
        /// 验证用户的正确性
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Task<Customer> ValidateCustomerAsync(string account, string password)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException("密码不能为空");

            var customer = _customerService.GetCustomerByAccount(account);
            if (customer != null)
            {
                bool passwordCorrect = false;
                switch (customer.PasswordFormat)
                {
                    case PasswordFormat.Clear:
                        {
                            passwordCorrect = password == customer.Password;
                        }
                        break;
                    case PasswordFormat.Encrypted:
                        {
                            passwordCorrect = _encryptionService.EncryptText(password) == customer.Password;
                        }
                        break;
                    case PasswordFormat.Hashed:
                        {
                            string saltKey = _encryptionService.CreateSaltKey(5);
                            passwordCorrect = _encryptionService.CreatePasswordHash(password, saltKey, _customerSettings.HashedPasswordFormat) == customer.Password;
                        }
                        break;
                    default:
                        break;
                }

                if (passwordCorrect) return Task.FromResult(customer);
            }

            return Task.FromResult<Customer>(null); ;
        }

        /// <summary>
        /// 验证用户密码
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public CustomerLoginResults ValidateCustomer(string account, string password)
        {
            var customer = _customerService.GetCustomerByAccount(account);

            if (customer == null)
                return CustomerLoginResults.CustomerNotExist;
            if (customer.Deleted)
                return CustomerLoginResults.Deleted;
            if (!customer.Active)
                return CustomerLoginResults.NotActive;
            //only registered can login
            if (!customer.IsRegistered())
                return CustomerLoginResults.NotRegistered;

            string pwd = "";
            switch (customer.PasswordFormat)
            {
                case PasswordFormat.Encrypted:
                    pwd = _encryptionService.EncryptText(password);
                    break;
                case PasswordFormat.Hashed:
                    pwd = _encryptionService.CreatePasswordHash(password, customer.PasswordSalt, _customerSettings.HashedPasswordFormat);
                    break;
                default:
                    pwd = password;
                    break;
            }

            bool isValid = pwd == customer.Password;
            if (!isValid)
                return CustomerLoginResults.WrongPassword;

            //save last login date
            customer.LastLoginDate = DateTime.Now;
            _customerService.UpdateCustomer(customer);
            return CustomerLoginResults.Successful;

        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual CustomerRegistrationResult RegisterCustomer(CustomerRegistrationRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (request.Customer == null)
                throw new ArgumentNullException("无法加载当前用户");

            var result = new CustomerRegistrationResult();
            if (request.Customer.IsSearchEngineAccount())
            {
                result.AddError("搜索引擎用户无法注册");
                return result;
            }
            if (request.Customer.IsBackgroundTaskAccount())
            {
                result.AddError("Background task account can't be registered");
                return result;
            }
            //暂时只有手机号认证
            //if (String.IsNullOrEmpty(request.Email))
            //{
            //    result.AddError("电子邮件地址不能为空");
            //    return result;
            //}
            //if (!CommonHelper.IsValidEmail(request.Email))
            //{
            //    result.AddError("电子邮件地址格式错误");
            //    return result;
            //}
            if (String.IsNullOrEmpty(request.MobilePhone))
            {
                result.AddError("手机号码不能为空");
                return result;
            }
            if (!CommonHelper.IsValidMobileNumber(request.MobilePhone))
            {
                result.AddError("手机号码格式错误");
                return result;
            }
            if (String.IsNullOrWhiteSpace(request.Password))
            {
                result.AddError("密码不能为空");
                return result;
            }
            //暂时
            //if (_customerSettings.UsernamesEnabled)
            //{
            //    if (String.IsNullOrEmpty(request.Username))
            //    {
            //        result.AddError("用户名不能为空");
            //        return result;
            //    }
            //}

            //用户惟一性检查
            if (_customerService.GetCustomerByMobileNumber(request.MobilePhone) != null)
            {
                result.AddError(string.Format("手机号码已经注册用户"));
                return result;
            }
            //if (_customerSettings.UsernamesEnabled)
            //{
            //    if (_customerService.GetCustomerByUsername(request.Username) != null)
            //    {
            //        result.AddError(string.Format("用户名已经注册用户"));
            //        return result;
            //    }
            //}

            //at this point request is valid
            request.Customer.UserName = request.Username;
            request.Customer.Email = request.Email;
            request.Customer.MobilePhone = request.MobilePhone;
            request.Customer.PasswordFormat = request.PasswordFormat;

            switch (request.PasswordFormat)
            {
                case PasswordFormat.Clear:
                    {
                        request.Customer.Password = request.Password;
                    }
                    break;
                case PasswordFormat.Encrypted:
                    {
                        request.Customer.Password = _encryptionService.EncryptText(request.Password);
                    }
                    break;
                case PasswordFormat.Hashed:
                    {
                        string saltKey = _encryptionService.CreateSaltKey(5);
                        request.Customer.PasswordSalt = saltKey;
                        request.Customer.Password = _encryptionService.CreatePasswordHash(request.Password, saltKey, _customerSettings.HashedPasswordFormat);
                    }
                    break;
                default:
                    break;
            }

            request.Customer.Active = request.IsApproved;

            //add to 'Registered' role
            var registeredRole = _customerService.GetCustomerRoleBySystemName(SystemCustomerRoleNames.Registered);
            if (registeredRole == null)
                throw new QMTechException("'Registered' role could not be loaded");
            request.Customer.CustomerRoles.Add(registeredRole);
            //remove from 'Guests' role
            var guestRole = request.Customer.CustomerRoles.FirstOrDefault(cr => cr.SystemName == SystemCustomerRoleNames.Guests);
            if (guestRole != null)
                request.Customer.CustomerRoles.Remove(guestRole);

            //Add reward points for customer registration (if enabled)
            if (_rewardPointsSettings.Enabled &&
                _rewardPointsSettings.PointsForRegistration > 0)
            {
                //TODO：注册获得积分
                // request.Customer.AddRewardPointsHistoryEntry(_rewardPointsSettings.PointsForRegistration, "注册获得积分");
            }

            _customerService.UpdateCustomer(request.Customer);
            return result;
        }


    }
}
