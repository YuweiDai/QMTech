using QMTech.Services.Catalog;
using QMTech.Web.Api.Extensions;
using QMTech.Web.Framework.Controllers;
using System.Linq;
using System.Web.Http;

namespace QMTech.Api.Controllers
{
    [RoutePrefix("Admin/Catalog/ProductTags")]
    public class ProductTagAdminController : BaseAdminApiController
    {
        private readonly IProductTagService _tagService;

        public ProductTagAdminController(IProductTagService tagService)
        {
            this._tagService = tagService;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult QueryProudctTags(string search = "")
        {
            var query = _tagService.GetAllProductTags( search);

            var response = query.Select(t => { return t.ToModel(); });

            return Ok(response);
        }
    }
}
