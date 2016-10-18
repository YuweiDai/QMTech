using System.Collections.Generic;

namespace QMTech.Core.Domain.Catalog
{
    /// <summary>
    /// 商品类别
    /// </summary>
    public partial class ProductCategory : BaseEntity
    {
        private ICollection<Product> _products;

        /// <summary>
        /// 类别名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类别描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 元数据关键字
        /// </summary>
        public string MetaKeywords { get; set; }

        /// <summary>
        /// 元数据描述
        /// </summary>
        public string MetaDescription { get; set; }

        /// <summary>
        /// 元数据标题
        /// </summary>
        public string MetaTitle { get; set; }

        /// <summary>
        /// 是否有优惠
        /// </summary>
        public bool HasDiscountsApplied { get; set; }

        /// <summary>
        /// 是否发布
        /// </summary>
        public bool Published { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 所属的商家
        /// </summary>
        public int  StoreId { get; set; }

        /// <summary>
        /// 关联的商品
        /// </summary>
        public virtual ICollection<Product> Products
        {
            get { return _products ?? (_products = new List<Product>()); }
            protected set { _products = value; }
        }

    }
}