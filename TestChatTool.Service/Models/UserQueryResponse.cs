using TestChatTool.Domain.Model;

namespace TestChatTool.Service.Models
{
    public class UserQueryResponse : BasicResponse
    {
        public User Data { get; set; }
    }
}
