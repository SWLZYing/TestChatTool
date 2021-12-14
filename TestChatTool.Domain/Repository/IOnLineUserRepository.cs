using System;
using System.Collections.Generic;
using TestChatTool.Domain.Model;

namespace TestChatTool.Domain.Repository
{
    public interface IOnLineUserRepository
    {
        (Exception ex, OnLineUser result) Upsert(OnLineUser info);

        (Exception ex, List<OnLineUser> result) FindRoomUser(string code);
        (Exception ex, List<(string, int)> result) FindAllUserCountByRoom();
    }
}
