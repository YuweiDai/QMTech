using QMTech.Core;
using QMTech.Core.Domain.Customers;
using QMTech.Services.Customers;
using QMTech.Services.Logging;
using QMTech.Services.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMTech.Services.Authentication.External
{
    /// <summary>
    /// External authorizer
    /// </summary>
    public partial class ExternalAuthorizer : IExternalAuthorizer
    {
        #region Fields

        private readonly IAuthenticationService _authenticationService;
        private readonly IOpenAuthenticationService _openAuthenticationService;
        //private readonly IGenericAttributeService _genericAttributeService;
        private readonly ICustomerRegistrationService _customerRegistrationService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IWorkContext _workContext;
        private readonly CustomerSettings _customerSettings;
        private readonly ExternalAuthenticationSettings _externalAuthenticationSettings;
        //private readonly IShoppingCartService _shoppingCartService;
        private readonly IWorkflowMessageService _workflowMessageService;
        #endregion

        #region Ctor

        public ExternalAuthorizer(IAuthenticationService authenticationService,
            IOpenAuthenticationService openAuthenticationService,
            //IGenericAttributeService genericAttributeService,
            ICustomerRegistrationService customerRegistrationService,
            ICustomerActivityService customerActivityService, 
            IWorkContext workContext, CustomerSettings customerSettings,
            ExternalAuthenticationSettings externalAuthenticationSettings,
            //IShoppingCartService shoppingCartService,
            IWorkflowMessageService workflowMessageService)
        {
            this._authenticationService = authenticationService;
            this._openAuthenticationService = openAuthenticationService;
            //this._genericAttributeService = genericAttributeService;
            this._customerRegistrationService = customerRegistrationService;
            this._customerActivityService = customerActivityService;          
            this._workContext = workContext;
            this._customerSettings = customerSettings;
            this._externalAuthenticationSettings = externalAuthenticationSettings;
            //this._shoppingCartService = shoppingCartService;
            this._workflowMessageService = workflowMessageService;
        }

        #endregion

        #region Utilities

        private bool RegistrationIsEnabled()
        {
            return _customerSettings.UserRegistrationType != UserRegistrationType.Disabled && !_externalAuthenticationSettings.AutoRegisterEnabled;
        }

        private bool AutoRegistrationIsEnabled()
        {
            return _customerSettings.UserRegistrationType != UserRegistrationType.Disabled && _externalAuthenticationSettings.AutoRegisterEnabled;
        }

        private bool AccountDoesNotExistAndUserIsNotLoggedOn(Customer userFound, Customer userLoggedIn)
        {
            return userFound == null && userLoggedIn == null;
        }

        private bool AccountIsAssignedToLoggedOnAccount(Customer userFound, Customer userLoggedIn)
        {
            return userFound.Id.Equals(userLoggedIn.Id);
        }

        private bool AccountAlreadyExists(Customer userFound, Customer userLoggedIn)
        {
            return userFound != null && userLoggedIn != null;
        }

        #endregion

        #region Methods

        public virtual AuthorizationResult Authorize(OpenAuthenticationParameters parameters)
        {
            var userFound = _openAuthenticationService.GetUser(parameters);

            var userLoggedIn = _workContext.CurrentCustomer.IsRegistered() ? _workContext.CurrentCustomer : null;

            if (AccountAlreadyExists(userFound, userLoggedIn))
            {
                if (AccountIsAssignedToLoggedOnAccount(userFound, userLoggedIn))
                {
                    // The person is trying to log in as himself.. bit weird
                    return new AuthorizationResult(OpenAuthenticationStatus.Authenticated);
                }

                var result = new AuthorizationResult(OpenAuthenticationStatus.Error);
                result.AddError("Account is already assigned");
                return result;
            }
            if (AccountDoesNotExistAndUserIsNotLoggedOn(userFound, userLoggedIn))
            {
                ExternalAuthorizerHelper.StoreParametersForRoundTrip(parameters);

                if (AutoRegistrationIsEnabled())
                {
                    #region Register user

                    var currentCustomer = _workContext.CurrentCustomer;
                    var details = new RegistrationDetails(parameters);
                    var randomPassword = CommonHelper.GenerateRandomDigitCode(20);


                    bool isApproved =
                        //standard registration
                        (_customerSettings.UserRegistrationType == UserRegistrationType.Standard) ||
                        //skip email validation?
                        (_customerSettings.UserRegistrationType == UserRegistrationType.EmailValidation &&
                         !_externalAuthenticationSettings.RequireEmailValidation);

                    var registrationRequest = new CustomerRegistrationRequest(currentCustomer,details.MobilePhone, details.EmailAddress,
                        _customerSettings.UsernamesEnabled ? details.UserName : details.EmailAddress, randomPassword, PasswordFormat.Clear, isApproved);
                    var registrationResult = _customerRegistrationService.RegisterCustomer(registrationRequest);
                    if (registrationResult.Success)
                    {
                        //store other parameters (form fields)
                        if (!String.IsNullOrEmpty(details.FirstName))
                            _genericAttributeService.SaveAttribute(currentCustomer, SystemCustomerAttributeNames.FirstName, details.FirstName);
                        if (!String.IsNullOrEmpty(details.LastName))
                            _genericAttributeService.SaveAttribute(currentCustomer, SystemCustomerAttributeNames.LastName, details.LastName);


                        userFound = currentCustomer;
                        _openAuthenticationService.AssociateExternalAccountWithUser(currentCustomer, parameters);
                        ExternalAuthorizerHelper.RemoveParameters();

                        //code below is copied from CustomerController.Register method

                        //authenticate
                        if (isApproved)
                            _authenticationService.SignIn(userFound ?? userLoggedIn, false);

                        //notifications
                        if (_customerSettings.NotifyNewCustomerRegistration)
                            _workflowMessageService.SendCustomerRegisteredNotificationMessage(currentCustomer);

                        if (isApproved)
                        {
                            //standard registration
                            //or
                            //skip email validation

                            //send customer welcome message
                            _workflowMessageService.SendCustomerWelcomeMessage(currentCustomer);

                            //result
                            return new AuthorizationResult(OpenAuthenticationStatus.AutoRegisteredStandard);
                        }
                        else if (_customerSettings.UserRegistrationType == UserRegistrationType.EmailValidation)
                        {
                            //email validation message
                            _genericAttributeService.SaveAttribute(currentCustomer, SystemCustomerAttributeNames.AccountActivationToken, Guid.NewGuid().ToString());
                            _workflowMessageService.SendCustomerEmailValidationMessage(currentCustomer, _workContext.WorkingLanguage.Id);

                            //result
                            return new AuthorizationResult(OpenAuthenticationStatus.AutoRegisteredEmailValidation);
                        }
                        else if (_customerSettings.UserRegistrationType == UserRegistrationType.AdminApproval)
                        {
                            //result
                            return new AuthorizationResult(OpenAuthenticationStatus.AutoRegisteredAdminApproval);
                        }
                    }
                    else
                    {
                        ExternalAuthorizerHelper.RemoveParameters();

                        var result = new AuthorizationResult(OpenAuthenticationStatus.Error);
                        foreach (var error in registrationResult.Errors)
                            result.AddError(string.Format(error));
                        return result;
                    }

                    #endregion
                }
                else if (RegistrationIsEnabled())
                {
                    return new AuthorizationResult(OpenAuthenticationStatus.AssociateOnLogon);
                }
                else
                {
                    ExternalAuthorizerHelper.RemoveParameters();

                    var result = new AuthorizationResult(OpenAuthenticationStatus.Error);
                    result.AddError("Registration is disabled");
                    return result;
                }
            }
            if (userFound == null)
            {
                _openAuthenticationService.AssociateExternalAccountWithUser(userLoggedIn, parameters);
            }

            //migrate shopping cart
            //_shoppingCartService.MigrateShoppingCart(_workContext.CurrentCustomer, userFound ?? userLoggedIn, true);
            //authenticate
            _authenticationService.SignIn(userFound ?? userLoggedIn, false);
            //activity log
            _customerActivityService.InsertActivity("PublicStore.Login", "登录",
                userFound ?? userLoggedIn);

            return new AuthorizationResult(OpenAuthenticationStatus.Authenticated);
        }

        #endregion
    }
}
