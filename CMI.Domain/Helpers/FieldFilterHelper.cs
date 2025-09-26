using CMI.Domain.Enums;
using CMI.Domain.QueryFilters;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace CMI.Domain.Helpers
{
    [ExcludeFromCodeCoverage]
    public static class FieldFilterHelper
    {
        private const string COMMA_CHARACTER = ",";

        private const string AND = "and";
        private const string BETWEEN = "between";
        private const string GREATER = ">";
        private const string LOWER = "<";
        private const string EQUAL = "=";
        private const string LIKE = "like";
        private const string WHERE = "where";
        private const string ORDER_BY = "order by";
        private const string ASCENDING = "asc";
        private const string DESCENDING = "desc";

        private const string FALSE = "0";
        private const string TRUE = "1";

        public static string BuildQuery(
            IEnumerable<FieldFilter> fieldsFilter
        )
        {
            return BuildQuery(
                addWhereClause: false,
                fieldsFilter
            );
        }

        public static string BuildQuery(
            bool addWhereClause,
            IEnumerable<FieldFilter> fieldsFilter
        )
        {
            StringBuilder query = new();

            if (!(fieldsFilter is not null && fieldsFilter.Any()))
            {
                return string.Empty;
            }

            foreach (var field in fieldsFilter)
            {
                if (addWhereClause)
                {
                    query.Append($"{WHERE}");
                }
                else
                {
                    query.Append($"{AND}");
                }

                if (DateTime.TryParse(field.Value, out DateTime dateTimeValue))
                {

                    switch (field.TypeDateTime)
                    {
                        case TypeDateTime.Range:
                            DateTime fechaEndDateConHoraFinal = new(
                                field.EndDate!.Value.Year,
                                field.EndDate!.Value.Month,
                                field.EndDate!.Value.Day,
                                23, 59, 59
                            );

                            query.Append($" {field.Field}");

                            query.Append($" {BETWEEN} '{field.Value}' {AND} '{fechaEndDateConHoraFinal.ToString("yyyy-MM-ddTHH:mm:ss")}'");
                            break;
                        case TypeDateTime.Greater:
                            query.Append($" {field.Field}");

                            query.Append($" {GREATER} '{field.Value}'");
                            break;
                        case TypeDateTime.Lower:
                            query.Append($" {field.Field}");

                            query.Append($" {LOWER} '{field.Value}'");
                            break;
                        case TypeDateTime.Equal:
                            query.Append($" CAST({field.Field} AS DATE)");

                            query.Append($" {EQUAL} '{field.Value}'");
                            break;
                        default:
                            query.Append($" {field.Field}");

                            query = new();
                            break;
                    }
                }
                else if (field.Value == TRUE || field.Value == FALSE)
                {
                    query.Append($" {field.Field}");

                    query.Append($" {EQUAL} '{field.Value}'");
                }
                else
                {
                    query.Append($" {field.Field}");

                    query.Append($" {LIKE} '%{field.Value}%'");
                }

                addWhereClause = false;
            }

            return query.ToString();
        }

        public static string BuildQueryOrderBy(
            IEnumerable<FieldFilter> fieldsFilter
        )
        {
            string orderByQuery = string.Join(
                COMMA_CHARACTER,
                fieldsFilter.Select(
                    filter => $"{filter.Field} {(filter.TypeOrderBy == TypeOrderBy.Ascending ? ASCENDING : DESCENDING)}"
                )
            );

            if (!string.IsNullOrEmpty(orderByQuery))
            {
                return $" {ORDER_BY} {orderByQuery}";
            }

            return string.Empty;
        }

        public static object[] BuildQueryArgs(IEnumerable<FieldFilter> listFilters)
        {
            string conditionQuery = BuildQuery(addWhereClause: true, listFilters);
            conditionQuery += BuildQueryOrderBy(
                listFilters!.Where(filter => filter.TypeOrderBy is not null)
            );
            return [conditionQuery];
        }
    }
}
