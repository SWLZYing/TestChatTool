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

            ServicePointManager.DefaultConnectionLimit = 50;

            using (var scope = AutofacConfig.Container.BeginLifetimeScope())
            {
                var hubClient = scope.Resolve<IHubClient>();
                hubClient.StartAsync();

                var home = scope.Resolve<Home>();
                home.Scope = scope;

                if (home.ShowDialog() == DialogResult.OK)
                {
                    if (home.IsAdmin)
                    {
                        var backstage = scope.Resolve<Backstage>();

                        backstage.Scope = scope;
                        backstage.SetUpUI(home.Admin); // 層級為Normal 不顯示創建按鍵
                        backstage.ShowDialog();
                    }
                    else
                    {
                        var room = scope.Resolve<Room>();

                        room.SetUpUI(home.User);
                        room.ShowDialog();
                    }
                }
            }
        }
    }
}
