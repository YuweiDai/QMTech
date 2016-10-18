using FluentValidation.Attributes;
using QMTech.Api.Validators.Catalog;
using QMTech.Web.Framework.Mvc;

namespace QMTech.Api.Models.Catalog
{
    //[Validator(typeof(ProductTagValidator))]
    public class ProductTagModel : BaseQMEntityModel
    {
        public ProductTagModel()
        {

        }

        public string Name { get; set; }

        //public int ProductId { get; set; }
    }
}