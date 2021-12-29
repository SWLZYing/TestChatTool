using Newtonsoft.Json;
using NLog;
using System;
using TestChatTool.Domain.Model;

namespace TestChatTool.Service.ActionHandler
{
    public class SendDeleteChatRoomActionHandler : IActionHandler
    {
        private readonly ILogger _logger = LogManager.GetLogger("ChatToolServer");

        public (Exception exception, NotifyType notifyType, ActionBase actionBase) ExecuteAction(ActionModule action)
        {
            try
            {
                var content = JsonConvert.DeserializeObject<SendDeleteChatRoomAction>(action.Content);

                return (null, NotifyType.BroadCast, new BroadCastDeleteChatRoomAction());
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{GetType().Name} ExcuteAction Exception");
                return (ex, NotifyType.None, null);
            }
        }
    }
}
