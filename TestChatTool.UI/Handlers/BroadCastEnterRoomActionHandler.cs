using Newtonsoft.Json;
using NLog;
using System;
using TestChatTool.Domain.Enum;
using TestChatTool.Domain.Model;
using TestChatTool.UI.Events.Interface;
using TestChatTool.UI.Handlers.Interface;
using TestChatTool.UI.Models;

namespace TestChatTool.UI.Handlers
{
    public class BroadCastEnterRoomActionHandler : IActionHandler
    {
        /// <summary>
        /// 紀錄Log
        /// </summary>
        private readonly ILogger _logger;
        private readonly ICallBackEventHandler _callBack;

        public BroadCastEnterRoomActionHandler(ICallBackEventHandler callBack)
        {
            _callBack = callBack;
            _logger = LogManager.GetLogger("ChatToolUI");
        }

        public bool Execute(ActionModule actionModule)
        {
            try
            {
                var content = JsonConvert.DeserializeObject<BroadCastEnterRoomAction>(actionModule.Content);

                _callBack.DoWork(new CallBackEventData
                {
                    Action = CallBackActionType.EnterRoom,
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
