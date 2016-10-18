using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QMTech.Core;
using QMTech.Core.Domain.Catalog;
using QMTech.Core.Data;
using QMTech.Services.Events;
using QMTech.Core.Caching;
using QMTech.Data;

namespace QMTech.Services.Catalog
{
    public class StoreService : IStoreService
    {
        private const string STORES_BY_ID_KEY = "QMTech.store.id-{0}";

        private const string STORES_PATTERN_KEY = "QMTech.store.";

        private readonly IRepository<Store> _storeRepository;
        private readonly IRepository<StorePicture> _storePictureRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;

        public StoreService(ICacheManager cacheManager,IRepository<Store> storeRepository,
             IRepository<StorePicture> storePictureRepository,
             IEventPublisher eventPublisher)
        {

            this._cacheManager = cacheManager;

            _storePictureRepository = storePictureRepository;
            _storeRepository = storeRepository;

            this._eventPublisher = eventPublisher;
        }

        /// <summary>
        /// 删除店铺
        /// </summary>
        /// <param name="store"></param>
        public void DeleteStore(Store store)
        {
            if (store == null)
                throw new ArgumentNullException("store");

            store.Deleted = true;
            UpdateStore(store);
        }

        /// <summary>
        /// 获取商家列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="showHidden"></param>
        /// <param name="sortConditions"></param>
        /// <returns></returns>
        public IPagedList<Store> GetAllStores(string search = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false, params PropertySortCondition[] sortConditions)
        {
            var query = _storeRepository.Table;

            query = query.Where(m => !m.Deleted);

            if (!showHidden) query = query.Where(s => s.Published);

            //实现查询
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(e => e.Name.Contains(search) || e.Address.Contains(search) || e.Tel.Contains(search) || e.StoreCategory.Name.Contains(search));
            }

            if (sortConditions != null && sortConditions.Length != 0)
            {
                query = query.Sort(sortConditions);
            }
            else
            {
                query = query.Sort(new PropertySortCondition[1] {
                    new PropertySortCondition("DisplayOrder", System.ComponentModel.ListSortDirection.Ascending)
                });
            }

            var stores = new PagedList<Store>(query, pageIndex, pageSize);
            return stores;
        }

        public Store GetStoreById(int storeId)
        {
            if (storeId == 0) return null;
             
            string key = string.Format(STORES_BY_ID_KEY, storeId);
            return _cacheManager.Get(key, () => _storeRepository.GetById(storeId));             
        }

        /// <summary>
        /// 名称是否唯一
        /// </summary>
        /// <param name="storeName"></param>
        /// <returns>true表示唯一</returns>
        public bool NameUniqueCheck(string storeName,int storeId=0)
        {
            var query = _storeRepository.Table;
            query = query.Where(c => !c.Deleted);

            if (!String.IsNullOrWhiteSpace(storeName))
            {
                var store = query.Where(c => c.Name == storeName).FirstOrDefault();
                if (store == null) return true;
                else
                    return store.Id == storeId;

            }
            else return true;            
        }

        
        public void InsertStore(Store store)
        {
            if (store == null)
                throw new ArgumentNullException("store");

            //插入前名称唯一性判断
            //if (!NameUniqueCheck(store.Name)) throw new ArgumentException(string.Format("商家名称 {0} 已经存在", store.Name));

            _storeRepository.Insert(store);

            //cache
            _cacheManager.RemoveByPattern(STORES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(store);
        }


        public void UpdateStore(Store store)
        {
            if (store == null)
                throw new ArgumentNullException("store");

            _storeRepository.Update(store);

            //cache
            _cacheManager.RemoveByPattern(STORES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(store);
        }

        #region 商家图片

        /// <summary>
        /// 删除商家图片
        /// </summary>
        /// <param name="storePicture"></param>
        public void DeleteStorePicture(StorePicture storePicture)
        {
            if (storePicture == null)
                throw new ArgumentNullException("storePicture");

            _storePictureRepository.Delete(storePicture);

            //event notification
            _eventPublisher.EntityDeleted(storePicture);
        }

        /// <summary>
        /// 获取商家的所有图片
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public IList<StorePicture> GetStorePicturesByProductId(int storeId)
        {
            var query = from sp in _storePictureRepository.Table
                        where sp.StoreId == storeId
                        orderby sp.DisplayOrder
                        select sp;

            var storePictures = query.ToList();
            return storePictures;
        }

        public StorePicture GetStorePictureById(int storePictureId)
        {
            if (storePictureId == 0)
                return null;

            return _storePictureRepository.GetById(storePictureId);
        }

        public void InsertStorePicture(StorePicture storePicture)
        {
            if (storePicture == null)
                throw new ArgumentNullException("storePicture");

            _storePictureRepository.Insert(storePicture);

            //event notification
            _eventPublisher.EntityInserted(storePicture);
        }

        public void UpdateStorePicture(StorePicture storePicture)
        {
            if (storePicture == null)
                throw new ArgumentNullException("storePicture");

            _storePictureRepository.Update(storePicture);

            //event notification
            _eventPublisher.EntityUpdated(storePicture);
        } 
        #endregion
    }
}
