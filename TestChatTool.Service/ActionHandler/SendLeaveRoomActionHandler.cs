using Newtonsoft.Json;
using NLog;
using System;
using TestChatTool.Domain.Model;
using TestChatTool.Service.ActionHandler.Interface;
using TestChatTool.Service.Enums;

namespace TestChatTool.Service.ActionHandler
{
    public class SendLeaveRoomActionHandler : IActionHandler
    {
        private ILogger logger = LogManager.GetLogger("ChatToolServer");

        public (Exception exception, NotifyType notifyType, ActionBase actionBase) ExecuteAction(ActionModule action)
        {
            try
            {
                var content = JsonConvert.DeserializeObject<SendLeaveRoomAction>(action.Content);

                return (null, NotifyType.BroadCast, new BroadCastLeaveRoomAction
                {
                    RoomCode = content.RoomCode,
                    Account = content.Account,
                    NickName = content.NickName,
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
