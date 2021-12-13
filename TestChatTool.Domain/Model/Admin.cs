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

        public Admin GenerateInstance(string acc, string pwd)
        {
            return new Admin
            {
                Account = acc,
                Password = pwd,
                AccountType = 1,
                LastDatetime = DateTime.MinValue,
                CreateDatetime = DateTime.Now,
                UpdateDatetime = DateTime.Now,
            };
        }

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
