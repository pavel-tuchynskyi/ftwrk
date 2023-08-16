using FTWRK.Application.Common.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.RegularExpressions;

namespace FTWRK.Persistance.Extensions
{
    public static class AggregateExtensions
    {
        public static IAggregateFluent<T> Filter<T>(this IAggregateFluent<T> source, Filter filter)
        {
            if (filter == null)
            {
                return source;
            }

            var props = GetProperties<T>();
            var builder = new FilterDefinitionBuilder<T>();
            var filterList = new List<FilterDefinition<T>>();

            foreach (var condition in filter.Conditions)
            {
                if(condition == null || !props.Contains(condition.Key))
                {
                    continue;
                }

                filterList.Add(HandleCondition(builder, condition));
            }

            var filterDef = filter.Operator == Operators.And ? builder.And(filterList) : builder.Or(filterList);

            return source.Match(filterDef);
        }

        private static List<string> GetProperties<T>()
        {
            var props = typeof(T).GetProperties().Select(x => x.Name).ToList();

            return props;
        }

        private static FilterDefinition<T> HandleCondition<T>(FilterDefinitionBuilder<T> builder, FilterCondition condition)
        {
            if (condition.ConditionType == FilterConditionType.Equal)
            {
                return builder.Eq(condition.Key, condition.Value);
            }
            else if (condition.ConditionType == FilterConditionType.Contains)
            {
                var value = condition.Value.ToString();
                var regex = BsonRegularExpression.Create(Regex.Escape(value));
                return builder.Regex(condition.Key, regex);
            }
            else if(condition.ConditionType == FilterConditionType.InArray)
            {
                return builder.ElemMatch(condition.Key, builder.Eq(condition.Key, condition.Value));
            }
            else
            {
                return null;
            }
        }

        public static IAggregateFluent<T> OrderBy<T>(this IAggregateFluent<T> source, string order)
        {
            if (string.IsNullOrEmpty(order))
            {
                return source;
            }

            var sortProp = order.Split(" ")[0];
            var prop = typeof(T).GetProperties().Select(p => p.Name).FirstOrDefault(x => x.Equals(sortProp, StringComparison.OrdinalIgnoreCase));

            if(string.IsNullOrEmpty(prop))
            {
                return source;
            }

            var sortBuilder = new SortDefinitionBuilder<T>();

            SortDefinition<T> sortDef;

            if(order.EndsWith(" desc"))
            {
                sortDef = sortBuilder.Descending(prop);
            }
            else
            {
                sortDef = sortBuilder.Ascending(prop);
            }

            return source.Sort(sortDef);
        }

        public static async Task<PagedList<T>> ToPagedListAsync<T>(this IAggregateFluent<T> source, int pageNumber, int pageSize)
        {
            var countResult = await source.Count().FirstOrDefaultAsync();
            var count = countResult != null ? (int)countResult.Count : 0;

            var items = await source.Skip((pageNumber - 1) * pageSize).Limit(pageSize).ToListAsync();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
