using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using System;
using TestChatTool.Domain.Enum;
using TestChatTool.Domain.Model;
using TestChatTool.Domain.Repository;
using TestChatTool.Persistent.MongoRepository;

namespace TestChatTool.Persistent.Tests.MongoRepository
{
    [TestClass]
    public class MongoUserRepositoryTests
    {
        private IUserRepository _repository;
        private const string _context = "mongodb://localhost:27017";

        [TestInitialize]
        public void Init()
        {
            var client = new MongoClient(_context);
            var db = client.GetDatabase("TestChatTool");
            // 清空測試資料表確保資料正確
            db.DropCollection("User");

            _repository = new MongoUserRepository(client);
        }

        [TestMethod]
        public void cerate_user()
        {
            var into = User.GenerateInstance("user", "pass", "u");
            into.Status = UserStatusType.Enable;
            var result = _repository.Create(into);

            Assert.IsNull(result.ex);
            Assert.IsTrue(result.isSuccess);
        }

        [TestMethod]
        public void cerate_user_exist()
        {
            _repository.Create(User.GenerateInstance("User001", "Pwd001", "u001"));

            var result = _repository.Create(User.GenerateInstance("User001", string.Empty, string.Empty));

            Assert.IsNotNull(result.ex);
            Assert.IsTrue(result.isAccDuplicate);
        }

        [TestMethod]
        public void query_user()
        {
            _repository.Create(User.GenerateInstance("query", "query", "query"));

            var result = _repository.Query("query");

            Assert.IsNull(result.ex);
            Console.WriteLine(result.result);
        }

        [TestMethod]
        public void update_user()
        {
            Console.WriteLine(_repository.Create(User.GenerateInstance("old", "old", "old")));

            var result = _repository.Update(User.GenerateInstance("old", string.Empty, "new"));

            Assert.IsNull(result.ex);
            Assert.AreEqual("new", result.result.NickName);
            Console.WriteLine(result.result);
        }

        [TestMethod]
        public void reset_pwd()
        {
            _repository.Create(User.GenerateInstance("old", "old", "old"));

            var result = _repository.ResetPwd("old", "old", "new");

            Assert.IsNull(result.ex);
            Assert.IsTrue(result.isSuccess);
        }

        [TestMethod]
        public void set_err_count()
        {
            Console.WriteLine(_repository.Create(User.GenerateInstance("user", "pwd", "u")));

            var result = _repository.SetErrCountAndStatus("user", 2);

            Assert.IsNull(result.ex);
            Assert.IsTrue(result.isSuccess);
        }

        [TestMethod]
        public void reset_err_count()
        {
            Console.WriteLine(_repository.Create(User.GenerateInstance("user", "pwd", "u")));

            var result = _repository.SetErrCountAndStatus("user", 0, UserStatusType.Unlock);

            Assert.IsNull(result.ex);
            Assert.IsTrue(result.isSuccess);
        }

        [TestMethod]
        public void sign_in_refresh()
        {
            Console.WriteLine(_repository.Create(User.GenerateInstance("user", "pwd", "u")));

            var result = _repository.SignInRefresh("user");

            Assert.IsNull(result.ex);
            Console.WriteLine(result.result);
        }

        [TestMethod]
        public void query_all_for_verify()
        {
            _repository.Create(User.GenerateInstance("user1", "pass", "u1"));
            _repository.Create(User.GenerateInstance("user2", "pass", "u2"));

            var result = _repository.GetAllForVerify();

            Assert.IsNull(result.ex);
            Assert.AreEqual(2, result.accs.Count);
        }

        [TestMethod]
        public void query_all_for_unlock()
        {
            _repository.Create(User.GenerateInstance("user1", "pass", "u1"));
            _repository.Create(User.GenerateInstance("user2", "pass", "u2"));
            _repository.Create(User.GenerateInstance("user3", "pass", "u3"));

            _repository.SetErrCountAndStatus("user2", 3, UserStatusType.Lock);

            var result = _repository.GetAllForUnlock();

            Assert.IsNull(result.ex);
            Assert.AreEqual(1, result.accs.Count);
        }
    }
}
