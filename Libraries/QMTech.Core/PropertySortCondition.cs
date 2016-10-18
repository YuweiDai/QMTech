using System.Collections.Generic;
using System.ComponentModel;

namespace QMTech.Core
{
    /// <summary>
    /// 排序
    /// </summary>
    public class PropertySortCondition
    {
        public PropertySortCondition(string propertyName) :
            this(propertyName, ListSortDirection.Ascending)
        {

        }

        public PropertySortCondition(string propertyName, ListSortDirection listSortDirection)
        {
            PropertyName = propertyName;
            ListSortDirection = listSortDirection;
        }

        public string PropertyName { get; set; }

        public ListSortDirection ListSortDirection { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sort">排序格式为field1,asc;field2,desc</param>
        /// <returns></returns>
        public static PropertySortCondition[] Instance(string sort)
        {
            if (string.IsNullOrEmpty(sort) || !(sort.Contains(";") && sort.Contains(","))) return new PropertySortCondition[0];

            sort = sort.toUpperCaseFirstOne();

            string[] fields = sort.TrimEnd(';').Split(';');
            var conditions = new List<PropertySortCondition>();

            //遍历循环
            foreach (var field in fields)
            {
                if (!field.Contains(",")) continue;

                conditions.Add(new PropertySortCondition(field.Split(',')[0], field.Split(',')[1].ToLower() == "desc" ? ListSortDirection.Descending : ListSortDirection.Ascending));
            }

            return conditions.ToArray();

        }
    }
}