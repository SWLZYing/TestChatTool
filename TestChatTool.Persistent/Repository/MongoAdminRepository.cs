using MongoDB.Driver;
using System;
using TestChatTool.Domain.Model;
using TestChatTool.Domain.Repository;

namespace TestChatTool.Persistent.Repository
{
    public class MongoAdminRepository : IAdminRepository
    {
        private readonly IMongoCollection<Admin> _collection;

        public MongoAdminRepository(MongoClient client)
        {
            _collection = client
                .GetDatabase("TestChatTool")
                .GetCollection<Admin>("Admin");
        }

        public (Exception ex, Admin result) CreateAdmin(Admin info)
        {
            try
            {
                _collection.InsertOne(info);

                return (null, info);
            }
            catch (MongoDuplicateKeyException mEx)
            {
                return (mEx, null);
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }

        public (Exception ex, Admin result) QueryAdmin(string acc)
        {
            try
            {
                return (null, _collection.Find(f => f.Account == acc).FirstOrDefault());
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }
    }
}
