using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TestChatTool.UI.Events;
using TestChatTool.UI.Events.Interface;
using TestChatTool.UI.Forms;
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

            // 註冊EventHandler
            builder.RegisterType<CallBackEventHandler>()
                .As<ICallBackEventHandler>()
                .SingleInstance();

            // 註冊短連結物件 ApiHelper
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

            // UI註冊
            builder.RegisterType<Home>()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .SingleInstance();

            builder.RegisterType<Register>()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .SingleInstance();

            builder.RegisterType<ChangePwd>()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .SingleInstance();

            builder.RegisterType<Backstage>()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .SingleInstance();

            builder.RegisterType<UserStatusMaintain>()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .SingleInstance();

            builder.RegisterType<RoomMaintain>()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .SingleInstance();

            builder.RegisterType<Room>()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .SingleInstance();

            container = builder.Build();
        }
    }
}
