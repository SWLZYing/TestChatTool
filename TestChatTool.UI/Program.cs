using Autofac;
using System;
using System.Windows.Forms;
using TestChatTool.UI.Applibs;
using TestChatTool.UI.Forms;

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

            using (var scope = AutofacConfig.Container.BeginLifetimeScope())
            {
                var home = scope.Resolve<Home>();
                home.Scope = scope;

                home.ShowDialog();
            }
        }
    }
}
