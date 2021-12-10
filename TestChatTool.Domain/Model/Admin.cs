using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;

namespace TestChatTool.Domain.Model
{
    public class Admin
    {
        [BsonId()]
        public string Account { get; set; }
        public string Password { get; set; }
        public int AccountType { get; set; }
        public DateTime LastDatetime { get; set; }
        public DateTime CreateDatetime { get; set; }
        public DateTime UpdateDatetime { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
