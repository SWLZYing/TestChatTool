using System.Collections.Generic;
using TestChatTool.Domain.Model;

namespace TestChatTool.Domain.Response
{
    public class ChatRoomGetAllResponse : BasicResponse
    {
        public IEnumerable<ChatRoom> Rooms { get; set; }
    }
}
