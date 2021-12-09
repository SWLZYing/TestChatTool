using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using System;
using TestChatTool.Domain.Model;
using TestChatTool.Domain.Repository;
using TestChatTool.Persistent.Repository;

namespace TestChatTool.Persistent.Tests.Repository
{
    [TestClass]
    public class MongoUserRepositoryTests
    {
        private IMongoUserRepository _repository;

        private const string _context = "mongodb://admin:123456@localhost:27017";

        [TestInitialize]
        public void Init()
        {
            var client = new MongoClient(_context);
            var db = client.GetDatabase("TestChatTool");
            // 清空資料表確保資料正確
            db.DropCollection("User");

            _repository = new MongoUserRepository(client);
        }

        [TestMethod]
        public void cerate_user()
        {
            var result = _repository.CreateUser(new User
            {
                Account = "user001",
                Password = "pwd001",
                NickName = "u001",
                Status = 0,
                ErrCount = 0,
                LastDatetime = DateTime.MinValue,
                CreateDatetime = DateTime.Now,
                UpdateDatetime = DateTime.Now,
            });

            Assert.IsNull(result.ex);
            Console.WriteLine(result.result);
        }
    }
}
