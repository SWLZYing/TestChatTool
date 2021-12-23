using System.Collections.Generic;

namespace TestChatTool.UI.Handlers.Interface
{
    public interface IHttpHandler
    {
        /// <summary>
        /// Http Post
        /// </summary>
        /// <param name="action">[controller]/[action]</param>
        /// <param name="parameters">參數Dictionary</param>
        /// <returns></returns>
        string CallApiPost(string action, Dictionary<string, object> parameters);

        /// <summary>
        /// Http Get
        /// </summary>
        /// <param name="action">[controller]/[action]</param>
        /// <param name="parameters">參數Dictionary</param>
        /// <returns></returns>
        string CallApiGet(string action, Dictionary<string, object> parameters);

        /// <summary>
        /// Http Put
        /// </summary>
        /// <param name="action">[controller]/[action]</param>
        /// <param name="parameters">參數Dictionary</param>
        /// <returns></returns>
        string CallApiPut(string action, Dictionary<string, object> parameters);
    }
}
