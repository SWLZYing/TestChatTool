using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using System;
using TestChatTool.Domain.Extension;
using TestChatTool.Domain.Model;
using TestChatTool.Domain.Repository;
using TestChatTool.Persistent.MongoRepository;

namespace TestChatTool.Persistent.Tests.MongoRepository
{
    [TestClass]
    public class MongoAdminRepositoryTests
    {
        private IAdminRepository _repository;
        private const string _context = "mongodb://localhost:27017";

        [TestInitialize]
        public void Init()
        {
            var client = new MongoClient(_context);
            var db = client.GetDatabase("TestChatTool");

            // 清空測試資料表確保資料正確
            db.DropCollection("Admin");

            _repository = new MongoAdminRepository(client);
        }

        [TestMethod]
        public void create_admin()
        {
            var result = _repository.Create(Admin.GenerateInstance("admin", "admin".ToMD5()));

            Assert.IsNull(result.ex);
            Assert.IsTrue(result.isSuccess);
        }

        [TestMethod]
        public void create_admin_exist()
        {
            _repository.Create(Admin.GenerateInstance("admin", "admin".ToMD5()));

            var result = _repository.Create(Admin.GenerateInstance("admin", "admin".ToMD5()));

            Assert.IsNotNull(result.ex);
            Assert.IsTrue(result.isAccDuplicate);
            Console.WriteLine(result.ex.Message);
        }

        [TestMethod]
        public void query_admin()
        {
            _repository.Create(Admin.GenerateInstance("admin", "admin".ToMD5()));

            var result = _repository.Query("admin");

            Assert.IsNull(result.ex);
            Console.WriteLine(result.result);
        }
    }
}
