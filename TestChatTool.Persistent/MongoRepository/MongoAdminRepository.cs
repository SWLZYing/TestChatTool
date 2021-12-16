using MongoDB.Driver;
using System;
using TestChatTool.Domain.Extension;
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

        public (Exception ex, bool isSuccess, bool isAccDuplicate) Create(Admin info)
        {
            try
            {
                info.Password = info.Password.ToMD5();

                _collection.InsertOne(info);

                return (null, true, false);
            }
            catch (MongoWriteException mEx)
            {
                if (mEx.WriteError.Category == ServerErrorCategory.DuplicateKey)
                {
                    return (mEx, false, true);
                }

                return (mEx, false, false);
            }
            catch (Exception ex)
            {
                return (ex, false, false);
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
