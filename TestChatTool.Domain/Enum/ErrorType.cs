namespace TestChatTool.Domain.Enum
{
    public enum ErrorType
    {
        // 1 資料檢核
        FieldNull = 1001,

        AccError = 1021,
        AccExist = 1022,

        PwdError = 1041,
        PwdErrorToLock = 1042,

        // 2 會員資料
        AccNotVerify = 2001,
        AccLock = 2002,

        // 9 系統流程
        Failed = 9001,
        SystemError = 9002,
        Success = 9999,
    }
}
