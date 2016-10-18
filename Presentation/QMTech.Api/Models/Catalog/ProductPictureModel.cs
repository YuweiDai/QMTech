using FluentValidation.Attributes;
using QMTech.Api.Validators.Catalog;
using QMTech.Web.Framework.Mvc;

namespace QMTech.Api.Models.Catalog
{
    //[Validator(typeof(ProductPictureValidator))]
    public class ProductPictureModel: BaseQMEntityModel
    {
        public int PictureId { get; set; }

        public int ProductId { get; set; }

        public int DisplayOrder { get; set; }

        /// <summary>
        /// 图片SEO名称
        /// </summary>
        public string SeoFilename { get; set; }

        /// <summary>
        /// Gets or sets the "alt" attribute for "img" HTML element. If empty, then a default rule will be used (e.g. product name)
        /// </summary>
        public string Alt { get; set; }

        /// <summary>
        /// Gets or sets the "title" attribute for "img" HTML element. If empty, then a default rule will be used (e.g. product name)
        /// </summary>
        public string Title { get; set; }

        public string Src { get; set; }        
    }
}