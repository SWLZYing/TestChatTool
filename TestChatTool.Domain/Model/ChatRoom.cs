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

        public ChatRoom GenerateInstance(string code, string name)
        {
            return new ChatRoom
            {
                Code = code,
                Name = name,
                CreateDatetime = DateTime.Now,
                UpdateDatetime = DateTime.Now,
            };
        }

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
