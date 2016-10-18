using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QMTech.Core;
using QMTech.Core.Domain.Catalog;
using QMTech.Core.Data;
using QMTech.Services.Messages;
using QMTech.Data;
using QMTech.Core.Caching;
using QMTech.Services.Events;
using QMTech.Core.Domain.Common;

namespace QMTech.Services.Catalog
{
    public class ProductService : IProductService
    {
        #region Constants
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : product ID
        /// </remarks>
        private const string PRODUCTS_BY_ID_KEY = "Qm.product.id-{0}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string PRODUCTS_PATTERN_KEY = "Qm.product.";
        #endregion

        #region Fields

        private readonly IRepository<Product> _productRepository;
        //private readonly IRepository<RelatedProduct> _relatedProductRepository;
        //private readonly IRepository<CrossSellProduct> _crossSellProductRepository;
        //private readonly IRepository<TierPrice> _tierPriceRepository;
        //private readonly IRepository<LocalizedProperty> _localizedPropertyRepository;
        //private readonly IRepository<AclRecord> _aclRepository;
        //private readonly IRepository<StoreMapping> _storeMappingRepository;
        private readonly IRepository<ProductPicture> _productPictureRepository;
        //private readonly IRepository<ProductSpecificationAttribute> _productSpecificationAttributeRepository;
        private readonly IRepository<ProductReview> _productReviewRepository;
        //private readonly IRepository<ProductWarehouseInventory> _productWarehouseInventoryRepository;
        //private readonly IProductAttributeService _productAttributeService;
        //private readonly IProductAttributeParser _productAttributeParser;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly IDataProvider _dataProvider;
        private readonly IDbContext _dbContext;
        private readonly ICacheManager _cacheManager;
        private readonly IWorkContext _workContext;
        //private readonly IStoreContext _storeContext;
        //private readonly LocalizationSettings _localizationSettings;
        private readonly CommonSettings _commonSettings;
        private readonly CatalogSettings _catalogSettings;
        private readonly IEventPublisher _eventPublisher;
        //private readonly IAclService _aclService;
        //private readonly IStoreMappingService _storeMappingService;

        #endregion


        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="productRepository">Product repository</param>
        /// <param name="relatedProductRepository">Related product repository</param>
        /// <param name="crossSellProductRepository">Cross-sell product repository</param>
        /// <param name="tierPriceRepository">Tier price repository</param>
        /// <param name="localizedPropertyRepository">Localized property repository</param>
        /// <param name="aclRepository">ACL record repository</param>
        /// <param name="storeMappingRepository">Store mapping repository</param>
        /// <param name="productPictureRepository">Product picture repository</param>
        /// <param name="productSpecificationAttributeRepository">Product specification attribute repository</param>
        /// <param name="productReviewRepository">Product review repository</param>
        /// <param name="productWarehouseInventoryRepository">Product warehouse inventory repository</param>
        /// <param name="productAttributeService">Product attribute service</param>
        /// <param name="productAttributeParser">Product attribute parser service</param>
        /// <param name="languageService">Language service</param>
        /// <param name="workflowMessageService">Workflow message service</param>
        /// <param name="dataProvider">Data provider</param>
        /// <param name="dbContext">Database Context</param>
        /// <param name="workContext">Work context</param>
        /// <param name="storeContext">Store context</param>
        /// <param name="localizationSettings">Localization settings</param>
        /// <param name="commonSettings">Common settings</param>
        /// <param name="catalogSettings">Catalog settings</param>
        /// <param name="eventPublisher">Event published</param>
        /// <param name="aclService">ACL service</param>
        /// <param name="storeMappingService">Store mapping service</param>
        public ProductService(ICacheManager cacheManager,
            IRepository<Product> productRepository,
            //IRepository<RelatedProduct> relatedProductRepository,
            //IRepository<CrossSellProduct> crossSellProductRepository,
            //IRepository<TierPrice> tierPriceRepository,
            IRepository<ProductPicture> productPictureRepository,
            //IRepository<LocalizedProperty> localizedPropertyRepository,
            //IRepository<AclRecord> aclRepository,
            //IRepository<StoreMapping> storeMappingRepository,
            //IRepository<ProductSpecificationAttribute> productSpecificationAttributeRepository,
            IRepository<ProductReview> productReviewRepository,
            //IRepository<ProductWarehouseInventory> productWarehouseInventoryRepository,
            //IProductAttributeService productAttributeService,
            //IProductAttributeParser productAttributeParser,
            //ILanguageService languageService,
            IWorkflowMessageService workflowMessageService,
            IDataProvider dataProvider,
            IDbContext dbContext,
            IWorkContext workContext,
            //IStoreContext storeContext,
            //LocalizationSettings localizationSettings,
            CommonSettings commonSettings,
            CatalogSettings catalogSettings,
            IEventPublisher eventPublisher
            //IAclService aclService,
            //IStoreMappingService storeMappingService
            )
        {
            this._cacheManager = cacheManager;
            this._productRepository = productRepository;
            //this._relatedProductRepository = relatedProductRepository;
            //this._crossSellProductRepository = crossSellProductRepository;
            //this._tierPriceRepository = tierPriceRepository;
            this._productPictureRepository = productPictureRepository;
            //this._localizedPropertyRepository = localizedPropertyRepository;
            //this._aclRepository = aclRepository;
            //this._storeMappingRepository = storeMappingRepository;
            //this._productSpecificationAttributeRepository = productSpecificationAttributeRepository;
            this._productReviewRepository = productReviewRepository;
            //this._productWarehouseInventoryRepository = productWarehouseInventoryRepository;
            //this._productAttributeService = productAttributeService;
            //this._productAttributeParser = productAttributeParser;
            //this._languageService = languageService;
            this._workflowMessageService = workflowMessageService;
            this._dataProvider = dataProvider;
            this._dbContext = dbContext;
            this._workContext = workContext;
            //this._storeContext = storeContext;
            //this._localizationSettings = localizationSettings;
            this._commonSettings = commonSettings;
            this._catalogSettings = catalogSettings;
            this._eventPublisher = eventPublisher;
            //this._aclService = aclService;
            //this._storeMappingService = storeMappingService;
        }

        #endregion

        #region Products

        /// <summary>
        /// 商家内商品名称唯一
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public bool NameUniqueCheck(string productName, int storeId,int productId=0)
        {
            var query = _productRepository.Table;
            query = query.Where(p => p.Store.Id == storeId && !p.Deleted);

            if (!String.IsNullOrWhiteSpace(productName))
            {
                var product = query.Where(c => c.Name == productName).FirstOrDefault();
                if (product == null) return true;
                else
                    return product.Id == storeId;

            }
            else return true;       
        }

        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="product"></param>
        public void DeleteProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException("product");

            product.Deleted = true;
            foreach(var picture in product.ProductPictures)
            {
                picture.Deleted = true;
            }

            UpdateProduct(product);
        }

        public IPagedList<Product> GetAllProducts(int storeId = 0, string search = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false, params PropertySortCondition[] sortConditions)
        {
            var query = _productRepository.Table;

            query = query.Where(m => !m.Deleted);

            if (!showHidden) query = query.Where(s => s.Published);

            if (storeId > 0)
                query = query.Where(s => s.Store.Id == storeId);

            //实现查询
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(e => e.Name.Contains(search) || e.ProductCategory.Name.Contains(search));
            }

            if (sortConditions != null && sortConditions.Length != 0)
            {
                query = query.Sort(sortConditions);
            }
            else
            {
                query = query.Sort(new PropertySortCondition[1] {
                    new PropertySortCondition("DisplayOrder", System.ComponentModel.ListSortDirection.Ascending)
                });
            }

            var products = new PagedList<Product>(query, pageIndex, pageSize);
            return products;
        }


        public IList<Product> GetAssociatedProducts(int parentGroupedProductId, int storeId = 0, int vendorId = 0, bool showHidden = false)
        {
            throw new NotImplementedException();
        }

        public int GetCategoryProductNumber(IList<int> categoryIds = null, int storeId = 0)
        {
            throw new NotImplementedException();
        }

        public Product GetProductById(int productId)
        {
            var product = _productRepository.GetById(productId);
            return product;
        }

        public Product GetProductBySku(string sku)
        {
            throw new NotImplementedException();
        }

        public IList<Product> GetProductsByIds(int[] productIds)
        {
            throw new NotImplementedException();
        }

        public IPagedList<Product> GetProductsByProductAtributeId(int productAttributeId, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            throw new NotImplementedException();
        }

        public void InsertProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException("商品信息为空");

            //名称唯一性判断
            //if (!NameUniqueCheck(product.Name, product.Store.Id, product.Id))
            //    throw new ArgumentException(string.Format("商品名称 {0} 已经存在于商家 {1} 中", product.Name, product.Store.Name));


            //insert
            _productRepository.Insert(product);

            //clear cache
            _cacheManager.RemoveByPattern(PRODUCTS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(product);
        }

        public IPagedList<Product> SearchProducts(int pageIndex = 0, int pageSize = int.MaxValue, IList<int> categoryIds = null, int manufacturerId = 0, int storeId = 0, int vendorId = 0, int warehouseId = 0, ProductType? productType = default(ProductType?), bool visibleIndividuallyOnly = false, bool? featuredProducts = default(bool?), decimal? priceMin = default(decimal?), decimal? priceMax = default(decimal?), int productTagId = 0, string keywords = null, bool searchDescriptions = false, bool searchSku = true, bool searchProductTags = false, int languageId = 0, IList<int> filteredSpecs = null, bool showHidden = false, bool? overridePublished = default(bool?))
        {
            throw new NotImplementedException();
        }

        public IPagedList<Product> SearchProducts(out IList<int> filterableSpecificationAttributeOptionIds, bool loadFilterableSpecificationAttributeOptionIds = false, int pageIndex = 0, int pageSize = int.MaxValue, IList<int> categoryIds = null, int manufacturerId = 0, int storeId = 0, int vendorId = 0, int warehouseId = 0, ProductType? productType = default(ProductType?), bool visibleIndividuallyOnly = false, bool? featuredProducts = default(bool?), decimal? priceMin = default(decimal?), decimal? priceMax = default(decimal?), int productTagId = 0, string keywords = null, bool searchDescriptions = false, bool searchSku = true, bool searchProductTags = false, int languageId = 0, IList<int> filteredSpecs = null, bool showHidden = false, bool? overridePublished = default(bool?))
        {
            throw new NotImplementedException();
        }

        public void UpdateHasDiscountsApplied(Product product)
        {
            throw new NotImplementedException();
        }

        public void UpdateHasTierPricesProperty(Product product)
        {
            throw new NotImplementedException();
        }

        public void UpdateProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException("product");

            _productRepository.Update(product);

            //event notification
            _eventPublisher.EntityUpdated(product);
        }

        public void UpdateProductReviewTotals(Product product)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Product pictures

        /// <summary>
        /// Deletes a product picture
        /// </summary>
        /// <param name="productPicture">Product picture</param>
        public virtual void DeleteProductPicture(ProductPicture productPicture)
        {
            if (productPicture == null)
                throw new ArgumentNullException("productPicture");

            _productPictureRepository.Delete(productPicture);

            //event notification
            _eventPublisher.EntityDeleted(productPicture);
        }

        /// <summary>
        /// Gets a product pictures by product identifier
        /// </summary>
        /// <param name="productId">The product identifier</param>
        /// <returns>Product pictures</returns>
        public virtual IList<ProductPicture> GetProductPicturesByProductId(int productId)
        {
            var query = from pp in _productPictureRepository.Table
                        where pp.ProductId == productId
                        orderby pp.DisplayOrder
                        select pp;
            var productPictures = query.ToList();
            return productPictures;
        }

        /// <summary>
        /// Gets a product picture
        /// </summary>
        /// <param name="productPictureId">Product picture identifier</param>
        /// <returns>Product picture</returns>
        public virtual ProductPicture GetProductPictureById(int productPictureId)
        {
            if (productPictureId == 0)
                return null;

            return _productPictureRepository.GetById(productPictureId);
        }

        /// <summary>
        /// Inserts a product picture
        /// </summary>
        /// <param name="productPicture">Product picture</param>
        public virtual void InsertProductPicture(ProductPicture productPicture)
        {
            if (productPicture == null)
                throw new ArgumentNullException("productPicture");

            _productPictureRepository.Insert(productPicture);

            //event notification
            _eventPublisher.EntityInserted(productPicture);
        }

        /// <summary>
        /// Updates a product picture
        /// </summary>
        /// <param name="productPicture">Product picture</param>
        public virtual void UpdateProductPicture(ProductPicture productPicture)
        {
            if (productPicture == null)
                throw new ArgumentNullException("productPicture");

            _productPictureRepository.Update(productPicture);

            //event notification
            _eventPublisher.EntityUpdated(productPicture);
        }

        #endregion
    }
}
