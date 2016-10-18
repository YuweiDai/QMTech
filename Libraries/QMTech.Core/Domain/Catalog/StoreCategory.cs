using QMTech.Core.Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMTech.Core.Domain.Catalog
{
    /// <summary>
    /// 商家类别
    /// </summary>
    public class StoreCategory : BaseEntity, IAclSupported
    {
        private ICollection<Store> _stores;

        /// <summary>
        /// 类别名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类别描述
        /// </summary>
        public string Description { get; set; }        

        /// <summary>
        /// 元数据关键字
        /// </summary>
        public string MetaKeywords { get; set; }

        /// <summary>
        /// 元数据描述
        /// </summary>
        public string MetaDescription { get; set; }

        /// <summary>
        /// 元数据标题
        /// </summary>
        public string MetaTitle { get; set; }

        /// <summary>
        /// 父节点Id
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 图片Id
        /// </summary>
        public int PictureId { get; set; }

        /// <summary>
        /// 是否有优惠
        /// </summary>
        public bool HasDiscountsApplied { get; set; }

        /// <summary>
        /// 是否启用访问控制
        /// </summary>
        public bool SubjectToAcl { get; set; }

        /// <summary>
        /// 是否发布
        /// </summary>
        public bool Published { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 关联的商家
        /// </summary>
        public virtual ICollection<Store> Stores {
            get { return _stores ?? (_stores = new List<Store>()); }
            protected set { _stores = value; }
        }
    }
}
