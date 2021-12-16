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
    public class UserControllerTests
    {
        private Mock<IUserRepository> _repo;

        [TestInitialize]
        public void Init()
        {
            _repo = new Mock<IUserRepository>();
        }

        [TestMethod]
        public void create_account()
        {
            _repo.Setup(s => s.Create(It.IsAny<User>()))
                .Returns((null, true, false));

            var controller = new UserController(_repo.Object);

            var result = controller.Create(new UserCreateRequest { Account = "user", Password = "user" });

            Assert.AreEqual((int)ErrorType.Success, result.Code);
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public void create_account_is_exist()
        {
            _repo.Setup(s => s.Create(It.IsAny<User>()))
                .Returns((new Exception(), false, true));

            var controller = new UserController(_repo.Object);

            var result = controller.Create(new UserCreateRequest { Account = "user", Password = "user" });

            Assert.AreEqual((int)ErrorType.AccExist, result.Code);
            Assert.IsFalse(result.IsSuccess);
        }

        [TestMethod]
        public void query_account()
        {
            _repo.Setup(s => s.Query(It.IsAny<string>()))
                .Returns((null, new User { Account = "user" }));

            var controller = new UserController(_repo.Object);

            var result = controller.Query("user");

            Assert.AreEqual((int)ErrorType.Success, result.Code);
            Console.WriteLine(result.Data);
        }

        [TestMethod]
        public void query_account_not_exist()
        {
            _repo.Setup(s => s.Query(It.IsAny<string>()))
                .Returns((null, null));

            var controller = new UserController(_repo.Object);

            var result = controller.Query("user");

            Assert.AreEqual((int)ErrorType.AccError, result.Code);
        }

        [TestMethod]
        public void query_account_not_init()
        {
            var controller = new UserController(_repo.Object);

            var result = controller.Query(string.Empty);

            Assert.AreEqual((int)ErrorType.FieldNull, result.Code);
        }

        [TestMethod]
        public void update_account()
        {
            _repo.Setup(s => s.Update(It.IsAny<User>()))
                .Returns((null, new User { Account = "user" }));

            var controller = new UserController(_repo.Object);

            var result = controller.Update(new UserUpdateRequest
            {
                Account = "user",
                NickName = "new",
            });

            Assert.AreEqual((int)ErrorType.Success, result.Code);
            Assert.IsNotNull(result.Data);
        }

        [TestMethod]
        public void update_not_into_account()
        {
            var controller = new UserController(_repo.Object);

            var result = controller.Update(new UserUpdateRequest
            {
                Account = "",
                NickName = "new",
            });

            Assert.AreEqual((int)ErrorType.FieldNull, result.Code);
            Assert.IsNull(result.Data);
        }

        [TestMethod]
        public void update_account_not_exist()
        {
            _repo.Setup(s => s.Update(It.IsAny<User>()))
                .Returns((null, null));

            var controller = new UserController(_repo.Object);

            var result = controller.Update(new UserUpdateRequest
            {
                Account = "new",
                NickName = "new",
            });

            Assert.AreEqual((int)ErrorType.AccError, result.Code);
            Assert.IsNull(result.Data);
        }

        [TestMethod]
        public void reset_pwd()
        {
            _repo.Setup(s => s.ResetPwd(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns((null, true));

            var controller = new UserController(_repo.Object);

            var result = controller.ResetPwd(new UserResetPwdRequest
            {
                Account = "old",
                OldPassWord = "old",
                NewPassWord = "new",
            });

            Assert.AreEqual((int)ErrorType.Success, result.Code);
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public void reset_pwd_failed()
        {
            _repo.Setup(s => s.ResetPwd(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns((null, false));

            var controller = new UserController(_repo.Object);

            var result = controller.ResetPwd(new UserResetPwdRequest
            {
                Account = "old",
                OldPassWord = "error",
                NewPassWord = "new",
            });

            Assert.AreEqual((int)ErrorType.Failed, result.Code);
            Assert.IsFalse(result.IsSuccess);
        }

        [TestMethod]
        public void set_error_count_and_status()
        {
            _repo.Setup(s => s.SetErrCountAndStatus(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<UserStatusType>()))
                .Returns((null, true));

            var controller = new UserController(_repo.Object);

            var result = controller.SetErrCountAndStatus(new UserSetErrCountAndStatusRequest
            {
                Account = "old",
            });

            Assert.AreEqual((int)ErrorType.Success, result.Code);
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public void set_error_count_and_status_failed()
        {
            _repo.Setup(s => s.SetErrCountAndStatus(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<UserStatusType>()))
                .Returns((null, false));

            var controller = new UserController(_repo.Object);

            var result = controller.SetErrCountAndStatus(new UserSetErrCountAndStatusRequest
            {
                Account = "old",
            });

            Assert.AreEqual((int)ErrorType.Failed, result.Code);
            Assert.IsFalse(result.IsSuccess);
        }
    }
}
