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
    public class BroadCastDeleteChatRoomActionHandler : IActionHandler
    {
        /// <summary>
        /// 紀錄Log
        /// </summary>
        private readonly ILogger _logger;
        private readonly ICallBackEventHandler _callBack;

        public BroadCastDeleteChatRoomActionHandler(ICallBackEventHandler callBack)
        {
            _callBack = callBack;
            _logger = LogManager.GetLogger("ChatToolUI");
        }

        public bool Execute(ActionModule actionModule)
        {
            try
            {
                var content = JsonConvert.DeserializeObject<BroadCastDeleteChatRoomAction>(actionModule.Content);

                _callBack.DoWork(new CallBackEventData
                {
                    Action = CallBackActionType.DeleteChatRoom,
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
