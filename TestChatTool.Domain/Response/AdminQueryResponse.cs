using TestChatTool.Domain.Model;

namespace TestChatTool.Domain.Response
{
    public class AdminQueryResponse : BasicResponse
    {
        public Admin Admin { get; set; }
    }
}
