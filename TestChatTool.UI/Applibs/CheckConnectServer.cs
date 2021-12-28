using Autofac;
using NLog;
using System.Threading;
using System.Threading.Tasks;
using TestChatTool.Domain.Model;
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
                var hubClient = scope.Resolve<IHubClient>();

                Task.Run(() =>
                {
                    while (true)
                    {
                        hubClient.SendAction(new BroadCastCheckConnectAction
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
