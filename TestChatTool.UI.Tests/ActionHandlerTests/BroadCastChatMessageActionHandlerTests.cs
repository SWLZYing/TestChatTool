using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using TestChatTool.Domain.Model;
using TestChatTool.UI.Forms;
using TestChatTool.UI.Handlers;
using TestChatTool.UI.Handlers.Interface;

namespace TestChatTool.UI.Tests.ActionHandlerTests
{
    [TestClass]
    public class BroadCastChatMessageActionHandlerTests
    {
        private Mock<IHttpHandler> _httpHandler;

        [TestInitialize]
        public void Init()
        {
            _httpHandler = new Mock<IHttpHandler>();
        }

        [TestMethod]
        public void 接收聊天訊息測試()
        {
            var handler = new BroadCastChatMessageActionHandler(new Room(), new Backstage());
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
