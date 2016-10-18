using QMTech.Core.Domain.Customers;

namespace QMTech.Services.Authentication
{
    /// <summary>
    /// Authentication service interface
    /// </summary>
    public partial interface IAuthenticationService
    {
        //void SignIn(Customer customer, bool createPersistentCookie);

        void SignOut();

        Customer GetAuthenticatedCustomer();
    }
}
