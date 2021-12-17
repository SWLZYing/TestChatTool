using TestChatTool.Domain.Model;

namespace TestChatTool.Domain.Response
{
    public class UserUpdateResponse : BasicResponse
    {
        public User Data { get; set; }
    }
}
