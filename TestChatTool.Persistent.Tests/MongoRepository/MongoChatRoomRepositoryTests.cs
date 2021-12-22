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
            var result = _repository.Create(ChatRoom.GenerateInstance("DCCAT", "Cat Room"));

            Assert.IsNull(result.ex);
            Assert.IsTrue(result.isSuccess);
        }

        [TestMethod]
        public void create_room_exist()
        {
            _repository.Create(ChatRoom.GenerateInstance("DCCAT", "Cat Room"));
            var result = _repository.Create(ChatRoom.GenerateInstance("DCCAT", string.Empty));

            Assert.IsNotNull(result.ex);
            Assert.IsTrue(result.isAccDuplicate);
        }

        [TestMethod]
        public void query_room()
        {
            _repository.Create(ChatRoom.GenerateInstance("DCCAT", "Cat Room"));
            var result = _repository.Query("DCCAT");

            Assert.IsNull(result.ex);
            Console.WriteLine(result.result);
        }

        [TestMethod]
        public void update_room_name()
        {
            Console.WriteLine(_repository.Create(ChatRoom.GenerateInstance("DCCAT", "Cat Room")));

            var result = _repository.Update("DCCAT", "New Cat Room");

            Assert.IsNull(result.ex);
            Console.WriteLine(result.result);
        }

        [TestMethod]
        public void get_all_room()
        {
            _repository.Create(ChatRoom.GenerateInstance("DCCAT", "Cat Room"));
            _repository.Create(ChatRoom.GenerateInstance("DCDOG", "Dog Room"));
            _repository.Create(ChatRoom.GenerateInstance("DCBIRD", "Bird Room"));
            var result = _repository.GetAll();

            Assert.IsNull(result.ex);
            Assert.AreEqual(3, result.rooms.Count);
        }
    }
}
