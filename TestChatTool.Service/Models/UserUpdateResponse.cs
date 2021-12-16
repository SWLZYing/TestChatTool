using TestChatTool.Domain.Model;

namespace TestChatTool.Service.Models
{
    public class UserUpdateResponse : BasicResponse
    {
        public User Data { get; set; }
    }
}
