using TestChatTool.Domain.Model;

namespace TestChatTool.UI.Handlers.Interface
{
    /// <summary>
    /// Action處理工廠
    /// </summary>
    public interface IActionHandler
    {
        /// <summary>
        /// 執行action
        /// </summary>
        /// <param name="actionModule"></param>
        /// <returns></returns>
        bool Execute(ActionModule actionModule);
    }
}
