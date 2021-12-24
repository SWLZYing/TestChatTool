using TestChatTool.Domain.Response;

namespace TestChatTool.UI.Helpers.Interface
{
    public interface ISignControllerApiHelper
    {
        AdminSignInResponse AdminSignIn(string account, string password);
        UserSignInResponse UserSignIn(string account, string password);
    }
}
