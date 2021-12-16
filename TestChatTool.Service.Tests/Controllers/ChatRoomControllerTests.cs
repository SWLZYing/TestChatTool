using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using TestChatTool.Domain.Enum;
using TestChatTool.Domain.Model;
using TestChatTool.Domain.Repository;
using TestChatTool.Service.Controllers;
using TestChatTool.Service.Models;

namespace TestChatTool.Service.Tests.Controllers
{
    [TestClass]
    public class ChatRoomControllerTests
    {
        private Mock<IChatRoomRepository> _repo;

        [TestInitialize]
        public void init()
        {
            _repo = new Mock<IChatRoomRepository>();
        }

        [TestMethod]
        public void create_room()
        {
            _repo.Setup(s => s.Create(It.IsAny<ChatRoom>()))
                .Returns((null, true, false));

            var controller = new ChatRoomController(_repo.Object);

            var result = controller.Create(new ChatRoomCreateRequest { Code = "ROOM" });

            Assert.AreEqual((int)ErrorType.Success, result.Code);
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public void create_room_exist()
        {
            _repo.Setup(s => s.Create(It.IsAny<ChatRoom>()))
                .Returns((new Exception(), false, true));

            var controller = new ChatRoomController(_repo.Object);

            var result = controller.Create(new ChatRoomCreateRequest { Code = "ROOM" });

            Assert.AreEqual((int)ErrorType.AccExist, result.Code);
            Assert.IsFalse(result.IsSuccess);
        }

        [TestMethod]
        public void query_account()
        {
            _repo.Setup(s => s.Query(It.IsAny<string>()))
                .Returns((null, new ChatRoom { Code = "room" }));

            var controller = new ChatRoomController(_repo.Object);

            var result = controller.Query("user");

            Assert.AreEqual((int)ErrorType.Success, result.Code);
            Console.WriteLine(result.Data);
        }

        [TestMethod]
        public void query_account_not_exist()
        {
            _repo.Setup(s => s.Query(It.IsAny<string>()))
                .Returns((null, null));

            var controller = new ChatRoomController(_repo.Object);

            var result = controller.Query("user");

            Assert.AreEqual((int)ErrorType.AccError, result.Code);
        }

        [TestMethod]
        public void query_account_not_init()
        {
            var controller = new ChatRoomController(_repo.Object);

            var result = controller.Query(string.Empty);

            Assert.AreEqual((int)ErrorType.FieldNull, result.Code);
        }
    }
}
