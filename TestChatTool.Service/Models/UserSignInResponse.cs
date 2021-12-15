using TestChatTool.Domain.Model;

namespace TestChatTool.Service.Models
{
    public class UserSignInResponse : BasicResponse
    {
        public User Data { get; set; }
    }
}
