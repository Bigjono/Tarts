using System;
using System.Reflection;

namespace Bronson.Utils
{
    public class EnumHelpers
    {

        /// <summary>
        /// Returns true is flagged enum has value passed
        /// </summary>
        public static bool HasState<T>(T tPropertyValue, T tValueToCheck)
        {
            return ((Convert.ToInt64(tPropertyValue) & (Convert.ToInt64(tValueToCheck))) == Convert.ToInt64(tValueToCheck));
        }

        /// <summary>
        /// Removes single or multiple states from flagged enum
        /// </summary>
        public static long RemoveState<T>(T tOriginalValue, T tBitToSet) where T : struct
        {
            if (HasState<T>(tOriginalValue, tBitToSet))
            {
                return ((Convert.ToInt64(tOriginalValue)) ^ (Convert.ToInt64(tBitToSet)));
            }
            else
                return Convert.ToInt64(tOriginalValue); // nothing to do
             
        }

        /// <summary>
        /// Sets single or multiple states on flagged enum
        /// </summary>
        public static long SetState<T>(T tOriginalValue, T tBitToSet) where T : struct
        {
            if (!HasState<T>(tOriginalValue, tBitToSet))
            {
                return (Convert.ToInt64(tOriginalValue) | (Convert.ToInt64(tBitToSet)));
            }
            else
                return Convert.ToInt64(tOriginalValue); // nothing to do

        }

        public static string GetDescription(object obj)
        {
            Enum en = obj as Enum;
            if (en == null)
                return obj.ToString();
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(EnumDescription), false);
                if (attrs != null && attrs.Length > 0)
                    return ((EnumDescription)attrs[0]).Text;
            }
            return en.ToString();

        }
    }

    public class EnumDescription : Attribute
    {
        public string Text { get; set; }
        public EnumDescription(string text)
        {
            Text = text;
        }
    }
}
