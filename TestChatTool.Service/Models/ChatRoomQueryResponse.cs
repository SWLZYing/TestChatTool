using TestChatTool.Domain.Model;

namespace TestChatTool.Service.Models
{
    public class ChatRoomQueryResponse : BasicResponse
    {
        public ChatRoom Data { get; set; }
    }
}
