using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using System;
using TestChatTool.Domain.Model;
using TestChatTool.Domain.Repository;
using TestChatTool.Persistent.MongoRepository;

namespace TestChatTool.Persistent.Tests.MongoRepository
{
    [TestClass]
    public class MongoChatRoomRepositoryTests
    {
        private IChatRoomRepository _repository;
        private const string _context = "mongodb://localhost:27017";

        [TestInitialize]
        public void Init()
        {
            var client = new MongoClient(_context);
            var db = client.GetDatabase("TestChatTool");

            // 清空測試資料表確保資料正確
            db.DropCollection("ChatRoom");

            _repository = new MongoChatRoomRepository(client);
        }

        [TestMethod]
        public void create_room()
        {
            var result = _repository.Create(ChatRoom.GenerateInstance("DC_CAT", "Cat Room"));

            Assert.IsNull(result.ex);
            Console.WriteLine(result.result);
        }

        [TestMethod]
        public void create_room_exist()
        {
            _repository.Create(ChatRoom.GenerateInstance("DC_CAT", "Cat Room"));
            var result = _repository.Create(ChatRoom.GenerateInstance("DC_CAT", string.Empty));

            Assert.IsNotNull(result.ex);
            Console.WriteLine(result.ex.Message);
        }

        [TestMethod]
        public void query_room()
        {
            _repository.Create(ChatRoom.GenerateInstance("DC_CAT", "Cat Room"));
            var result = _repository.Query("DC_CAT");

            Assert.IsNull(result.ex);
            Console.WriteLine(result.result);
        }

        [TestMethod]
        public void update_room_name()
        {
            Console.WriteLine(_repository.Create(ChatRoom.GenerateInstance("DC_CAT", "Cat Room")));

            var result = _repository.Update("DC_CAT", "New Cat Room");

            Assert.IsNull(result.ex);
            Console.WriteLine(result.result);
        }
    }
}
