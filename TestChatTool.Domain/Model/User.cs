using System;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace TestChatTool.Domain.Model
{
    public class User
    {
        [BsonId()]
        public string Account { get; set; }
        public string Password { get; set; }
        public string NickName { get; set; }
        public int Status { get; set; }
        public int ErrCount { get; set; }
        public DateTime LastDatetime { get; set; }
        public DateTime CreateDatetime { get; set; }
        public DateTime UpdateDatetime { get; set; }

        public static User GenerateInstance(string acc, string pwd, string name)
        {
            return new User
            {
                Account = acc,
                Password = pwd,
                NickName = name,
                Status = 0,
                ErrCount = 0,
                LastDatetime = DateTime.MinValue,
                CreateDatetime = DateTime.Now,
                UpdateDatetime = DateTime.Now,
            };
        }

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
