using System;
using Newtonsoft.Json;
using NLog;
using TestChatTool.Domain.Model;
using TestChatTool.UI.Forms;
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
        private readonly Room _room;
        private readonly Backstage _backstage;

        public BroadCastLeaveRoomActionHandler(Room room, Backstage backstage)
        {
            _room = room;
            _backstage = backstage;
            _logger = LogManager.GetLogger("ChatToolUI");
        }

        public bool Execute(ActionModule actionModule)
        {
            try
            {
                var content = JsonConvert.DeserializeObject<BroadCastLeaveRoomAction>(actionModule.Content);

                if (_room.User?.NickName != content.NickName)
                {
                    _room.BroadCastLeaveRoom(content);
                }

                if (_backstage.Admin != null)
                {
                    _backstage.BroadCastLeaveRoom(content);
                }

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
