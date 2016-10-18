using System;
using System.Linq;
using QMTech.Core.Infrastructure;
using AutoMapper;
using QMTech.Api.Models.Catalog;
using QMTech.Core.Domain.Catalog;

namespace QMTech.Web.Api.Infrastructure
{
    public class AutoMapperStartupTask : IStartupTask
    {
        public int Order
        {
            get { return 0; }
        }

        public void Execute()
        {

            #region category
            //store
            Mapper.CreateMap<StoreModel, Store>()
              .ForMember(dest => dest.StartTime, mo => mo.MapFrom(src => string.IsNullOrEmpty(src.StartTime) ? Convert.ToDateTime("00:00:00") : Convert.ToDateTime(src.StartTime)))
              .ForMember(dest => dest.EndTime, mo => mo.MapFrom(src => string.IsNullOrEmpty(src.EndTime) ? Convert.ToDateTime("23:59:59") : Convert.ToDateTime(src.EndTime)));

            Mapper.CreateMap<Store, StoreModel>()
              .ForMember(dest => dest.Name, mo => mo.MapFrom(src => src.Published ? src.Name : (src.Name + "（未发布）")))
              .ForMember(dest => dest.StartTime, mo => mo.MapFrom(src => src.StartTime.HasValue ? src.StartTime.Value.ToString("HH:mm") : ""))
              .ForMember(dest => dest.EndTime, mo => mo.MapFrom(src => src.EndTime.HasValue ? src.EndTime.Value.ToString("HH:mm") : ""))
              .ForMember(dest => dest.ProductsCount, mo => mo.MapFrom(src => src.Products.Count));


            Mapper.CreateMap<StoreCategoryModel, StoreCategory>();

            Mapper.CreateMap<StoreCategory, StoreCategoryModel>();

            Mapper.CreateMap<ProductCategoryModel, ProductCategory>();

            Mapper.CreateMap<ProductCategory, ProductCategoryModel>();


            Mapper.CreateMap<ProductTagModel, ProductTag>();
            Mapper.CreateMap<ProductTag, ProductTagModel>();

            //product
            Mapper.CreateMap<ProductModel, Product>()
            .ForMember(dest => dest.ProductTags, mo => mo.Ignore())    //忽略模型转换，手动更新，避免EF报错
            .ForMember(dest => dest.ProductPictures, mo => mo.Ignore());

            Mapper.CreateMap<Product, ProductModel>()
              //.ForMember(dest => dest.ProductPictures, mo => mo.Ignore())    //忽略模型转换，手动更新，避免EF报错
              .ForMember(dest=>dest.ProductCategory,mo=>mo.MapFrom(src=>src.ProductCategory.Name))
              .ForMember(dest => dest.AvailableDateRange, mo => mo.MapFrom(src => src.AvailableStartDateTime.HasValue && src.AvailableEndDateTime.HasValue ?
              string.Format("{0} - {1}", src.AvailableStartDateTime.Value.ToString("yyyy年MM月dd日"), src.AvailableEndDateTime.Value.ToString("yyyy年MM月dd日")) : ""))
              .ForMember(dest => dest.SpecialDateRange, mo => mo.MapFrom(src => src.SpecialPriceStartDateTime.HasValue && src.SpecialPriceEndDateTime.HasValue ?
              string.Format("{0} - {1}", src.SpecialPriceStartDateTime.Value.ToString("yyyy年MM月dd日 HH:mm"), src.SpecialPriceEndDateTime.Value.ToString("yyyy年MM月dd日 HH:mm")) : ""));

            Mapper.CreateMap<ProductPictureModel, ProductPicture>();
              //.ForMember(dest => dest.Picture.SeoFilename, mo => mo.MapFrom(src => src.SeoFilename))
              //.ForMember(dest => dest.Picture.AltAttribute, mo => mo.MapFrom(src => src.AltAttribute))
              //.ForMember(dest => dest.Picture.TitleAttribute, mo => mo.MapFrom(src => src.TitleAttribute));
            Mapper.CreateMap<ProductPicture, ProductPictureModel>()
              .ForMember(dest => dest.SeoFilename, mo => mo.MapFrom(src => src.Picture.SeoFilename))
              .ForMember(dest => dest.Alt, mo => mo.MapFrom(src => src.Picture.AltAttribute))
              .ForMember(dest => dest.Title, mo => mo.MapFrom(src => src.Picture.TitleAttribute));

            #endregion
        }
    }
}