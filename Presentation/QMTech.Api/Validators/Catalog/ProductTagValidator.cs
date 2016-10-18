using FluentValidation;
using QMTech.Api.Models.Catalog;
using QMTech.Web.Framework.Validators;

namespace QMTech.Api.Validators.Catalog
{
    public class ProductTagValidator : BaseQMValidator<ProductTagModel>
    {
        public ProductTagValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("不能为空");
        }
    }
}