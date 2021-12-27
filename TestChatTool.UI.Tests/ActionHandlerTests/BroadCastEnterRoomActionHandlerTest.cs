using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using TestChatTool.Domain.Model;
using TestChatTool.UI.Forms;
using TestChatTool.UI.Handlers;
using TestChatTool.UI.Helpers.Interface;
using TestChatTool.UI.SignalR;

namespace TestChatTool.UI.Tests.ActionHandlerTests
{
    [TestClass]
    public class BroadCastEnterRoomActionHandlerTest
    {
        private Mock<IUserControllerApiHelper> _userControllerApi;
        private Mock<IOnLineUserControllerApiHelper> _onLineUserControllerApi;
        private Mock<IChatRoomControllerApiHelper> _chatRoomControllerApi;
        private Mock<IHubClient> _hubClient;

        [TestInitialize]
        public void Init()
        {
            _userControllerApi = new Mock<IUserControllerApiHelper>();
            _onLineUserControllerApi = new Mock<IOnLineUserControllerApiHelper>();
            _chatRoomControllerApi = new Mock<IChatRoomControllerApiHelper>();
            _hubClient = new Mock<IHubClient>();
        }

        [TestMethod]
        public void user_leave_room()
        {
            var handler = new BroadCastEnterRoomActionHandler(
                new Room(_userControllerApi.Object, _onLineUserControllerApi.Object, _chatRoomControllerApi.Object, _hubClient.Object),
                new Backstage(_userControllerApi.Object, _onLineUserControllerApi.Object, _chatRoomControllerApi.Object, _hubClient.Object));
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
