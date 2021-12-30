using Newtonsoft.Json;
using NLog;
using System;
using TestChatTool.Domain.Model;
using TestChatTool.Service.ActionHandler.Interface;
using TestChatTool.Service.Enums;

namespace TestChatTool.Service.ActionHandler
{
    public class SendUpsertChatRoomActionHandler : IActionHandler
    {
        private readonly ILogger _logger = LogManager.GetLogger("ChatToolServer");

        public (Exception exception, NotifyType notifyType, ActionBase actionBase) ExecuteAction(ActionModule action)
        {
            try
            {
                var content = JsonConvert.DeserializeObject<SendUpsertChatRoomAction>(action.Content);

                return (null, NotifyType.BroadCast, new BroadCastUpsertChatRoomAction());
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{GetType().Name} ExcuteAction Exception");
                return (ex, NotifyType.None, null);
            }
        }
    }
}
