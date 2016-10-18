using QMTech.Core;
using QMTech.Core.Domain.Catalog;
using QMTech.Core.Domain.Customers;
using QMTech.Core.Domain.Messages;
using QMTech.Services.Events;
using System.Collections.Generic;

namespace QMTech.Services.Messages
{
    public partial interface IMessageTokenProvider
    {


        void AddStoreTokens(IList<Token> tokens, EmailAccount emailAccount);

        void AddCustomerTokens(IList<Token> tokens, Customer customer);
    }
}