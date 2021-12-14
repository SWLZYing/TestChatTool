using Autofac;
using Autofac.Integration.WebApi;
using MongoDB.Driver;
using System.Reflection;
using TestChatTool.Domain.Repository;
using TestChatTool.Persistent.MongoRepository;

namespace TestChatTool.Service.AppLibs
{
    internal static class AutofacConfig
    {
        /// <summary>
        /// autofac 裝載介面跟實作的容器
        /// </summary>
        private static IContainer _container;

        public static IContainer Container
        {
            get
            {
                if (_container == null)
                {
                    Register();
                }

                return _container;
            }
        }

        private static void Register()
        {
            var builder = new ContainerBuilder();
            var asm = Assembly.GetExecutingAssembly();
            builder.RegisterApiControllers(asm); // 註冊全部的 api-controller

            // 註冊DB Repository
            var client = new MongoClient(ConfigHelper.MongoDbContext);
            builder.Register(c => new MongoUserRepository(client)).As<IUserRepository>().SingleInstance();
            builder.Register(c => new MongoAdminRepository(client)).As<IAdminRepository>().SingleInstance();
            builder.Register(c => new MongoChatRoomRepository(client)).As<IChatRoomRepository>().SingleInstance();
            builder.Register(c => new MongoOnLineUserRepository(client)).As<IOnLineUserRepository>().SingleInstance();

            _container = builder.Build();
        }
    }
}
