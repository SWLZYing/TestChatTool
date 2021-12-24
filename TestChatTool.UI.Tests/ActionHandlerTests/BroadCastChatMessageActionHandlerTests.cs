using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using TestChatTool.Domain.Model;
using TestChatTool.UI.Forms;
using TestChatTool.UI.Handlers;
using TestChatTool.UI.Handlers.Interface;
using TestChatTool.UI.SignalR;

namespace TestChatTool.UI.Tests.ActionHandlerTests
{
    [TestClass]
    public class BroadCastChatMessageActionHandlerTests
    {
        private Mock<IHttpHandler> _httpHandler;
        private Mock<IHubClient> _hubClient;

        [TestInitialize]
        public void Init()
        {
            _httpHandler = new Mock<IHttpHandler>();
            _hubClient = new Mock<IHubClient>();
        }

        [TestMethod]
        public void 接收聊天訊息測試()
        {
            var handler = new BroadCastChatMessageActionHandler(
                new Room(_httpHandler.Object, _hubClient.Object),
                new Backstage(_httpHandler.Object, _hubClient.Object));
            var result = handler.Execute(new ActionModule()
            {
                Action = "TEST_TEST",
                Content = JsonConvert.SerializeObject(new BroadCastChatMessageAction()
                {
                    NickName = "TEST001",
                    Message = "123456",
                    CreateDateTime = DateTime.Now
                })
            });

            Assert.IsTrue(result);
        }
    }
}
