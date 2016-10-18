using System.Collections.Generic;

namespace QMTech.Core.Domain.Catalog
{
    public partial class StoreTag : BaseEntity
    {
        public int TagId { get; set; }

        public int StoreId { get; set; }

        public int DisplayOrder { get; set; }

        public virtual Store Store { get; set; }
    }
}