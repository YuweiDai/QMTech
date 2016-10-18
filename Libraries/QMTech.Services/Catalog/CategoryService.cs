using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QMTech.Core;
using QMTech.Core.Domain.Catalog;
using QMTech.Core.Data;
using QMTech.Services.Events;
using QMTech.Core.Caching;

namespace QMTech.Services.Catalog
{
    public class CategoryService : ICategoryService
    {
        #region Constants
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : category ID
        /// </remarks>
        private const string CATEGORIES_BY_ID_KEY = "QMTech.category.id-{0}";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : parent category ID
        /// {1} : show hidden records?
        /// {2} : current customer ID
        /// {3} : store ID
        /// </remarks>
        private const string CATEGORIES_BY_PARENT_CATEGORY_ID_KEY = "QMTech.category.byparent-{0}-{1}-{2}";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// {1} : category ID
        /// {2} : page index
        /// {3} : page size
        /// {4} : current customer ID
        /// {5} : store ID
        /// </remarks>
        private const string PRODUCTCATEGORIES_ALLBYCATEGORYID_KEY = "QMTech.productcategory.allbycategoryid-{0}-{1}-{2}-{3}-{4}-{5}";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// {1} : product ID
        /// {2} : current customer ID
        /// {3} : store ID
        /// </remarks>
        private const string PRODUCTCATEGORIES_ALLBYPRODUCTID_KEY = "QMTech.productcategory.allbyproductid-{0}-{1}-{2}-{3}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string CATEGORIES_PATTERN_KEY = "QMTech.category.";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string PRODUCTCATEGORIES_PATTERN_KEY = "QMTech.productcategory.";

        #endregion

        #region Fields

        private readonly IRepository<StoreCategory> _storeCategoryRepository;
        private readonly IRepository<ProductCategory> _productCategoryRepository;
        private readonly IRepository<Product> _productRepository;
        //private readonly IRepository<AclRecord> _aclRepository;
        //private readonly IRepository<StoreMapping> _storeMappingRepository;
        private readonly IWorkContext _workContext;
        //private readonly IStoreContext _storeContext;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;
        //private readonly IStoreMappingService _storeMappingService;
        //private readonly IAclService _aclService;
        private readonly CatalogSettings _catalogSettings;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="storeCategoryRepository">Category repository</param>
        /// <param name="productCategoryRepository">ProductCategory repository</param>
        /// <param name="productRepository">Product repository</param>
        /// <param name="aclRepository">ACL record repository</param>
        /// <param name="storeMappingRepository">Store mapping repository</param>
        /// <param name="workContext">Work context</param>
        /// <param name="storeContext">Store context</param>
        /// <param name="eventPublisher">Event publisher</param>
        /// <param name="storeMappingService">Store mapping service</param>
        /// <param name="aclService">ACL service</param>
        /// <param name="catalogSettings">Catalog settings</param>
        public CategoryService(ICacheManager cacheManager,
            IRepository<StoreCategory> storeCategoryRepository,
            IRepository<ProductCategory> productCategoryRepository,
            IRepository<Product> productRepository,
            //IRepository<AclRecord> aclRepository,
            //IRepository<StoreMapping> storeMappingRepository,
            IWorkContext workContext,
            //IStoreContext storeContext,
            IEventPublisher eventPublisher,
            //IStoreMappingService storeMappingService,
            //IAclService aclService,
            CatalogSettings catalogSettings)
        {
            this._cacheManager = cacheManager;
            this._storeCategoryRepository = storeCategoryRepository;
            this._productCategoryRepository = productCategoryRepository;
            this._productRepository = productRepository;
            //this._aclRepository = aclRepository;
            //this._storeMappingRepository = storeMappingRepository;
            this._workContext = workContext;
            //this._storeContext = storeContext;
            this._eventPublisher = eventPublisher;
            //this._storeMappingService = storeMappingService;
            //this._aclService = aclService;
            this._catalogSettings = catalogSettings;
        }

        #endregion

        #region 商家类别 Methods

        /// <summary>
        /// Delete category，删除后所有子类别自动合并到上一级
        /// </summary>
        /// <param name="storeCategory">Category</param>
        public virtual void DeleteStoreCategory(StoreCategory storeCategory)
        {
            if (storeCategory == null)
                throw new ArgumentNullException("商家类别");

            storeCategory.Deleted = true;
            UpdateStoreCategory(storeCategory);

            //reset a "Parent category" property of all child subcategories
            var subcategories = GetAllStoreCategoriesByParentCategoryId(storeCategory.Id, true);
            foreach (var subStoreCategory in subcategories)
            {
                subStoreCategory.ParentId = storeCategory.ParentId;
                UpdateStoreCategory(subStoreCategory);
            }
        }

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="categoryName">Category name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Categories</returns>
        public virtual IPagedList<StoreCategory> GetAllStoreCategories(string categoryName = "", 
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _storeCategoryRepository.Table;
            if (!showHidden)
                query = query.Where(c => c.Published);
            if (!String.IsNullOrWhiteSpace(categoryName))
                query = query.Where(c => c.Name.Contains(categoryName));
            query = query.Where(c => !c.Deleted);
            query = query.OrderBy(c => c.ParentId).ThenBy(c => c.DisplayOrder);

            if (!showHidden && (!_catalogSettings.IgnoreAcl || !_catalogSettings.IgnoreStoreLimitations))
            {
                //if (!_catalogSettings.IgnoreAcl)
                //{
                //    //ACL (access control list)
                //    var allowedCustomerRolesIds = _workContext.CurrentCustomer.GetCustomerRoleIds();
                //    query = from c in query
                //            join acl in _aclRepository.Table
                //            on new { c1 = c.Id, c2 = "Category" } equals new { c1 = acl.EntityId, c2 = acl.EntityName } into c_acl
                //            from acl in c_acl.DefaultIfEmpty()
                //            where !c.SubjectToAcl || allowedCustomerRolesIds.Contains(acl.CustomerRoleId)
                //            select c;
                //}
                //if (!_catalogSettings.IgnoreStoreLimitations)
                //{
                //    //Store mapping
                //    var currentStoreId = _storeContext.CurrentStore.Id;
                //    query = from c in query
                //            join sm in _storeMappingRepository.Table
                //            on new { c1 = c.Id, c2 = "Category" } equals new { c1 = sm.EntityId, c2 = sm.EntityName } into c_sm
                //            from sm in c_sm.DefaultIfEmpty()
                //            where !c.LimitedToStores || currentStoreId == sm.StoreId
                //            select c;
                //}

                //only distinct categories (group by ID)
                query = from c in query
                        group c by c.Id
                        into cGroup
                        orderby cGroup.Key
                        select cGroup.FirstOrDefault();
                query = query.OrderBy(c => c.ParentId).ThenBy(c => c.DisplayOrder);
            }

            var unsortedCategories = query.ToList();

            //sort categories
            var sortedCategories = unsortedCategories;//.SortCategoriesForTree();

            //paging
            return new PagedList<StoreCategory>(sortedCategories, pageIndex, pageSize);
        }

      
        /// <summary>
        /// Gets all categories filtered by parent category identifier
        /// </summary>
        /// <param name="parentStoreCategoryId">Parent category identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Categories</returns>
        public virtual IList<StoreCategory> GetAllStoreCategoriesByParentCategoryId(int parentStoreCategoryId,
            bool showHidden = false)
        {
            string key = string.Format(CATEGORIES_BY_PARENT_CATEGORY_ID_KEY, parentStoreCategoryId, showHidden, 0);
            return _cacheManager.Get(key, () =>
            {
                var query = _storeCategoryRepository.Table;
                if (!showHidden)
                    query = query.Where(c => c.Published);
                query = query.Where(c => c.ParentId == parentStoreCategoryId);
                query = query.Where(c => !c.Deleted);
                query = query.OrderBy(c => c.DisplayOrder);

                if (!showHidden && (!_catalogSettings.IgnoreAcl || !_catalogSettings.IgnoreStoreLimitations))
                {
                    if (!_catalogSettings.IgnoreAcl)
                    {
                        //    //ACL (access control list)
                        //    var allowedCustomerRolesIds = _workContext.CurrentCustomer.GetCustomerRoleIds();
                        //    query = from c in query
                        //            join acl in _aclRepository.Table
                        //            on new { c1 = c.Id, c2 = "Category" } equals new { c1 = acl.EntityId, c2 = acl.EntityName } into c_acl
                        //            from acl in c_acl.DefaultIfEmpty()
                        //            where !c.SubjectToAcl || allowedCustomerRolesIds.Contains(acl.CustomerRoleId)
                        //            select c;
                        //}
                        //if (!_catalogSettings.IgnoreStoreLimitations)
                        //{
                        //    //Store mapping
                        //    var currentStoreId = _storeContext.CurrentStore.Id;
                        //    query = from c in query
                        //            join sm in _storeMappingRepository.Table
                        //            on new { c1 = c.Id, c2 = "Category" } equals new { c1 = sm.EntityId, c2 = sm.EntityName } into c_sm
                        //            from sm in c_sm.DefaultIfEmpty()
                        //            where !c.LimitedToStores || currentStoreId == sm.StoreId
                        //            select c;
                        //}
                        //only distinct categories (group by ID)
                        query = from c in query
                                group c by c.Id
                                into cGroup
                                orderby cGroup.Key
                                select cGroup.FirstOrDefault();
                        query = query.OrderBy(c => c.DisplayOrder);
                    }
                }

                var categories = query.ToList();
                return categories;
            });

        }

        /// <summary>
        /// Gets all categories displayed on the home page
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Categories</returns>
        public virtual IList<StoreCategory> GetAllCategoriesDisplayedOnHomePage(bool showHidden = false)
        {
            var query = from c in _storeCategoryRepository.Table
                        orderby c.DisplayOrder
                        where c.Published &&
                        !c.Deleted 
                        select c;

            var categories = query.ToList();
            //if (!showHidden)
            //{
            //    categories = categories
            //        .Where(c => _aclService.Authorize(c) && _storeMappingService.Authorize(c))
            //        .ToList();
            //}

            return categories;
        }

        /// <summary>
        /// Gets a category
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <returns>Category</returns>
        public virtual StoreCategory GetStoreCategoryById(int categoryId)
        {
            if (categoryId == 0)
                return null;

            string key = string.Format(CATEGORIES_BY_ID_KEY, categoryId);
            return _cacheManager.Get(key, () => _storeCategoryRepository.GetById(categoryId));
        }

        /// <summary>
        /// 类别名称唯一性检查，商铺类别、商铺类自定义类别内要求唯一       
        /// </summary>
        /// <param name="storeCategoryName"></param>
        /// <returns>返回为true，表示不可用</returns>
        public virtual bool StoreCategoryNameUniqueCheck(string storeCategoryName, int storeCategoryId = 0)
        {
            var query = _storeCategoryRepository.Table;
            query = query.Where(sc => !sc.Deleted);

            if (!String.IsNullOrWhiteSpace(storeCategoryName))
            {
                var storeCategory = query.Where(c => c.Name == storeCategoryName).FirstOrDefault();
                if (storeCategory == null) return true;
                else
                    return storeCategory.Id == storeCategoryId;

            }
            else return true;
        }

        /// <summary>
        /// Inserts store category
        /// </summary>
        /// <param name="category">Category</param>
        public virtual void InsertStoreCategory(StoreCategory category)
        {
            if (category == null)
                throw new ArgumentNullException("store category");

            _storeCategoryRepository.Insert(category);

            //cache
            _cacheManager.RemoveByPattern(CATEGORIES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(PRODUCTCATEGORIES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(category);
        }

        /// <summary>
        /// Updates the category
        /// </summary>
        /// <param name="category">Category</param>
        public virtual void UpdateStoreCategory(StoreCategory category)
        {
            if (category == null)
                throw new ArgumentNullException("category");

            //validate category hierarchy
            var parentCategory = GetStoreCategoryById(category.ParentId);
            while (parentCategory != null)
            {
                if (category.Id == parentCategory.Id)
                {
                    category.ParentId = 0;
                    break;
                }
                parentCategory = GetStoreCategoryById(parentCategory.ParentId);
            }

            _storeCategoryRepository.Update(category);

            //cache
            _cacheManager.RemoveByPattern(CATEGORIES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(PRODUCTCATEGORIES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(category);
        }

        /// <summary>
        /// Update HasDiscountsApplied property (used for performance optimization)
        /// </summary>
        /// <param name="category">Category</param>
        public virtual void UpdateHasDiscountsApplied(StoreCategory category)
        {
            if (category == null)
                throw new ArgumentNullException("category");

         //   category.HasDiscountsApplied = category.AppliedDiscounts.Count > 0;
            UpdateStoreCategory(category);
        }

        #endregion

        #region 商品类别 Methods
        /// <summary>
        /// Deletes a product category mapping
        /// </summary>
        /// <param name="productCategory">Product category</param>
        public virtual void DeleteProductCategory(ProductCategory productCategory)
        {
            if (productCategory == null)
                throw new ArgumentNullException("productCategory");

            _productCategoryRepository.Delete(productCategory);

            //cache
            _cacheManager.RemoveByPattern(CATEGORIES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(PRODUCTCATEGORIES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(productCategory);
        }

        /// <summary>
        /// 获取商家下的商品类别
        /// </summary>
        /// <param name="isStoreCategory"></param>
        /// <param name="showHidden"></param>
        /// <returns></returns>
        public virtual IList<ProductCategory> GetAllProductCategories(int storeId , bool showHidden = false)
        {
            var query = _productCategoryRepository.Table;
            query = query.Where(c => c.StoreId == storeId);
            if (!showHidden)
                query = query.Where(c => c.Published);
            query = query.Where(c => !c.Deleted);
            query = query.OrderBy(c => c.DisplayOrder);

            return query.ToList();

        }

        /// <summary>
        /// Gets a product category mapping 
        /// </summary>
        /// <param name="productCategoryId">Product category mapping identifier</param>
        /// <returns>Product category mapping</returns>
        public virtual ProductCategory GetProductCategoryById(int productCategoryId)
        {
            if (productCategoryId == 0)
                return null;

            return _productCategoryRepository.GetById(productCategoryId);
        }

        /// <summary>
        /// 商品类别唯一性检查
        /// </summary>
        /// <param name="productCategoryName"></param>
        /// <param name="storeId"></param>
        /// <param name="showHidden"></param>
        /// <returns></returns>
        public virtual bool ProductCategoryNameUniqueCheck(int storeId, string productCategoryName, int productCategoryId = 0)
        {
            var query = _productCategoryRepository.Table;
            query = query.Where(pc => !pc.Deleted && pc.StoreId == storeId);

            if (!String.IsNullOrWhiteSpace(productCategoryName))
            {
                var productCategory = query.Where(c => c.Name == productCategoryName).FirstOrDefault();
                if (productCategory == null) return true;
                else
                    return productCategory.Id == productCategoryId;

            }
            else return true;
        }

        public virtual void InsertProductCategory(ProductCategory productCategory)
        {
            if (productCategory == null)
                throw new ArgumentNullException("product category");

            _productCategoryRepository.Insert(productCategory);

            //cache
            _cacheManager.RemoveByPattern(CATEGORIES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(PRODUCTCATEGORIES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(productCategory);
        }


        /// <summary>
        /// Updates the product category mapping 
        /// </summary>
        /// <param name="productCategory">>Product category mapping</param>
        public virtual void UpdateProductCategory(ProductCategory productCategory)
        {
            if (productCategory == null)
                throw new ArgumentNullException("productCategory");

            _productCategoryRepository.Update(productCategory);

            //cache
            _cacheManager.RemoveByPattern(CATEGORIES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(PRODUCTCATEGORIES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(productCategory);
        } 
        #endregion

    }
}
