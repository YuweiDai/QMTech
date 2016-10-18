using QMTech.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMTech.Services.Customers
{
    public static class CustomerExtensions
    {
        /// <summary>
        /// Gets a value indicating whether customer is in a certain customer role
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="customerRoleSystemName">Customer role system name</param>
        /// <param name="onlyActiveCustomerRoles">A value indicating whether we should look only in active customer roles</param>
        /// <returns>Result</returns>
        public static bool IsInCustomerRole(this Customer customer,
            string customerRoleSystemName, bool onlyActiveCustomerRoles = true)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            if (String.IsNullOrEmpty(customerRoleSystemName))
                throw new ArgumentNullException("customerRoleSystemName");

            var result = customer.CustomerRoles
                .FirstOrDefault(cr => (!onlyActiveCustomerRoles || cr.Active) && (cr.SystemName == customerRoleSystemName)) != null;
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether customer a search engine
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <returns>Result</returns>
        public static bool IsSearchEngineAccount(this Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            if (!customer.IsSystemAccount || String.IsNullOrEmpty(customer.SystemName))
                return false;

            var result = customer.SystemName.Equals(SystemCustomerNames.SearchEngine, StringComparison.InvariantCultureIgnoreCase);
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether the customer is a built-in record for background tasks
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <returns>Result</returns>
        public static bool IsBackgroundTaskAccount(this Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            if (!customer.IsSystemAccount || String.IsNullOrEmpty(customer.SystemName))
                return false;

            var result = customer.SystemName.Equals(SystemCustomerNames.BackgroundTask, StringComparison.InvariantCultureIgnoreCase);
            return result;
        }

        /// <summary>
        /// 客户是否为管理员
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="onlyActiveCustomerRoles"></param>
        /// <returns></returns>
        public static bool IsAdmin(this Customer customer, bool onlyActiveCustomerRoles = true)
        {
            return IsInCustomerRole(customer, SystemCustomerRoleNames.Administrators, onlyActiveCustomerRoles);
        }

        /// <summary>
        /// Gets a value indicating whether customer is registered
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="onlyActiveCustomerRoles">A value indicating whether we should look only in active customer roles</param>
        /// <returns>Result</returns>
        public static bool IsRegistered(this Customer customer, bool onlyActiveCustomerRoles = true)
        {
            return IsInCustomerRole(customer, SystemCustomerRoleNames.Registered, onlyActiveCustomerRoles);
        }
    }
}
