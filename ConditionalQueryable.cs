using ConditionalLinq.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace ConditionalLinq
{
    public static class ConditionalQueryable
    {
        /// <summary>
        /// global default option in front end, maybe empty string or "-1" or other value
        /// </summary>
        private const string DEFAULT_OPTION = "";

        public static IQueryable<T> MayWhere<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicate)
        {
            return condition ? source.Where(predicate) : source;
        }

        public static IQueryable<T> MayWhere<T>(this IQueryable<T> source, bool condition, Func<IQueryable<T>, IQueryable<T>> customChain)
        {
            return condition ? customChain(source) : source;
        }

        public static IQueryable<T> MayWhere<T, TProperty>(this IQueryable<T> source, TProperty? nullable, Func<TProperty, Expression<Func<T, bool>>> predicateBuilder) where TProperty : struct
        {
            if (!nullable.HasValue)
            {
                return source;
            }
            return source.Where(predicateBuilder(nullable.Value));
        }

        /// <summary>
        /// could be useful when handling value of select element in front end, each select element has corresponding property type in back end model
        /// </summary>
        public static IQueryable<T> MayWhere<T, TProperty>(this IQueryable<T> source, string selectedValue, Func<TProperty, Expression<Func<T, bool>>> predicateBuilder, bool ignoreDefaultOption = true)
        {
            if (selectedValue == null || (ignoreDefaultOption && selectedValue == DEFAULT_OPTION))
            {
                return source;
            }

            switch (predicateBuilder)
            {
                case Func<int, Expression<Func<T, bool>>> predicateBuilderInt:
                    return MayWhereInt(source, selectedValue, predicateBuilderInt);
                case Func<bool, Expression<Func<T, bool>>> predicateBuilderBool:
                    return MayWhereBool(source, selectedValue, predicateBuilderBool);
                case Func<string, Expression<Func<T, bool>>> predicateBuilderString:
                    return MayWhereString(source, selectedValue, predicateBuilderString);
                default:
                    throw new NotSupportedException("unsupported TProperty type");
            }
        }

        public static IQueryable<T> MayWhereInt<T>(this IQueryable<T> source, string value, Func<int, Expression<Func<T, bool>>> predicateBuilder)
        {
            if (!int.TryParse(value, out int typed_value))
            {
                return source;
            }
            return source.Where(predicateBuilder(typed_value));
        }

        public static IQueryable<T> MayWhereBool<T>(this IQueryable<T> source, string value, Func<bool, Expression<Func<T, bool>>> predicateBuilder)
        {
            if (!bool.TryParse(value, out bool typed_value))
            {
                return source;
            }
            return source.Where(predicateBuilder(typed_value));
        }

        public static IQueryable<T> MayWhereString<T>(this IQueryable<T> source, string value, Func<string, Expression<Func<T, bool>>> predicateBuilder)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return source;
            }
            return source.Where(predicateBuilder(value));
        }

        public static IQueryable<T> MayWhereDateTime<T>(this IQueryable<T> source, string value, Func<DateTime, Expression<Func<T, bool>>> predicateBuilder)
        {
            if (!DateTime.TryParse(value, out DateTime typed_value))
            {
                return source;
            }
            return source.Where(predicateBuilder(typed_value));
        }

        #region about pagination

        public static IQueryable<T> Page<T>(this IQueryable<T> source, Pager pager)
        {
            if (pager == null || !pager.IsValid)
            {
                return source;
            }
            return source.Page(pager.PageIndex, pager.PageSize);
        }

        public static IQueryable<T> Page<T>(this IQueryable<T> source, int pageIndex, int pageSize)
        {
            return source.Skip(pageSize * pageIndex).Take(pageSize);
        }

        #endregion
    }
}
