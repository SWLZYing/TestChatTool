using System;
using Newtonsoft.Json;
using NLog;
using TestChatTool.Domain.Model;
using TestChatTool.Service.ActionHandler.Interface;
using TestChatTool.Service.Enums;

namespace TestChatTool.Service.ActionHandler
{
    /// <summary>
    /// 發送聊天訊息處理
    /// </summary>
    public class SendChatMessageActionHandler : IActionHandler
    {
        private ILogger logger = LogManager.GetLogger("ChatToolServer");

        public (Exception exception, NotifyType notifyType, ActionBase actionBase) ExecuteAction(ActionModule action)
        {
            try
            {
                var content = JsonConvert.DeserializeObject<SendChatMessageAction>(action.Content);

                return (null, NotifyType.BroadCast, new BroadCastChatMessageAction()
                {
                    RoomCode = content.RoomCode,
                    NickName = content.NickName,
                    Message = content.Message,
                    CreateDateTime = content.CreateDateTime
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
