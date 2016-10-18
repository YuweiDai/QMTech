using System;
using System.Collections.Generic;
using QMTech.Core.Domain.Customers;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace QMTech.Services.Customers
{
    public interface ICustomerService //: IUserStore<Customer, int>, IUserLockoutStore<Customer, int>,IUserPasswordStore<Customer, int>, IUserTwoFactorStore<Customer, int>
    {
        /// <summary>
        /// Delete a customer
        /// </summary>
        /// <param name="customer">Customer</param>
        void DeleteCustomer(Customer customer);

        /// <summary>
        /// Gets a customer
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        /// <returns>A customer</returns>
        Customer GetCustomerById(int customerId);

        /// <summary>
        /// Get customers by identifiers
        /// </summary>
        /// <param name="customerIds">Customer identifiers</param>
        /// <returns>Customers</returns>
        IList<Customer> GetCustomersByIds(int[] customerIds);

        /// <summary>
        /// Gets a customer by GUID
        /// </summary>
        /// <param name="customerGuid">Customer GUID</param>
        /// <returns>A customer</returns>
        Customer GetCustomerByGuid(Guid customerGuid);

        /// <summary>
        /// Get customer by email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>Customer</returns>
        Customer GetCustomerByEmail(string email);

        /// <summary>
        /// Get customer by mobile number
        /// </summary>
        /// <param name="mobileNumber"></param>
        /// <returns></returns>
        Customer GetCustomerByMobileNumber(string mobileNumber);

        /// <summary>
        /// Get customer by system role
        /// </summary>
        /// <param name="systemName">System name</param>
        /// <returns>Customer</returns>
        Customer GetCustomerBySystemName(string systemName);

        /// <summary>
        /// Get customer by username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>Customer</returns>
        Customer GetCustomerByUsername(string username);

        Customer GetCustomerByAccount(string username);

        void UpdateCustomer(Customer customer);

        /// <summary>
        /// Insert a guest customer
        /// </summary>
        /// <returns>Customer</returns>
        Customer InsertGuestCustomer();

        /// <summary>
        /// Insert a customer
        /// </summary>
        /// <param name="customer">Customer</param>
        void InsertCustomer(Customer customer);

        #region Customer roles

        /// <summary>
        /// Delete a customer role
        /// </summary>
        /// <param name="customerRole">Customer role</param>
        void DeleteCustomerRole(CustomerRole customerRole);

        /// <summary>
        /// Gets a customer role
        /// </summary>
        /// <param name="customerRoleId">Customer role identifier</param>
        /// <returns>Customer role</returns>
        CustomerRole GetCustomerRoleById(int customerRoleId);

        /// <summary>
        /// Gets a customer role
        /// </summary>
        /// <param name="systemName">Customer role system name</param>
        /// <returns>Customer role</returns>
        CustomerRole GetCustomerRoleBySystemName(string systemName);

        /// <summary>
        /// Gets all customer roles
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Customer roles</returns>
        IList<CustomerRole> GetAllCustomerRoles(bool showHidden = false);

        /// <summary>
        /// Inserts a customer role
        /// </summary>
        /// <param name="customerRole">Customer role</param>
        void InsertCustomerRole(CustomerRole customerRole);

        /// <summary>
        /// Updates the customer role
        /// </summary>
        /// <param name="customerRole">Customer role</param>
        void UpdateCustomerRole(CustomerRole customerRole);

        #endregion
    }
}
