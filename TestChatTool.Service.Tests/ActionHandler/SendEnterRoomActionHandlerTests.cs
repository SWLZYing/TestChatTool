using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using TestChatTool.Domain.Model;
using TestChatTool.Service.ActionHandler;
using TestChatTool.Service.Enums;

namespace TestChatTool.Service.Tests.ActionHandler
{
    [TestClass]
    public class SendEnterRoomActionHandlerTests
    {
        [TestMethod]
        public void enter_room()
        {
            var handler = new SendEnterRoomActionHandler();
            var result = handler.ExecuteAction(new ActionModule()
            {
                Content = JsonConvert.SerializeObject(new SendEnterRoomAction
                {
                    RoomCode = "HALL",
                    Account = "TEST001",
                    NickName = "TEST001",
                })
            });

            Assert.IsNull(result.exception);
            Assert.AreEqual(result.notifyType, NotifyType.BroadCast);
            Assert.IsNotNull(result.actionBase);
        }
    }
}
