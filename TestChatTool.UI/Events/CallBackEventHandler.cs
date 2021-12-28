using System;
using TestChatTool.Domain.Model;
using TestChatTool.UI.Events.Interface;

namespace TestChatTool.UI.Events
{
    public class CallBackEventHandler : ICallBackEventHandler
    {
        private event Action<CallBackEventData> _events;

        public void Add(Action<CallBackEventData> action)
        {
            _events += action;
        }

        public void Remove(Action<CallBackEventData> action)
        {
            _events -= action;
        }

        public void DoWork(CallBackEventData eventData)
        {
            if (_events != null)
            {
                _events(eventData);
            }
        }
    }
}
