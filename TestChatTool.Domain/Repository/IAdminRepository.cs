using System;
using TestChatTool.Domain.Model;

namespace TestChatTool.Domain.Repository
{
    public interface IAdminRepository
    {
        (Exception ex, bool isSuccess, bool isAccDuplicate) Create(Admin info);
        (Exception ex, Admin admin) Query(string acc);
    }
}
