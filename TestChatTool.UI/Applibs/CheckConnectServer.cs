using Autofac;
using Microsoft.AspNet.SignalR.Client;
using NLog;
using System.Threading;
using System.Threading.Tasks;
using TestChatTool.Domain.Enum;
using TestChatTool.Domain.Model;
using TestChatTool.UI.Events.Interface;
using TestChatTool.UI.Models;
using TestChatTool.UI.SignalR;

namespace TestChatTool.UI.Applibs
{
    internal static class CheckConnectServer
    {
        private static readonly ILogger _logger = LogManager.GetLogger(nameof(CheckConnectServer));

        public static void ServerStart(bool isAdmin, string account, string nickName = "")
        {
            using (var scope = AutofacConfig.Container.BeginLifetimeScope())
            {
                var callBackEvent = scope.Resolve<ICallBackEventHandler>();
                var hubClient = scope.Resolve<IHubClient>();

                Task.Run(() =>
                {
                    while (true)
                    {
                        hubClient.SendAction(new CheckConnectAction
                        {
                            IsAdmin = isAdmin,
                            Account = account,
                            NickName = nickName,
                        });

                        SpinWait.SpinUntil(() => false, 30000);
                    }
                });
            }

            _logger.Info($"{nameof(CheckConnectServer)} Start.");
        }
    }
}
