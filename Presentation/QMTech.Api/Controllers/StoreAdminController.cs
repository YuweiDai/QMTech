using QMTech.Api.Models.Catalog;
using QMTech.Core;
using QMTech.Core.Domain.Catalog;
using QMTech.Services.Catalog;
using QMTech.Services.Logging;
using QMTech.Services.Media;
using QMTech.Web.Api.Extensions;
using QMTech.Web.Framework.Controllers;
using QMTech.Web.Framework.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace QMTech.Api.Controllers
{
    [RoutePrefix("Admin/Catalog/Stores")]
    public class StoreAdminController : BaseAdminApiController
    {
        private readonly ICategoryService _categoryService;
        private readonly IStoreService _storeService;
        private readonly IPictureService _pictureService;
        private readonly ICustomerActivityService _customerActivityService;

        public StoreAdminController(ICategoryService categoryService, IStoreService storeService, IPictureService pictureService,
            ICustomerActivityService customerActivityService)
        {
            _categoryService = categoryService;
            _storeService = storeService;
            _pictureService = pictureService;

            _customerActivityService = customerActivityService;
        }

        #region utility

        /// <summary>
        /// 商家模型通用性处理
        /// </summary>
        /// <param name="store"></param>
        /// <param name="storeModel"></param>
        protected void PrepareStore(Store store,StoreModel storeModel)
        {
            //营业时间判断 
            if (!store.AllOpened)
            {
                //考虑营业时间至凌晨，日期自动加1天
                if (store.StartTime.Value > store.EndTime.Value) store.EndTime.Value.AddDays(1);
                else if ((store.EndTime.Value - store.StartTime.Value).TotalMilliseconds <= 30 * 6000)
                {
                    store.StartTime = store.EndTime = null;
                    store.AllOpened = true;
                }
            }

            if (store.DisplayOrder < 0) store.DisplayOrder = 0;

            var category = _categoryService.GetStoreCategoryById(storeModel.StoreCategoryId);
            store.StoreCategory = category;


        }

        #endregion

        #region 商家API
        [HttpGet]
        [Route("Unique/{name}")]
        public IHttpActionResult UniqueCheck(string name)
        {
            var result = !_storeService.NameUniqueCheck(name);

            return Ok(result);
        }

        [HttpGet]
        [Route("{storeId:int}")]
        public IHttpActionResult Get(int storeId)
        {
            var store = _storeService.GetStoreById(storeId);
            if (store == null || store.Deleted)
                return NotFound();

            var model = store.ToModel();

            //获取logo图片
            var logoPicture = store.StorePictures.Where(sp => sp.IsLogo).SingleOrDefault();
            if (logoPicture != null)
            {
                model.LogoUrl = _pictureService.GetPictureUrl(logoPicture.PictureId);
            }

            //activity log
            _customerActivityService.InsertActivity("GetStoreInfo", "获取 名为 {0} 的商家信息", store.Name);

            return Ok(model);
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll(string query = "", string sort = "", int pageSize = Int32.MaxValue, int pageIndex = 0, bool showHidden = false)
        {
            //初始化排序条件
            var sortConditions = PropertySortCondition.Instance(sort);

            //特殊字段排序调整
            if (sort.StartsWith("CategoryName")) sortConditions[0].PropertyName = "Category";


            var stores = _storeService.GetAllStores(query, pageIndex, pageSize, showHidden, sortConditions);

            var response = new ListResponse<StoreModel>
            {
                Paging = new Paging
                {
                    PageIndex = pageIndex,
                    PageSize = pageIndex,
                    Total = stores.TotalCount,
                    FilterCount = string.IsNullOrEmpty(query) ? stores.TotalCount : stores.Count,
                },
                Data = stores.Select(s =>
                {
                    var storeModel = s.ToModel();
                    storeModel.StoreCategory = s.StoreCategory.GetFormattedBreadCrumb(_categoryService);


                    if (storeModel.AllOpened) storeModel.Opend = true;
                    else
                    {
                        var date = Convert.ToDateTime(string.Format("{0} {1}", s.StartTime.Value.ToLongDateString(), DateTime.Now.ToShortTimeString()));

                        storeModel.Opend = date >= s.StartTime.Value && date <= s.EndTime.Value;
                    }

                    return storeModel;
                })
            };

            response.Statistics = new StoresStatisticsModel
            {
                StoresCount = stores.Count,
                ProductCount = response.Data.Sum(s => s.ProductsCount),
                CoorperateCount = response.Data.Where(s => !s.SelfSupport).Count(),
                InSalesCount = response.Data.Where(s => s.Opend).Count()
            };

            //activity log
            _customerActivityService.InsertActivity("GetStoreList", "获取商家列表信息");

            return Ok(response);
        }

        /// <summary>
        /// 新增商家
        /// </summary>
        /// <param name="storeModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(StoreModel storeModel)
        {
            var store = storeModel.ToEntity();

            //预处理
            PrepareStore(store, storeModel);

            //保存图片
            var base64 = HttpUtility.HtmlDecode(storeModel.Logo.Trim());
            var picture = _pictureService.InsertPicture(base64);

            //保存商店
            _storeService.InsertStore(store);

            //关联
            var storePicture = new StorePicture
            {
                IsLogo = true,
                Picture = picture,
                Store = store,
                DisplayOrder = 0
            };

            _storeService.InsertStorePicture(storePicture);

            //activity log
            _customerActivityService.InsertActivity("AddNewStore", "增加 名为 {0} 的商店", store.Name);
            
            return Ok(store.ToModel());
        }

        /// <summary>
        /// 更新商家
        /// </summary>
        /// <param name="storeModel"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{storeId:int}")]
        public IHttpActionResult UpdateStore(int storeId,StoreModel storeModel)
        {
            var store = _storeService.GetStoreById(storeId);
            if (store == null || store.Deleted) return NotFound();
            store = storeModel.ToEntity(store);

            //预处理
            PrepareStore(store, storeModel);

            //更新LOGO图片
            if (!string.IsNullOrEmpty(storeModel.Logo))
            {
                var base64 = HttpUtility.HtmlDecode(storeModel.Logo.Trim());
                var picture = _pictureService.InsertPicture(base64);

                foreach (var storePicture in store.StorePictures)
                {
                    if(storePicture.IsLogo)
                    {
                        storePicture.Picture = picture;

                        _storeService.UpdateStorePicture(storePicture);
                    }
                }
            }

            //保存商店
            _storeService.UpdateStore(store);

            //activity log
            _customerActivityService.InsertActivity("UpdateStore", "更新 名为 {0} 的商店的基本信息", store.Name);

            //SuccessNotification(_localizationService.GetResource("Admin.Catalog.Categories.Added"));

            return Ok(store.ToModel());
        }

        [HttpDelete]
        [Route("{storeId:int}")]
        public IHttpActionResult DeleteStore(int storeId)
        {

            var store = _storeService.GetStoreById(storeId);
            if (store == null || store.Deleted) return NotFound();

            _storeService.DeleteStore(store);

            //activity log
            _customerActivityService.InsertActivity("DeleteStore", "删除 名为 {0} 的商家", store.Name);

            //通知
            //SuccessNotification(_localizationService.GetResource("Admin.Catalog.Categories.Deleted"));

            return Ok();
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="storeIdString"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{storeIdString}")]
        public IHttpActionResult DeleteStore(string storeIdString)
        {
            var idStringArr = storeIdString.Split('_');
            foreach (var idStr in idStringArr)
            {
                int id = 0;
                if (!int.TryParse(idStr, out id)) continue;

                var store = _storeService.GetStoreById(id);
                if (store == null) continue;

                _storeService.DeleteStore(store);
            }


            //活动日志
            _customerActivityService.InsertActivity("DeleteStores", "批量删除 Id为 {0} 的商家",storeIdString);

            //通知
            //SuccessNotification(_localizationService.GetResource("Admin.Catalog.Categories.Deleted"));

            return Ok();
        }
        #endregion


        #region 商品分类API

        /// <summary>
        /// 类别名称唯一性检查，商铺类别、商铺类自定义类别内要求唯一        
        /// </summary>
        /// <param name="name"></param>
        /// <param name="storeId"></param>
        /// <returns>返回为true，表示不可用</returns>
        [HttpGet]
        [Route("{storeId:int}/ProductCategories/Unique/{name}")]
        public IHttpActionResult UniqueCheck(int storeId ,string name)
        {
            var result = _categoryService.ProductCategoryNameUniqueCheck(storeId,name);

            return Ok(result);
        }

        /// <summary>
        /// 获取所有商品类别
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{storeId:int}/ProductCategories")]
        public IHttpActionResult GetProductCategories(int storeId = 0, bool showHidden = false, bool selectelist = false)
        {
            var categories = _categoryService.GetAllProductCategories(storeId, showHidden).
                Select(pc => pc.ToModel());

            //活动日志
            _customerActivityService.InsertActivity("GetProductCategories", "获取商家商品分类");

            if (selectelist)
            {
                var response = new List<SelectListItem>();

                response = categories.Select(pc =>
                {
                    return new SelectListItem
                    {
                        Label = pc.Name,
                        Value = pc.Id
                    };
                }).ToList();

                return Ok(response);
            }
            else return Ok(categories);
        }

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="categoryModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{storeId:int}/ProductCategories")]
        public IHttpActionResult CreateProductCategory(ProductCategoryModel categoryModel)
        {
            var category = categoryModel.ToEntity();

            _categoryService.InsertProductCategory(category);

            //activity log
            _customerActivityService.InsertActivity("AddNewProductCategory", "增加 名为 {0} 的商品类别", category.Name);

            //SuccessNotification(_localizationService.GetResource("Admin.Catalog.Categories.Added"));

            return Ok(category.ToModel());
        }

        [HttpPut]
        [Route("{storeId:int}/ProductCategories/{productCategoryId:int}")]
        public IHttpActionResult UpdateProductCategory(int productCategoryId, ProductCategoryModel categoryModel)
        {

            var category = _categoryService.GetProductCategoryById(productCategoryId);
            if (category == null || category.Deleted) return NotFound();

            category = categoryModel.ToEntity(category);

            _categoryService.UpdateProductCategory(category);

            //activity log
            _customerActivityService.InsertActivity("UpdateProductCategory", "更新 名为 {0} 的商品类别", category.Name);

            return Ok(category.ToModel());

        }

        /// <summary>
        /// 删除类别，单个删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("ProductCategories/{productCategoryId:int}")]
        public IHttpActionResult DeleteProductCategory(int productCategoryId)
        {

            var category = _categoryService.GetProductCategoryById(productCategoryId);
            if (category == null) return NotFound();

            _categoryService.DeleteProductCategory(category);

            //activity log
            _customerActivityService.InsertActivity("DeleteProductCategory", "删除 名为 {0} 的商品类别", category.Name);

            //通知
            //SuccessNotification(_localizationService.GetResource("Admin.Catalog.Categories.Deleted"));

            return Ok();
        }


        #endregion
    }
}
