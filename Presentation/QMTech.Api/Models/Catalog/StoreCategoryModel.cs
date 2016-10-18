using FluentValidation.Attributes;
using QMTech.Api.Validators.Catalog;
using QMTech.Web.Framework.Mvc;

namespace QMTech.Api.Models.Catalog
{
    //[Validator(typeof(StoreCategoryValidator))]
    public class StoreCategoryModel : BaseQMEntityModel
    {
        public string Name { get; set; }

        public int ParentId { get; set; }

        public int DisplayOrder { get; set; }

        public string Description { get; set; }

        public bool Published { get; set; }        
    }
}