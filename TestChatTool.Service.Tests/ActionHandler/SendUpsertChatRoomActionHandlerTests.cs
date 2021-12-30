using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using TestChatTool.Domain.Model;
using TestChatTool.Service.ActionHandler;
using TestChatTool.Service.Enums;

namespace TestChatTool.Service.Tests.ActionHandler
{
    [TestClass]
    public class SendUpsertChatRoomActionHandlerTests
    {
        [TestMethod]
        public void upsert_room()
        {
            var handler = new SendUpsertChatRoomActionHandler();
            var result = handler.ExecuteAction(new ActionModule()
            {
                Content = JsonConvert.SerializeObject(new SendUpsertChatRoomAction())
            });

            Assert.IsNull(result.exception);
            Assert.AreEqual(result.notifyType, NotifyType.BroadCast);
        }
    }
}
