using Autofac;
using NLog;
using System.Threading;
using System.Threading.Tasks;
using TestChatTool.Domain.Enum;
using TestChatTool.UI.Events.Interface;
using TestChatTool.UI.Models;

namespace TestChatTool.UI.Applibs
{
    internal static class CheckConnectServer
    {
        private static readonly ILogger _logger = LogManager.GetLogger(nameof(CheckConnectServer));

        public static void ServerStart()
        {
            using (var scope = AutofacConfig.Container.BeginLifetimeScope())
            {
                var callBackEvent = scope.Resolve<ICallBackEventHandler>();

                Task.Run(() =>
                {
                    while (true)
                    {
                        SpinWait.SpinUntil(() => false, 30000);

                        callBackEvent.DoWork(new CallBackEventData { Action = CallBackActionType.CheckConnect });
                    }
                });
            }

            _logger.Info($"{nameof(CheckConnectServer)} Start.");
        }
    }
}
