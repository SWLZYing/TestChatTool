namespace TestChatTool.Domain.Enum
{
    public enum ErrorType
    {
        // 1 傳入資訊
        FieldNull = 1001,

        // 2 資料檢核
        AccError = 2001,
        PwdError = 2002,
        PwdErrorToLock = 2003,

        // 3 會員資料
        AccNotVerify = 3001,
        AccLock = 3002,

        // 9 系統流程
        Failed = 9001,
        Success = 9999,
    }
}
