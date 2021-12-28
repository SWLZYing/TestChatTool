using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using TestChatTool.Domain.Model;
using TestChatTool.UI.Events.Interface;
using TestChatTool.UI.Handlers;

namespace TestChatTool.UI.Tests.ActionHandlerTests
{
    [TestClass]
    public class BroadCastChatMessageActionHandlerTests
    {
        private Mock<ICallBackEventHandler> _callBackEvent;

        [TestInitialize]
        public void Init()
        {
            _callBackEvent = new Mock<ICallBackEventHandler>();
        }

        [TestMethod]
        public void 接收聊天訊息測試()
        {
            var handler = new BroadCastChatMessageActionHandler(_callBackEvent.Object);
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
