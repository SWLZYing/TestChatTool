using TestChatTool.UI.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using TestChatTool.UI.Handlers;
using Moq;
using TestChatTool.UI.Handlers.Interface;
using TestChatTool.Domain.Model;

namespace TestChatTool.UI.Tests.ActionHandlerTests
{
    [TestClass]
    public class BroadCastLogoutActionHandlerTest
    {
        private Mock<IHttpHandler> _httpHandler;

        [TestInitialize]
        public void Init()
        {
            _httpHandler = new Mock<IHttpHandler>();
        }

        [TestMethod]
        public void 使用者移除單元測試()
        {
            var handler = new BroadCastLogoutActionHandler(new Room(), new Backstage());
            var result = handler.Execute(new ActionModule()
            {
                Content = JsonConvert.SerializeObject(new BroadCastLogoutAction()
                {
                    NickName = "TEST001"
                })
            });

            Assert.IsTrue(result);
        }
    }
}
