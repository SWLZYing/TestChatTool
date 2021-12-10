﻿using System;
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

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
