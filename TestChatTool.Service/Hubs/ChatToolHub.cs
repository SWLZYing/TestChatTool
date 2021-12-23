using Autofac;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using NLog;
using System;
using System.Threading.Tasks;
using TestChatTool.Domain.Extension;
using TestChatTool.Domain.Model;
using TestChatTool.Service.ActionHandler;
using TestChatTool.Service.AppLibs;

namespace TestChatTool.Service.Hubs
{
    [HubName("ChatToolHub")]
    public class ChatToolHub : Hub
    {
        /// <summary>
        /// logger
        /// </summary>
        private static ILogger logger = LogManager.GetLogger("ChatToolServer");

        /// <summary>
        /// 連線時觸發
        /// </summary>
        /// <returns></returns>
        public override Task OnConnected()
        {
            logger.Info($"{Context.ConnectionId} Connected");
            return base.OnConnected();
        }

        /// <summary>
        /// 段線時觸發
        /// </summary>
        /// <param name="stopCalled"></param>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            logger.Info($"{Context.ConnectionId} Disconnected");
            return base.OnDisconnected(stopCalled);
        }

        /// <summary>
        /// 斷線重連時觸發
        /// </summary>
        /// <returns></returns>
        public override Task OnReconnected()
        {
            logger.Info($"{Context.ConnectionId} Reconnected");
            return base.OnReconnected();
        }

        /// <summary>
        /// 獲取action並執行
        /// </summary>
        /// <param name="action"></param>
        public void SendAction(ActionModule action)
        {
            logger.Trace($"{GetType().Name} SendAction Receipt: {JsonConvert.SerializeObject(action)}");

            try
            {
                using (var scope = AutofacConfig.Container.BeginLifetimeScope())
                {
                    var actionHandler = scope.ResolveNamed<IActionHandler>(action.Action.ToLower());
                    var excuteActionResult = actionHandler.ExecuteAction(action);

                    if (excuteActionResult.exception != null)
                    {
                        throw excuteActionResult.exception;
                    }

                    // 單一通知
                    if (excuteActionResult.notifyType == NotifyType.Single && excuteActionResult.actionBase != null)
                    {
                        Clients.Caller.BroadCastAction(new ActionModule()
                        {
                            Action = excuteActionResult.actionBase.Action(),
                            Content = excuteActionResult.actionBase.ToString()
                        });
                    }
                    // 全域廣播
                    else if (excuteActionResult.notifyType == NotifyType.BroadCast && excuteActionResult.actionBase != null)
                    {
                        Clients.All.BroadCastAction(new ActionModule()
                        {
                            Action = excuteActionResult.actionBase.Action(),
                            Content = excuteActionResult.actionBase.ToString()
                        });
                    }
                }
            }
            //// 忽略沒註冊的項目
            catch (Autofac.Core.Registration.ComponentNotRegisteredException)
            {
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{GetType().Name} SendAction Exception");
            }
        }
    }
}
