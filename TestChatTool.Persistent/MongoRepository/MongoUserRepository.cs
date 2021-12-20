using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using TestChatTool.Domain.Enum;
using TestChatTool.Domain.Extension;
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

        public (Exception ex, bool isSuccess, bool isAccDuplicate) Create(User info)
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
                    Builders<User>.Filter.Eq(e => e.Password, oldPwd.ToMD5()));
                var update = Builders<User>.Update
                    .Set(s => s.Password, newPwd.ToMD5())
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

        public (Exception ex, bool isSuccess) SetErrCountAndStatus(string acc, int errCount, UserStatusType status = UserStatusType.Disabled)
        {
            try
            {
                var filter = Builders<User>.Filter.And(
                    Builders<User>.Filter.Eq(e => e.Account, acc));
                var update = status > 0
                    ? Builders<User>.Update
                    .Set(s => s.ErrCount, errCount)
                    .Set(s => (int)s.Status, (int)status)
                    .Set(s => s.UpdateDatetime, DateTime.Now)
                    : Builders<User>.Update
                    .Set(s => s.ErrCount, errCount)
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

        public (Exception ex, User result) SignInRefresh(string acc)
        {
            try
            {
                var filter = Builders<User>.Filter.And(
                    Builders<User>.Filter.Eq(e => e.Account, acc));
                var update = Builders<User>.Update
                    .Set(s => s.ErrCount, 0)
                    .Set(s => s.LastDatetime, DateTime.Now)
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

        public (Exception ex, List<string> accs) GetAllForVerify()
        {
            try
            {
                var result = _collection.Find(f => f.Status == UserStatusType.Disabled).ToList();

                var accs = result.Select(s => s.Account).ToList();

                return (null, accs);
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }

        public (Exception ex, List<string> accs) GetAllForUnlock()
        {
            try
            {
                var result = _collection.Find(f => f.Status == UserStatusType.Lock).ToList();

                var accs = result.Select(s => s.Account).ToList();

                return (null, accs);
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }
    }
}
