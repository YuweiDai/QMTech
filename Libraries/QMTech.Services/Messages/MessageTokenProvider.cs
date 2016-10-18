using QMTech.Core;
using QMTech.Core.Domain.Catalog;
using QMTech.Core.Domain.Customers;
using QMTech.Core.Domain.Messages;
using QMTech.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMTech.Services.Messages
{
    public class MessageTokenProvider:IMessageTokenProvider
    {
        #region Fields

        //private readonly IDateTimeHelper _dateTimeHelper;
        //private readonly IPriceFormatter _priceFormatter;
        private readonly IWorkContext _workContext;
        //private readonly IDownloadService _downloadService;
        //private readonly IOrderService _orderService;
        //private readonly IPaymentService _paymentService;
        //private readonly IProductAttributeParser _productAttributeParser;
        //private readonly IAddressAttributeFormatter _addressAttributeFormatter;
        //private readonly IStoreService _storeService;
        //private readonly IStoreContext _storeContext;

        private readonly MessageTemplatesSettings _templatesSettings;
        private readonly CatalogSettings _catalogSettings;

        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Ctor

        public MessageTokenProvider(
            //IDateTimeHelper dateTimeHelper,
            //IPriceFormatter priceFormatter,
            IWorkContext workContext,
            //IDownloadService downloadService,
            //IOrderService orderService,
            //IPaymentService paymentService,
            //IStoreService storeService,
            //IStoreContext storeContext,
            //IProductAttributeParser productAttributeParser,
            //IAddressAttributeFormatter addressAttributeFormatter,
            MessageTemplatesSettings templatesSettings,
            CatalogSettings catalogSettings,
            IEventPublisher eventPublisher)
        {         
            //this._dateTimeHelper = dateTimeHelper;
            //this._priceFormatter = priceFormatter; 
            this._workContext = workContext;
            //this._downloadService = downloadService;
            //this._orderService = orderService;
            //this._paymentService = paymentService;
            //this._productAttributeParser = productAttributeParser;
            //this._addressAttributeFormatter = addressAttributeFormatter;
            //this._storeService = storeService;
            //this._storeContext = storeContext;

            this._templatesSettings = templatesSettings;
            this._catalogSettings = catalogSettings;
            this._eventPublisher = eventPublisher;
        }

        #endregion

        public virtual void AddStoreTokens(IList<Token> tokens,  EmailAccount emailAccount)
        {
            if (emailAccount == null)
                throw new ArgumentNullException("emailAccount");

            //tokens.Add(new Token("Store.Name", store.Name));
            //tokens.Add(new Token("Store.URL", store.Url, true));
            //tokens.Add(new Token("Store.Email", emailAccount.Email));
            //tokens.Add(new Token("Store.CompanyName", store.CompanyName));
            //tokens.Add(new Token("Store.CompanyAddress", store.CompanyAddress));
            //tokens.Add(new Token("Store.CompanyPhoneNumber", store.CompanyPhoneNumber));
            //tokens.Add(new Token("Store.CompanyVat", store.CompanyVat));

            //event notification
            //_eventPublisher.EntityTokensAdded(store, tokens);
        }

        public virtual void AddCustomerTokens(IList<Token> tokens, Customer customer)
        {
            tokens.Add(new Token("Customer.Email", customer.Email));
            tokens.Add(new Token("Customer.Username", customer.UserName));
            //tokens.Add(new Token("Customer.FullName", customer.GetFullName()));
            //tokens.Add(new Token("Customer.FirstName", customer.GetAttribute<string>(SystemCustomerAttributeNames.FirstName)));
            //tokens.Add(new Token("Customer.LastName", customer.GetAttribute<string>(SystemCustomerAttributeNames.LastName)));
            //tokens.Add(new Token("Customer.VatNumber", customer.GetAttribute<string>(SystemCustomerAttributeNames.VatNumber)));
            //tokens.Add(new Token("Customer.VatNumberStatus", ((VatNumberStatus)customer.GetAttribute<int>(SystemCustomerAttributeNames.VatNumberStatusId)).ToString()));



            //note: we do not use SEO friendly URLS because we can get errors caused by having .(dot) in the URL (from the email address)
            //TODO add a method for getting URL (use routing because it handles all SEO friendly URLs)
            //string passwordRecoveryUrl = string.Format("{0}passwordrecovery/confirm?token={1}&email={2}", GetStoreUrl(), customer.GetAttribute<string>(SystemCustomerAttributeNames.PasswordRecoveryToken), HttpUtility.UrlEncode(customer.Email));
            //string accountActivationUrl = string.Format("{0}customer/activation?token={1}&email={2}", GetStoreUrl(), customer.GetAttribute<string>(SystemCustomerAttributeNames.AccountActivationToken), HttpUtility.UrlEncode(customer.Email));
            //var wishlistUrl = string.Format("{0}wishlist/{1}", GetStoreUrl(), customer.CustomerGuid);
            //tokens.Add(new Token("Customer.PasswordRecoveryURL", passwordRecoveryUrl, true));
            //tokens.Add(new Token("Customer.AccountActivationURL", accountActivationUrl, true));
            //tokens.Add(new Token("Wishlist.URLForCustomer", wishlistUrl, true));

            //event notification
            _eventPublisher.EntityTokensAdded(customer, tokens);
        }


    }
}
