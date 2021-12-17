using TestChatTool.Domain.Model;

namespace TestChatTool.Domain.Response
{
    public class AdminSignInResponse : BasicResponse
    {
        public Admin Data { get; set; }
    }
}
