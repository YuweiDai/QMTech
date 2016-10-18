using FluentValidation;
using QMTech.Api.Models.Catalog;
using QMTech.Services.Catalog;
using QMTech.Web.Framework.Validators;

namespace QMTech.Api.Validators.Catalog
{

    public class ProductCategoryValidator : BaseQMValidator<ProductCategoryModel>
    {
        private readonly ICategoryService _productCategoryService = null;

        public ProductCategoryValidator(ICategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;

            this.RuleFor(pc => pc.Name).NotEmpty().WithMessage("名称不能为空").WithMessage("商品类别名称 {0} 已经存在", pc => pc.Name)
                .Length(1, 400).WithMessage("名称不能超过200个汉字");
        }

        /// <summary>
        /// 名称唯一性验证
        /// </summary>
        /// <param name="productCategoryModel"></param>
        /// <param name="productCategoryName"></param>
        /// <returns></returns>
        private bool BeUniqueName(ProductCategoryModel productCategoryModel, string productCategoryName)
        {
            return _productCategoryService.ProductCategoryNameUniqueCheck(productCategoryModel.StoreId, productCategoryName, productCategoryModel.Id);
        }
    }
}