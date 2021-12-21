using System.Collections.Generic;

namespace TestChatTool.Domain.Response
{
    public class ChatRoomGetAllResponse : BasicResponse
    {
        public List<(string, string)> Data { get; set; }
    }
}
