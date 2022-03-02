using DGP.Genshin.GamebarWidget.Helper;
using System;
using System.Linq;

namespace DGP.Genshin.GamebarWidget.MiHoYoAPI
{
    /// <summary>
    /// 为MiHoYo接口请求器 <see cref="Requester"/> 提供2代动态密钥
    /// </summary>
    public class DynamicSecretProvider2 : Md5Converter
    {
        /// <summary>
        /// 防止从外部创建 <see cref="DynamicSecretProvider2"/> 的实例
        /// </summary>
        private DynamicSecretProvider2() { }

        /// <summary>
        /// 似乎已经与版本号无关，自2.11.1以来未曾改变salt
        /// </summary>
        public const string AppVersion = "2.16.1";

        /// <summary>
        /// 米游社的盐
        /// 计算过程：https://gist.github.com/Lightczx/373c5940b36e24b25362728b52dec4fd
        /// </summary>
        private static readonly string APISalt = "xV8v4Qu54lUKrEYFZkJhB8cuOh9Asafs";

        public static string Create(string queryUrl, object postBody = null)
        {
            //unix timestamp
            long t = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            //random
            string r = GetRandomString();
            //body
            string b = postBody is null ? "" : Json.Stringify(postBody);
            //query
            string q = "";
            string[] urlParts = queryUrl.Split('?');
            if (urlParts.Length == 2)
            {
                string[] queryParams = urlParts[1].Split('&').OrderBy(x => x).ToArray();
                q = string.Join("&", queryParams);
            }
            //check
            string check = GetComputedMd5($"salt={APISalt}&t={t}&r={r}&b={b}&q={q}");

            return $"{t},{r},{check}";
        }

        private static readonly Random random = new Random();
        private static string GetRandomString()
        {
            return random.Next(100000, 200000).ToString();
        }
    }
}
