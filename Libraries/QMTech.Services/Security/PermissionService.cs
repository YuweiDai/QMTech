using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QMTech.Core.Domain.Customers;
using QMTech.Core.Domain.Security;
using QMTech.Core.Data;
using QMTech.Services.Customers;
using QMTech.Core;
using QMTech.Core.Caching;

namespace QMTech.Services.Security
{
    /// <summary>
    /// 认证权限
    /// </summary>
    public class PermissionService : IPermissionService
    {

        #region 常量
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : customer role ID
        /// {1} : permission system name
        /// </remarks>
        private const string PERMISSIONS_ALLOWED_KEY = "QM.permission.allowed-{0}-{1}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string PERMISSIONS_PATTERN_KEY = "QM.permission.";
        #endregion

        #region 字段

        private readonly IRepository<PermissionRecord> _permissionRecordRepository;
        private readonly ICustomerService _customerService;
        private readonly IWorkContext _workContext;
        private readonly ICacheManager _cacheManager;

        #endregion

        public PermissionService(IRepository<PermissionRecord> permissionRecordRepository,
                        ICustomerService customerService,
                        IWorkContext workContext,
                        ICacheManager cacheManager)
        {
            this._permissionRecordRepository = permissionRecordRepository;
            this._customerService = customerService;
            this._workContext = workContext;
            this._cacheManager = cacheManager;
        }

        #region 常用方法

        protected virtual bool Authorize(string permissionRecordSystemName, CustomerRole customerRole)
        {
            if (string.IsNullOrEmpty(permissionRecordSystemName))
                return false;

            string key = string.Format(PERMISSIONS_ALLOWED_KEY, customerRole.Id, permissionRecordSystemName);
            return _cacheManager.Get(key, () =>
            {
                foreach (var permission1 in customerRole.PermissionRecords)
                    if (permission1.SystemName.Equals(permissionRecordSystemName, StringComparison.InvariantCultureIgnoreCase))
                        return true;

                return false;
            });
        } 
        #endregion

        public virtual bool Authorize(string permissionRecordSystemName)
        {
            return Authorize(permissionRecordSystemName, _workContext.CurrentUser);
        }

        public virtual bool Authorize(PermissionRecord permission)
        {
            return Authorize(permission, _workContext.CurrentUser);
        }

        public virtual bool Authorize(string permissionRecordSystemName, Customer customer)
        {
            if (string.IsNullOrEmpty(permissionRecordSystemName))
                return false;
            var customerRoles = customer.CustomerRoles.Where(cr => cr.Active);
            foreach (var role in customerRoles)
                if (Authorize(permissionRecordSystemName, role))
                    return true;

            //未找到权限
            return false;
        }

        public bool Authorize(PermissionRecord permission, Customer customer)
        {
            if (permission == null)
                return false;

            if (customer == null)
                return false;

            return Authorize(permission.SystemName, customer);
        }

        public void DeletePermissionRecord(PermissionRecord permission)
        {
            throw new NotImplementedException();
        }

        public IList<PermissionRecord> GetAllPermissionRecords()
        {
            throw new NotImplementedException();
        }

        public PermissionRecord GetPermissionRecordById(int permissionId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 通过权限系统名称获取权限
        /// </summary>
        /// <param name="systemName"></param>
        /// <returns></returns>
        public virtual PermissionRecord GetPermissionRecordBySystemName(string systemName)
        {
            if (string.IsNullOrWhiteSpace(systemName)) return null;

            var query = from pr in _permissionRecordRepository.Table
                        where pr.SystemName == systemName
                        orderby pr.Id
                        select pr;

            var permissionRecord = query.FirstOrDefault();
            return permissionRecord;
        }

        /// <summary>
        /// 新增权限
        /// </summary>
        /// <param name="permission"></param>
        public void InsertPermissionRecord(PermissionRecord permission)
        {
            if (permission == null)
                throw new ArgumentNullException("permission");

            _permissionRecordRepository.Insert(permission);
            _cacheManager.RemoveByPattern(PERMISSIONS_PATTERN_KEY);
        }

        /// <summary>
        /// 权限初始化
        /// </summary>
        /// <param name="permissionProvider"></param>
        public void InstallPermissions(IPermissionProvider permissionProvider)
        {
            //添加新的权限
            var permissions = permissionProvider.GetPermissions();
            foreach(var permission in permissions)
            {
                //是否有相同名称的权限
                var permission1 = GetPermissionRecordBySystemName(permission.SystemName);
                if(permission1==null)
                {
                    permission1 = new PermissionRecord
                    {
                        Name = permission.Name,
                        SystemName = permission.SystemName,
                        Category = permission.Category
                    };

                    //获取默认的角色-权限对象
                    var defaultPermissions = permissionProvider.GetDefaultPermissions();
                    //遍历默认的权限对象
                    foreach(var defaultPermission in defaultPermissions)
                    {
                        //角色是否存在，反之创建角色
                        var customerRole = _customerService.GetCustomerRoleBySystemName(defaultPermission.CustomerRoleSystemName);
                        if(customerRole==null)
                        {
                            //创建角色
                            customerRole = new CustomerRole
                            {
                                Name = defaultPermission.CustomerRoleSystemName,
                                Active = true,
                                SystemName = defaultPermission.CustomerRoleSystemName
                            };

                            _customerService.InsertCustomerRole(customerRole);
                        }

                        //判断 默认角色权限集合中是否有权限
                        var defaultMappingProvided = (from p in defaultPermission.PermissionRecords
                                                      where p.SystemName == permission1.SystemName
                                                      select p
                                                    ).Any();

                        //是否已存在当前映射
                        var mappingExsits = (from p in customerRole.PermissionRecords
                                             where p.SystemName == permission1.SystemName
                                             select p).Any();

                        if(defaultMappingProvided && !mappingExsits)
                        {
                            permission1.CustomerRoles.Add(customerRole);
                        }
                            
                    }

                    //保存新的权限
                    InsertPermissionRecord(permission1);
                }
            }

        }

        public void UninstallPermissions(IPermissionProvider permissionProvider)
        {
            throw new NotImplementedException();
        }

        public void UpdatePermissionRecord(PermissionRecord permission)
        {
            throw new NotImplementedException();
        }
    }
}
