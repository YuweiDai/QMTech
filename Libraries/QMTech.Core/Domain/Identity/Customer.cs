using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMTech.Core.Domain.Identity
{
    public class Customer : BaseEntity
    {
        private ICollection<CustomerRole> _customerRoles;

        public Customer()
        {
            this.CustomerGuid = Guid.NewGuid();          
        }

        public bool Active { get; set; }

        public Guid CustomerGuid { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }


        public string LastIpAddress { get; set; }

        public DateTime LastActivityDate { get; set; }

        public DateTime? LastLoginDate { get; set; }

        /// <summary>
        /// Gets or sets the customer roles
        /// </summary>
        public virtual ICollection<CustomerRole> CustomerRoles
        {
            get { return _customerRoles ?? (_customerRoles = new List<CustomerRole>()); }
            protected set { _customerRoles = value; }
        }

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

    }
}
