using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using TestChatTool.UI.Handlers;
using Moq;
using TestChatTool.Domain.Model;
using TestChatTool.UI.Events.Interface;

namespace TestChatTool.UI.Tests.ActionHandlerTests
{
    [TestClass]
    public class BroadCastLeaveRoomActionHandlerTest
    {
        private Mock<ICallBackEventHandler> _callBackEvent;

        [TestInitialize]
        public void Init()
        {
            _callBackEvent = new Mock<ICallBackEventHandler>();
        }

        [TestMethod]
        public void 使用者移除單元測試()
        {
            var handler = new BroadCastLeaveRoomActionHandler(_callBackEvent.Object);
            var result = handler.Execute(new ActionModule()
            {
                Content = JsonConvert.SerializeObject(new BroadCastLeaveRoomAction()
                {
                    NickName = "TEST001"
                })
            });

            Assert.IsTrue(result);
        }
    }
}
