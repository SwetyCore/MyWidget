using System.Security.Cryptography;
using System.Text;

namespace DGP.Genshin.GamebarWidget.MiHoYoAPI
{
    /// <summary>
    /// 为动态密钥提供器提供Md5算法
    /// </summary>
    public abstract class Md5Converter
    {
        public static string GetComputedMd5(string source)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] result = md5.ComputeHash(Encoding.UTF8.GetBytes(source));

                StringBuilder builder = new StringBuilder();
                foreach (byte b in result)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
