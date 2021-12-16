using TestChatTool.Domain.Model;

namespace TestChatTool.Service.Models
{
    public class AdminQueryResponse : BasicResponse
    {
        public Admin Data { get; set; }
    }
}
