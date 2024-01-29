namespace ExternalProcessor.Helper
{
    public static class ApplicationHelper
    {
        public static bool IsInteger(decimal value)
        {
            // Compares the rounded value with the original value
            return value == Math.Round(value);
        }
    }
}
