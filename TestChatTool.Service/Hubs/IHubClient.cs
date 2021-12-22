using TestChatTool.Domain.Model;

namespace TestChatTool.Service.Hubs
{
    /// <summary>
    /// hubclient 介面
    /// </summary>
    public interface IHubClient
    {
        /// <summary>
        /// 廣撥用
        /// </summary>
        void BroadCastAction<A>(A act)
            where A : ActionBase;
    }
}
