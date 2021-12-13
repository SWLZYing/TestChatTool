using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using System;
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
            var result = _repository.Create(new User().GenerateInstance("User001", "Pwd001", "u001"));

            Assert.IsNull(result.ex);
            Console.WriteLine(result.result);
        }

        [TestMethod]
        public void cerate_user_exist()
        {
            _repository.Create(new User().GenerateInstance("User001", "Pwd001", "u001"));

            var result = _repository.Create(new User().GenerateInstance("User001", string.Empty, string.Empty));

            Assert.IsNotNull(result.ex);
            Console.WriteLine(result.ex.Message);
        }

        [TestMethod]
        public void query_user()
        {
            _repository.Create(new User().GenerateInstance("query", "query", "query"));

            var result = _repository.Query("query");

            Assert.IsNull(result.ex);
            Console.WriteLine(result.result);
        }

        [TestMethod]
        public void update_user()
        {
            Console.WriteLine(_repository.Create(new User().GenerateInstance("old", "old", "old")));

            var result = _repository.Update(new User().GenerateInstance("old", string.Empty, "new"));

            Assert.IsNull(result.ex);
            Assert.AreEqual("new", result.result.NickName);
            Console.WriteLine(result.result);
        }

        [TestMethod]
        public void reset_pwd()
        {
            _repository.Create(new User().GenerateInstance("old", "old", "old"));

            var result = _repository.ResetPwd("old", "old", "new");

            Assert.IsNull(result.ex);
            Assert.IsTrue(result.isSuccess);
        }
    }
}
