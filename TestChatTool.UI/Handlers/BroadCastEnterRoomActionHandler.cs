using Newtonsoft.Json;
using NLog;
using System;
using TestChatTool.Domain.Model;
using TestChatTool.UI.Forms;
using TestChatTool.UI.Handlers.Interface;

namespace TestChatTool.UI.Handlers
{
    public class BroadCastEnterRoomActionHandler : IActionHandler
    {
        /// <summary>
        /// 紀錄Log
        /// </summary>
        private readonly ILogger _logger;
        private readonly Room _room;
        private readonly Backstage _backstage;

        public BroadCastEnterRoomActionHandler(Room room, Backstage backstage)
        {
            _room = room;
            _backstage = backstage;
            _logger = LogManager.GetLogger("ChatToolUI");
        }

        public bool Execute(ActionModule actionModule)
        {
            try
            {
                var content = JsonConvert.DeserializeObject<BroadCastEnterRoomAction>(actionModule.Content);

                if (_room.User?.NickName != content.NickName)
                {
                    _room.BroadCastEnterRoom(content);
                }

                if (_backstage.Admin != null)
                {
                    _backstage.BroadCastEnterRoom(content);
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
