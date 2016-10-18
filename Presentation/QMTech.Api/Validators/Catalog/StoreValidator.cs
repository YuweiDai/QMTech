using FluentValidation;
using QMTech.Api.Models.Catalog;
using QMTech.Core.Domain.Common;
using QMTech.Services.Catalog;
using QMTech.Web.Framework.Validators;
using System;
using System.Text.RegularExpressions;

namespace QMTech.Api.Validators.Catalog
{
    public class StoreValidator: BaseQMValidator<StoreModel>
    {
        private readonly IStoreService _storeService = null;


        public StoreValidator(IStoreService storeService, 
            ICategoryService categoryService,
            CommonSettings commonSettings)
        {
            _storeService = storeService;

            RuleFor(s => s.Name).NotEmpty().WithMessage("名称不能为空").Must(BeUniqueName).WithMessage("名称 {0} 已存在", s => s.Name);

            RuleFor(s => s.Address).NotEmpty().WithMessage("地址不能为空").Length(2, 60).WithMessage("店铺地址字段长度为2-30个汉字");
            RuleFor(s => s.StoreCategoryId).Must(categoryId => {
                var storeCategory = categoryService.GetStoreCategoryById(categoryId);
                return storeCategory != null && !storeCategory.Deleted;
            }).WithMessage(string.Format("类别不存在"));

            RuleFor(s => s.Logo).Must(delegate (StoreModel storeModel, string logo) {
                if (string.IsNullOrEmpty(logo))
                    return !string.IsNullOrEmpty(storeModel.LogoUrl);
                return true;
            }).WithMessage("商家Logo不能为空");  

            RuleFor(s => s.Tel).NotEmpty().WithMessage("商家联系方式不能为空").Matches(commonSettings.TelAndMobliePartten).WithMessage("店铺联系方式不是正确的电话号码格式");
            //RuleFor(s => s.Description).NotEmpty().WithMessage("描述不能为空").Length(2, 300).WithMessage("描述字段长度不超过150个汉字");

            //后续 再调整
            //RuleFor(s => s.Person).Must(delegate (StoreModel storeModel,string person){
            //    if (!storeModel.SelfSupport)
            //    {
            //        return !string.IsNullOrWhiteSpace(person);
            //    }
            //    else return true;
            //}).WithMessage("非自营商家，运营人不嫩为空");

            //RuleFor(s => s.PersonTel).Must(delegate (StoreModel storeModel, string personTel) {
            //    if (!storeModel.SelfSupport)
            //    {
            //        return System.Text.RegularExpressions.Regex.IsMatch(personTel, commonSettings.TelAndMobliePartten);
            //    }
            //    else return true;
            //}).WithMessage("运营人联系方式不是正确的电话号码格式");

            RuleFor(s => s.StartTime).Must(delegate (StoreModel storeModel, string startTime) {
                if (!storeModel.AllOpened)
                {
                    return Regex.IsMatch(startTime, commonSettings.Time24Partten) && startTime.Length <= 5;
                }
                else return true;
            }).WithMessage("不是一个正确的24小时制时间");

            RuleFor(s => s.EndTime).Must(delegate (StoreModel storeModel, string endTime) {
                if (!storeModel.AllOpened)
                {
                    return Regex.IsMatch(endTime, commonSettings.Time24Partten) && endTime.Length <= 5;
                }
                else return true;
            }).WithMessage("不是一个正确的24小时制时间");

            RuleFor(s=>s.Lon).Must(delegate (StoreModel storeModel, float lon) {
                return Math.Abs(lon) > 0 && Math.Abs(lon) < 130;
            }).WithMessage("空间数据不正确");

            RuleFor(s => s.Lat).Must(delegate (StoreModel storeModel, float lat) {
                return Math.Abs(lat) > 0 && Math.Abs(lat) < 40;
            }).WithMessage("空间数据不正确");

            //RuleFor(s => s.MetaTitle).NotEmpty().WithMessage("SEO标题不能为空");
            //RuleFor(s => s.MetaKeywords).NotEmpty().WithMessage("SEO关键字不能为空");
            //RuleFor(s => s.Description).NotEmpty().WithMessage("商家描述不能为空");      

            Custom(storeModel =>
            {
                return null;
            });
        }

        /// <summary>
        /// 名称唯一性验证
        /// </summary>
        /// <param name="storeModel"></param>
        /// <param name="storeName"></param>
        /// <returns></returns>
        private bool BeUniqueName(StoreModel storeModel, string storeName)
        {
            return _storeService.NameUniqueCheck(storeName, storeModel.Id);
        }
    }
}