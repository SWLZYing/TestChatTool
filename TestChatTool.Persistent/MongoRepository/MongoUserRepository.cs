using MongoDB.Driver;
using System;
using TestChatTool.Domain.Model;
using TestChatTool.Domain.Repository;

namespace TestChatTool.Persistent.MongoRepository
{
    public class MongoUserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _collection;

        public MongoUserRepository(MongoClient client)
        {
            _collection = client
                .GetDatabase("TestChatTool")
                .GetCollection<User>("User");
        }

        public (Exception ex, User result) Create(User info)
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

        public (Exception ex, User result) Query(string acc)
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

        public (Exception ex, User result) Update(User info)
        {
            try
            {
                var filter = Builders<User>.Filter.Eq(e => e.Account, info.Account);
                var update = Builders<User>.Update
                    .Set(s => s.NickName, info.NickName)
                    .Set(s => s.UpdateDatetime, DateTime.Now);

                var result = _collection.FindOneAndUpdate(
                    filter,
                    update,
                    new FindOneAndUpdateOptions<User, User>() { IsUpsert = false, ReturnDocument = ReturnDocument.After });

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

                var result = _collection.UpdateOne(
                    filter,
                    update,
                    new UpdateOptions() { IsUpsert = false }).ModifiedCount > 0;

                return (null, result);
            }
            catch (Exception ex)
            {
                return (ex, false);
            }
        }
    }
}
