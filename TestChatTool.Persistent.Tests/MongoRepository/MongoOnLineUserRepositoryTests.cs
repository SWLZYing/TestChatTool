using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using System;
using TestChatTool.Domain.Model;
using TestChatTool.Domain.Repository;
using TestChatTool.Persistent.MongoRepository;

namespace TestChatTool.Persistent.Tests.MongoRepository
{
    [TestClass]
    public class MongoOnLineUserRepositoryTests
    {
        private IOnLineUserRepository _repository;
        private const string _context = "mongodb://localhost:27017";

        [TestInitialize]
        public void Init()
        {
            var client = new MongoClient(_context);
            var db = client.GetDatabase("TestChatTool");

            // 清空測試資料表確保資料正確
            db.DropCollection("OnLineUser");

            _repository = new MongoOnLineUserRepository(client);
        }

        [TestMethod]
        public void insert_online_user()
        {
            var result = _repository.Upsert(new OnLineUser().GenerateInstance("USER001", "u1", "DC_CAT"));

            Assert.IsNull(result.ex);
            Console.WriteLine(result.result);
        }

        [TestMethod]
        public void update_online_user()
        {
            // 進入聊天室
            Console.WriteLine(_repository.Upsert(new OnLineUser().GenerateInstance("USER001", "u1", "DC_CAT")));
            // 變換聊天室
            var result = _repository.Upsert(new OnLineUser().GenerateInstance("USER001", "", "DC_GOG"));

            Assert.IsNull(result.ex);
            Console.WriteLine(result.result);
        }
    }
}
