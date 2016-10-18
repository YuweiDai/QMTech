using System.Web.Routing;

namespace QMTech.Web.Framework.Mvc.Routes
{
    public interface IRouteProvider
    {
        void RegisterRoutes(RouteCollection routes);

        int Priority { get; }
    }
}
