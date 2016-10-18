using QMTech.Core.Caching;
using QMTech.Core.Data;
using QMTech.Core.Domain.Catalog;
using QMTech.Core.Domain.Common;
using QMTech.Data;
using QMTech.Services.Events;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMTech.Services.Catalog
{
    public class ProductTagService:IProductTagService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : store ID
        /// </remarks>
        private const string TAG_COUNT_KEY = "Qm.productTag.count-{0}";

        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string TAG_PATTERN_KEY = "Qm.productTag.";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : store ID
        /// </remarks>
        private const string PRODUCTTAG_COUNT_KEY = "Qm.producttag.count-{0}";

        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string PRODUCTTAG_PATTERN_KEY = "Qm.producttag.";

        #endregion

        #region Fields

        private readonly IRepository<ProductTag> _tagRepository;
        private readonly IRepository<ProductTag> _productTagRepository;
        private readonly IDataProvider _dataProvider;
        private readonly IDbContext _dbContext;
        private readonly CommonSettings _commonSettings;
        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="productTagRepository">Product productTag repository</param>
        /// <param name="dataProvider">Data provider</param>
        /// <param name="dbContext">Database Context</param>
        /// <param name="commonSettings">Common settings</param>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="eventPublisher">Event published</param>
        public ProductTagService(
            IRepository<ProductTag> tagRepository,
            IRepository<ProductTag> productTagRepository,
            IDataProvider dataProvider,
            IDbContext dbContext,
            CommonSettings commonSettings,
            ICacheManager cacheManager,
            IEventPublisher eventPublisher)
        {
            this._tagRepository = tagRepository;
            this._productTagRepository = productTagRepository;
            this._dataProvider = dataProvider;
            this._dbContext = dbContext;
            this._commonSettings = commonSettings;
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Nested classes

        private class ProductTagWithCount
        {
            public int ProductTagId { get; set; }
            public int ProductCount { get; set; }
        }

        #endregion         

        #region ProductTag Methods

        /// <summary>
        /// Delete a product productTag
        /// </summary>
        /// <param name="productTag">Product productTag</param>
        public virtual void DeleteProductTag(ProductTag productTag)
        {
            if (productTag == null)
                throw new ArgumentNullException("productTag");

            _tagRepository.Delete(productTag);

            //cache
            _cacheManager.RemoveByPattern(TAG_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(productTag);
        }

        /// <summary>
        /// Gets all  ProductTags
        /// </summary>
        /// <returns> ProductTags</returns>
        public virtual IList<ProductTag> GetAllProductTags( string search="")
        {
            var query = from productTag in _tagRepository.Table
                        where !productTag.Deleted
                        select productTag;

            if (!string.IsNullOrEmpty(search))
                query = query.Where(t => t.Name.Contains(search));

            return query.ToList();
        }

        /// <summary>
        /// Gets  productTag
        /// </summary>
        /// <param name="TagId"> productTag identifier</param>
        /// <returns> productTag</returns>
        public virtual ProductTag GetProductTagById(int TagId)
        {
            if (TagId == 0)
                return null;

            return _tagRepository.GetById(TagId);
        }

        /// <summary>
        /// Gets  productTag by name
        /// </summary>
        /// <param name="name"> productTag name</param>
        /// <returns> productTag</returns>
        public virtual ProductTag GetProductTagByName(string name)
        {
            var query = from pt in _tagRepository.Table
                        where pt.Name == name
                        select pt;

            var ProductTag = query.FirstOrDefault();
            return ProductTag;
        }

        /// <summary>
        /// Inserts a  productTag
        /// </summary>
        /// <param name="productTag"> productTag</param>
        public virtual void InsertProductTag(ProductTag ProductTag)
        {
            if (ProductTag == null)
                throw new ArgumentNullException("productTag");

            _tagRepository.Insert(ProductTag);

            //cache
            _cacheManager.RemoveByPattern(TAG_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(ProductTag);
        }

        /// <summary>
        /// Updates the  productTag
        /// </summary>
        /// <param name="productTag"> productTag</param>
        public virtual void UpdateProductTag(ProductTag ProductTag)
        {
            if (ProductTag == null)
                throw new ArgumentNullException("productTag");

            _tagRepository.Update(ProductTag);

            //cache
            _cacheManager.RemoveByPattern(TAG_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(ProductTag);
        }


        /// <summary>
        /// 获取被标签的商品数目
        /// </summary>
        /// <param name="productTagId">Product productTag identifier</param>
        /// <returns>Number of products</returns>
        public virtual int GetProductTagedProductCount(int tagId)
        {
            var productTag = GetProductTagById(tagId);

            return productTag.Products.Count();
        }

        #endregion

         
    }
}
