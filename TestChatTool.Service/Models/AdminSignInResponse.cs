using TestChatTool.Domain.Model;

namespace TestChatTool.Service.Models
{
    public class AdminSignInResponse : BasicResponse
    {
        public Admin Data { get; set; }
    }
}
