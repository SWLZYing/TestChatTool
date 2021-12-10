using MongoDB.Driver;
using System;
using TestChatTool.Domain.Model;
using TestChatTool.Domain.Repository;

namespace TestChatTool.Persistent.Repository
{
    public class MongoUserRepository : IMongoUserRepository
    {
        private readonly MongoClient _client;
        private readonly IMongoDatabase _mongoDb;
        private readonly IMongoCollection<User> _collection;

        public MongoUserRepository(MongoClient client)
        {
            _client = client;
            _mongoDb = _client.GetDatabase("TestChatTool");
            _collection = _mongoDb.GetCollection<User>("User");
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

        public (Exception ex, User result) UpdateUser(User info)
        {
            try
            {
                var filter = Builders<User>.Filter.Eq(e => e.Account, info.Account);
                var update = Builders<User>.Update
                    .Set(s => s.NickName, info.NickName)
                    .Set(s => s.UpdateDatetime, DateTime.Now);

                _collection.UpdateOne(filter, update);

                var result = _collection.Find(f => f.Account == info.Account).FirstOrDefault();

                return (null, result);
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }

        public (Exception ex, bool isSuccess) ResetPwd(string acc, string oldPwd, string newPwd)
        {
            try
            {
                var filter = Builders<User>.Filter.And(
                    Builders<User>.Filter.Eq(e => e.Account, acc),
                    Builders<User>.Filter.Eq(e => e.Password, oldPwd));
                var update = Builders<User>.Update
                    .Set(s => s.Password, newPwd)
                    .Set(s => s.UpdateDatetime, DateTime.Now);

                var isSuccess = _collection.UpdateOne(filter, update, new UpdateOptions() { IsUpsert = false }).MatchedCount > 0;

                return (null, isSuccess);
            }
            catch (Exception ex)
            {
                return (ex, false);
            }

        }
    }
}
