namespace TestChatTool.Domain.Enum
{
    public enum CallBackActionType
    {
        /// <summary>
        /// 聊天訊息
        /// </summary>
        ChatMessage,

        /// <summary>
        /// 進入聊天室通知
        /// </summary>
        EnterRoom,

        /// <summary>
        /// 離開聊天室通知
        /// </summary>
        LeaveRoom,

        /// <summary>
        /// 確認連線
        /// </summary>
        CheckConnect,

        /// <summary>
        /// 聊天室異動
        /// </summary>
        UpsertChatRoom,

        /// <summary>
        /// 聊天室刪除
        /// </summary>
        DeleteChatRoom,
    }
}
