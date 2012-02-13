using System.Text.RegularExpressions;

namespace System
{
    public static class Validation
    {

        /// <summary>
        /// 6-15 alphanumeric characters, and not containing "password"
        /// </summary>
        public static bool ValidatePassword(this string value)
        {
            Boolean result = false;
            if (!string.IsNullOrEmpty(value))
            {
                Regex Regex1 = new Regex(@"^[0-9A-Za-z]{6,15}$");
                result = Regex1.IsMatch(value);
                if (value.ToLower().Contains("password"))
                    result = false;
            }
            return result;
        }

        public static bool ValidateEmail(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Trim();
                Regex Regex1 = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
                return Regex1.IsMatch(value);
            }
            return false;
        }

        /// <summary>
        ///	Matches UK postcodes according to the following rules:
        ///	1. LN NLL (eg. N1 1AA)
        ///	2. LLN NLL (eg. SW4 0QL)
        ///	3. LNN NLL (eg. M23 4PJ)
        ///	4. LLNN NLL (eg. WS14 0JT)
        ///	5. LLNL NLL (eg. SW1N 4TB)
        ///	6. LNL NLL (eg. W1C 8LQ)
        /// 7. LLNL NLL (eg. WC9E 8LE).
        /// </summary>
        public static bool ValidateUKPostCode(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Trim();
                var Regex1 = new Regex(@"^[a-zA-Z]{1,2}[0-9][0-9A-Za-z]{0,1} {0,1}[0-9][A-Za-z]{2}$");
                return Regex1.IsMatch(value);
            }
            return false;
        }

        public static bool ValidateDateTime(this string value)
        {
            bool retval = true;
            try
            {
                Convert.ToDateTime(value);
            }
            catch { retval = false; }
            return retval;
        }

        public static bool ValidateMobileNumber(this string value)
        {
            if(!string.IsNullOrEmpty(value))
            {
                value = value.Replace(" ", "");
                value = value.Replace("-", "");
                value = value.Replace("+", "");
                
                value = value.Trim();

                var rx = new Regex(@"^[0-9]{11,12}$"); 

                return rx.IsMatch(value);
            }

            return false;
        }

    }
}
