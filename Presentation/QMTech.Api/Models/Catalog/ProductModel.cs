using FluentValidation.Attributes;
using QMTech.Api.Models.Media;
using QMTech.Api.Validators.Catalog;
using QMTech.Web.Framework.Mvc;
using System;
using System.Collections.Generic;

namespace QMTech.Api.Models.Catalog
{
    //[Validator(typeof(ProductValidator))]
    public class ProductModel : BaseQMEntityModel
    {

        public ProductModel()
        {
            //ProductTags = new List<ProductTagModel>();
        }

        #region 组合商品属性
        /// <summary>
        /// Gets or sets the product type identifier
        /// </summary>
        public int ProductTypeId { get; set; }

        /// <summary>
        /// Gets or sets the parent product identifier. It's used to identify associated products (only with "grouped" products)
        /// </summary>
        public int ParentGroupedProductId { get; set; }

        /// <summary>
        /// Gets or sets the values indicating whether this product is visible in catalog or search results.
        /// It's used when this product is associated to some "grouped" one
        /// This way associated products could be accessed/added/etc only from a grouped product details page
        /// </summary>
        public bool VisibleIndividually { get; set; }
        #endregion

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        public string ProductCategory { get; set; }

        /// <summary>
        /// Gets or sets the short description
        /// </summary>
        public string ShortDescription { get; set; }
        /// <summary>
        /// Gets or sets the full description
        /// </summary>
        public string FullDescription { get; set; }

        /// <summary>
        /// Gets or sets the admin comment
        /// </summary>
        public string AdminComment { get; set; }

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

        public bool AllowCustomerReviews { get; set; }

        /// <summary>
        /// Gets or sets the rating sum (approved reviews)
        /// </summary>
        public int ApprovedRatingSum { get; set; }

        /// <summary>
        /// Gets or sets the rating sum (not approved reviews)
        /// </summary>
        public int NotApprovedRatingSum { get; set; }

        public int ApprovedTotalReviews { get; set; }

        public int NotApprovedTotalReviews { get; set; }

        /// <summary>
        /// Gets or sets the stock quantity
        /// </summary>
        public int StockQuantity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to display stock availability
        /// </summary>
        public bool DisplayStockAvailability { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to display stock quantity
        /// </summary>
        public bool DisplayStockQuantity { get; set; }

        /// <summary>
        /// Gets or sets the minimum stock quantity
        /// </summary>
        public int MinStockQuantity { get; set; }

        /// <summary>
        /// Gets or sets the low stock activity identifier
        /// </summary>
        public int LowStockActivityId { get; set; }

        /// <summary>
        /// Gets or sets the quantity when admin should be notified
        /// </summary>
        public int NotifyAdminForQuantityBelow { get; set; }

        /// <summary>
        /// 禁用购买
        /// </summary>
        public bool DisableBuyButton { get; set; }

        /// <summary>
        /// Gets or sets the price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the old price
        /// </summary>
        public decimal OldPrice { get; set; }

        /// <summary>
        /// 成本价
        /// </summary>
        public decimal ProductCost { get; set; }

        /// <summary>
        /// 特价
        /// </summary>
        public decimal? SpecialPrice { get; set; }

        public string SpecialDateRange { get; set; }

        public bool HasDiscountsApplied { get; set; }

        public string AvailableDateRange { get; set; }

        public int DisplayOrder { get; set; }
        
        public bool Published { get; set; }

        /// <summary>
        /// 类别Id
        /// </summary>
        public int ProductCategoryId { get; set; }

        /// <summary>
        /// 商家Id
        /// </summary>
        public int StoreId { get; set; }


        public virtual ICollection<ProductTagModel> ProductTags { get; set; }


        public virtual ICollection<ProductPictureModel> ProductPictures { get; set; }


    }
}