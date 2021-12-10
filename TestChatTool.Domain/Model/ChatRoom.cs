using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;

namespace TestChatTool.Domain.Model
{
    public class ChatRoom
    {
        [BsonId()]
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime CreateDatetime { get; set; }
        public DateTime UpdateDatetime { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
