using QMTech.Core.Domain.Catalog;
using System;
using System.Collections.Generic;

namespace QMTech.Services.Catalog
{
    public static class StoreCategoryExtensions
    {
        /// <summary>
        /// 获取格式化面包屑类别
        /// </summary>
        /// <param name="category"></param>
        /// <param name="categoryService"></param>
        /// <param name="seperator"></param>
        /// <returns></returns>
        public static string GetFormattedBreadCrumb(this StoreCategory category, ICategoryService categoryService, string seperator = ">>")
        {
            string result = string.Empty;

            var breadcrumb = GetCategoryBreadCrumb(category, categoryService, true);

            for(int i=0;i<breadcrumb.Count;i++)
            {
                var categoryName = breadcrumb[i].Name;
                result = string.IsNullOrEmpty(result) ? categoryName : string.Format("{0} {1} {2}", result, seperator, categoryName);
            }

            return result;
        }


        /// <summary>
        /// 获取面包屑类型
        /// </summary>
        /// <param name="category"></param>
        /// <param name="categoryService"></param>
        /// <param name="showHidden"></param>
        /// <returns></returns>
        public static IList<StoreCategory> GetCategoryBreadCrumb(this StoreCategory category, ICategoryService categoryService, bool showHidden = false)
        {
            if (category == null) throw new ArgumentNullException("category");

            var result = new List<StoreCategory>();

            //防止循环引用
            var alreadyProcessedCategoryIds = new List<int>();

            while (category != null && //not null
                          !category.Deleted && //not deleted
                          (showHidden || category.Published) && //published
                                                                //(showHidden || aclService.Authorize(category)) && //ACL
                                                                //(showHidden || storeMappingService.Authorize(category)) && //Store mapping
                          !alreadyProcessedCategoryIds.Contains(category.Id)) //prevent circular references
            {
                result.Add(category);

                alreadyProcessedCategoryIds.Add(category.Id);

                category = categoryService.GetStoreCategoryById(category.ParentId);
            }
            result.Reverse();
            return result;
        }
    }
}
