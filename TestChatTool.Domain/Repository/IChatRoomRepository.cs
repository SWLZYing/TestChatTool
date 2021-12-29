using System;
using System.Collections.Generic;
using TestChatTool.Domain.Model;

namespace TestChatTool.Domain.Repository
{
    public interface IChatRoomRepository
    {
        (Exception ex, bool isSuccess, bool isAccDuplicate) Create(ChatRoom info);
        (Exception ex, ChatRoom room) Query(string code);
        (Exception ex, ChatRoom room) Update(string code, string name);
        (Exception ex, bool isSuccess) Delete(string code);
        (Exception ex, IEnumerable<ChatRoom> rooms) GetAll();
    }
}
