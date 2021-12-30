using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using TestChatTool.Domain.Model;
using TestChatTool.UI.Events.Interface;
using TestChatTool.UI.Handlers;

namespace TestChatTool.UI.Tests.ActionHandlerTests
{
    [TestClass]
    public class BroadCastDeleteChatRoomActionHandlerTests
    {
        private Mock<ICallBackEventHandler> _callBackEvent;

        [TestInitialize]
        public void Init()
        {
            _callBackEvent = new Mock<ICallBackEventHandler>();
        }

        [TestMethod]
        public void room_delete_broad_cast()
        {
            var handler = new BroadCastDeleteChatRoomActionHandler(_callBackEvent.Object);
            var result = handler.Execute(new ActionModule()
            {
                Action = "TEST_TEST",
                Content = JsonConvert.SerializeObject(new BroadCastDeleteChatRoomAction())
            });

            Assert.IsTrue(result);
        }
    }
}
