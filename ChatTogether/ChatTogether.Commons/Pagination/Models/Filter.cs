namespace ChatTogether.Commons.Pagination.Models
{
    public enum FieldType
    {
        STRING_TYPE,
        INT_TYPE
    }

    public enum Operation
    {
        LT,
        LE,
        EQ,
        GE,
        GT
    }

    public class Filter
    {
        public string FieldName { get; set; }
        public FieldType Type { get; set; }
        public Operation Operation { get; set; }
        public string Value { get; set; }
    }
}
