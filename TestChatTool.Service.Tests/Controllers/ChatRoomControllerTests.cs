using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
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
        public void query_room()
        {
            _repo.Setup(s => s.Query(It.IsAny<string>()))
                .Returns((null, new ChatRoom { Code = "room" }));

            var controller = new ChatRoomController(_repo.Object);

            var result = controller.Query("user");

            Assert.AreEqual((int)ErrorType.Success, result.Code);
            Console.WriteLine(result.Room);
        }

        [TestMethod]
        public void query_room_not_exist()
        {
            _repo.Setup(s => s.Query(It.IsAny<string>()))
                .Returns((null, null));

            var controller = new ChatRoomController(_repo.Object);

            var result = controller.Query("user");

            Assert.AreEqual((int)ErrorType.AccError, result.Code);
        }

        [TestMethod]
        public void query_room_not_init()
        {
            var controller = new ChatRoomController(_repo.Object);

            var result = controller.Query(string.Empty);

            Assert.AreEqual((int)ErrorType.FieldNull, result.Code);
        }

        [TestMethod]
        public void update_room()
        {
            _repo.Setup(s => s.Update(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((null, new ChatRoom { Code = "ROOM01" }));

            var controller = new ChatRoomController(_repo.Object);

            var result = controller.Update(new ChatRoomUpdateRequest
            {
                Code = "ROOM01",
                Name = "new",
            });

            Assert.AreEqual((int)ErrorType.Success, result.Code);
            Assert.IsNotNull(result.Room);
        }

        [TestMethod]
        public void delete_room()
        {
            _repo.Setup(s => s.Delete(It.IsAny<string>()))
                .Returns((null, true));

            var controller = new ChatRoomController(_repo.Object);

            var result = controller.Delete("ROOM01");

            Assert.AreEqual((int)ErrorType.Success, result.Code);
        }

        [TestMethod]
        public void update_not_into_room_code()
        {
            var controller = new ChatRoomController(_repo.Object);

            var result = controller.Update(new ChatRoomUpdateRequest
            {
                Code = "",
                Name = "new",
            });

            Assert.AreEqual((int)ErrorType.FieldNull, result.Code);
            Assert.IsNull(result.Room);
        }

        [TestMethod]
        public void update_room_not_exist()
        {
            _repo.Setup(s => s.Update(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((null, null));

            var controller = new ChatRoomController(_repo.Object);

            var result = controller.Update(new ChatRoomUpdateRequest
            {
                Code = "new",
                Name = "new",
            });

            Assert.AreEqual((int)ErrorType.AccError, result.Code);
            Assert.IsNull(result.Room);
        }

        [TestMethod]
        public void get_all_room()
        {
            _repo.Setup(s => s.GetAll())
                .Returns((null, new List<ChatRoom>()));

            var controller = new ChatRoomController(_repo.Object);

            var result = controller.GetAll();

            Assert.AreEqual((int)ErrorType.Success, result.Code);
        }
    }
}
