﻿using TestChatTool.Domain.Model;

namespace TestChatTool.Domain.Response
{
    public class ChatRoomQueryResponse : BasicResponse
    {
        public ChatRoom Room { get; set; }
    }
}
