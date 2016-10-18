using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMTech.Core.Domain.Catalog
{
    public class Store : BaseEntity
    {
        private ICollection<StorePicture> _storePictures;
        private ICollection<StoreTag> _storeTags;
        private ICollection<Product> _products;

        #region 基本信息
        /// <summary>
        /// Gets or sets the store name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the company address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public float Lon { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public float Lat { get; set; }

        /// <summary>
        /// 商店内联系方式
        /// </summary>
        public string Tel { get; set; } 

        /// <summary>
        /// 描述信息
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 公告
        /// </summary>
        public string Noticement { get; set; }

        /// <summary>
        /// 评分
        /// </summary>
        public float Stars { get; set; }

        #endregion

        #region SEO
        /// <summary>
        /// Gets or sets the meta keywords
        /// </summary>
        public string MetaKeywords { get; set; }
        /// <summary>
        /// Gets or sets the meta description
        /// </summary>
        public string MetaDescription { get; set; }
        /// <summary>
        /// Gets or sets the meta title
        /// </summary>
        public string MetaTitle { get; set; }
        #endregion

        #region 运营信息
        public bool SelfSupport { get; set; }

        public string Person { get; set; }

        public string PersonTel { get; set; }

        public string SalesNumber { get; set; }


        /// <summary>
        /// 是否为24小时营业
        /// </summary>
        public bool AllOpened { get; set; }

        /// <summary>
        /// 营业开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 营业停止之间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 是否发布
        /// </summary>
        public bool Published { get; set; }

        /// <summary>
        /// 是否停业
        /// </summary>
        public bool Closed { get; set; } 
        #endregion

        public virtual StoreCategory StoreCategory { get; set; }

        /// <summary>
        /// Gets or sets the collection of ProductPicture
        /// </summary>
        public virtual ICollection<StorePicture> StorePictures
        {
            get { return _storePictures ?? (_storePictures = new List<StorePicture>()); }
            protected set { _storePictures = value; }
        }

        /// <summary>
        /// Gets or sets the product tags
        /// </summary>
        public virtual ICollection<StoreTag> StoreTags
        {
            get { return _storeTags ?? (_storeTags = new List<StoreTag>()); }
            protected set { _storeTags = value; }
        }

        /// <summary>
        /// 商家内的商品
        /// </summary>
        public virtual ICollection<Product> Products
        {
            get { return _products ?? (_products = new List<Product>()); }
            protected set { _products = value; }
        }
    }
}
