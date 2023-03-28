namespace GlobalUtilities.Extensions
{
    public static class StringHelper
    {
        public static String EmptyStringIfNull(this String value)
        {
            if (value == null)
            {
                return String.Empty;
            }

            return value;
        }
    }
}
