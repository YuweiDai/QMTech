using FluentValidation.Attributes;
using QMTech.Api.Validators.Catalog;
using QMTech.Web.Framework.Mvc;
using System;

namespace QMTech.Api.Models.Catalog
{
    //[Validator(typeof(StoreValidator))]
    public class StoreModel : BaseQMEntityModel
    {
        #region 基本信息
        /// <summary>
        /// Gets or sets the store name
        /// </summary>
        public string Name { get; set; }

        public int StoreCategoryId { get; set; }

        /// <summary>
        /// 商家类别名称
        /// </summary>
        public string StoreCategory { get; set; }

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

        /// <summary>
        /// 图片上传 base64
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        /// 图片Url
        /// </summary>
        public string LogoUrl { get; set; }

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

        /// <summary>
        /// 是否为自营
        /// </summary>
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
        public string StartTime { get; set; }

        /// <summary>
        /// 营业停止之间
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// 是否发布
        /// </summary>
        public bool Published { get; set; }

        /// <summary>
        /// 是否停业
        /// </summary>
        public bool Opend { get; set; }

        #endregion

        #region 统计信息

        public int VistedCount { get; set; }

        public int ProductsCount { get; set; }

        public int OrdersCount { get; set; }

        public int FavoritesCount { get; set; }

        #endregion

        #region Nested Class 相关的内部类

        #endregion
    }
}