using TestChatTool.Domain.Model;

namespace TestChatTool.Domain.Response
{
    public class UserSignInResponse : BasicResponse
    {
        public User User { get; set; }
    }
}
