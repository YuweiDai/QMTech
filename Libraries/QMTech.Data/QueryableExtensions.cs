using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using QMTech.Core;

namespace QMTech.Data 
{
    /// <summary>
    /// Queryable extensions
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Include
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="queryable">Queryable</param>
        /// <param name="includeProperties">A list of properties to include</param>
        /// <returns>New queryable</returns>
        public static IQueryable<T> IncludeProperties<T>(this IQueryable<T> queryable,
            params Expression<Func<T, object>>[] includeProperties)
        {
            if (queryable == null)
                throw new ArgumentNullException("queryable");

            foreach (Expression<Func<T, object>> includeProperty in includeProperties)
                queryable = queryable.Include(includeProperty);

            return queryable;
        }


        #region 排序扩展方法

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, ListSortDirection listSortDirection)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentNullException("propertyName");

            return QuerySortHelper<T>.OrderBy(source, propertyName, listSortDirection);
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, PropertySortCondition condition)
        {
            if (condition == null)
                throw new ArgumentNullException("condition");
            return QuerySortHelper<T>.OrderBy(source, condition.PropertyName, condition.ListSortDirection);
        }

        public static IQueryable<T> ThenBy<T>(this IQueryable<T> source, string propertyName, ListSortDirection listSortDirection)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentNullException("propertyName");

            return QuerySortHelper<T>.ThenBy(source, propertyName, listSortDirection);
        }

        public static IQueryable<T> ThenBy<T>(this IQueryable<T> source, PropertySortCondition condition)
        {
            if (condition == null)
                throw new ArgumentNullException("condition");
            return QuerySortHelper<T>.ThenBy(source, condition.PropertyName, condition.ListSortDirection);
        }

        public static IQueryable<T> Sort<T>(this IQueryable<T> source, params PropertySortCondition[] conditions)
        {
            int index = 0;

            foreach (var condition in conditions)
            {
                if (index == 0)
                    source = QuerySortHelper<T>.OrderBy(source, condition.PropertyName, condition.ListSortDirection);
                else
                    source = QuerySortHelper<T>.ThenBy(source, condition.PropertyName, condition.ListSortDirection);
            }

            return source;
        }

        //public static IQueryable<T> Search<T>(this IQueryable<T> source,PropertySearchCondition condition)
        //{
        //    foreach (var field in condition.SearchableFields)
        //    {
        //        var type = typeof(T);

        //        ParameterExpression param = Expression.Parameter(type);
        //        MemberExpression body = null;

        //        PropertyInfo propertyInfo = type.GetProperty(field);
        //        if (propertyInfo == null)
        //            throw new ArgumentNullException(string.Format("属性{0}不存在类型{1}", field, type.FullName));

        //        switch (propertyInfo.PropertyType.Name)
        //        {
        //            case "String":

        //                break;
        //            case "DateTime":
        //                break;
        //            case "Int":
        //                break;
        //        }

        //        body = Expression.Property(param, field);
                
        //    }
        //}

        private static Expression<Func<TElement, bool>> GetWhereInExpression<TElement, TValue>(Expression<Func<TElement, TValue>> propertySelector, IEnumerable<TValue> values)
        {
            ParameterExpression p = propertySelector.Parameters.Single();
            if (!values.Any())
                return e => false;

            var equals = values.Select(value => (Expression)Expression.Equal(propertySelector.Body, Expression.Constant(value, typeof(TValue))));
            var body = equals.Aggregate<Expression>((accumulate, equal) => Expression.Or(accumulate, equal));

            return Expression.Lambda<Func<TElement, bool>>(body, p);
        }

        /// <summary>  
        /// Return the element that the specified property's value is contained in the specifiec values  
        /// </summary>  
        /// <typeparam name="TElement">The type of the element.</typeparam>  
        /// <typeparam name="TValue">The type of the values.</typeparam>  
        /// <param name="source">The source.</param>  
        /// <param name="propertySelector">The property to be tested.</param>  
        /// <param name="values">The accepted values of the property.</param>  
        /// <returns>The accepted elements.</returns>  
        public static IQueryable<TElement> WhereIn<TElement, TValue>(this IQueryable<TElement> source, Expression<Func<TElement, TValue>> propertySelector, params TValue[] values)
        {
            return source.Where(GetWhereInExpression(propertySelector, values));
        }

        /// <summary>  
        /// Return the element that the specified property's value is contained in the specifiec values  
        /// </summary>  
        /// <typeparam name="TElement">The type of the element.</typeparam>  
        /// <typeparam name="TValue">The type of the values.</typeparam>  
        /// <param name="source">The source.</param>  
        /// <param name="propertySelector">The property to be tested.</param>  
        /// <param name="values">The accepted values of the property.</param>  
        /// <returns>The accepted elements.</returns>  
        public static IQueryable<TElement> WhereIn<TElement, TValue>(this IQueryable<TElement> source, Expression<Func<TElement, TValue>> propertySelector, IEnumerable<TValue> values)
        {
            return source.Where(GetWhereInExpression(propertySelector, values));
        }

        #region Nested Class

        /// <summary>
        /// 排序辅助
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private static class QuerySortHelper<T>
        {
            private static readonly ConcurrentDictionary<string, LambdaExpression> Cache = new ConcurrentDictionary<string, LambdaExpression>();

            internal static IOrderedQueryable<T> OrderBy(IQueryable<T> source, string propertyName, ListSortDirection sortDirection = ListSortDirection.Ascending)
            {
                dynamic keySelector = GetLambdaExpression(propertyName);
                return sortDirection == ListSortDirection.Ascending ?
                    Queryable.OrderBy(source, keySelector) :
                    Queryable.OrderByDescending(source, keySelector);
            }

            internal static IOrderedQueryable<T> ThenBy(IQueryable<T> source, string propertyName, ListSortDirection sortDirection = ListSortDirection.Ascending)
            {
                dynamic keySelector = GetLambdaExpression(propertyName);
                return sortDirection == ListSortDirection.Ascending ?
                    Queryable.ThenBy(source, keySelector) :
                    Queryable.ThenByDescending(source, keySelector);
            }

            private static LambdaExpression GetLambdaExpression(string propertyName)
            {
                if (Cache.ContainsKey(propertyName))
                    return Cache[propertyName];
                //获取对象类型
                var TType = typeof(T);

                ParameterExpression param = Expression.Parameter(TType);
                MemberExpression body = null;

                //获取属性
                var property = TType.GetProperty(propertyName);
                if (property == null)
                    throw new ArgumentNullException(string.Format("属性{0}不存在类型{1}", propertyName, TType.FullName));

                //如果字段类型是导航属性
                //判断是否有 DisplayOrder 字段，没有则按Name排序
                if (property.PropertyType.BaseType.Equals(typeof(Core.BaseEntity)))
                {
                    body = Expression.Property(param, property);

                    var propertyBaseType = property.PropertyType.BaseType;
                    if (propertyBaseType.GetProperty("DisplayOrder") != null)
                        body = Expression.Property(body, "DisplayOrder");
                    else
                        body = Expression.Property(body, "Name");
                }
                else
                    body = Expression.Property(param, propertyName);

                LambdaExpression keySelector = Expression.Lambda(body, param);

                Cache[propertyName] = keySelector;
                return keySelector;
            }
        }

        #endregion 


        #endregion

    }
}