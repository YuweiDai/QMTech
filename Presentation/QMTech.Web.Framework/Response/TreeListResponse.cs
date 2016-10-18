using Newtonsoft.Json;
using System.Collections.Generic;

namespace QMTech.Web.Framework.Response
{
    /// <summary>
    /// 树形数据响应
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TreeList<T>
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("children")]
        public IList<TreeList<T>> Children { get; set; }

        /// <summary>
        /// 动态属性集合
        /// </summary>
        [JsonProperty("data")]
        public T Data { get; set; }
    }
}
