using System;
using Newtonsoft.Json;
using NLog;
using TestChatTool.Domain.Model;
using TestChatTool.UI.Forms;
using TestChatTool.UI.Handlers.Interface;

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
        private ILogger _logger = LogManager.GetLogger("ChatToolUI");

        private Room _room;
        private Backstage _backstage;

        public BroadCastChatMessageActionHandler(Room room, Backstage backstage)
        {
            _room = room;
            _backstage = backstage;
        }

        public bool Execute(ActionModule actionModule)
        {
            try
            {
                var roomCode = actionModule.Action.Split('_')[0];
                var content = JsonConvert.DeserializeObject<BroadCastChatMessageAction>(actionModule.Content);
                _room.ChatMessageAppend(roomCode, content);
                _backstage.ChatMessageAppend(roomCode, content);
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
