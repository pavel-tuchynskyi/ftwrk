namespace FTWRK.Application.Common.Models
{
    public class QueryParameters
    {
        public Filter Filter { get; set; }
        public string OrderBy { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class Filter
    {
        public Operators Operator { get; set; }
        public List<FilterCondition> Conditions { get; set; }
    }

    public class FilterCondition
    {
        public string Key { get; set; }
        public FilterConditionType ConditionType { get; set; }
        public object Value { get; set; }
    }

    public enum FilterConditionType
    {
        Equal,
        Contains,
        InArray
    }

    public enum Operators
    {
        And,
        Or
    }
}
