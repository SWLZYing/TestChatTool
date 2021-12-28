using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using TestChatTool.Domain.Model;
using TestChatTool.UI.Events.Interface;
using TestChatTool.UI.Handlers;

namespace TestChatTool.UI.Tests.ActionHandlerTests
{
    [TestClass]
    public class BroadCastEnterRoomActionHandlerTest
    {
        private Mock<ICallBackEventHandler> _callBackEvent;

        [TestInitialize]
        public void Init()
        {
            _callBackEvent = new Mock<ICallBackEventHandler>();
        }

        [TestMethod]
        public void user_leave_room()
        {
            var handler = new BroadCastEnterRoomActionHandler(_callBackEvent.Object);
            var result = handler.Execute(new ActionModule()
            {
                Content = JsonConvert.SerializeObject(new BroadCastEnterRoomAction()
                {
                    NickName = "TEST001",
                    RoomCode = "HALL",
                })
            });

            Assert.IsTrue(result);
        }
    }
}
