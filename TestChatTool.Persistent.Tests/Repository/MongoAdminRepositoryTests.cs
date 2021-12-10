using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using System;
using TestChatTool.Domain.Model;
using TestChatTool.Domain.Repository;
using TestChatTool.Persistent.Repository;

namespace TestChatTool.Persistent.Tests.Repository
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
            var result = _repository.CreateAdmin(NewAdmin("admin", "admin"));

            Assert.IsNull(result.ex);
            Console.WriteLine(result.result);
        }

        [TestMethod]
        public void create_admin_exist()
        {
            _repository.CreateAdmin(NewAdmin("admin", "admin"));

            var result = _repository.CreateAdmin(NewAdmin("admin", string.Empty));

            Assert.IsNotNull(result.ex);
            Console.WriteLine(result.ex.Message);
        }

        [TestMethod]
        public void query_admin()
        {
            _repository.CreateAdmin(NewAdmin("admin", "admin"));

            var result = _repository.QueryAdmin("admin");

            Assert.IsNull(result.ex);
            Console.WriteLine(result.result);
        }

        private Admin NewAdmin(string acc, string pwd)
        {
            return new Admin
            {
                Account = acc,
                Password = pwd,
                AccountType = 1,
                LastDatetime = DateTime.MinValue,
                CreateDatetime = DateTime.Now,
                UpdateDatetime = DateTime.Now,
            };
        }
    }
}
