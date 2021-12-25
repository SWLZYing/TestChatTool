using TestChatTool.Domain.Response;

namespace TestChatTool.UI.Helpers.Interface
{
    public interface IOnLineUserControllerApiHelper
    {
        OnLineUserUpsertResponse Upsert(string account, string nickName, string roomCode);
        OnLineUserFindRoomUserResponse FindRoomUser(string roomCode);
    }
}
