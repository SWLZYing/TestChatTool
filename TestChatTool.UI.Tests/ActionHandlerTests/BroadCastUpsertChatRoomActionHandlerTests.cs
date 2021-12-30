using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using TestChatTool.Domain.Model;
using TestChatTool.UI.Events.Interface;
using TestChatTool.UI.Handlers;

namespace TestChatTool.UI.Tests.ActionHandlerTests
{
    [TestClass]

    public class BroadCastUpsertChatRoomActionHandlerTests
    {
        private Mock<ICallBackEventHandler> _callBackEvent;

        [TestInitialize]
        public void Init()
        {
            _callBackEvent = new Mock<ICallBackEventHandler>();
        }

        [TestMethod]
        public void room_upsert_broad_cast()
        {
            var handler = new BroadCastUpsertChatRoomActionHandler(_callBackEvent.Object);
            var result = handler.Execute(new ActionModule()
            {
                Action = "TEST_TEST",
                Content = JsonConvert.SerializeObject(new BroadCastUpsertChatRoomAction())
            });

            Assert.IsTrue(result);
        }
    }
}
