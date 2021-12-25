using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
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
                    new CreateIndexOptions(){ ExpireAfter = TimeSpan.FromSeconds(300)})
            });
        }

        public (Exception ex, OnLineUser user) Upsert(OnLineUser info)
        {
            try
            {
                var filter = Builders<OnLineUser>.Filter.Eq(e => e.Account, info.Account);
                var update = Builders<OnLineUser>.Update
                    .SetOnInsert(s => s.Account, info.Account)
                    .Set(s => s.NickName, info.NickName)
                    .Set(s => s.RoomCode, info.RoomCode)
                    .Set(s => s.UpdateDatetime, DateTime.Now);

                var result = _collection.FindOneAndUpdate(
                     filter,
                     update,
                     new FindOneAndUpdateOptions<OnLineUser, OnLineUser>() { IsUpsert = true, ReturnDocument = ReturnDocument.After });

                return (null, result);
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }

        public (Exception ex, IEnumerable<OnLineUser> users) FindRoomUser(string code)
        {
            try
            {
                var result = _collection.Find(f => f.RoomCode == code).ToList();

                return (null, result);
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }

        public (Exception ex, IEnumerable<(string roomCode, int userCount)> result) FindAllUserCountByRoom()
        {
            try
            {
                var result = _collection.Find(f => f.RoomCode != string.Empty).ToList();

                var byRoom = result
                    .GroupBy(g => g.RoomCode)
                    .Select(s => new ValueTuple<string, int>(s.Key, s.Count()))
                    .ToList();

                return (null, byRoom);
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }
    }
}
