using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;

namespace TestChatTool.Domain.Model
{
    public class MongoModelBase
    {
        [BsonId(IdGenerator = typeof(CombGuidGenerator))]
        public Guid _id { get; set; }
    }
}
