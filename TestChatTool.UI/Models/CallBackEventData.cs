using System;
using TestChatTool.Domain.Enum;

namespace TestChatTool.UI.Models
{
    public class CallBackEventData
    {
        /// <summary>
        /// ActionName
        /// </summary>
        public CallBackActionType Action { get; set; }

        /// <summary>
        /// 聊天室代碼
        /// </summary>
        public string RoomCode { get; set; }

        /// <summary>
        /// 帳號
        /// </summary>
        public string Account { get; set; }

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
