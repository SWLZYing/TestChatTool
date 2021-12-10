using System;
using TestChatTool.Domain.Model;

namespace TestChatTool.Domain.Repository
{
    public interface IAdminRepository
    {
        (Exception ex, Admin result) Create(Admin info);
        (Exception ex, Admin result) Query(string acc);
    }
}
