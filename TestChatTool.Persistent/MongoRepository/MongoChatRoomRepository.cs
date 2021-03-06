using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public (Exception ex, bool isSuccess, bool isAccDuplicate) Create(ChatRoom info)
        {
            try
            {
                var now = DateTime.Now;
                info.CreateDatetime = now;
                info.UpdateDatetime = now;

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

        public (Exception ex, ChatRoom room) Query(string code)
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

        public (Exception ex, ChatRoom room) Update(string code, string name)
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

        public (Exception ex, bool isSuccess) Delete(string code)
        {
            try
            {
                var filter = Builders<ChatRoom>.Filter.Eq(e => e.Code, code);

                var result = _collection.DeleteOne(filter);

                return (null, result.IsAcknowledged);
            }
            catch (Exception ex)
            {
                return (ex, false);
            }
        }

        public (Exception ex, IEnumerable<ChatRoom> rooms) GetAll()
        {
            try
            {
                var fiter = Builders<ChatRoom>.Filter.Empty;
                var result = _collection.Find(fiter).ToList();

                return (null, result);
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }
    }
}
