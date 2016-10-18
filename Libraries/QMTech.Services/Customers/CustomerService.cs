using System;
using System.Collections.Generic;
using System.Linq;
using QMTech.Core.Data;
using QMTech.Data;
using QMTech.Core.Caching;
using QMTech.Services.Events;
using QMTech.Core.Domain.Common;
using QMTech.Core;
using QMTech.Core.Domain.Customers;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using QMTech.Services.Security;

namespace QMTech.Services.Customers
{
    /// <summary>
    /// 用户服务
    /// </summary>
    public class CustomerService : ICustomerService
    {

        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// </remarks>
        private const string CUSTOMERROLES_ALL_KEY = "QM.customerrole.all-{0}";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : system name
        /// </remarks>
        private const string CUSTOMERROLES_BY_SYSTEMNAME_KEY = "QM.customerrole.systemname-{0}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string CUSTOMERROLES_PATTERN_KEY = "QM.customerrole.";

        #endregion

        #region Fields

        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<CustomerRole> _customerRoleRepository;
        private readonly IRepository<GenericAttribute> _gaRepository;
        //private readonly IRepository<Order> _orderRepository;
        //private readonly IRepository<ForumPost> _forumPostRepository;
        //private readonly IRepository<ForumTopic> _forumTopicRepository;
        //private readonly IRepository<BlogComment> _blogCommentRepository;
        //private readonly IRepository<NewsComment> _newsCommentRepository;
        //private readonly IRepository<PollVotingRecord> _pollVotingRecordRepository;
        //private readonly IRepository<ProductReview> _productReviewRepository;
        //private readonly IRepository<ProductReviewHelpfulness> _productReviewHelpfulnessRepository;
        //private readonly IGenericAttributeService _genericAttributeService;
        private readonly IDataProvider _dataProvider;
        private readonly IDbContext _dbContext;
        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly CustomerSettings _customerSettings;
        private readonly CommonSettings _commonSettings;
        
        #endregion

        #region 构造函数

        public CustomerService(ICacheManager cacheManager,
            IRepository<Customer> customerRepository,
            IRepository<CustomerRole> customerRoleRepository,
            IRepository<GenericAttribute> gaRepository,
            //IRepository<Order> orderRepository,
            //IRepository<ForumPost> forumPostRepository,
            //IRepository<ForumTopic> forumTopicRepository,
            //IRepository<BlogComment> blogCommentRepository,
            //IRepository<NewsComment> newsCommentRepository,
            //IRepository<PollVotingRecord> pollVotingRecordRepository,
            //IRepository<ProductReview> productReviewRepository,
            //IRepository<ProductReviewHelpfulness> productReviewHelpfulnessRepository,
            //IGenericAttributeService genericAttributeService,
            IDataProvider dataProvider,
            IDbContext dbContext,
            IEventPublisher eventPublisher,
        CustomerSettings customerSettings,
            CommonSettings commonSettings)
        {
            this._cacheManager = cacheManager;
            this._customerRepository = customerRepository;
            this._customerRoleRepository = customerRoleRepository;
            this._gaRepository = gaRepository;
            //this._orderRepository = orderRepository;
            //this._forumPostRepository = forumPostRepository;
            //this._forumTopicRepository = forumTopicRepository;
            //this._blogCommentRepository = blogCommentRepository;
            //this._newsCommentRepository = newsCommentRepository;
            //this._pollVotingRecordRepository = pollVotingRecordRepository;
            //this._productReviewRepository = productReviewRepository;
            //this._productReviewHelpfulnessRepository = productReviewHelpfulnessRepository;
            //this._genericAttributeService = genericAttributeService;
            this._dataProvider = dataProvider;
            this._dbContext = dbContext;
            this._eventPublisher = eventPublisher;

            this._customerSettings = customerSettings;
            this._commonSettings = commonSettings;
        }

        #endregion

        #region ICustomer 实现
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="customer"></param>
        public void DeleteCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            if (customer.IsSystemAccount)
                throw new QMTechException(string.Format("System customer account ({0}) could not be deleted", customer.SystemName));

            customer.Deleted = true;

            if (_customerSettings.SuffixDeletedCustomers)
            {
                if (!String.IsNullOrEmpty(customer.Email))
                    customer.Email += "-DELETED";
                if (!String.IsNullOrEmpty(customer.UserName))
                    customer.UserName += "-DELETED";
                if (!String.IsNullOrEmpty(customer.MobilePhone))
                    customer.UserName += "-DELETED";
            }

            UpdateCustomer(customer);
        }

        /// <summary>
        /// 通过id获取用户
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public Customer GetCustomerById(int customerId)
        {
            if (customerId == 0)
                return null;

            return _customerRepository.GetById(customerId);
        }

        public virtual IList<Customer> GetCustomersByIds(int[] customerIds)
        {
            if (customerIds == null || customerIds.Length == 0)
                return new List<Customer>();

            var query = from c in _customerRepository.Table
                        where customerIds.Contains(c.Id)
                        select c;
            var customers = query.ToList();
            //sort by passed identifiers
            var sortedCustomers = new List<Customer>();
            foreach (int id in customerIds)
            {
                var customer = customers.Find(x => x.Id == id);
                if (customer != null)
                    sortedCustomers.Add(customer);
            }
            return sortedCustomers;
        }

        /// <summary>
        /// 通过手机号码获取顾客
        /// </summary>
        /// <param name="mobileNumber"></param>
        /// <returns></returns>
        public Customer GetCustomerByMobileNumber(string mobileNumber)
        {
            if (string.IsNullOrEmpty(mobileNumber))
                return null;

            var query = from c in _customerRepository.Table
                        orderby c.Id
                        where c.MobilePhone == mobileNumber
                        select c;

            var customer = query.FirstOrDefault();
            return customer;
        }

        public Customer GetCustomerByGuid(Guid customerGuid)
        {
            if (customerGuid == Guid.Empty)
                return null;

            var query = from c in _customerRepository.Table
                        orderby c.Id
                        where c.CustomerGuid == customerGuid
                        select c;

            var customer = query.FirstOrDefault();
            return customer;
        }

        /// <summary>
        /// 通过邮件地址获取顾客
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Customer GetCustomerByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return null;

            var query = from c in _customerRepository.Table
                        orderby c.Id
                        where c.Email == email
                        select c;

            var customer = query.FirstOrDefault();
            return customer;
        }

        public Customer GetCustomerBySystemName(string systemName)
        {
            if (string.IsNullOrWhiteSpace(systemName))
                return null;

            var query = from c in _customerRepository.Table
                        orderby c.Id
                        where c.SystemName == systemName
                        select c;
            var customer = query.FirstOrDefault();
            return customer;
        }

        /// <summary>
        /// Get customer by username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>Customer</returns>
        public virtual Customer GetCustomerByUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
                return null;

            var query = from c in _customerRepository.Table
                        orderby c.Id
                        where c.UserName == username
                        select c;

            var customer = query.FirstOrDefault();
            return customer;
        }

        public Customer GetCustomerByAccount(string account)
        {
            if (string.IsNullOrWhiteSpace(account))
                throw new ArgumentNullException("账户");

            Customer customer = null;
            if (CommonHelper.IsValidMobileNumber(account))
                customer = GetCustomerByMobileNumber(account);
            else if (CommonHelper.IsValidEmail(account))
                customer = GetCustomerByEmail(account);
            else
                customer = GetCustomerByUsername(account);

            return customer;
        }

        /// <summary>
        /// 新增客户
        /// </summary>
        /// <param name="customer"></param>
        public void InsertCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            _customerRepository.Insert(customer);

            //event notification
            _eventPublisher.EntityInserted(customer);
        }

        public Customer InsertGuestCustomer()
        {
            var customer = new Customer
            {
                CustomerGuid = Guid.NewGuid(),
                Active = true,
                CreatedOn = DateTime.Now,
                LastActivityDate = DateTime.Now,
            };

            //add to 'Guests' role
            var guestRole = GetCustomerRoleBySystemName(SystemCustomerRoleNames.Guests);
            if (guestRole == null)
                throw new QMTechException("角色 '访客' 不能被加载");
            customer.CustomerRoles.Add(guestRole);

            _customerRepository.Insert(customer);

            return customer;
        }

        public void UpdateCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            _customerRepository.Update(customer);

            //event notification
            _eventPublisher.EntityUpdated(customer);
        } 
        #endregion

        #region Customer roles

        /// <summary>
        /// Delete a customer role
        /// </summary>
        /// <param name="customerRole">Customer role</param>
        public virtual void DeleteCustomerRole(CustomerRole customerRole)
        {
            if (customerRole == null)
                throw new ArgumentNullException("customerRole");

            if (customerRole.IsSystemRole)
                throw new QMTechException("System role could not be deleted");

            _customerRoleRepository.Delete(customerRole);

            _cacheManager.RemoveByPattern(CUSTOMERROLES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(customerRole);
        }

        /// <summary>
        /// Gets a customer role
        /// </summary>
        /// <param name="customerRoleId">Customer role identifier</param>
        /// <returns>Customer role</returns>
        public virtual CustomerRole GetCustomerRoleById(int customerRoleId)
        {
            if (customerRoleId == 0)
                return null;

            return _customerRoleRepository.GetById(customerRoleId);
        }

        /// <summary>
        /// Gets a customer role
        /// </summary>
        /// <param name="systemName">Customer role system name</param>
        /// <returns>Customer role</returns>
        public virtual CustomerRole GetCustomerRoleBySystemName(string systemName)
        {
            if (String.IsNullOrWhiteSpace(systemName))
                return null;

            string key = string.Format(CUSTOMERROLES_BY_SYSTEMNAME_KEY, systemName);
            return _cacheManager.Get(key, () =>
            {
                var query = from cr in _customerRoleRepository.Table
                            orderby cr.Id
                            where cr.SystemName == systemName
                            select cr;
                var customerRole = query.FirstOrDefault();
                return customerRole;
            });
        }

        /// <summary>
        /// Gets all customer roles
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Customer roles</returns>
        public virtual IList<CustomerRole> GetAllCustomerRoles(bool showHidden = false)
        {
            string key = string.Format(CUSTOMERROLES_ALL_KEY, showHidden);
            return _cacheManager.Get(key, () =>
        {
                var query = from cr in _customerRoleRepository.Table
                            orderby cr.Name
                            where (showHidden || cr.Active)
                            select cr;
                var customerRoles = query.ToList();
                return customerRoles;
            });
        }

        /// <summary>
        /// Inserts a customer role
        /// </summary>
        /// <param name="customerRole">Customer role</param>
        public virtual void InsertCustomerRole(CustomerRole customerRole)
        {
            if (customerRole == null)
                throw new ArgumentNullException("customerRole");

            _customerRoleRepository.Insert(customerRole);

            _cacheManager.RemoveByPattern(CUSTOMERROLES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(customerRole);
        }

        /// <summary>
        /// Updates the customer role
        /// </summary>
        /// <param name="customerRole">Customer role</param>
        public virtual void UpdateCustomerRole(CustomerRole customerRole)
        {
            if (customerRole == null)
                throw new ArgumentNullException("customerRole");

            _customerRoleRepository.Update(customerRole);

            _cacheManager.RemoveByPattern(CUSTOMERROLES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(customerRole);
        }

        #endregion
    }
}
