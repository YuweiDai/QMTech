using AutoMapper;
using QMTech.Api.Models.Catalog;
using QMTech.Core.Domain.Catalog;

namespace QMTech.Web.Api.Extensions
{
    /// <summary>
    /// 实体到模型映射
    /// </summary>
    public static class MappingExtensions
    {
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            return Mapper.Map<TSource, TDestination>(source);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return Mapper.Map(source, destination);
        }



        #region catalog

        public static StoreCategoryModel ToModel(this StoreCategory entity)
        {
            return entity.MapTo<StoreCategory, StoreCategoryModel>();
        }

        public static StoreCategory ToEntity(this StoreCategoryModel model)
        {
            return model.MapTo<StoreCategoryModel, StoreCategory>();
        }

        public static StoreCategory ToEntity(this StoreCategoryModel model, StoreCategory destination)
        {
            return model.MapTo(destination);
        }

        public static StoreModel ToModel(this Store entity)
        {
            return entity.MapTo<Store, StoreModel>();
        }

        public static Store ToEntity(this StoreModel model)
        {
            return model.MapTo<StoreModel, Store>();
        }

        public static Store ToEntity(this StoreModel model, Store destination)
        {
            return model.MapTo(destination);
        }

        public static ProductCategoryModel ToModel(this ProductCategory entity)
        {
            return entity.MapTo<ProductCategory, ProductCategoryModel>();
        }

        public static ProductCategory ToEntity(this ProductCategoryModel model)
        {
            return model.MapTo<ProductCategoryModel, ProductCategory>();
        }

        public static ProductCategory ToEntity(this ProductCategoryModel model, ProductCategory destination)
        {
            return model.MapTo(destination);
        }

        public static ProductTagModel ToModel(this ProductTag entity)
        {
            return entity.MapTo<ProductTag, ProductTagModel>();
        }

        public static ProductTag ToEntity(this ProductTagModel model)
        {
            return model.MapTo<ProductTagModel, ProductTag>();
        }

        public static ProductTag ToEntity(this ProductTagModel model, ProductTag destination)
        {
            return model.MapTo(destination);
        }

        public static ProductModel ToModel(this Product entity)
        {
            return entity.MapTo<Product, ProductModel>();
        }

        public static Product ToEntity(this ProductModel model)
        {
            return model.MapTo<ProductModel, Product>();
        }

        public static Product ToEntity(this ProductModel model, Product destination)
        {
            return model.MapTo(destination);
        }

        public static ProductPictureModel ToModel(this ProductPicture entity)
        {
            return entity.MapTo<ProductPicture, ProductPictureModel>();
        }

        public static ProductPicture ToEntity(this ProductPictureModel model)
        {
            return model.MapTo<ProductPictureModel, ProductPicture>();
        }

        public static ProductPicture ToEntity(this ProductPictureModel model, ProductPicture destination)
        {
            return model.MapTo(destination);
        }
        #endregion
    }
}