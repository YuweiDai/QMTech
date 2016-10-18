using QMTech.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMTech.Services.Messages
{
    public interface IWorkflowMessageService
    {
        #region Customer workflow

        /// <summary>
        /// Sends 'New customer' notification message to a store owner
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <returns>Queued email identifier</returns>
        int SendCustomerRegisteredNotificationMessage(Customer customer);

        /// <summary>
        /// Sends a welcome message to a customer
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <returns>Queued email identifier</returns>
        int SendCustomerWelcomeMessage(Customer customer);

        /// <summary>
        /// Sends an email validation message to a customer
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <returns>Queued email identifier</returns>
        int SendCustomerEmailValidationMessage(Customer customer);

        /// <summary>
        /// Sends password recovery message to a customer
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <returns>Queued email identifier</returns>
        int SendCustomerPasswordRecoveryMessage(Customer customer);

        #endregion
    }
}
