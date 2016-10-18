using QMTech.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QMTech.Web.Framework.Themes
{
    /// <summary>
    /// Themable view engine
    /// </summary>
    public abstract class ThemeableVirtualPathProviderViewEngine : VirtualPathProviderViewEngine
    {
        #region Fields

        internal Func<string, string> GetExtensionThunk;

        private readonly string[] _emptyLocations = null;

        #endregion

        protected ThemeableVirtualPathProviderViewEngine()
        {
            GetExtensionThunk = new Func<string, string>(VirtualPathUtility.GetExtension);
        }

        #region Utilities

        /// <summary>
        /// 核心方法，查找View
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="locations"></param>
        /// <param name="areaLocations"></param>
        /// <param name="locationsPropertyName"></param>
        /// <param name="name"></param>
        /// <param name="controllerName"></param>
        /// <param name="cacheKeyPrefix"></param>
        /// <param name="useCache"></param>
        /// <param name="searchedLocations"></param>
        /// <returns></returns>
        protected virtual string GetPath(ControllerContext controllerContext, string[] locations, string[] areaLocations, string locationsPropertyName,
            string name, string controllerName, string cacheKeyPrefix, bool useCache, out string[] searchedLocations)
        {
            searchedLocations = _emptyLocations;
            if (string.IsNullOrEmpty(name))
                return string.Empty;

            string areaName = GetAreaName(controllerContext.RouteData);
            if (!string.IsNullOrEmpty(areaName) && areaName.Equals("admin", StringComparison.InvariantCultureIgnoreCase))
            {
                var newLocations = areaLocations.ToList();
                newLocations.Insert(0, "~/Administration/Views/{1}/{0}.cshtml");
                newLocations.Insert(0, "~/Administration/Views/Shared/{0}.cshtml");
                areaLocations = newLocations.ToArray();
            }

            bool flag = !string.IsNullOrEmpty(areaName);
            List<ViewLocation> viewLocations = GetViewLocations(locations, flag ? areaLocations : null);
            if (viewLocations.Count == 0)
            {
                throw new InvalidOperationException(string.Format("属性集合不能为null或空", new object[] { locationsPropertyName }));
            }

            bool flag2 = IsSpecificPath(name);
            string key = this.CreateCacheKey(cacheKeyPrefix, name, flag2 ? string.Empty : controllerName, areaName);
            if (useCache)
            {
                var cached = this.ViewLocationCache.GetViewLocation(controllerContext.HttpContext, key);
                if (cached != null)
                    return cached;
            }

            if (!flag2)
                return this.GetPathFromGeneralName(controllerContext, viewLocations, name, controllerName, areaName, key, ref searchedLocations);

            return this.GetPathFromSpecificName(controllerContext, name, key, ref searchedLocations);
        }

        protected virtual string GetPathFromSpecificName(ControllerContext controllerContext, string name, string cacheKey, ref string[] searchedLocations)
        {
            string virtualPath = name;
            if (!this.FilePathIsSupported(name) || !this.FileExists(controllerContext, name))
            {
                virtualPath = string.Empty;
                searchedLocations = new[] { name };
            }

            this.ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, virtualPath);
            return virtualPath;
        }

        protected virtual bool FilePathIsSupported(string virtualPath)
        {
            if (this.FileExtensions == null)
                return true;

            string str = this.GetExtensionThunk(virtualPath).TrimStart(new[] { '.' });
            return this.FileExtensions.Contains(str, StringComparer.OrdinalIgnoreCase);
        }

        protected virtual string GetPathFromGeneralName(ControllerContext controllerContext, List<ViewLocation> locations, string name, string controllerName,
            string areaName, string cacheKey, ref string[] seachedLocations)
        {
            string virtualPath = string.Empty;
            seachedLocations = new string[locations.Count];
            for (int i = 0; i < locations.Count; i++)
            {
                string str2 = locations[i].Format(name, controllerName, areaName);
                if (this.FileExists(controllerContext, str2))
                {
                    seachedLocations = _emptyLocations;
                    virtualPath = str2;
                    this.ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, virtualPath);
                    return virtualPath;
                }
                seachedLocations[i] = str2;
            }
            return virtualPath;
        }

        protected virtual string CreateCacheKey(string prefix, string name, string controllerName, string areaName)
        {
            return string.Format(CultureInfo.InvariantCulture, ":ViewCacheEntry:{0}:{1}:{2}:{3}:{4}",
                new object[] { base.GetType().AssemblyQualifiedName, prefix, name, controllerName, areaName });
        }

        protected virtual List<ViewLocation> GetViewLocations(string[] viewLocationFormats, string[] areaViewLocationsFormats)
        {
            var list = new List<ViewLocation>();
            if (areaViewLocationsFormats != null)
            {
                list.AddRange(areaViewLocationsFormats.Select(str => new AreaAwareViewLocation(str)).Cast<ViewLocation>());
            }
            if (viewLocationFormats != null)
            {
                list.AddRange(viewLocationFormats.Select(str2 => new ViewLocation(str2)));
            }
            return list;
        }

        protected virtual bool IsSpecificPath(string name)
        {
            char ch = name[0];
            if (ch != '~')
            {
                return (ch == '/');
            }
            return true;
        }

        protected virtual string GetAreaName(RouteData routeData)
        {
            object obj2;
            if (routeData.DataTokens.TryGetValue("area", out obj2))
            {
                return obj2 as string;
            }
            return GetAreaName(routeData.Route);
        }

        protected virtual string GetAreaName(RouteBase route)
        {
            var area = route as IRouteWithArea;
            if (area != null)
                return area.Area;

            var route2 = route as Route;
            if ((route2 != null) && route2.DataTokens != null)
            {
                return (route2.DataTokens["area"] as string);
            }
            return null;
        }

        #endregion

        #region Method

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            string[] strArray, strArray2;

            if (controllerContext == null)
                throw new ArgumentNullException("controllerContext");
            if (string.IsNullOrEmpty(viewName))
                throw new ArgumentException("视图名不能为空", "viewName");

            string requiredString = controllerContext.RouteData.GetRequiredString("controller");
            string str2 = this.GetPath(controllerContext, this.ViewLocationFormats, this.AreaViewLocationFormats, "ViewLocationFormats", viewName, requiredString, "View", useCache, out strArray);
            string str3 = this.GetPath(controllerContext, this.MasterLocationFormats, this.AreaMasterLocationFormats, "MasterLocationFormats", masterName, requiredString, "Master", useCache, out strArray2);
            if (!string.IsNullOrEmpty(str2) && (!string.IsNullOrEmpty(str3) || string.IsNullOrEmpty(masterName)))
                return new ViewEngineResult(this.CreateView(controllerContext, str2, str3), this);

            if (strArray2 == null)
                strArray2 = new string[0];

            return new ViewEngineResult(strArray.Union(strArray2));
        }

        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            string[] strArray;
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            if (string.IsNullOrEmpty(partialViewName))
            {
                throw new ArgumentException("Partial view name cannot be null or empty.", "partialViewName");
            }

            string requiredString = controllerContext.RouteData.GetRequiredString("controller");
            string str2 = this.GetPath(controllerContext, this.PartialViewLocationFormats, this.AreaPartialViewLocationFormats, "PartialViewLocationFormats", partialViewName, requiredString, "Partial", useCache, out strArray);
            if (string.IsNullOrEmpty(str2))
            {
                return new ViewEngineResult(strArray);
            }
            return new ViewEngineResult(this.CreatePartialView(controllerContext, str2), this);

        }

        #endregion
    }

    /// <summary>
    /// 后台管理系统页面查找
    /// </summary>
    public class AreaAwareViewLocation : ViewLocation
    {
        public AreaAwareViewLocation(string virtualPathFormatString)
            : base(virtualPathFormatString)
        {
        }

        public override string Format(string viewName, string controllerName, string areaName)
        {
            return string.Format(CultureInfo.InvariantCulture, _virtualPathFormatString, viewName, controllerName, areaName);
        }
    }

    /// <summary>
    /// 首页系统View查找
    /// </summary>
    public class ViewLocation
    {
        protected readonly string _virtualPathFormatString;

        public ViewLocation(string virtualPathFormatString)
        {
            _virtualPathFormatString = virtualPathFormatString;
        }

        public virtual string Format(string viewName, string controllerName, string areaName)
        {
            return string.Format(CultureInfo.InvariantCulture, _virtualPathFormatString, viewName, controllerName);
        }
    }
}