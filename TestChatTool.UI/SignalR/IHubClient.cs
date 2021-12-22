using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using TestChatTool.Domain.Model;

namespace TestChatTool.UI.SignalR
{
    /// <summary>
    /// hubclient 介面
    /// </summary>
    public interface IHubClient
    {
        /// <summary>
        /// 連結狀態
        /// </summary>
        ConnectionState State { get; }

        /// <summary>
        /// 發送action
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="act"></param>
        void SendAction<T>(T act) where T : ActionBase;

        /// <summary>
        /// 啟動hubClient
        /// </summary>
        Task StartAsync();
    }
}
