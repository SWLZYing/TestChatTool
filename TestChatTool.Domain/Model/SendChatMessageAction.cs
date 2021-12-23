using System;

namespace TestChatTool.Domain.Model
{
    /// <summary>
    /// 發送聊天訊息
    /// </summary>
    public class SendChatMessageAction : ActionBase
    {
        public override string Action()
            => $"SendChatMessage";

        /// <summary>
        /// 暱稱
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
        public DateTime CreateDateTime { get; set; }
    }
}
