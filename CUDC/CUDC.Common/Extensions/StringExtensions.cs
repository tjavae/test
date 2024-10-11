namespace CUDC.Common.Extensions
{
    public static class StringExtensions
    {
        public static string Left(this string str, int length)
        {
            if (length < str?.Length)
            {
                return str[..length];
            }
            return str;
        }

        public static string Right(this string str, int length)
        {
            if (length < str?.Length)
            {
                return str[^length..];
            }
            return str;
        }
    }
}
