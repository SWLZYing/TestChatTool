using TestChatTool.Domain.Model;

namespace TestChatTool.Service.Models
{
    public class ChatRoomUpdateResponse : BasicResponse
    {
        public ChatRoom Data { get; set; }
    }
}
