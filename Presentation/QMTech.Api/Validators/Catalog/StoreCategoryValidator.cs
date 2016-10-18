using FluentValidation;
using QMTech.Api.Models.Catalog;
using QMTech.Services.Catalog;
using QMTech.Web.Framework.Validators;

namespace QMTech.Api.Validators.Catalog
{
    public class StoreCategoryValidator : BaseQMValidator<StoreCategoryModel>
    {
        private readonly ICategoryService _categoryService;

        public StoreCategoryValidator(ICategoryService categoryService)
        {
            _categoryService = categoryService;

            this.RuleFor(sc => sc.Name).NotEmpty().WithMessage("名称不能为空").Must(BeUniqueName).WithMessage("名称 {0} 已存在", s => s.Name)
                .Length(1, 400).WithMessage("名称不能超过200个汉字");
        }


        /// <summary>
        /// 名称唯一性验证
        /// </summary>
        /// <param name="storeCategoryModel"></param>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        private bool BeUniqueName(StoreCategoryModel storeCategoryModel, string categoryName)
        {
            return _categoryService.StoreCategoryNameUniqueCheck(categoryName, storeCategoryModel.Id);
        }
    }
}