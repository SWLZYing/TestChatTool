using TestChatTool.UI.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using TestChatTool.UI.Handlers;
using Moq;
using TestChatTool.UI.Handlers.Interface;
using TestChatTool.Domain.Model;
using TestChatTool.UI.SignalR;

namespace TestChatTool.UI.Tests.ActionHandlerTests
{
    [TestClass]
    public class BroadCastLogoutActionHandlerTest
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
        public void 使用者移除單元測試()
        {
            var handler = new BroadCastLogoutActionHandler(
                new Room(_httpHandler.Object, _hubClient.Object),
                new Backstage(_httpHandler.Object, _hubClient.Object));
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
