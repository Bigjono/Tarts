namespace System
{
    public static class Formats
    {

        
        /// <summary>
        /// Numeric Format convets numbers into comma seperated numeric string e.g. 450000 - 450,000
        /// </summary>
        public static string ToNumericString(this int num)
        {
            return String.Format("{0:C}", num).Replace("£", "");
        }

        /// <summary>
        /// Numeric Format convets numbers into comma seperated numeric string e.g. 450000 - 450,000
        /// </summary>
        public static string ToNumericString(this decimal? num)
        {
            if (num != null)
                return String.Format("{0:C}", num).Replace("£", "");
            else
                return string.Empty;
        }

        /// <summary>
        /// Numeric Format convets numbers into comma seperated numeric string e.g. 450000 - 450,000
        /// </summary>
        public static string ToNumericString(this decimal num)
        {
            return String.Format("{0:C}", num).Replace("£", "");
        }

        /// <summary>
        /// Numeric Format convets numbers into comma seperated numeric string e.g. 450000 - 450,000.00
        /// </summary>
        public static string ToCurrencyString(this decimal val)
        {
            return String.Format("{0:C}", val);
        }
        /// <summary>
        /// Numeric Format convets numbers into comma seperated numeric string e.g. 450000 - 450,000.00
        /// </summary>
        public static string ToCurrencyString(this decimal? val)
        {
            if (val == null)
                return "";
            return String.Format("{0:C}", val);
        }

        /// <summary>
        /// Numeric Format convets numbers into comma seperated numeric string e.g. 450000 - 450,000.00
        /// </summary>
        public static string ToCurrencyString(this float val)
        {
            return String.Format("{0:C}", val);
        }
        /// <summary>
        /// Numeric Format convets numbers into comma seperated numeric string e.g. 450000 - 450,000.00
        /// </summary>
        public static string ToCurrencyString(this float? val)
        {
            if (val == null)
                return "";
            return String.Format("{0:C}", val);
        }

        /// <summary>
        /// Numeric Format convets numbers into comma seperated numeric string e.g. 450000 - 450,000.00
        /// </summary>
        public static string ToCurrencyString(this double val)
        {
            return String.Format("{0:C}", val);
        }

      
        public static string ToPrefixedString(this int num, string prefix)
        {
            return prefix + num.ToString();
        }
    }
}
