using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using System;
using System.Linq;
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
            var result = _repository.Upsert(OnLineUser.GenerateInstance("USER001", "u1", "DCCAT"));

            Assert.IsNull(result.ex);
            Console.WriteLine(result.user);
        }

        [TestMethod]
        public void update_online_user()
        {
            // 進入聊天室
            Console.WriteLine(_repository.Upsert(OnLineUser.GenerateInstance("USER001", "u1", "DCCAT")));
            // 變換聊天室
            var result = _repository.Upsert(OnLineUser.GenerateInstance("USER001", string.Empty, "DCGOG"));

            Assert.IsNull(result.ex);
            Console.WriteLine(result.user);
        }

        [TestMethod]
        public void query_room_users()
        {
            _repository.Upsert(OnLineUser.GenerateInstance("USER001", "u1", "DCCAT"));
            _repository.Upsert(OnLineUser.GenerateInstance("USER002", "u2", "DCGOG"));
            _repository.Upsert(OnLineUser.GenerateInstance("USER003", "u3", "DCGOG"));

            var result = _repository.FindRoomUser("DCGOG");

            Assert.IsNull(result.ex);
            Assert.AreEqual(2, result.users.ToList().Count);
        }

        [TestMethod]
        public void query_all_room_user_count()
        {
            _repository.Upsert(OnLineUser.GenerateInstance("USER001", "u1", "DCCAT"));
            _repository.Upsert(OnLineUser.GenerateInstance("USER002", "u2", "DCGOG"));
            _repository.Upsert(OnLineUser.GenerateInstance("USER003", "u3", "DCGOG"));
            _repository.Upsert(OnLineUser.GenerateInstance("USER004", "u4", "DCBIRD"));

            var result = _repository.FindAllUserCountByRoom();

            Assert.IsNull(result.ex);
            Assert.AreEqual(3, result.result.ToList().Count);

            var str = string.Join(", ", result.result.Select(s => $"{s.Item1}:{s.Item2}"));
            Console.WriteLine($"Room's User Count => {str}.");
        }
    }
}
