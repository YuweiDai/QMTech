using System;
using System.Linq;
using System.Web.Http.Dispatcher;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Net;
using System.Collections.Generic;
using System.Web.Http.Routing;
using System.Globalization;
using System.Text;

namespace QMTech.Web.Framework.WebAPI
{
    //public class AreaHttpControllerSelector : DefaultHttpControllerSelector
    //{
    //    private readonly HttpConfiguration _configuration;
    //    /// <summary>
    //    /// Lazy 当前程序集中包含的所有IHttpController反射集合，TKey为小写的Controller
    //    /// </summary>
    //    private readonly Lazy<ILookup<string, Type>> _apiControllerTypes;
    //    private ILookup<string, Type> ApiControllerTypes
    //    {
    //        get
    //        {
    //            return this._apiControllerTypes.Value;
    //        }
    //    }
    //    /// <summary>
    //    /// Initializes a new instance of the AreaHttpControllerSelector class
    //    /// </summary>
    //    /// <param name="configuration"></param>
    //    public AreaHttpControllerSelector(HttpConfiguration configuration)
    //        : base(configuration)
    //    {
    //        this._configuration = configuration;
    //        this._apiControllerTypes = new Lazy<ILookup<string, Type>>(this.GetApiControllerTypes);
    //    }
    //    /// <summary>
    //    /// 获取当前程序集中 IHttpController反射集合
    //    /// </summary>
    //    /// <returns></returns>
    //    private ILookup<string, Type> GetApiControllerTypes()
    //    {
    //        IAssembliesResolver assembliesResolver = this._configuration.Services.GetAssembliesResolver();
    //        return this._configuration.Services.GetHttpControllerTypeResolver()
    //            .GetControllerTypes(assembliesResolver)
    //            .ToLookup(t => t.Name.ToLower().Substring(0, t.Name.Length - DefaultHttpControllerSelector.ControllerSuffix.Length), t => t);
    //    }
    //    /// <summary>
    //    /// Selects a System.Web.Http.Controllers.HttpControllerDescriptor for the given System.Net.Http.HttpRequestMessage.
    //    /// </summary>
    //    /// <param name="request"></param>
    //    /// <returns></returns>
    //    public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
    //    {
    //        HttpControllerDescriptor des = null;
    //        string controllerName = this.GetControllerName(request);
    //        if (!string.IsNullOrWhiteSpace(controllerName))
    //        {
    //            var groups = this.ApiControllerTypes[controllerName.ToLower()];
    //            if (groups != null && groups.Any())
    //            {
    //                string endString;
    //                var routeDic = request.GetRouteData().Values;//存在controllerName的话必定能取到IHttpRouteData
    //                if (routeDic.Count > 1)
    //                {
    //                    StringBuilder tmp = new StringBuilder();
    //                    foreach (var key in routeDic.Keys)
    //                    {
    //                        tmp.Append('.');
    //                        tmp.Append(routeDic[key]);
    //                        if (key.Equals(DefaultHttpControllerSelector.ControllerSuffix, StringComparison.CurrentCultureIgnoreCase))
    //                        {//如果是control，则代表命名空间结束
    //                            break;
    //                        }
    //                    }
    //                    tmp.Append(DefaultHttpControllerSelector.ControllerSuffix);
    //                    endString = tmp.ToString();
    //                }
    //                else
    //                {
    //                    endString = string.Format(".{0}{1}", controllerName, DefaultHttpControllerSelector.ControllerSuffix);
    //                }
    //                //取NameSpace节点数最少的Type
    //                var type = groups.Where(t => t.FullName.EndsWith(endString, StringComparison.CurrentCultureIgnoreCase))
    //                    .OrderBy(t => t.FullName.Count(s => s == '.')).FirstOrDefault();//默认返回命名空间节点数最少的第一项
    //                if (type != null)
    //                {
    //                    des = new HttpControllerDescriptor(this._configuration, controllerName, type);
    //                }
    //            }
    //        }
    //        if (des == null)
    //        {
    //            throw new HttpResponseException(request.CreateErrorResponse(HttpStatusCode.NotFound,
    //                string.Format("No route providing a controller name was found to match request URI '{0}'", request.RequestUri)));
    //        }
    //        return des;
    //    }
    //}

    public class AreaHttpControllerSelector : IHttpControllerSelector
    {
        private const string NamespaceKey = "area";
        private const string ControllerKey = "controller";

        private readonly HttpConfiguration _configuration;
        private readonly Lazy<Dictionary<string, HttpControllerDescriptor>> _controllers;
        private readonly HashSet<string> _duplicates;

        public AreaHttpControllerSelector(HttpConfiguration config)
        {
            _configuration = config;
            _duplicates = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _controllers = new Lazy<Dictionary<string, HttpControllerDescriptor>>(InitializeControllerDictionary);
        }

        private Dictionary<string, HttpControllerDescriptor> InitializeControllerDictionary()
        {
            var dictionary = new Dictionary<string, HttpControllerDescriptor>(StringComparer.OrdinalIgnoreCase);

            // Create a lookup table where key is "namespace.controller". The value of "namespace" is the last
            // segment of the full namespace. For example:
            // MyApplication.Controllers.V1.ProductsController => "V1.Products"
            IAssembliesResolver assembliesResolver = _configuration.Services.GetAssembliesResolver();
            IHttpControllerTypeResolver controllersResolver = _configuration.Services.GetHttpControllerTypeResolver();

            ICollection<Type> controllerTypes = controllersResolver.GetControllerTypes(assembliesResolver);

            foreach (Type t in controllerTypes)
            {
                var segments = t.Namespace.Split(Type.Delimiter);

                // For the dictionary key, strip "Controller" from the end of the type name.
                // This matches the behavior of DefaultHttpControllerSelector.
                var controllerName = t.Name.Remove(t.Name.Length - DefaultHttpControllerSelector.ControllerSuffix.Length);

                var key = String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}.{1}", segments[segments.Length - 1], controllerName);

                // Check for duplicate keys.
                if (dictionary.Keys.Contains(key))
                {
                    _duplicates.Add(key);
                }
                else
                {
                    dictionary[key] = new HttpControllerDescriptor(_configuration, t.Name, t);
                }
            }

            // Remove any duplicates from the dictionary, because these create ambiguous matches. 
            // For example, "Foo.V1.ProductsController" and "Bar.V1.ProductsController" both map to "v1.products".
            foreach (string s in _duplicates)
            {
                dictionary.Remove(s);
            }
            return dictionary;
        }

        // Get a value from the route data, if present.
        private static T GetRouteVariable<T>(IHttpRouteData routeData, string name)
        {
            object result = null;
            if (routeData.Values.TryGetValue(name, out result))
            {
                return (T)result;
            }
            return default(T);
        }

        public HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            IHttpRouteData routeData = request.GetRouteData();
            if (routeData == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            // Get the namespace and controller variables from the route data.
            string namespaceName = GetRouteVariable<string>(routeData, NamespaceKey);
            if (namespaceName == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            string controllerName = GetRouteVariable<string>(routeData, ControllerKey);
            if (controllerName == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            // Find a matching controller.
            string key = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", namespaceName, controllerName);

            HttpControllerDescriptor controllerDescriptor;
            if (_controllers.Value.TryGetValue(key, out controllerDescriptor))
            {
                return controllerDescriptor;
            }
            else if (_duplicates.Contains(key))
            {
                throw new HttpResponseException(
                    request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                        "Multiple controllers were found that match this request."));
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        public IDictionary<string, HttpControllerDescriptor> GetControllerMapping()
        {
            return _controllers.Value;
        }
    }


}