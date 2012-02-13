using System.Globalization;
namespace System
{
    public static class Conversions
    {

        /// <summary>
        /// Returns string empty if value is null
        /// </summary>
        public static string ConvertToString(this object value)
        {
            if (value == null)
                return string.Empty;
            else
                return value.ToString();
        }


        public static int ConvertToInt32(this object value, int defVal)
        {
            if (value == null)
                return defVal;
            int retVal = defVal;
            try { retVal = System.Convert.ToInt32(value); }
            catch { }
            return retVal;
        }
       
        public static int? ConvertToNullableInt32(this object value)
        {
            if (value == null)
                return null;
            int? retVal = null;
            try
            {
                retVal = System.Convert.ToInt32(value);
            }
            catch { }
            return retVal;
        }

       
        public static decimal ConvertToDecimal(this object value, decimal defVal)
        {
            if (value == null)
                return defVal;
            decimal retVal = defVal;
            try
            {
                retVal = System.Convert.ToDecimal(value, new CultureInfo("en-US"));
            }
            catch { }
            return retVal;
        }

        /// <summary>
        /// Handles string representation of decimal numbers including currency and percentages
        /// </summary>
        public static decimal ConvertToDecimal(this string value, decimal defVal)
        {
            if (string.IsNullOrEmpty(value))
                return defVal;
            decimal retVal = defVal;
            try
            {
                value = value.Replace("£", "").Replace("$", "").Replace("%", "").Replace("€", "");
                retVal = System.Convert.ToDecimal(value, new CultureInfo("en-US"));
            }
            catch { }
            return retVal;
        }

       
        public static decimal? ConvertToNullableDecimal(this object value)
        {
            if (value == null)
                return null;
            decimal? retVal = null;
            try
            {
                retVal = System.Convert.ToDecimal(value, new CultureInfo("en-US"));
            }
            catch { }
            return retVal;
        }

        /// <summary>
        /// Handles string representation of decimal numbers including currency and percentages
        /// </summary>
        public static decimal? ConvertToNullableDecimal(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            decimal? retVal = null;
            try
            {
                value = value.Replace("£", "").Replace("$", "").Replace("%", "").Replace("€", "");
                retVal = System.Convert.ToDecimal(value, new CultureInfo("en-US"));
            }
            catch { }
            return retVal;
        }


        public static double ConvertToDouble(this object value, double defVal)
        {
            if (value == null)
                return defVal;
            double retVal = defVal;
            try
            {
                retVal = System.Convert.ToDouble(value, new CultureInfo("en-US"));
            }
            catch { }
            return retVal;
        }

       
        public static DateTime ConvertToDateTime(this object value, DateTime defVal)
        {
            if (value == null)
                return defVal;
            DateTime retVal = defVal;
            try
            {
                DateTime? nullable = Bronson.Utils.DateHelpers.GetCalculatedDateTime(value);
                if (nullable == null)
                    retVal = System.Convert.ToDateTime(value);
                else
                    retVal = nullable.Value;
            }
            catch { }
            return retVal;
        }

       
        public static DateTime? ConvertToNullableDateTime(this object value)
        {
            if (value == null)
                return null;
            DateTime? retVal = null;
            try
            {
                retVal = Bronson.Utils.DateHelpers.GetCalculatedDateTime(value);
                if (retVal == null)
                    retVal = System.Convert.ToDateTime(value);
            }
            catch { }
            return retVal;
        }


        public static bool ConvertToBool(this object value, bool defVal)
        {
            bool retVal = defVal;
            try
            {
                if (value == null)
                    return defVal;

                if (value.ToString().Trim() == "1")
                    retVal = true;
                else if (value.ToString().Trim() == "0")
                    retVal = false;
                else if (value.ToString().Trim() == "on")
                    retVal = true;
                else if (value.ToString().Trim() == "checked")
                    retVal = true;
                else
                    retVal = System.Convert.ToBoolean(value);
            }
            catch { }
            return retVal;
        }
       

        public static bool? ConvertToNullableBool(this object value)
        {
            bool? retVal = null;
            if (value == null)
                return null;
            try
            {
                if (value.ToString().Trim() == "1")
                    retVal = true;
                else if (value.ToString().Trim() == "0")
                    retVal = false;
                else if (value.ToString().Trim() == "on")
                    retVal = true;
                else if (value.ToString().Trim() == "checked")
                    retVal = true;
                else
                    retVal = System.Convert.ToBoolean(value);
            }
            catch { }
            return retVal;
        }

       

           public static decimal Invert(this decimal? item)
        {
            if (item != null)
                return item.Value * -1;

            return 0;


        }

        public static decimal Invert(this decimal item)
        {
            
            return item * -1;
        }

        public static string ToCurrency(this decimal? item)
        {
            return item != null ? item.Value.ToCurrency() : ((decimal)0).ToCurrency();
        }

        public static string ToCurrency(this decimal value)
        {

            return value.ToString("C");
        }



        public static string ToNumber(this decimal? item)
        {
            return item != null ? item.Value.ToNumber() : ((decimal)0).ToNumber();
        }

        public static string ToNumber(this decimal value)
        {

            return value.ToString("N");
        }
     

    }
}
