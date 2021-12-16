using Autofac;
using NLog;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;
using TestChatTool.Domain.Model;
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
                CheckBasicSettingExist();

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

        private static void CheckBasicSettingExist()
        {
            using (var scope = AutofacConfig.Container.BeginLifetimeScope())
            {
                // 確認管理員
                var adminRepo = scope.Resolve<IAdminRepository>();
                var adminQ = adminRepo.Query("admin");

                if (adminQ.ex != null)
                {
                    _logger.Error(adminQ.ex, $"QueryAdmin Exception");
                }

                if (adminQ.result == null)
                {
                    var pwd = "admin";
                    var admin = Admin.GenerateInstance("admin", pwd);

                    var adminC = adminRepo.Create(admin);

                    if (adminC.ex != null)
                    {
                        _logger.Error(adminC.ex, $"CreateAdmin Exception");
                    }
                }

                // 確認大廳
                var roomRepo = scope.Resolve<IChatRoomRepository>();
                var roomQ = roomRepo.Query("HALL");

                if (roomQ.ex != null)
                {
                    _logger.Error(roomQ.ex, $"QueryChatRoom Exception");
                }

                if (roomQ.result == null)
                {
                    var admin = ChatRoom.GenerateInstance("HALL", "大廳");

                    var roomC = roomRepo.Create(admin);

                    if (roomC.ex != null)
                    {
                        _logger.Error(roomC.ex, $"CreateChatRoom Exception");
                    }
                }
            }
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
