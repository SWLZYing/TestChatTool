using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using TestChatTool.Domain.Model;
using TestChatTool.Service.ActionHandler;
using TestChatTool.Service.Enums;

namespace TestChatTool.Service.Tests.ActionHandler
{
    [TestClass]
    public class SendDeleteChatRoomActionHandlerTests
    {
        [TestMethod]
        public void delete_room()
        {
            var handler = new SendDeleteChatRoomActionHandler();
            var result = handler.ExecuteAction(new ActionModule()
            {
                Content = JsonConvert.SerializeObject(new SendDeleteChatRoomAction())
            });

            Assert.IsNull(result.exception);
            Assert.AreEqual(result.notifyType, NotifyType.BroadCast);
        }
    }
}
