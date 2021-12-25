using TestChatTool.Domain.Model;

namespace TestChatTool.Domain.Response
{
    public class UserQueryResponse : BasicResponse
    {
        public User User { get; set; }
    }
}
