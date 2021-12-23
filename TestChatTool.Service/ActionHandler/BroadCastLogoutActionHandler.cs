using Newtonsoft.Json;
using NLog;
using System;
using TestChatTool.Domain.Model;

namespace TestChatTool.Service.ActionHandler
{
    public class BroadCastLogoutActionHandler : IActionHandler
    {
        private ILogger logger = LogManager.GetLogger("ChatToolServer");

        public (Exception exception, NotifyType notifyType, ActionBase actionBase) ExecuteAction(ActionModule action)
        {
            try
            {
                var content = JsonConvert.DeserializeObject<BroadCastLogoutAction>(action.Content);

                return (null, NotifyType.BroadCast, new BroadCastLogoutAction()
                {
                    NickName = content.NickName,
                    RoomCode = content.RoomCode,
                });
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{GetType().Name} ExcuteAction Exception");
                return (ex, NotifyType.None, null);
            }
        }
    }
}
