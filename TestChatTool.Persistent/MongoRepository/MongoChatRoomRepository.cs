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
                return (null, _collection.Find(f => f.Code == code).FirstOrDefault());
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }

        public (Exception ex, ChatRoom result) Update(ChatRoom info)
        {
            try
            {
                var filter = Builders<ChatRoom>.Filter.Eq(e => e.Code, info.Code);
                var update = Builders<ChatRoom>.Update
                    .Set(s => s.Name, info.Name)
                    .Set(s => s.UpdateDatetime, info.UpdateDatetime);

                return (null, _collection.FindOneAndUpdate(
                    filter,
                    update,
                    new FindOneAndUpdateOptions<ChatRoom, ChatRoom>() { IsUpsert = false, ReturnDocument = ReturnDocument.After }));
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }
    }
}
