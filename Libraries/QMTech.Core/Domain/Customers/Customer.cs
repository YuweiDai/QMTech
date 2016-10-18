using Microsoft.AspNet.Identity;
using QMTech.Core.Domain.Common;
using System;
using System.Collections.Generic;

namespace QMTech.Core.Domain.Customers
{

    public class Customer : BaseEntity, IUser<int>
    {
        private ICollection<ExternalAuthenticationRecord> _externalAuthenticationRecords;
        private ICollection<CustomerRole> _customerRoles;
        //private ICollection<ShoppingCartItem> _shoppingCartItems;
        private ICollection<RewardPointsHistory> _rewardPointsHistory;
        //private ICollection<ReturnRequest> _returnRequests;
        private ICollection<Address> _addresses;


        public Customer()
        {
            this.CustomerGuid = Guid.NewGuid();          
        }

        public string Email { get; set; }

        public string UserName { get; set; }

        public bool Active { get; set; }

        public Guid CustomerGuid { get; set; }

        public string MobilePhone { get; set; }

        public string LastIpAddress { get; set; }

        public DateTime LastActivityDate { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public string AdminComment { get; set; }
        public string Password { get; set; }

        public int PasswordFormatId { get;  set; }

        public PasswordFormat PasswordFormat
        {
            get { return (PasswordFormat)PasswordFormatId; }
            set { this.PasswordFormatId = (int)value; }
        }

        public string PasswordSalt { get; set; }

        public bool IsSystemAccount { get; set; }

        public string SystemName { get; set; }

        /// <summary>
        /// Gets or sets customer generated content
        /// </summary>
        public virtual ICollection<ExternalAuthenticationRecord> ExternalAuthenticationRecords
        {
            get { return _externalAuthenticationRecords ?? (_externalAuthenticationRecords = new List<ExternalAuthenticationRecord>()); }
            protected set { _externalAuthenticationRecords = value; }
        }

        /// <summary>
        /// Gets or sets reward points history
        /// </summary>
        public virtual ICollection<RewardPointsHistory> RewardPointsHistory
        {
            get { return _rewardPointsHistory ?? (_rewardPointsHistory = new List<RewardPointsHistory>()); }
            protected set { _rewardPointsHistory = value; }
        }

        /// <summary>
        /// Gets or sets the customer roles
        /// </summary>
        public virtual ICollection<CustomerRole> CustomerRoles
        {
            get { return _customerRoles ?? (_customerRoles = new List<CustomerRole>()); }
            protected set { _customerRoles = value; }
        }

        public virtual ICollection<Address> Addresses
        {
            get { return _addresses ?? (_addresses = new List<Address>()); }
            protected set { _addresses = value; }
        }

    }
}
