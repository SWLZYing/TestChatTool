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

                home.ShowDialog();
            }
        }
    }
}
