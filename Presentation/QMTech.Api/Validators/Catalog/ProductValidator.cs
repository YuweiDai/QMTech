using FluentValidation;
using FluentValidation.Results;
using QMTech.Api.Models.Catalog;
using QMTech.Services.Catalog;
using QMTech.Web.Framework.Validators;

namespace QMTech.Api.Validators.Catalog
{
    public class ProductValidator : BaseQMValidator<ProductModel>
    {
        private readonly IProductService _productService = null;

        public ProductValidator(IProductService productService, ICategoryService categoryService, IStoreService storeService)
        {
            _productService = productService;

            RuleFor(p => p.Name).NotEmpty().WithMessage("商品名称不能为空").Must(BeUniqueName).WithMessage("商品名称 {0} 已经存在", p => p.Name);

            RuleFor(p => p.StoreId).Must(storeId =>
            {
                var store = storeService.GetStoreById(storeId);
                return store != null && !store.Deleted;
            }).WithMessage(string.Format("商家不存在"));

            RuleFor(p => p.ProductCategoryId).Must(categoryId =>
            {
                var productCategory = categoryService.GetStoreCategoryById(categoryId);
                return productCategory != null && !productCategory.Deleted;
            }).WithMessage(string.Format("类别不存在"));

            RuleFor(p => p.SpecialDateRange).Must(delegate (ProductModel productModel, string specialDateRange)
            {

                if (productModel.SpecialPrice.HasValue && productModel.SpecialPrice.Value > 0)
                {
                    return !string.IsNullOrWhiteSpace(productModel.SpecialDateRange);
                }

                return true;
            }).WithMessage("特价起止日期不能为空");

            Custom(p =>
            {
                return null;
            });
        }


        /// <summary>
        /// 名称唯一性验证
        /// </summary>
        /// <param name="productModel"></param>
        /// <param name="productName"></param>
        /// <returns></returns>
        private bool BeUniqueName(ProductModel productModel, string productName)
        {
            return _productService.NameUniqueCheck(productName,productModel.StoreId, productModel.Id);
        }
    }
}