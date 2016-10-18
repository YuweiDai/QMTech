using FluentValidation.Attributes;
using QMTech.Api.Validators.Customer;
using QMTech.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QMTech.Api.Models.Customer
{
    //[Validator(typeof(RegisterValidator))]
    public class RegisterModel : BaseQMModel
    {
        public RegisterModel()
        {

        }

        public string Mobile { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public bool DisplayCaptcha { get; set; }
    }
}