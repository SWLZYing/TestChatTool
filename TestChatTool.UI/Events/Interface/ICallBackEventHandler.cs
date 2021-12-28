using System;
using TestChatTool.Domain.Model;

namespace TestChatTool.UI.Events.Interface
{
    public interface ICallBackEventHandler
    {
        /// <summary>
        /// 加入事件
        /// </summary>
        /// <param name="action"></param>
        void Add(Action<CallBackEventData> action);

        /// <summary>
        /// 移除事件
        /// </summary>
        /// <param name="action"></param>
        void Remove(Action<CallBackEventData> action);

        /// <summary>
        /// 執行事件
        /// </summary>
        /// <param name="eventData"></param>
        void DoWork(CallBackEventData eventData);
    }
}
