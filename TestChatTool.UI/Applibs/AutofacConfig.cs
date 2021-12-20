using Autofac;
using System.Reflection;
using TestChatTool.UI.Forms;
using TestChatTool.UI.Handlers;
using TestChatTool.UI.Handlers.Interface;

namespace TestChatTool.UI.Applibs
{
    internal static class AutofacConfig
    {
        /// <summary>
        /// autofac 裝載介面跟實作的容器
        /// </summary>
        private static IContainer container;

        public static IContainer Container
        {
            get
            {
                if (container == null)
                {
                    Register();
                }

                return container;
            }
        }

        /// <summary>
        /// 註冊容器
        /// </summary>
        public static void Register()
        {
            var builder = new ContainerBuilder();
            var asm = Assembly.GetExecutingAssembly();

            //// 取出當前執行assembly, 讓繼承IActionHandler且名稱結尾為ActionHandler的對應事件名稱
            //// ex LoginResultAction對應的是LoginResultActionHandler
            //builder.RegisterAssemblyTypes(asm)
            //    .Where(t => t.IsAssignableTo<IActionHandler>())
            //    .Named<IActionHandler>(t => t.Name.Replace("ActionHandler", string.Empty).ToLower())
            //    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
            //    .SingleInstance();

            //builder.RegisterType<HubClient>()
            //    .WithParameter("url", ConfigHelper.SignalrUrl)
            //    .WithParameter("hubName", ConfigHelper.SignalrHubName)
            //    .As<IHubClient>()
            //    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
            //    .SingleInstance();

            builder.RegisterType<HttpHandler>()
                .WithParameter("serviceUrl", ConfigHelper.ServiceUrl)
                .As<IHttpHandler>()
                .SingleInstance();

            builder.RegisterType<Home>()
                .SingleInstance();

            builder.RegisterType<Register>()
                .SingleInstance();

            builder.RegisterType<ChangePwd>()
                .SingleInstance();

            builder.RegisterType<Backstage>()
                .SingleInstance();

            builder.RegisterType<UserMaintain>()
                .SingleInstance();

            builder.RegisterType<Room>()
                .SingleInstance();

            container = builder.Build();
        }
    }
}
