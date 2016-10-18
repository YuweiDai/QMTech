using QMTech.Api.Models.Customer;
using QMTech.Core;
using QMTech.Core.Domain.Customers;
using QMTech.Services.Authentication;
using QMTech.Services.Common;
using QMTech.Services.Customers;
using QMTech.Services.Messages;
using QMTech.Web.Framework;
using QMTech.Web.Framework.Controllers;
using QMTech.Web.Framework.Response;
using System;
using System.Web;
using System.Web.Http;

namespace QMTech.Api.Controllers.Client
{
    [RoutePrefix("Customers")]
    public class CustomerController : BaseApiController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ICustomerService _customerService;
        private readonly ICustomerRegistrationService _customerRegistrationService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IWorkflowMessageService _workflowMessageService;

        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;
        private readonly CustomerSettings _customerSettings;

        public CustomerController(
            IAuthenticationService authenticationService, 
            ICustomerService customerService,
                 ICustomerRegistrationService customerRegistrationService,
             IGenericAttributeService genericAttributeService,
            IWorkflowMessageService workflowMessageService,
             IWebHelper webHelper,
            IWorkContext workContext,
            CustomerSettings customerSettings)
        {
            _authenticationService = authenticationService;
            _customerService = customerService;
            _customerRegistrationService = customerRegistrationService;
            _genericAttributeService = genericAttributeService;
            _workflowMessageService = workflowMessageService;

            _webHelper = webHelper;
            _workContext = workContext;
            _customerSettings = customerSettings;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public IHttpActionResult Register(RegisterModel model)
        {
            //判断当前是否为认证用户
            if (ControllerContext.RequestContext.Principal.Identity.IsAuthenticated)
                return BadRequest("当前用户已经注册");

            //检查是否允许注册用户
            if (_customerSettings.UserRegistrationType == UserRegistrationType.Disabled)
            {
                return BadRequest("用户注册已关闭");
            }

            if (_workContext.CurrentUser.IsRegistered()) return BadRequest("当前用户已注册");

            var customer = _workContext.CurrentUser;
            if (customer.IsRegistered()) return BadRequest("当前用户已经注册");

            //TODO：自定义属性

            //TODO：验证码

            if (_customerSettings.UsernamesEnabled && model.Username != "")
            {
                model.Username = model.Username.Trim();
            }

            bool isApproved = _customerSettings.UserRegistrationType == UserRegistrationType.Standard;
            var registrationRequest = new CustomerRegistrationRequest(customer, model.Email, model.Mobile, model.Username,
                model.Password, _customerSettings.DefaultPasswordFormat, isApproved);

            var registrationResult = _customerRegistrationService.RegisterCustomer(registrationRequest);
            if (registrationResult.Success)
            {
                //associated with external account (if possible)

                //insert default address (if possible)

                //notifications
                //_workflowMessageService

                switch (_customerSettings.UserRegistrationType)
                {
                    case UserRegistrationType.EmailValidation:
                        {
                            //email validation message
                            _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.AccountActivationToken, Guid.NewGuid().ToString());
                            _workflowMessageService.SendCustomerEmailValidationMessage(customer);

                            //result
                            return Ok(new ApiResponseResult { Code = 1, Message = "邮件认证" });
                        }
                    case UserRegistrationType.AdminApproval:
                        {
                            return Ok(new ApiResponseResult { Code = 1, Message = "管理员认证" });
                        }
                    default:
                        {
                            //send customer welcome message
                            _workflowMessageService.SendCustomerWelcomeMessage(customer);

                            return Ok(new ApiResponseResult { Code = 1, Message = "注册成功" });
                        }
                }
            }

            //errors
            foreach (var error in registrationResult.Errors)
                ModelState.AddModelError("", error);
            return BadRequest(ModelState);
        }

        #region 登陆实现
        //[HttpPost]
        //[Route("Login")]
        //public IHttpActionResult Login(LoginModel model)
        //{

        //    var loginResult = _customerRegistrationService.ValidateCustomer(model.Account, model.Password);

        //    if (loginResult == CustomerLoginResults.Successful)
        //    {
        //        var customer = _customerService.GetCustomerByAccount(model.Account);

        //        //migrate shopping cart
        //        //_shoppingCartService.MigrateShoppingCart(_workContext.CurrentCustomer, customer, true);

        //        //sign in new customer
        //        _authenticationService.SignIn(customer, model.RememberMe);

        //        //activity log
        //        //_customerActivityService.InsertActivity("PublicStore.Login", _localizationService.GetResource("ActivityLog.PublicStore.Login"), customer);

        //        return Ok();
        //    }
        //    else
        //    {
        //        switch (loginResult)
        //        {
        //            case CustomerLoginResults.NotActive:
        //                return BadRequest("用户未激活");
        //            default:
        //                return BadRequest("用户名不存在或密码不正确");
        //        }
        //    }
        //} 
        #endregion

        // GET: api/Customer/5
        [HttpGet]
        [Route("{id:int}")]
        [Authorize]
        public string Get(int id)
        {
            var user = System.Web.HttpContext.Current.User;
            return user.Identity.Name;
        }

        [HttpGet]
        [Route("hhhhh")]
        //[QMTech.Web.Framework.Security.QMAuthorization]
        public string Get()
        {


            return "value";
        }

        [HttpGet]
        [Route("Test")]
        //[QMTech.Web.Framework.Security.QMAuthorization2]
        public IHttpActionResult test()
        {
            var model = new TestModel()
            {
                Id = "123",
                Name = "234"
            };

            return Ok(model);
        }

        public class TestModel
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        // POST: api/Customer
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Customer/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Customer/5
        public void Delete(int id)
        {
        }
    }
}
