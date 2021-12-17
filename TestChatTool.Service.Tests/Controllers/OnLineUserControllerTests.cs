using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using TestChatTool.Domain.Enum;
using TestChatTool.Domain.Model;
using TestChatTool.Domain.Repository;
using TestChatTool.Service.Controllers;
using TestChatTool.Service.Models;

namespace TestChatTool.Service.Tests.Controllers
{
    [TestClass]
    public class OnLineUserControllerTests
    {
        private Mock<IOnLineUserRepository> _repo;

        [TestInitialize]
        public void Init()
        {
            _repo = new Mock<IOnLineUserRepository>();
        }

        [TestMethod]
        public void on_line_user()
        {
            _repo.Setup(s => s.Upsert(It.IsAny<OnLineUser>()))
                .Returns((null, new OnLineUser { Account = "user" }));

            var controller = new OnLineUserController(_repo.Object);

            var result = controller.Upsert(new OnLineUserUpsertRequest { Account = "user", RoomCode = "ROOM01" });

            Assert.AreEqual((int)ErrorType.Success, result.Code);
            Assert.IsNotNull(result.Data);
        }

        [TestMethod]
        public void on_line_user_not_code()
        {
            var controller = new OnLineUserController(_repo.Object);

            var result = controller.Upsert(new OnLineUserUpsertRequest { Account = "user", RoomCode = string.Empty });

            Assert.AreEqual((int)ErrorType.FieldNull, result.Code);
            Assert.IsNull(result.Data);
        }

        [TestMethod]
        public void on_line_user_in_room()
        {
            _repo.Setup(s => s.FindRoomUser(It.IsAny<string>()))
                .Returns((null, new List<OnLineUser> { new OnLineUser { Account = "user" } }));

            var controller = new OnLineUserController(_repo.Object);

            var result = controller.FindRoomUser("ROOM01");

            Assert.AreEqual((int)ErrorType.Success, result.Code);
            Assert.IsNotNull(result.Data);
        }

        [TestMethod]
        public void on_line_user_in_room_not_code()
        {
            var controller = new OnLineUserController(_repo.Object);

            var result = controller.FindRoomUser(string.Empty);

            Assert.AreEqual((int)ErrorType.FieldNull, result.Code);
            Assert.IsNull(result.Data);
        }

        [TestMethod]
        public void find_all_room_user_count()
        {
            _repo.Setup(s => s.FindAllUserCountByRoom())
                .Returns((null, new List<(string, int)>()));

            var controller = new OnLineUserController(_repo.Object);

            var result = controller.FindAllUserCountByRoom();

            Assert.AreEqual((int)ErrorType.Success, result.Code);
            Assert.IsNotNull(result.Data);
        }
    }
}
