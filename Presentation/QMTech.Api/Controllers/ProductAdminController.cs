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
using System.Web.Http;

namespace QMTech.Api.Controllers
{
    [RoutePrefix("Admin/Catalog/Stores/{storeId:int}")]
    public class ProductAdminController: BaseAdminApiController
    {
        #region Fields

        private readonly IProductService _productService;
        //private readonly IProductTemplateService _productTemplateService;
        private readonly ICategoryService _categoryService;
        //private readonly IManufacturerService _manufacturerService;
        //private readonly ICustomerService _customerService;
        //private readonly IUrlRecordService _urlRecordService;
        private readonly IWorkContext _workContext;
        //private readonly ILanguageService _languageService;
        //private readonly ILocalizationService _localizationService;
        //private readonly ILocalizedEntityService _localizedEntityService;
        //private readonly ISpecificationAttributeService _specificationAttributeService;
        private readonly IPictureService _pictureService;
        
        private readonly IProductTagService _productTagService;
        //private readonly ICopyProductService _copyProductService;
        //private readonly IPdfService _pdfService;
        //private readonly IExportManager _exportManager;
        //private readonly IImportManager _importManager;
        private readonly ICustomerActivityService _customerActivityService;
        //private readonly IPermissionService _permissionService;
        //private readonly IAclService _aclService;
        private readonly IStoreService _storeService;
        //private readonly IOrderService _orderService;
        //private readonly IStoreMappingService _storeMappingService;
        //private readonly IVendorService _vendorService;
        //private readonly IShippingService _shippingService;
        //private readonly IShipmentService _shipmentService;
        //private readonly ICurrencyService _currencyService;
        //private readonly CurrencySettings _currencySettings;
        //private readonly IMeasureService _measureService;
        //private readonly MeasureSettings _measureSettings;
        //private readonly AdminAreaSettings _adminAreaSettings;
        //private readonly IDateTimeHelper _dateTimeHelper;
        //private readonly IDiscountService _discountService;
        //private readonly IProductAttributeService _productAttributeService;
        //private readonly IBackInStockSubscriptionService _backInStockSubscriptionService;
        //private readonly IShoppingCartService _shoppingCartService;
        //private readonly IProductAttributeFormatter _productAttributeFormatter;
        //private readonly IProductAttributeParser _productAttributeParser;
        //private readonly IDownloadService _downloadService;

        #endregion

        #region Constructors

        public ProductAdminController(IProductService productService,
            //IProductTemplateService productTemplateService,
            ICategoryService categoryService,
            //IManufacturerService manufacturerService,
            //ICustomerService customerService,
            //IUrlRecordService urlRecordService,
            IWorkContext workContext,       
            //ISpecificationAttributeService specificationAttributeService,
            IPictureService pictureService,
            IProductTagService productTagService,
            //ICopyProductService copyProductService,
            //IPdfService pdfService,
            //IExportManager exportManager,
            //IImportManager importManager,
            ICustomerActivityService customerActivityService,
            //IPermissionService permissionService,
            //IAclService aclService,
            IStoreService storeService)
            //IOrderService orderService,
            //IStoreMappingService storeMappingService,
            // IVendorService vendorService,
            //IShippingService shippingService,
            //IShipmentService shipmentService,
            //ICurrencyService currencyService,
            //CurrencySettings currencySettings,
            //IMeasureService measureService,
            //MeasureSettings measureSettings,
            //AdminAreaSettings adminAreaSettings,
            //IDateTimeHelper dateTimeHelper,
            //IDiscountService discountService,
            //IProductAttributeService productAttributeService,
            //IBackInStockSubscriptionService backInStockSubscriptionService,
            //IShoppingCartService shoppingCartService,
            //IProductAttributeFormatter productAttributeFormatter,
            //IProductAttributeParser productAttributeParser,
            //IDownloadService downloadService)
        {
            this._productService = productService;
            //this._productTemplateService = productTemplateService;
            this._categoryService = categoryService;
            //this._manufacturerService = manufacturerService;
            //this._customerService = customerService;
            //this._urlRecordService = urlRecordService;
            this._workContext = workContext;
           
            //this._specificationAttributeService = specificationAttributeService;
            this._pictureService = pictureService;
            //this._taxCategoryService = taxCategoryService;
            this._productTagService = productTagService;
            //this._copyProductService = copyProductService;
            //this._pdfService = pdfService;
            //this._exportManager = exportManager;
            //this._importManager = importManager;
            this._customerActivityService = customerActivityService;
            //this._permissionService = permissionService;
            //this._aclService = aclService;
            this._storeService = storeService;
            //this._orderService = orderService;
            //this._storeMappingService = storeMappingService;
            //this._vendorService = vendorService;
            //this._shippingService = shippingService;
            //this._shipmentService = shipmentService;
            //this._currencyService = currencyService;
            //this._currencySettings = currencySettings;
            //this._measureService = measureService;
            //this._measureSettings = measureSettings;
            //this._adminAreaSettings = adminAreaSettings;
            //this._dateTimeHelper = dateTimeHelper;
            //this._discountService = discountService;
            //this._productAttributeService = productAttributeService;
            //this._backInStockSubscriptionService = backInStockSubscriptionService;
            //this._shoppingCartService = shoppingCartService;
            //this._productAttributeFormatter = productAttributeFormatter;
            //this._productAttributeParser = productAttributeParser;
            //this._downloadService = downloadService;
        }

        #endregion

        #region Utility

        /// <summary>
        /// 商品模型数据预处理
        /// </summary>
        /// <param name="productModel"></param>
        private void ProductPreProcessed(Product product, ProductModel productModel)
        {
            if (product.DisplayOrder < 0) product.DisplayOrder = 0;

            //有效期转换
            if (!string.IsNullOrWhiteSpace(productModel.AvailableDateRange))
            {
                var availableRange = CommonHelper.ConvertDateTimeRangeFromString(productModel.AvailableDateRange);
                if (availableRange != null)
                {
                    product.AvailableStartDateTime = availableRange[0];
                    product.AvailableEndDateTime = availableRange[1];
                }
            }

            //特价有效期转换
            if (productModel.SpecialPrice.HasValue && productModel.SpecialPrice.Value > 0 && !string.IsNullOrWhiteSpace(productModel.SpecialDateRange))
            {
                var specialAvailableRange = CommonHelper.ConvertDateTimeRangeFromString(productModel.SpecialDateRange);
                if (specialAvailableRange != null)
                {
                    product.SpecialPriceStartDateTime = specialAvailableRange[0];
                    product.SpecialPriceEndDateTime = specialAvailableRange[1];
                }
            }
        }

        /// <summary>
        /// 准备商品图片模型
        /// </summary>
        [NonAction]
        protected virtual void PrepareProductPictures(ProductModel productModel)
        {
            foreach (var productPictureModel in productModel.ProductPictures)
            {
                productPictureModel.Src = _pictureService.GetPictureUrl(productPictureModel.PictureId, 200);
            }

            productModel.ProductPictures = productModel.ProductPictures.OrderBy(pp => pp.DisplayOrder).ToList();


        }

        /// <summary>
        /// 保存商品标签
        /// </summary>
        /// <param name="product"></param>
        [NonAction]
        protected virtual void SaveProductTags(Product product, IEnumerable<ProductTagModel> productTags)
        {
            var existingProductTags = product.ProductTags.ToList();
            var productTagsToRemove = new List<ProductTag>();

            foreach (var existingProductTag in existingProductTags)
            {
                if (productTags.Where(pp => pp.Id == existingProductTag.Id).Count() == 0)
                {
                    productTagsToRemove.Add(existingProductTag);
                }
            }

            //遍历当前的标签集合
            foreach (var productTagModel in productTags)
            {
                var tag = productTagModel.ToEntity();
                if (productTagModel.Id > 0)
                {
                    tag = _productTagService.GetProductTagById(productTagModel.Id);
                }

                if (existingProductTags.Where(ept => ept.Id == tag.Id).Count() <= 0)
                    product.ProductTags.Add(tag);
            }

            //删除不存在的图片
            foreach (var productTag in productTagsToRemove)
            {
                product.ProductTags.Remove(productTag);
            }
        }

        /// <summary>
        /// 保存商品图片
        /// </summary>
        /// <param name="product"></param>
        /// <param name="productPictures"></param>
        [NonAction]
        protected virtual void SaveProductPictures(Product product, IEnumerable<ProductPictureModel> productPictures)
        {
            var existingProductPictures = product.ProductPictures.ToList();
            var productPicturesToRemove = new List<ProductPicture>();

            foreach (var existingProductPicture in existingProductPictures)
            {

                if (productPictures.Where(pp => pp.Id == existingProductPicture.Id).Count() == 0)
                {
                    productPicturesToRemove.Add(existingProductPicture);
                }
            }

            foreach (var newProductPictureModel in productPictures)
            {
                var productPicture = newProductPictureModel.ToEntity();
                if (productPicture.Id != 0)
                {
                    productPicture = _productService.GetProductPictureById(newProductPictureModel.Id);
                    productPicture = newProductPictureModel.ToEntity(productPicture);
                }

                var picture = _pictureService.GetPictureById(newProductPictureModel.PictureId);
                if (picture == null || picture.Deleted) continue;

                picture = _pictureService.UpdatePicture(picture.Id, "", newProductPictureModel.Alt, newProductPictureModel.Title);
                productPicture.Picture = picture;

                if (newProductPictureModel.Id == 0) product.ProductPictures.Add(productPicture);
                else _productService.UpdateProductPicture(productPicture);
            }

            //删除不存在的图片
            foreach (var productPicture in productPicturesToRemove)
            {
                _productService.DeleteProductPicture(productPicture);
            }
        }

        #endregion

        /// <summary>
        /// 当前商家内是否有相同名称的商品
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Unique/{name}")]
        public IHttpActionResult UniqueCheck(int storeId, string name, int productId = 0)
        {
            var result = !_productService.NameUniqueCheck(name, storeId, productId);

            return Ok(result);
        }


        [HttpGet]
        [Route("Products")]
        public IHttpActionResult GetAll(int storeId=0,string query ="", string sort = "", int pageSize = Int32.MaxValue, int pageIndex = 0, bool showHidden = false)
        {
            //获取未删除商家的商品
            var store = _storeService.GetStoreById(storeId);
            if (store == null || store.Deleted)
                return NotFound();

            //初始化排序条件
            var sortConditions = PropertySortCondition.Instance(sort);

            //特殊字段排序调整
            if (sort.StartsWith("CategoryName")) sortConditions[0].PropertyName = "Category";


            var products = _productService.GetAllProducts(storeId,query, pageIndex, pageSize, showHidden, sortConditions);

            var response = new ListResponse<ProductModel>
            {
                Paging = new Paging
                {
                    PageIndex = pageIndex,
                    PageSize = pageIndex,
                    Total = products.TotalCount,
                    FilterCount = string.IsNullOrEmpty(query) ? products.TotalCount : products.Count,
                },
                Data = products.Select(s =>
                {
                    var productModel = s.ToModel();
                    productModel.ProductCategory = s.ProductCategory.Name;
                    return productModel;
                })
            };

            _customerActivityService.InsertActivity("GetProducts", "获取 {0} 的商品列表", store.Name);

            return Ok(response);
        }


        /// <summary>
        /// 新增商品
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Products")]        
        public IHttpActionResult CreateProduct(int storeId, ProductModel productModel)
        {
            if (storeId != productModel.StoreId) return BadRequest("参数不一致");

            var product = productModel.ToEntity();

            //销售日期、有效期等处理
            ProductPreProcessed(product, productModel);

            var store = _storeService.GetStoreById(storeId);
            if (store == null || store.Deleted) return BadRequest("商家不存在");
            product.Store = store;

            var category = _categoryService.GetProductCategoryById(productModel.ProductCategoryId);
            if (category == null || category.Deleted) return BadRequest("商品类别不存在");
            product.ProductCategory = category;

            //activity log
            _customerActivityService.InsertActivity("AddNewProduct", "新增名为 {0} 的商品", product.Name);

            //SuccessNotification(_localizationService.GetResource("Admin.Catalog.Categories.Added"));

            _productService.InsertProduct(product);

            return Ok(product.ToModel());
        }

        [HttpPut]
        [Route("Products/{productId:int}")]
        public IHttpActionResult UpdateProduct(ProductModel productModel)
        {
            var product = _productService.GetProductById(productModel.Id);
            if (product == null || product.Deleted) return NotFound();

            product = productModel.ToEntity(product);

            //销售日期、有效期等处理
            ProductPreProcessed(product, productModel);

            var category = _categoryService.GetProductCategoryById(productModel.ProductCategoryId);
            if (category == null || category.Deleted) return BadRequest("商品类别不存在");
            product.ProductCategory = category;

            //保存商品标签
            SaveProductTags(product, productModel.ProductTags);

            //保存商品图片
            SaveProductPictures(product, productModel.ProductPictures);


            //activity log           
            _customerActivityService.InsertActivity("UpdateProduct", "更新名为 {0} 的商品", product.Name);

            //SuccessNotification(_localizationService.GetResource("Admin.Catalog.Categories.Added"));

            _productService.UpdateProduct(product);

            var model = product.ToModel();
            PrepareProductPictures(model);


            return Ok(model);
        }

        [HttpGet]
        [Route("Products/{productId:int}")]
        public IHttpActionResult GetProductBy(int productId)
        {
            var product = _productService.GetProductById(productId);
            if (product == null || product.Deleted) return NotFound();

            var model = product.ToModel();
            PrepareProductPictures(model);

            return Ok(model);
        }

        [HttpDelete]
        [Route("Products/{productId:int}")]
        public IHttpActionResult DeleteProduct(int productId)
        {
            var product = _productService.GetProductById(productId);

            if (product == null) return NotFound();

            _productService.DeleteProduct(product);

            //活动日志
            _customerActivityService.InsertActivity("DeleteProduct", "删除名为 {0} 的商品", product.Name);

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
        [Route("Products/{productIdString}")]
        public IHttpActionResult DeleteProducts(string productIdString)
        {
            var idStringArr = productIdString.Split('_');
            foreach (var idStr in idStringArr)
            {
                int id = 0;
                if (!int.TryParse(idStr, out id)) continue;

                var product = _productService.GetProductById(id);

                if (product == null) return NotFound();

                _productService.DeleteProduct(product);
            }


            //活动日志
            _customerActivityService.InsertActivity("DeleteProducts", "删除Id为 {0} 的商品", productIdString);

            //通知
            //SuccessNotification(_localizationService.GetResource("Admin.Catalog.Categories.Deleted"));

            return Ok();
        }
    }
}