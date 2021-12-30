using Autofac;
using System;
using System.Net;
using System.Windows.Forms;
using TestChatTool.UI.Applibs;
using TestChatTool.UI.Forms;
using TestChatTool.UI.SignalR;

namespace TestChatTool.UI
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 設定基本連線數
            ServicePointManager.DefaultConnectionLimit = 50;

            using (var scope = AutofacConfig.Container.BeginLifetimeScope())
            {
                // 啟動長連結
                var hubClient = scope.Resolve<IHubClient>();
                hubClient.StartAsync();

                var home = scope.Resolve<Home>();

                if (home.ShowDialog() == DialogResult.OK)
                {
                    if (home.IsAdmin)
                    {
                        var backstage = scope.Resolve<Backstage>();
                        backstage.SetUpUI(home.Admin);
                        backstage.ShowDialog();
                    }
                    else
                    {
                        // 使用者登入成功啟動HC
                        CheckConnectServer.ServerStart();

                        var room = scope.Resolve<Room>();
                        room.SetUpUI(home.User);
                        room.ShowDialog();
                    }
                }
            }
        }
    }
}
