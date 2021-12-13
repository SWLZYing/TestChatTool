using System;
using TestChatTool.Domain.Model;

namespace TestChatTool.Domain.Repository
{
    public interface IChatRoomRepository
    {
        (Exception ex, ChatRoom result) Create(ChatRoom info);
        (Exception ex, ChatRoom result) Query(string code);
        (Exception ex, ChatRoom result) Update(string code, string name);
    }
}
