using QMTech.Core;
using QMTech.Core.Domain.Catalog;
using System.Collections.Generic;

namespace QMTech.Services.Catalog
{
    public interface IStoreService
    {
        void DeleteStore(Store store);

        IPagedList<Store> GetAllStores(string search = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false, params PropertySortCondition[] sortConditions);

        Store GetStoreById(int storeId);

        /// <summary>
        /// 名称是否唯一
        /// </summary>
        /// <param name="storeName"></param>
        /// <returns>true表示唯一</returns>
        bool NameUniqueCheck(string storeName, int storeId = 0);

        void InsertStore(Store store);

        void UpdateStore(Store store);


        #region Product pictures

        /// <summary>
        /// Deletes a product picture
        /// </summary>
        /// <param name="StorePicture">Product picture</param>
        void DeleteStorePicture(StorePicture StorePicture);

        /// <summary>
        /// Gets a product pictures by product identifier
        /// </summary>
        /// <param name="storeId">The product identifier</param>
        /// <returns>Product pictures</returns>
        IList<StorePicture> GetStorePicturesByProductId(int storeId);

        /// <summary>
        /// Gets a product picture
        /// </summary>
        /// <param name="storePictureId">Product picture identifier</param>
        /// <returns>Product picture</returns>
        StorePicture GetStorePictureById(int storePictureId);

        /// <summary>
        /// Inserts a product picture
        /// </summary>
        /// <param name="StorePicture">Product picture</param>
        void InsertStorePicture(StorePicture StorePicture);

        /// <summary>
        /// Updates a product picture
        /// </summary>
        /// <param name="StorePicture">Product picture</param>
        void UpdateStorePicture(StorePicture StorePicture);

        #endregion
    }
}
