using QMTech.Core.Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMTech.Core.Domain.Catalog
{
    /// <summary>
    /// 表示类别，可以是商品类别，也可以是店铺类别
    /// </summary>
    public class Category : BaseEntity, IAclSupported
    {
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
        public int ParentCategoryId { get; set; }

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
    }
}
