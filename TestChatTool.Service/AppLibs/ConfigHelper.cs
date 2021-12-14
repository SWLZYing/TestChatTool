namespace TestChatTool.Service.AppLibs
{
    internal static class ConfigHelper
    {
        public static string ServiceUrl => $"http://localhost:9001";

        public static string MongoDbContext => "mongodb://localhost:27017";
    }
}
