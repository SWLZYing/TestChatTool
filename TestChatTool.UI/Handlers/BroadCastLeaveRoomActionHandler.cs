using System;
using Newtonsoft.Json;
using NLog;
using TestChatTool.Domain.Enum;
using TestChatTool.Domain.Model;
using TestChatTool.UI.Events.Interface;
using TestChatTool.UI.Handlers.Interface;

namespace TestChatTool.UI.Handlers
{
    /// <summary>
    /// 使用者登出通知
    /// </summary>
    public class BroadCastLeaveRoomActionHandler : IActionHandler
    {
        /// <summary>
        /// 紀錄Log
        /// </summary>
        private readonly ILogger _logger;
        private readonly ICallBackEventHandler _callBack;

        public BroadCastLeaveRoomActionHandler(ICallBackEventHandler callBack)
        {
            _callBack = callBack;
            _logger = LogManager.GetLogger("ChatToolUI");
        }

        public bool Execute(ActionModule actionModule)
        {
            try
            {
                var content = JsonConvert.DeserializeObject<BroadCastLeaveRoomAction>(actionModule.Content);

                _callBack.DoWork(new CallBackEventData
                {
                    Action = CallBackActionType.LeaveRoom,
                    RoomCode = content.RoomCode,
                    Account = content.Account,
                    NickName = content.NickName,
                });

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{GetType().Name} Excute Exception");
                return false;
            }
        }
    }
}
