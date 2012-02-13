using Bronson.Utils;

namespace System
{
    public static class Enums
    {
        public static string ToEnumDescription(this object en)
        {
            return EnumHelpers.GetDescription(en);
        }
    }
}
