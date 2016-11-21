using QMTech.Core.Domain.Customers;
using QMTech.Services.Customers;
using System;
using System.Security.Claims;
using System.Web;

namespace QMTech.Services.Authentication
{
    public class TokenAuthenticationService : IAuthenticationService
    {
        private readonly HttpContextBase _httpContext;
        private readonly ICustomerService _customerService;
        private readonly CustomerSettings _customerSettings;

        private Customer _cachedCustomer;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="httpContext">HTTP context</param>
        /// <param name="customerService">Customer service</param>
        /// <param name="customerSettings">Customer settings</param>
        public TokenAuthenticationService(HttpContextBase httpContext,
            ICustomerService customerService, CustomerSettings customerSettings)
        {
            this._httpContext = httpContext;
            this._customerService = customerService;
            this._customerSettings = customerSettings;
        }

        public void SignOut()
        {
            System.Web.HttpContext.Current.User = null;
        }

        /// <summary>
        /// 获取已认证的用户
        /// </summary>
        /// <returns></returns>
        public virtual Customer GetAuthenticatedCustomer()
        {
            if (_cachedCustomer != null)
                return _cachedCustomer;

            if (_httpContext == null ||
                _httpContext.Request == null ||
                !_httpContext.Request.IsAuthenticated)
            {
                return null;
            }

            var principal = HttpContext.Current.User as ClaimsPrincipal;
            var guidClaim = principal.FindFirst(ct => ct.Type == ClaimTypes.NameIdentifier);
            if (guidClaim == null) return null;

            var customer = _customerService.GetCustomerByGuid(new Guid(guidClaim.Value));
            if (customer != null && customer.Active && !customer.Deleted && customer.IsRegistered())
                _cachedCustomer = customer;
            return _cachedCustomer;
        }
    }
}
