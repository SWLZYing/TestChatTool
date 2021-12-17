using TestChatTool.Domain.Model;

namespace TestChatTool.Service.Models
{
    public class OnLineUserUpsertResponse : BasicResponse
    {
        public OnLineUser Data { get; set; }
    }
}
