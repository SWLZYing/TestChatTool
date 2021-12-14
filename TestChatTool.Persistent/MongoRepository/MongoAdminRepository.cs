using MongoDB.Driver;
using System;
using TestChatTool.Domain.Model;
using TestChatTool.Domain.Repository;

namespace TestChatTool.Persistent.MongoRepository
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

        public (Exception ex, Admin result) Create(Admin info)
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

        public (Exception ex, Admin result) Query(string acc)
        {
            try
            {
                var result = _collection.Find(f => f.Account == acc).FirstOrDefault();

                return (null, result);
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }
    }
}
