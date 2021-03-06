﻿using Microsoft.AspNet.Identity;
using QMTech.Core.Domain.Security;
using System.Collections.Generic;

namespace QMTech.Core.Domain.Customers
{
    /// <summary>
    /// Represents a customer role
    /// </summary>
    public partial class CustomerRole : BaseEntity, IRole<int>
    {
        //private ICollection<PermissionRecord> _permissionRecords;

        /// <summary>
        /// Gets or sets the customer role name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer role is active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer role is system
        /// </summary>
        public bool IsSystemRole { get; set; }

        /// <summary>
        /// Gets or sets the customer role system name
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// Gets or sets a product identifier that is required by this customer role. 
        /// A customer is added to this customer role once a specified product is purchased.
        /// </summary>
        public int PurchasedWithProductId { get; set; }

        /// <summary>
        /// Gets or sets the permission records 暂时粗粒度的基于角色控制
        /// </summary>
        //public virtual ICollection<PermissionRecord> PermissionRecords
        //{
        //    get { return _permissionRecords ?? (_permissionRecords = new List<PermissionRecord>()); }
        //    protected set { _permissionRecords = value; }
        //}
    }
}
