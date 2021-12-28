using System;
using TestChatTool.Domain.Enum;

namespace TestChatTool.Domain.Model
{
    public class CallBackEventData
    {
        /// <summary>
        /// ActionName
        /// </summary>
        public CallBackActionType Action { get; set; }

        /// <summary>
        /// 房間代碼
        /// </summary>
        public string RoomCode { get; set; }

        /// <summary>
        /// 暱稱
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 訊息內容
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 創建時間
        /// </summary>
        public DateTime? CreateDateTime { get; set; }
    }
}
