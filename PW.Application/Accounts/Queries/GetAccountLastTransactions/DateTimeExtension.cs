using System;
using System.Collections.Generic;
using System.Text;

namespace PW.Application.Accounts.Queries.GetAccountLastTransactions
{
    public static class DateTimeExtension
    {
        public static DateTime MarkUnspecifiedDateAsUtc(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day,
                date.Hour, date.Minute, date.Second, DateTimeKind.Utc);
        }

        public static DateTime? MarkUnspecifiedDateAsUtc(this DateTime? date)
        {
            if (!date.HasValue)
                return null;
            return new DateTime(date.Value.Year, date.Value.Month, date.Value.Day,
                date.Value.Hour, date.Value.Minute, date.Value.Second, DateTimeKind.Utc);
        }
    }
}
