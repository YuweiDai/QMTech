using QMTech.Core.Domain.Media;

namespace QMTech.Core.Domain.Catalog
{
    public  class StorePicture : BaseEntity
    {
        /// <summary>
        /// Gets or sets the store identifier
        /// </summary>
        public int StoreId { get; set; }

        /// <summary>
        /// Gets or sets the picture identifier
        /// </summary>
        public int PictureId { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 是否为商店Logo
        /// </summary>
        public bool IsLogo { get; set; }

        /// <summary>
        /// Gets the picture
        /// </summary>
        public virtual Picture Picture { get; set; }

        /// <summary>
        /// Gets the Store
        /// </summary>
        public virtual Store Store { get; set; }
    }
}