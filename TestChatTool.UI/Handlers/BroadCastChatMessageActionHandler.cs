using System;
using Newtonsoft.Json;
using NLog;
using TestChatTool.Domain.Enum;
using TestChatTool.Domain.Model;
using TestChatTool.UI.Events.Interface;
using TestChatTool.UI.Handlers.Interface;
using TestChatTool.UI.Models;

namespace TestChatTool.UI.Handlers
{
    /// <summary>
    /// 廣播聊天訊息處理
    /// </summary>
    public class BroadCastChatMessageActionHandler : IActionHandler
    {
        /// <summary>
        /// 紀錄Log
        /// </summary>
        private readonly ILogger _logger;
        private readonly ICallBackEventHandler _callBack;

        public BroadCastChatMessageActionHandler(ICallBackEventHandler callBack)
        {
            _callBack = callBack;
            _logger = LogManager.GetLogger("ChatToolUI");
        }

        public bool Execute(ActionModule actionModule)
        {
            try
            {
                var content = JsonConvert.DeserializeObject<BroadCastChatMessageAction>(actionModule.Content);

                _callBack.DoWork(new CallBackEventData
                {
                    Action = CallBackActionType.ChatMessage,
                    RoomCode = content.RoomCode,
                    NickName = content.NickName,
                    Message = content.Message,
                    CreateDateTime = content.CreateDateTime,
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
