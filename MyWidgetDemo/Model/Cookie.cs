using System.Collections.Generic;
using System.Linq;

namespace DGP.Genshin.GamebarWidget.Model
{
    internal class Cookie
    {
        public Cookie(string cookieValue)
        {
            CookieValue = cookieValue;
        }

        public string DisplayName 
        { 
            get 
            {
                IDictionary<string, string> dict = GetCookiePairs(CookieValue);
                return dict.TryGetValue("account_id", out string account) ? account : "无效的Cookie";
            } 
        }

        public string CookieValue { get; }

        /// <summary>
        /// 获取Cookie的键值对
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        private IDictionary<string, string> GetCookiePairs(string cookie)
        {
            Dictionary<string, string> cookieDictionary = new Dictionary<string, string>();

            string[] values = cookie.TrimEnd(';').Split(';');
            foreach (string[] parts in values.Select(c => c.Split(new[] { '=' }, 2)))
            {
                string cookieName = parts[0].Trim();
                string cookieValue;

                if (parts.Length == 1)
                {
                    //Cookie attribute
                    cookieValue = string.Empty;
                }
                else
                {
                    cookieValue = parts[1];
                }

                cookieDictionary[cookieName] = cookieValue;
            }

            return cookieDictionary;
        }
    }
}
