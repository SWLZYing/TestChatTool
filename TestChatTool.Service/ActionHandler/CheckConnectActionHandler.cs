using Newtonsoft.Json;
using NLog;
using System;
using TestChatTool.Domain.Model;
using TestChatTool.Domain.Repository;

namespace TestChatTool.Service.ActionHandler
{
    public class CheckConnectActionHandler : IActionHandler
    {
        private readonly ILogger _logger;
        private readonly IOnLineUserRepository _onLineUserRepository;

        public CheckConnectActionHandler(IOnLineUserRepository onLineUserRepository)
        {
            _onLineUserRepository = onLineUserRepository;
            _logger = LogManager.GetLogger("ChatToolServer");
        }

        public (Exception exception, NotifyType notifyType, ActionBase actionBase) ExecuteAction(ActionModule action)
        {
            try
            {
                var content = JsonConvert.DeserializeObject<CheckConnectAction>(action.Content);

                var upsert = _onLineUserRepository.Upsert(new OnLineUser
                {
                    Account = content.Account,
                    NickName = content.NickName,
                    RoomCode = content.RoomCode,
                });

                if (upsert.ex != null)
                {
                    _logger.Error(upsert.ex, $"{GetType().Name} Upsert Exception");
                    return (upsert.ex, NotifyType.None, null);
                }

                return (null, NotifyType.None, null);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{GetType().Name} ExcuteAction Exception");
                return (ex, NotifyType.None, null);
            }
        }
    }
}
