using FluentValidation.Attributes;
using QMTech.Api.Validators.Customer;
using QMTech.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QMTech.Api.Models.Customer
{
    //[Validator(typeof(LoginValidator))]
    public class LoginModel:BaseQMModel
    {
        /// <summary>
        /// 用户名 可能是邮件 手机号 用户名
        /// </summary>
        public string Account { get; set; }

        public string Email { get; set; }

        public bool UsernameEnabled { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}