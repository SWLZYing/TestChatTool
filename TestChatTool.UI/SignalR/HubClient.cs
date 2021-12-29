using System;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Microsoft.AspNet.SignalR.Client;
using NLog;
using TestChatTool.Domain.Enum;
using TestChatTool.Domain.Model;
using TestChatTool.UI.Events.Interface;
using TestChatTool.UI.Handlers.Interface;
using TestChatTool.UI.Models;

namespace TestChatTool.UI.SignalR
{

    /// <summary>
    /// hubclient 實作
    /// </summary>
    public class HubClient : IHubClient
    {
        /// <summary>
        /// 連結狀態
        /// </summary>
        public ConnectionState State
            => hubConnection?.State ?? ConnectionState.Disconnected;

        /// <summary>
        /// 紀錄Log
        /// </summary>
        private ILogger logger = LogManager.GetLogger("ChatToolUI");

        /// <summary>
        /// HubClient連線物件
        /// </summary>
        private HubConnection hubConnection;

        /// <summary>
        /// HubProxy
        /// </summary>
        private IHubProxy hubProxy;

        /// <summary>
        /// 連線字串
        /// </summary>
        private string url;

        /// <summary>s
        /// HubName
        /// </summary>
        private string hubName;

        /// <summary>
        /// IOC 關連表
        /// </summary>
        private IIndex<string, IActionHandler> handlerSets;

        /// <summary>
        /// CallBackEvent
        /// </summary>
        private readonly ICallBackEventHandler callBackEvent;

        public HubClient(
            string url,
            string hubName,
            IIndex<string, IActionHandler> handlerSets,
            ICallBackEventHandler callBackEvent)
        {
            this.url = url;
            this.hubName = hubName;
            this.handlerSets = handlerSets;
            this.callBackEvent = callBackEvent;
        }

        /// <summary>
        /// 發送action
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="act"></param>
        public void SendAction<T>(T act) where T : ActionBase
        {
            var sendAction = new ActionModule()
            {
                Action = act.Action(),
                Content = act.ToString()
            };

            if (State != ConnectionState.Connected)
            {
                logger.Warn($"{GetType().Name} SendAction State:{State}, Action: {sendAction.Action}, Content: {sendAction.Content}");
                return;
            }

            hubProxy.Invoke<ActionModule>("SendAction", sendAction).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    logger.Error(task.Exception, $"{GetType().Name} SendAction Fail Action: {sendAction.Action}, Content: {sendAction.Content}");
                }
                else
                {
                    logger.Trace($"{GetType().Name} SendAction >> Action: {sendAction.Action}, Content: {sendAction.Content}");
                }
            });
        }

        /// <summary>
        /// 啟動hubClient
        /// </summary>
        public async Task StartAsync()
        {
            hubConnection = new HubConnection(url);
            hubConnection.TransportConnectTimeout = TimeSpan.FromSeconds(30);
            hubConnection.Error += HubConnection_Error;
            hubConnection.StateChanged += HubConnection_StateChanged;
            hubProxy = hubConnection.CreateHubProxy(hubName);
            hubProxy.On<ActionModule>("BroadCastAction", action => BroadCastAction(action));
            // 連線開啟
            await hubConnection.Start().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    logger.Error(task.Exception, $"{GetType().Name} HubConnection 啟動失敗");
                }
                else
                {

                }
            });
        }

        /// <summary>
        /// 監聽 BroadCastAction
        /// </summary>
        /// <param name="action"></param>
        private void BroadCastAction(ActionModule action)
        {
            try
            {
                logger.Trace($"{GetType().Name} BroadCastAction Action: {action.Action}, Content: {action.Content}");

                if (handlerSets.TryGetValue(action.Action.ToLower(), out var handler))
                {
                    handler.Execute(action);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{GetType()} BroadCastAction Exception");
            }
        }

        /// <summary>
        /// 狀態切換
        /// </summary>
        /// <param name="obj"></param>
        private void HubConnection_StateChanged(StateChange obj)
        {
            // 變更UI狀態
            callBackEvent.DoWork(new CallBackEventData { Action = CallBackActionType.CheckConnect });
        }

        /// <summary>
        /// signalr連線上遇到錯誤
        /// </summary>
        /// <param name="obj"></param>
        private void HubConnection_Error(Exception obj)
        {
            logger.Error(obj, $"{GetType().Name} HubConnection_Error");
        }
    }
}
