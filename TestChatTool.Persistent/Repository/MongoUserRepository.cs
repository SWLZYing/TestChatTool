using MongoDB.Driver;
using System;
using TestChatTool.Domain.Model;
using TestChatTool.Domain.Repository;

namespace TestChatTool.Persistent.Repository
{
    public class MongoUserRepository : IMongoUserRepository
    {
        private MongoClient _client { get; set; }
        private IMongoDatabase _mongoDb { get; set; }
        private IMongoCollection<User> _collection { get; set; }

        public MongoUserRepository(MongoClient client)
        {
            _client = client;
            _mongoDb = _client.GetDatabase("TestChatTool");
            _collection = _mongoDb.GetCollection<User>("User");
            _collection.Indexes.CreateOne(Builders<User>.IndexKeys.Descending(c => c.Account));
        }


        public (Exception ex, User result) CreateUser(User info)
        {
            try
            {
                _collection.InsertOne(info);

                var result = _collection.Find(f => f.Account == info.Account).FirstOrDefault();

                return (null, result);
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }

        public (Exception ex, User result) QueryUser(string acc)
        {
            throw new NotImplementedException();
        }

        public (Exception ex, bool isSuccess) ResetPwd(string acc, string oldPwd, string newPwd)
        {
            throw new NotImplementedException();
        }

        public (Exception ex, User result) UpdateUser(User info)
        {
            throw new NotImplementedException();
        }
    }
}
