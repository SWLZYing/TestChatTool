using MongoDB.Driver;
using System;
using System.Collections.Generic;
using TestChatTool.Domain.Model;
using TestChatTool.Domain.Repository;

namespace TestChatTool.Persistent.MongoRepository
{
    public class MongoOnLineUserRepository : IOnLineUserRepository
    {
        private readonly IMongoCollection<OnLineUser> _collection;

        public MongoOnLineUserRepository(MongoClient client)
        {
            _collection = client
                .GetDatabase("TestChatTool")
                .GetCollection<OnLineUser>("OnLineUser");

            _collection.Indexes.CreateMany(new CreateIndexModel<OnLineUser>[]
            {
                new CreateIndexModel<OnLineUser>(
                    Builders<OnLineUser>.IndexKeys.Descending(p => p.UpdateDatetime),
                    new CreateIndexOptions(){ ExpireAfter = TimeSpan.FromSeconds(30)})
            });
        }

        public (Exception ex, OnLineUser result) Upsert(OnLineUser info)
        {
            try
            {
                var filter = Builders<OnLineUser>.Filter.Eq(e => e.Account, info.Account);
                var update = Builders<OnLineUser>.Update
                    .SetOnInsert(s => s.Account, info.Account)
                    .Set(s => s.NickName, info.NickName)
                    .Set(s => s.RoomCode, info.RoomCode)
                    .Set(s => s.UpdateDatetime, DateTime.Now);

                return (null, _collection.FindOneAndUpdate(
                     filter,
                     update,
                     new FindOneAndUpdateOptions<OnLineUser, OnLineUser>() { IsUpsert = true, ReturnDocument = ReturnDocument.After }));
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }

        public (Exception ex, List<OnLineUser> result) FindRoomUser(string code)
        {
            try
            {
                return (null, _collection.Find(f => f.RoomCode == code).ToList());
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }
    }
}
