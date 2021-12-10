using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;

namespace TestChatTool.Domain.Model
{
    public class OnLineUser
    {
        [BsonId()]
        public string Account { get; set; }
        public string NickName { get; set; }
        public string RoomCode { get; set; }
        public DateTime UpdateDatetime { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
