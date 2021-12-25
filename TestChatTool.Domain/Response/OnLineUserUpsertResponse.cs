using TestChatTool.Domain.Model;

namespace TestChatTool.Domain.Response
{
    public class OnLineUserUpsertResponse : BasicResponse
    {
        public OnLineUser User { get; set; }
    }
}
