using System;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using TestChatTool.Domain.Enum;

namespace TestChatTool.Domain.Model
{
    public class User
    {
        [BsonId()]
        public string Account { get; set; }
        public string Password { get; set; }
        public string NickName { get; set; }
        /// <summary>
        /// 狀態 0-未啟用 1-啟用 2-鎖定 3-解除鎖定(未重啟)
        /// </summary>
        public UserStatusType Status { get; set; } = 0;
        public int ErrCount { get; set; } = 0;
        public DateTime? LastDatetime { get; set; }
        public DateTime CreateDatetime { get; set; }
        public DateTime UpdateDatetime { get; set; }

        public static User GenerateInstance(string acc, string pwd, string name)
        {
            return new User
            {
                Account = acc,
                Password = pwd,
                NickName = name,
                Status = UserStatusType.Disabled,
                ErrCount = 0,
                CreateDatetime = DateTime.Now,
                UpdateDatetime = DateTime.Now,
            };
        }

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
