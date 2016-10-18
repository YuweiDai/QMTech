using QMTech.Core.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMTech.Services.Catalog
{
    /// <summary>
    /// Product ProductTag service interface
    /// </summary>
    public partial interface IProductTagService
    {

        /// <summary>
        /// Gets all ProductTags
        /// </summary>
        /// <returns>Product ProductTags</returns>
        IList<ProductTag> GetAllProductTags(string search = "");       

        /// <summary>
        /// Gets a ProductTag by name
        /// </summary>
        /// <param name="name">Product ProductTag name</param>
        /// <returns>Product ProductTag</returns>
        ProductTag GetProductTagByName(string name);

        /// <summary>
        /// Delete a product ProductTag
        /// </summary>
        /// <param name="productTag">Product ProductTag</param>
        void DeleteProductTag(ProductTag productTag);

        /// <summary>
        /// Gets product ProductTag
        /// </summary>
        /// <param name="productTagId">Product ProductTag identifier</param>
        /// <returns>Product ProductTag</returns>
        ProductTag GetProductTagById(int productTagId);

        /// <summary>
        /// Inserts a product ProductTag
        /// </summary>
        /// <param name="productTag">Product ProductTag</param>
        void InsertProductTag(ProductTag productTag);

        /// <summary>
        /// Updates the product ProductTag
        /// </summary>
        /// <param name="productTag">Product ProductTag</param>
        void UpdateProductTag(ProductTag productTag);
    }
}

