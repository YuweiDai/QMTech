using QMTech.Core;
using QMTech.Core.Domain.Catalog;
using System.Collections.Generic;

namespace QMTech.Services.Catalog
{
    /// <summary>
    /// 包括商家以及商家内的商品分类服务
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// 删除商家类别
        /// </summary>
        /// <param name="storeCateogry"></param>
        void DeleteStoreCategory(StoreCategory storeCateogry);

        void DeleteProductCategory(ProductCategory productCateogry);

        /// <summary>
        /// Gets all store categories
        /// </summary>
        /// <param name="storeCategoryName">Category name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Categories</returns>
        IPagedList<StoreCategory> GetAllStoreCategories(string storeCategoryName = "", 
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        /// <summary>
        /// 获取所有的商品类别
        /// </summary>
        /// <returns></returns>
        IList<ProductCategory> GetAllProductCategories(int storeId = 0, bool showHidden = false);

        /// <summary>
        /// Gets all categories filtered by parent category identifier
        /// </summary>
        /// <param name="parentCategoryId">Parent category identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Categories</returns>
        IList<StoreCategory> GetAllStoreCategoriesByParentCategoryId(int parentCategoryId,
            bool showHidden = false);

        /// <summary>
        /// Gets a store category
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <returns>Category</returns>
        StoreCategory GetStoreCategoryById(int categoryId);

        ProductCategory GetProductCategoryById(int categoryId);

        /// <summary>
        /// 商家类别名称唯一性检查
        /// </summary>
        /// <param name="storeCategoryName"></param>
        /// <returns></returns>
        bool StoreCategoryNameUniqueCheck(string storeCategoryName, int storeCategoryId = 0);

        /// <summary>
        /// 商品类别名称唯一性检查
        /// </summary>
        /// <param name="productCategoryName"></param>
        /// <param name="storeId"></param>
        /// <returns></returns>
        bool ProductCategoryNameUniqueCheck(int storeId, string productCategoryName, int productCategoryId = 0);

        /// <summary>
        /// Inserts category
        /// </summary>
        /// <param name="storeCategory">Category</param>
        void InsertStoreCategory(StoreCategory storeCategory);

        void InsertProductCategory(ProductCategory productCategory);

        /// <summary>
        /// Updates the category
        /// </summary>
        /// <param name="storeCategory">Category</param>
        void UpdateStoreCategory(StoreCategory storeCategory);

        void UpdateProductCategory(ProductCategory productCategory);

        /// <summary>
        /// Update HasDiscountsApplied property (used for performance optimization)
        /// </summary>
        /// <param name="category">Category</param>
        void UpdateHasDiscountsApplied(StoreCategory category);

        
    }
}
