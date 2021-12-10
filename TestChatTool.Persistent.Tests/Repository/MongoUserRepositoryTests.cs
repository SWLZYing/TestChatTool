using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using System;
using System.Threading;
using TestChatTool.Domain.Model;
using TestChatTool.Domain.Repository;
using TestChatTool.Persistent.Repository;

namespace TestChatTool.Persistent.Tests.Repository
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
            var result = _repository.CreateUser(NewUser("User001", "Pwd001", "u001"));

            Assert.IsNull(result.ex);
            Console.WriteLine(result.result);
        }

        [TestMethod]
        public void cerate_user_exist()
        {
            _repository.CreateUser(NewUser("User001", "Pwd001", "u001"));

            var result = _repository.CreateUser(NewUser("User001", string.Empty, string.Empty));

            Assert.IsNotNull(result.ex);
            Console.WriteLine(result.ex.Message);
        }

        [TestMethod]
        public void query_user()
        {
            _repository.CreateUser(NewUser("query", "query", "query"));

            var result = _repository.QueryUser("query");

            Assert.IsNull(result.ex);
            Console.WriteLine(result.result);
        }

        [TestMethod]
        public void update_user()
        {
            _repository.CreateUser(NewUser("old", "old", "old"));

            var result = _repository.UpdateUser(NewUser("old", string.Empty, "new"));

            Assert.IsNull(result.ex);
            Console.WriteLine(result.result);
        }

        [TestMethod]
        public void reset_pwd()
        {
            _repository.CreateUser(NewUser("old", "old", "old"));

            var result = _repository.ResetPwd("old", "old", "new");

            Assert.IsNull(result.ex);
            Assert.IsTrue(result.isSuccess);
        }

        private User NewUser(string acc, string pwd, string name)
        {
            return new User
            {
                Account = acc,
                Password = pwd,
                NickName = name,
                Status = 0,
                ErrCount = 0,
                LastDatetime = DateTime.MinValue,
                CreateDatetime = DateTime.Now,
                UpdateDatetime = DateTime.Now,
            };
        }
    }
}
