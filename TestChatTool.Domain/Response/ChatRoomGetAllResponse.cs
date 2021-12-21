using System.Collections.Generic;

namespace TestChatTool.Domain.Response
{
    public class ChatRoomGetAllResponse : BasicResponse
    {
        public List<(string Code, string Name)> Data { get; set; }
    }
}
