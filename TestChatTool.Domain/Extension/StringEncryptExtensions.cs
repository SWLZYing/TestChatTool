using System;
using System.Security.Cryptography;
using System.Text;

namespace TestChatTool.Domain.Extension
{
    public static class StringEncryptExtensions
    {
        public static string ToMD5(this string into)
        {
            using (var encrypt = MD5.Create())
            {
                //將字串編碼成 UTF8 位元組陣列
                var bytes = Encoding.UTF8.GetBytes(into);

                //取得雜湊值位元組陣列
                var hash = encrypt.ComputeHash(bytes);

                return BitConverter.ToString(hash)
                    .Replace("-", string.Empty)
                    .ToLower();
            }
        }
    }
}
