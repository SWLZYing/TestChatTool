using System.Collections.Generic;

namespace TestChatTool.UI.Handlers.Interface
{
    public interface IHttpHandler
    {
        string CallApiPost(string action, Dictionary<string, object> parameters);
        string CallApiPut(string action, Dictionary<string, object> parameters);
    }
}
