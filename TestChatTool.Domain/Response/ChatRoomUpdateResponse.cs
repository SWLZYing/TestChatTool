using TestChatTool.Domain.Model;

namespace TestChatTool.Domain.Response
{
    public class ChatRoomUpdateResponse : BasicResponse
    {
        public ChatRoom Room { get; set; }
    }
}
