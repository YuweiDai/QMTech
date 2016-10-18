using QMTech.Api.Models.Catalog;
using QMTech.Web.Api.Extensions;
using QMTech.Services.Catalog;
using QMTech.Web.Framework.Response;
using System.Collections.Generic;
using System.Web.Http;
using QMTech.Core.Domain.Catalog;
using QMTech.Web.Framework.Controllers;
using QMTech.Services.Logging;

namespace QMTech.Api.Controllers
{
    [RoutePrefix("Admin/Catalog/StoreCategory")]
    public class StoreCategoryAdminController : BaseAdminApiController
    {
        private readonly ICategoryService _storeCategoryService;

        private readonly ICustomerActivityService _customerActivityService;

        public StoreCategoryAdminController(ICategoryService storeCategoryService,
                        ICustomerActivityService customerActivityService)
        {
            _storeCategoryService = storeCategoryService;
            _customerActivityService = customerActivityService;
        }

        #region Utility

        /// <summary>
        /// 获取子类别生成目录树
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [NonAction]
        private IList<TreeList<StoreCategoryModel>> LoadChildrenCategories(int parentId,bool showHidden)
        {

            var treeResponse = new List<TreeList<StoreCategoryModel>>();

            //获取所有一级节点
            var Categories = _storeCategoryService.GetAllStoreCategoriesByParentCategoryId(parentId, showHidden);

            foreach (var storeCategory in Categories)
            {
                var obj = new TreeList<StoreCategoryModel>()
                {
                    Label = storeCategory.Published ? storeCategory.Name : storeCategory.Name + "（未发布）",
                    Children = LoadChildrenCategories(storeCategory.Id, showHidden),
                    Data = storeCategory.ToModel()        
                };

                treeResponse.Add(obj);
            }

            return treeResponse;
        }

        /// <summary>
        /// 根据目录从属关系生成select子项
        /// </summary>
        /// <param name="list"></param>
        /// <param name="categories"></param>
        /// <param name="seperator"></param>
        /// <param name="breadCrumb"></param>
        /// <param name="showHidden"></param>
        [NonAction]
        private void GetFormattedBreadCrumbSelectListItems(IList<SelectListItem> list, IList<StoreCategory> categories, string seperator, string breadCrumb = "", bool showHidden = false)
        {
            foreach (var category in categories)
            {
                var subCategories = _storeCategoryService.GetAllStoreCategoriesByParentCategoryId(category.Id, showHidden);
                string tempBreadCrumbText = string.IsNullOrEmpty(breadCrumb) ? category.Name : string.Format("{0} {1} {2}{3}", breadCrumb, seperator, category.Name, category.Published ? "" : "（未发布）");

                if (subCategories.Count > 0)
                    GetFormattedBreadCrumbSelectListItems(list, subCategories, seperator, tempBreadCrumbText, showHidden);
                else
                {
                    var selectItem = new SelectListItem()
                    {
                        Label = tempBreadCrumbText,
                        Value = category.Id
                    };
                    list.Add(selectItem);
                }
            }
        }

        #endregion

        /// <summary>
        /// 生成下拉选择集合
        /// </summary>
        /// <param name="showHidden"></param>
        /// <returns></returns>
        [Route("SelectList")]
        public IHttpActionResult GetStoreCategoryList(bool showHidden = false)
        {
            var response = new List<SelectListItem>();

            //获取根节点
            var rootCategories = _storeCategoryService.GetAllStoreCategoriesByParentCategoryId(0, showHidden);
            GetFormattedBreadCrumbSelectListItems(response, rootCategories, ">>", string.Empty, showHidden);
            return Ok(response);
        }


        [Route("TreeView")]
        public IHttpActionResult GetStoreCategoryTreeView(bool showHidden = false)
        {
            var response = LoadChildrenCategories(0, showHidden);

            return Ok(response);
        }


        /// <summary>
        /// 类别名称唯一性检查，商铺类别、商铺类自定义类别内要求唯一        
        /// </summary>
        /// <param name="name"></param>
        /// <param name="storeId"></param>
        /// <returns>返回为true，表示不可用</returns>
        [HttpGet]
        [Route("Unique/{name}")]
        public IHttpActionResult UniqueCheck(string name, int storeId = 0)
        {
            var result = !_storeCategoryService.StoreCategoryNameUniqueCheck(name);

            return Ok(result);
        }

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="categoryModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(StoreCategoryModel categoryModel)
        {
            var category = categoryModel.ToEntity();

            if (ModelState.IsValid)
            {
                _storeCategoryService.InsertStoreCategory(category);

                //activity log
                _customerActivityService.InsertActivity("AddNewStoreCategory", "新增名为 {0} 的商家类别", category.Name);

                //SuccessNotification(_localizationService.GetResource("Admin.Catalog.Categories.Added"));

                return Ok(category.ToModel());
            }
            else return BadRequest("类别模型数据错误");
        }

        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult Update(int id, StoreCategoryModel categoryModel)
        {

            var category = _storeCategoryService.GetStoreCategoryById(id);
            if (category == null || category.Deleted) return NotFound();

            category = categoryModel.ToEntity(category);

            _storeCategoryService.UpdateStoreCategory(category);
            _customerActivityService.InsertActivity("UpdateStoreCategory", "更新名为 {0} 的商家类别", category.Name);
            return Ok(category.ToModel());
        }

        /// <summary>
        /// 删除类别，单个删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {

            var category = _storeCategoryService.GetStoreCategoryById(id);
            if (category == null) return NotFound();

            _storeCategoryService.DeleteStoreCategory(category);

            //活动日志
            _customerActivityService.InsertActivity("DeleteStoreCategory", "删除名为 {0} 的商家类别", category.Name);

            //通知
            //SuccessNotification(_localizationService.GetResource("Admin.Catalog.Categories.Deleted"));

            return Ok();
        }

    }
}
