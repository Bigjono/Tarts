using System;

namespace Bronson.Utils
{
    public class DateHelpers
    {

        public static DateTime GetDateDifference(DateTime startDate, DateTime endDate, ref int years, ref int months)
        {

            TimeSpan Span = endDate - startDate;
            DateTime diffDate = DateTime.MinValue + Span;

            //The month can be one out as different months have different days in them. 
            int daysInStartDateMonth = DateTime.DaysInMonth(startDate.Year, startDate.Month);
            int daysInEndDateMonth = DateTime.DaysInMonth(endDate.Year, endDate.Month);
            int diff = daysInEndDateMonth - daysInStartDateMonth;
            diffDate = diffDate.AddDays(diff);//So adjust the diffDate manually

            years = diffDate.Year - 1;
            months = diffDate.Month - 1;
            return diffDate;
        }

        /// <summary>
        /// Attempts to create date from string representation such as +7days or -2months
        /// </summary>
        public static DateTime? GetCalculatedDateTime(object dateRepresentation)
        {
            try
            {
                string val = dateRepresentation.ToString().ToLower();
                if (val == "now")
                    return DateTime.Now;
                int integerVal = 0;
                bool isMinus = val.StartsWith("-");
                bool isPlus = val.StartsWith("+");
                val = val.Replace("-", "").Replace("+", "");
                if ((isMinus) || (isPlus))
                {
                    if (val.Contains("days"))
                    {
                        integerVal = val.Replace("days", "").ConvertToInt32(0);
                        if (integerVal != 0)
                            return (isMinus) ? DateTime.Now.AddDays(-integerVal) : DateTime.Now.AddDays(integerVal);
                    }
                    else if (val.Contains("months"))
                    {
                        integerVal = val.Replace("months", "").ConvertToInt32(0);
                        if (integerVal != 0)
                            return (isMinus) ? DateTime.Now.AddMonths(-integerVal) : DateTime.Now.AddDays(integerVal);
                    }
                    else if (val.Contains("years"))
                    {
                        integerVal = val.Replace("years", "").ConvertToInt32(0);
                        if (integerVal != 0)
                            return (isMinus) ? DateTime.Now.AddYears(-integerVal) : DateTime.Now.AddDays(integerVal);
                    }
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static DateTime GetMondayThisWeek()
        {
            int substract = (1 - (int)DateTime.Now.DayOfWeek);
            DateTime dt = DateTime.Now.AddDays(substract);
            return new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
        }
    }
}
