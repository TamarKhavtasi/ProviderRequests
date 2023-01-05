
using System.Security.Cryptography;
using System.Text;

namespace Idram.Api;

public static class ChecksumExtension
{
    public static string ComputeChecksum(this List<string> list)
    {
        var content = string.Join(":", list);
        var str = "";
        using (MD5 md5 = MD5.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(content);
            byte[] hash = md5.ComputeHash(bytes);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte num in hash)
                stringBuilder.Append(num.ToString("X2"));
            str= stringBuilder.ToString();
        }
        return str.ToLower();
    }
}
