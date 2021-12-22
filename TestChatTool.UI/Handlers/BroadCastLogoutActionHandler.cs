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
    public class BroadCastLogoutActionHandler : IActionHandler
    {
        /// <summary>
        /// 紀錄Log
        /// </summary>
        private ILogger _logger = LogManager.GetLogger("ChatToolUI");

        private Room _room;
        private Backstage _backstage;

        public BroadCastLogoutActionHandler(Room room, Backstage backstage)
        {
            _room = room;
            _backstage = backstage;
        }

        public bool Execute(ActionModule actionModule)
        {
            try
            {
                var content = JsonConvert.DeserializeObject<BroadCastLogoutAction>(actionModule.Content);

                if (_room.User?.NickName == content.NickName)
                {
                    // 使用者登出後續事項
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
