using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using TestChatTool.Domain.Model;
using TestChatTool.Domain.Repository;
using TestChatTool.Service.ActionHandler;
using TestChatTool.Service.Enums;

namespace TestChatTool.Service.Tests.ActionHandler
{
    [TestClass]
    public class CheckConnectActionHandlerTests
    {
        [TestMethod]
        public void check_connect()
        {
            var repo = new Mock<IOnLineUserRepository>();
            repo.Setup(s => s.Upsert(It.IsAny<OnLineUser>()))
                .Returns((null, null));

            var handler = new CheckConnectActionHandler(repo.Object);
            var result = handler.ExecuteAction(new ActionModule()
            {
                Content = JsonConvert.SerializeObject(new CheckConnectAction()
                {
                    Account = "TEST001",
                    NickName = "T1",
                    RoomCode = "HALL",
                })
            });

            Assert.IsNull(result.exception);
            Assert.AreEqual(result.notifyType, NotifyType.None);
        }
    }
}
