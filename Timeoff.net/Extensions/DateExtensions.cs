namespace Timeoff
{
    public static class DateExtensions
    {
        public static string ToJsFormat(this string format)
        {
            return format.Replace("MM", "mm");
        }
    }
}