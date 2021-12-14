namespace TestChatTool.Domain.Extension
{
    public static class StringExtensions
    {
        public static bool IsNullOrWhiteSpace(this string into)
        {
            return string.IsNullOrWhiteSpace(into);
        }
    }
}
