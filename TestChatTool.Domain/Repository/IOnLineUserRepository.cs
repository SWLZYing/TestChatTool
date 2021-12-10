using System;
using System.Collections.Generic;
using TestChatTool.Domain.Model;

namespace TestChatTool.Domain.Repository
{
    public interface IOnLineUserRepository
    {
        (Exception ex, OnLineUser result) Creeate(OnLineUser info);
        (Exception ex, OnLineUser result) Update(OnLineUser info);
        (Exception ex, bool isSuccess) Delete(string acc);

        (Exception ex, List<OnLineUser> result) FindRoomUser(string code);
    }
}
