using System.Diagnostics.CodeAnalysis;

namespace GameBarWidget.Utilities
{
    public class IntegerUtil
    {
        [SuppressMessage("ReSharper", "ConvertIfStatementToReturnStatement")]
        public static string GetOrdinalSuffix(int num)
        {
            if (num.ToString().EndsWith("11")) return "th";
            if (num.ToString().EndsWith("12")) return "th";
            if (num.ToString().EndsWith("13")) return "th";
            if (num.ToString().EndsWith("1")) return "st";
            if (num.ToString().EndsWith("2")) return "nd";
            if (num.ToString().EndsWith("3")) return "rd";

            return "th";
        }
    }
}