using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using System;
using TestChatTool.Domain.Model;
using TestChatTool.Domain.Repository;
using TestChatTool.Persistent.Repository;

namespace TestChatTool.Persistent.Tests.Repository
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
            var result = _repository.Create(NewRoom("DC_CAT", "Cat Room"));

            Assert.IsNull(result.ex);
            Console.WriteLine(result.result);
        }

        [TestMethod]
        public void create_room_exist()
        {
            _repository.Create(NewRoom("DC_CAT", "Cat Room"));
            var result = _repository.Create(NewRoom("DC_CAT", string.Empty));

            Assert.IsNotNull(result.ex);
            Console.WriteLine(result.ex.Message);
        }

        [TestMethod]
        public void query_room()
        {
            _repository.Create(NewRoom("DC_CATS", "Cat Room"));
            var result = _repository.Query("DC_CAT");

            Assert.IsNull(result.ex);
            Console.WriteLine(result.result);
        }

        [TestMethod]
        public void update_room()
        {
            _repository.Create(NewRoom("old", "old"));
            var result = _repository.Update(NewRoom("old", "new"));

            Assert.IsNull(result.ex);
            Assert.AreEqual("new", result.result.Name);
            Console.WriteLine(result.result);
        }

        private static ChatRoom NewRoom(string code, string name)
        {
            return new ChatRoom
            {
                Code = code,
                Name = name,
                CreateDatetime = DateTime.Now,
                UpdateDatetime = DateTime.Now,
            };
        }
    }
}
