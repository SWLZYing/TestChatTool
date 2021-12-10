using System;
using TestChatTool.Domain.Model;

namespace TestChatTool.Domain.Repository
{
    public interface IAdminRepository
    {
        (Exception ex, Admin result) CreateAdmin(Admin info);
        (Exception ex, Admin result) QueryAdmin(string acc);
    }
}
