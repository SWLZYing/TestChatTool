using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TestChatTool.UI.Forms;
using TestChatTool.UI.Handlers;
using TestChatTool.UI.Handlers.Interface;
using TestChatTool.UI.Helpers;
using TestChatTool.UI.Helpers.Interface;
using TestChatTool.UI.SignalR;

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
            // Create a service collection
            // 注入IHttpClientFactory
            var services = new ServiceCollection();
            services.AddHttpClient();

            var providerFactory = new AutofacServiceProviderFactory();
            var builder = providerFactory.CreateBuilder(services);
            var asm = Assembly.GetExecutingAssembly();

            // 取出當前執行assembly, 讓繼承IActionHandler且名稱結尾為ActionHandler的對應事件名稱
            builder.RegisterAssemblyTypes(asm)
                .Where(t => t.IsAssignableTo<IActionHandler>())
                .Named<IActionHandler>(t => t.Name.Replace("ActionHandler", string.Empty).ToLower())
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .SingleInstance();

            builder.RegisterType<HubClient>()
                .WithParameter("url", ConfigHelper.SignalrUrl)
                .WithParameter("hubName", ConfigHelper.SignalrHubName)
                .As<IHubClient>()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .SingleInstance();

            // 注入短連結物件ApiHelper
            builder.RegisterType<AdminControllerApiHelper>()
                .As<IAdminControllerApiHelper>()
                .SingleInstance();

            builder.RegisterType<SignControllerApiHelper>()
                .As<ISignControllerApiHelper>()
                .SingleInstance();

            builder.RegisterType<UserControllerApiHelper>()
                .As<IUserControllerApiHelper>()
                .SingleInstance();

            builder.RegisterType<OnLineUserControllerApiHelper>()
                .As<IOnLineUserControllerApiHelper>()
                .SingleInstance();

            builder.RegisterType<ChatRoomControllerApiHelper>()
                .As<IChatRoomControllerApiHelper>()
                .SingleInstance();

            builder.RegisterType<HttpHandler>()
                .WithParameter("serviceUrl", ConfigHelper.ServiceUrl)
                .As<IHttpHandler>()
                .SingleInstance();

            // UI註冊
            builder.RegisterType<Home>()
                .SingleInstance();

            builder.RegisterType<Register>()
                .SingleInstance();

            builder.RegisterType<ChangePwd>()
                .SingleInstance();

            builder.RegisterType<Backstage>()
                .SingleInstance();

            builder.RegisterType<UserStatusMaintain>()
                .SingleInstance();

            builder.RegisterType<RoomMaintain>()
                .SingleInstance();

            builder.RegisterType<Room>()
                .SingleInstance();

            container = builder.Build();
        }
    }
}
