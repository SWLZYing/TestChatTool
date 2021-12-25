using TestChatTool.UI.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using TestChatTool.UI.Handlers;
using Moq;
using TestChatTool.UI.Handlers.Interface;
using TestChatTool.Domain.Model;
using TestChatTool.UI.SignalR;
using TestChatTool.UI.Helpers.Interface;

namespace TestChatTool.UI.Tests.ActionHandlerTests
{
    [TestClass]
    public class BroadCastLogoutActionHandlerTest
    {
        private Mock<IUserControllerApiHelper> _userControllerApi;
        private Mock<IOnLineUserControllerApiHelper> _onLineUserControllerApi;
        private Mock<IChatRoomControllerApiHelper> _chatRoomControllerApi;
        private Mock<IHttpHandler> _httpHandler;
        private Mock<IHubClient> _hubClient;

        [TestInitialize]
        public void Init()
        {
            _userControllerApi = new Mock<IUserControllerApiHelper>();
            _onLineUserControllerApi = new Mock<IOnLineUserControllerApiHelper>();
            _chatRoomControllerApi = new Mock<IChatRoomControllerApiHelper>();
            _httpHandler = new Mock<IHttpHandler>();
            _hubClient = new Mock<IHubClient>();
        }

        [TestMethod]
        public void 使用者移除單元測試()
        {
            var handler = new BroadCastLogoutActionHandler(
                new Room(_httpHandler.Object, _hubClient.Object),
                new Backstage(_userControllerApi.Object, _onLineUserControllerApi.Object, _chatRoomControllerApi.Object, _hubClient.Object));
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
