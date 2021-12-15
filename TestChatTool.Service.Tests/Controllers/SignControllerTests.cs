using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Net;
using TestChatTool.Domain.Enum;
using TestChatTool.Domain.Extension;
using TestChatTool.Domain.Model;
using TestChatTool.Domain.Repository;
using TestChatTool.Service.Controllers;
using TestChatTool.Service.Models;

namespace TestChatTool.Service.Tests.Controllers
{
    [TestClass]
    public class SignControllerTests
    {
        private Mock<IAdminRepository> _adminRepo;
        private Mock<IUserRepository> _userRepo;
        private Mock<IOnLineUserRepository> _onLineRepo;

        [TestInitialize]
        public void Into()
        {
            _adminRepo = new Mock<IAdminRepository>();
            _userRepo = new Mock<IUserRepository>();
            _onLineRepo = new Mock<IOnLineUserRepository>();
        }

        [TestMethod]
        public void admin_sign_in()
        {
            _adminRepo.Setup(s => s.Query(It.IsAny<string>()))
                .Returns((null, Admin.GenerateInstance("admin", "pass".ToMD5())));

            var controller = new SignController(_adminRepo.Object, _userRepo.Object, _onLineRepo.Object);

            var response = controller.AdminSignIn(new SignInRequest { Account = "admin", Password = "pass" });

            Assert.IsNotNull(response.Data);
            Console.WriteLine(response.Data);
        }

        [TestMethod]
        public void admin_sign_in_not_acc_or_pass()
        {
            var controller = new SignController(_adminRepo.Object, _userRepo.Object, _onLineRepo.Object);

            var response = controller.AdminSignIn(new SignInRequest { Account = "", Password = "" });

            Assert.AreEqual(response.Code, (int)ErrorType.FieldNull);
        }

        [TestMethod]
        public void admin_sign_in_acc_not_exist()
        {
            _adminRepo.Setup(s => s.Query(It.IsAny<string>()))
                .Returns((null, null));

            var controller = new SignController(_adminRepo.Object, _userRepo.Object, _onLineRepo.Object);

            var response = controller.AdminSignIn(new SignInRequest { Account = "admin", Password = "pass" });

            Assert.AreEqual(response.Code, (int)ErrorType.AccError);
        }

        [TestMethod]
        public void admin_sign_in_pass_error()
        {
            _adminRepo.Setup(s => s.Query(It.IsAny<string>()))
                .Returns((null, Admin.GenerateInstance("admin", "pass".ToMD5())));

            var controller = new SignController(_adminRepo.Object, _userRepo.Object, _onLineRepo.Object);

            var response = controller.AdminSignIn(new SignInRequest { Account = "admin", Password = "error" });

            Assert.AreEqual(response.Code, (int)ErrorType.PwdError);
        }

        [TestMethod]
        public void user_sign_in()
        {
            var into = User.GenerateInstance("user", "pass".ToMD5(), "u");
            into.Status = 1;

            _userRepo.Setup(s => s.Query(It.IsAny<string>()))
                .Returns((null, into));
            _userRepo.Setup(s => s.SignInRefresh(It.IsAny<string>()))
                .Returns((null, into));

            var controller = new SignController(_adminRepo.Object, _userRepo.Object, _onLineRepo.Object);

            var response = controller.UserSignIn(new SignInRequest { Account = "user", Password = "pass" });

            Assert.IsNotNull(response.Data);
            Console.WriteLine(response.Data);
        }

        [TestMethod]
        public void user_not_verify()
        {
            _userRepo.Setup(s => s.Query(It.IsAny<string>()))
                .Returns((null, User.GenerateInstance("user", "pass".ToMD5(), "u")));

            var controller = new SignController(_adminRepo.Object, _userRepo.Object, _onLineRepo.Object);

            var response = controller.UserSignIn(new SignInRequest { Account = "user", Password = "pass" });

            Assert.AreEqual((int)ErrorType.AccNotVerify, response.Code);
        }

        [TestMethod]
        public void user_lock()
        {
            var into = User.GenerateInstance("user", "pass".ToMD5(), "u");
            into.Status = 2;

            _userRepo.Setup(s => s.Query(It.IsAny<string>()))
                .Returns((null, into));

            var controller = new SignController(_adminRepo.Object, _userRepo.Object, _onLineRepo.Object);

            var response = controller.UserSignIn(new SignInRequest { Account = "user", Password = "pass" });

            Assert.AreEqual((int)ErrorType.AccLock, response.Code);
        }

        [TestMethod]
        public void user_sign_in_pass_err()
        {
            var into = User.GenerateInstance("user", "pass".ToMD5(), "u");
            into.Status = 1;

            _userRepo.Setup(s => s.Query(It.IsAny<string>()))
                .Returns((null, into));
            _userRepo.Setup(s => s.SetErrCountAndStatus(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns((null, true));

            var controller = new SignController(_adminRepo.Object, _userRepo.Object, _onLineRepo.Object);

            var response = controller.UserSignIn(new SignInRequest { Account = "user", Password = "error" });

            Assert.AreEqual((int)ErrorType.PwdError, response.Code);
        }

        [TestMethod]
        public void user_sign_in_pass_err_to_Lock()
        {
            var into = User.GenerateInstance("user", "pass".ToMD5(), "u");
            into.Status = 1;
            into.ErrCount = 2;

            _userRepo.Setup(s => s.Query(It.IsAny<string>()))
                .Returns((null, into));
            _userRepo.Setup(s => s.SetErrCountAndStatus(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns((null, true));

            var controller = new SignController(_adminRepo.Object, _userRepo.Object, _onLineRepo.Object);

            var response = controller.UserSignIn(new SignInRequest { Account = "user", Password = "error" });

            Assert.AreEqual((int)ErrorType.PwdErrorToLock, response.Code);
        }
    }
}
