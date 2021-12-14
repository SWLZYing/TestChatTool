using MongoDB.Driver;
using System;
using TestChatTool.Domain.Model;
using TestChatTool.Domain.Repository;

namespace TestChatTool.Persistent.MongoRepository
{
    public class MongoChatRoomRepository : IChatRoomRepository
    {
        private readonly IMongoCollection<ChatRoom> _collection;

        public MongoChatRoomRepository(MongoClient client)
        {
            _collection = client
                .GetDatabase("TestChatTool")
                .GetCollection<ChatRoom>("ChatRoom");
        }

        public (Exception ex, ChatRoom result) Create(ChatRoom info)
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

        public (Exception ex, ChatRoom result) Query(string code)
        {
            try
            {
                var result = _collection.Find(f => f.Code == code).FirstOrDefault();

                return (null, result);
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }

        public (Exception ex, ChatRoom result) Update(string code, string name)
        {
            try
            {
                var filter = Builders<ChatRoom>.Filter.Eq(e => e.Code, code);
                var update = Builders<ChatRoom>.Update
                    .Set(s => s.Name, name)
                    .Set(s => s.UpdateDatetime, DateTime.Now);

                var result = _collection.FindOneAndUpdate(
                    filter,
                    update,
                    new FindOneAndUpdateOptions<ChatRoom, ChatRoom>() { IsUpsert = false, ReturnDocument = ReturnDocument.After });

                return (null, result);
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }
    }
}
