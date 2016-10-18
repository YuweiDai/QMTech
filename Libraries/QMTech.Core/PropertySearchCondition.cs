using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMTech.Core
{
    /// <summary>
    /// 搜索条件
    /// </summary>
    public class PropertySearchCondition
    {
        public PropertySearchCondition()
        {
            this.SearchableFields = new List<string>();
        }

        public string Value { get; set; }

        /// <summary>
        /// 是否为正则表达式
        /// </summary>
        public bool Regex { get; set; }

        /// <summary>
        /// 可以搜索的字段
        /// </summary>
        public IList<string> SearchableFields { get; set; }
    }
}
