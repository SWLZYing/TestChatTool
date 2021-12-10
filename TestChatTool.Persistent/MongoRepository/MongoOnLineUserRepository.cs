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
        }

        public (Exception ex, OnLineUser result) Creeate(OnLineUser info)
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

        public (Exception ex, OnLineUser result) Update(OnLineUser info)
        {
            throw new NotImplementedException();
        }

        public (Exception ex, bool isSuccess) Delete(string acc)
        {
            throw new NotImplementedException();
        }

        public (Exception ex, List<OnLineUser> result) FindRoomUser(string code)
        {
            throw new NotImplementedException();
        }
    }
}
