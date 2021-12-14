using Autofac;
using NLog;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;
using TestChatTool.Domain.Repository;

namespace TestChatTool.Service.AppLibs
{
    internal static class ChatToolServer
    {
        private static ILogger _logger = LogManager.GetLogger(nameof(ChatToolServer));

        public static void ServerStart()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    DrawView();
                    SpinWait.SpinUntil(() => false, 1000);
                }
            });

            _logger.Info($"{nameof(ChatToolServer)} Start.");
        }

        public static void ServerStop()
        {
            _logger.Info($"{nameof(ChatToolServer)} Stop.");
        }

        /// <summary>
        /// 畫畫面
        /// </summary>
        public static void DrawView()
        {
            Console.Clear();
            Console.WriteLine($"Listen On:{ConfigHelper.ServiceUrl}");

            using (var scope = AutofacConfig.Container.BeginLifetimeScope())
            {
                var repo = scope.Resolve<IOnLineUserRepository>();
                var result = repo.FindAllUserCountByRoom();

                if (result.ex != null)
                {
                    _logger.Error(result.ex, $"FindAllUserCountByRoom Exception");
                }

                var joinString = result.result.Count > 0
                    ? string.Join(", ", result.result.Select(s => $"{s.Item1}:{s.Item2}"))
                    : "No Any User.";

                Console.WriteLine("Room's User Count");
                Console.WriteLine($"  {joinString}");
            }

            Console.WriteLine($"Current Memory Usage:{(int)((GC.GetTotalMemory(true) / 1024f))}(KB)");
            Console.WriteLine($"Process Memory Usage:{(int)((Process.GetCurrentProcess().PrivateMemorySize64 / 1024f))}(KB)");
            Console.WriteLine($"Memory Cache Usage:{(int)(MemoryCache.Default.GetApproximateSize() / 1024f)}(KB)");
            Console.WriteLine($"Handle count:{Process.GetCurrentProcess().HandleCount}");
            Console.WriteLine($"{DateTime.Now}");
        }
    }
}
