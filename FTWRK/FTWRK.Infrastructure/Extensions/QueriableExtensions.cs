using AutoMapper;
using AutoMapper.QueryableExtensions;
using FTWRK.Application.Common.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;

namespace FTWRK.Infrastructure.Extensions
{
    public static class MongoExtensions
    {
        public static IMongoQueryable<T> Filter<T>(this IQueryable<T> source, Filter filter)
        {
            if (filter == null)
            {
                return (IMongoQueryable<T>)source;
            }

            var queryBuilder = new StringBuilder();

            var op = filter.Operator == Operators.And ? " && " : " || ";

            var props = GetProperties<T>();

            foreach (var condition in filter.Conditions)
            {
                if (!props.Any(x => x == condition.Key))
                {
                    return (IMongoQueryable<T>)source;
                }

                queryBuilder.Append(HandleFilterCondition(condition));
                queryBuilder.Append(op);
            }

            var query = queryBuilder.ToString().TrimEnd(new char[] { ' ', '&', '|' });

            return (IMongoQueryable<T>)source.Where(query);
        }

        private static List<string> GetProperties<T>()
        {
            var props = typeof(T).GetProperties().Select(x => x.Name).ToList();
            
            return props;
        }

        private static string HandleFilterCondition(FilterCondition condition)
        {
            return condition.ConditionType switch
            {
                FilterConditionType.Contains => $"{condition.Key}.Contains(\"{condition.Value}\")",
                FilterConditionType.Equal => $"{condition.Key}.Equals(\"{condition.Value}\")",
                FilterConditionType.InArray => $"{condition.Key}.Any(x => x == \"{condition.Value}\")",
                _ => string.Empty
            };
        }

        public static IMongoQueryable<T> Sort<T>(this IQueryable<T> source, string orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return (IMongoQueryable<T>)source;
            }

            var props = GetProperties<T>();
            var orderParam = orderBy.Split(" ")[0];

            if (!props.Any(x => x == orderParam))
            {
                return (IMongoQueryable<T>)source;
            }

            var queryBuilder = new StringBuilder();
            var direction = orderBy.EndsWith(" desc") ? "descending" : "ascending";

            queryBuilder.Append($"{orderParam} {direction}");
            var query = queryBuilder.ToString();

            return (IMongoQueryable<T>)source.OrderBy(query);
        }

        public static UpdateDefinition<T> CreateUpdateDefinition<T>(this UpdateDefinitionBuilder<T> builder, T item)
        {
            var type = typeof(T);
            var props = GetProperties<T>();
            var updateDefList = new List<UpdateDefinition<T>>();
            foreach (var property in props)
            {
                var value = typeof(T).GetProperty(property).GetValue(item, null);
                
                if (value == null)
                {
                    continue;
                }
                var valType = value.GetType();
                var expression = CreateSetExpression<T>(property);
                var setMethod = typeof(UpdateDefinitionBuilder<T>).GetMethods().Where(x => x.Name == "Set").ToList();
                var meth = setMethod[1].MakeGenericMethod(new[] { valType });
                updateDefList.Add((UpdateDefinition<T>)meth.Invoke(builder, new object[] { expression, value }));
            }

            UpdateDefinition<T> update = builder.Combine(updateDefList);

            return update;
        }

        private static LambdaExpression CreateSetExpression<T>(string key)
        {
            var parameterExp = Expression.Parameter(typeof(T));
            var propertyExp = Expression.Property(parameterExp, key);

            return Expression.Lambda(propertyExp, parameterExp);
        }

        public static async Task<PagedList<T>> ToPagedListAsync<T>(this IMongoQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }

        public static async Task<PagedList<R>> ToPagedListAsync<T, R>(this IMongoQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedList<R>(items.Cast<R>(), count, pageNumber, pageSize);
        }

        public static IMongoQueryable<R> ProjectTo<T, R>(this IMongoQueryable<T> source, IConfigurationProvider configuration)
        {
            return (IMongoQueryable<R>)source.ProjectTo<R>(configuration);
        }
    }
}
