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
    public class AdminControllerTests
    {
        private Mock<IAdminRepository> _repo;

        [TestInitialize]
        public void Into()
        {
            _repo = new Mock<IAdminRepository>();
        }

        [TestMethod]
        public void create_normal()
        {
            _repo.Setup(s => s.Create(It.IsAny<Admin>()))
                .Returns((null, true, false));

            var controller = new AdminController(_repo.Object);

            var result = controller.Create(new AdminCreateRequest
            {
                Account = "Admin31",
                Password = "Admin31",
                AccountType = AdminType.Normal,
            });

            Assert.AreEqual((int)ErrorType.Success, result.Code);
        }

        [TestMethod]
        public void create_account_is_exist()
        {
            _repo.Setup(s => s.Create(It.IsAny<Admin>()))
                .Returns((new Exception(), false, true));

            var controller = new AdminController(_repo.Object);

            var result = controller.Create(new AdminCreateRequest
            {
                Account = "Admin",
                Password = "Admin",
                AccountType = AdminType.Normal,
            });

            Assert.AreEqual((int)ErrorType.AccExist, result.Code);
        }

        [TestMethod]
        public void query_account()
        {
            _repo.Setup(s => s.Query(It.IsAny<string>()))
                .Returns((null, new Admin { Account = "admin" }));

            var controller = new AdminController(_repo.Object);

            var result = controller.Query("admin");

            Assert.AreEqual((int)ErrorType.Success, result.Code);
            Console.WriteLine(result.Data);
        }

        [TestMethod]
        public void query_account_not_exist()
        {
            _repo.Setup(s => s.Query(It.IsAny<string>()))
                .Returns((null, null));

            var controller = new AdminController(_repo.Object);

            var result = controller.Query("admin");

            Assert.AreEqual((int)ErrorType.AccError, result.Code);
        }

        [TestMethod]
        public void query_account_not_init()
        {
            var controller = new AdminController(_repo.Object);

            var result = controller.Query(string.Empty);

            Assert.AreEqual((int)ErrorType.FieldNull, result.Code);
        }
    }
}
