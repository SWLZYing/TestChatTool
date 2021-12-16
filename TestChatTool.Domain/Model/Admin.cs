using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using TestChatTool.Domain.Enum;

namespace TestChatTool.Domain.Model
{
    public class Admin
    {
        [BsonId()]
        public string Account { get; set; }
        public string Password { get; set; }
        /// <summary>
        /// 使用者類型 1-Admin 2-Normal
        /// </summary>
        public AdminType AccountType { get; set; }
        public DateTime CreateDatetime { get; set; }
        public DateTime UpdateDatetime { get; set; }

        public static Admin GenerateInstance(string acc, string pwd)
        {
            return new Admin
            {
                Account = acc,
                Password = pwd,
                AccountType = AdminType.Admin,
                CreateDatetime = DateTime.Now,
                UpdateDatetime = DateTime.Now,
            };
        }

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
