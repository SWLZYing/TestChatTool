using System;
using TestChatTool.Domain.Model;
using TestChatTool.Service.Enums;

namespace TestChatTool.Service.ActionHandler.Interface
{
    /// <summary>
    /// Action 處理介面
    /// </summary>
    public interface IActionHandler
    {
        /// <summary>
        /// 處理action
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        (Exception exception, NotifyType notifyType, ActionBase actionBase) ExecuteAction(ActionModule action);
    }
}
