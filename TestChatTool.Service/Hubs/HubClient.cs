using System;
using System.Threading;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using NLog;
using TestChatTool.Domain.Model;

namespace TestChatTool.Service.Hubs
{
    /// <summary>
    /// hubclient 實作
    /// </summary>
    public class HubClient : IHubClient
    {
        private ILogger logger = LogManager.GetLogger("ChatToolServer");

        private IHubContext hubContext
        {
            get
                => GlobalHost.ConnectionManager.GetHubContext<ChatToolHub>();
        }

        /// <summary>
        /// 廣撥用
        /// </summary>
        public void BroadCastAction<A>(A act) where A : ActionBase
        {
            var sendAction = new ActionModule()
            {
                Action = act.Action(),
                Content = act.ToString()
            };

            try
            {
                logger.Trace($"{GetType().Name} BroadCastAction: {JsonConvert.SerializeObject(sendAction)}");
                hubContext.Clients.All.BroadCastAction(sendAction);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{GetType().Name} BroadCastAction Exception");
                bool runing = true;
                while (runing)
                {
                    SpinWait.SpinUntil(() => runing = false, 500);
                }

                hubContext.Clients.All.BroadCastAction(sendAction);
            }
        }
    }
}
