namespace Sula.Core.Extensions
{
    public static class StringExtensions
    {
        public static string TrimUrl(this string value)
        {
            if (value.EndsWith("/"))
            {
                return value.Substring(0, value.Length - 1);
            }

            return value;
        }
    }
}