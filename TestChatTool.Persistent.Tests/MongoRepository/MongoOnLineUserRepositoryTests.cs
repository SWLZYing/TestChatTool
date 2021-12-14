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
            var result = _repository.Upsert(new OnLineUser().GenerateInstance("USER001", string.Empty, "DC_GOG"));

            Assert.IsNull(result.ex);
            Console.WriteLine(result.result);
        }

        [TestMethod]
        public void query_room_users()
        {
            _repository.Upsert(new OnLineUser().GenerateInstance("USER001", "u1", "DC_CAT"));
            _repository.Upsert(new OnLineUser().GenerateInstance("USER002", "u2", "DC_GOG"));
            _repository.Upsert(new OnLineUser().GenerateInstance("USER003", "u3", "DC_GOG"));

            var result = _repository.FindRoomUser("DC_GOG");

            Assert.IsNull(result.ex);
            Assert.AreEqual(2, result.result.Count);
        }

        [TestMethod]
        public void query_all_room_user_count()
        {
            _repository.Upsert(new OnLineUser().GenerateInstance("USER001", "u1", "DC_CAT"));
            _repository.Upsert(new OnLineUser().GenerateInstance("USER002", "u2", "DC_GOG"));
            _repository.Upsert(new OnLineUser().GenerateInstance("USER003", "u3", "DC_GOG"));
            _repository.Upsert(new OnLineUser().GenerateInstance("USER004", "u4", "DC_BIRD"));

            var result = _repository.FindAllUserCountByRoom();

            Assert.IsNull(result.ex);
            Assert.AreEqual(3, result.result.Count);

            var str = string.Join(", ", result.result.Select(s => $"{s.Item1}:{s.Item2}"));
            Console.WriteLine($"Room's User Count => {str}.");
        }
    }
}
