using DGP.Genshin.GamebarWidget.Model;
using System.Collections.Generic;
using Windows.Storage;

namespace DGP.Genshin.GamebarWidget.Helper
{
    internal static class Setting
    {
        private static readonly ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        public static List<Cookie> Cookies
        {
            get
            {
                return localSettings.Values.TryGetValue("cookies", out object cookieString)
                    ? Json.ToObject<List<Cookie>>(cookieString as string)
                    : new List<Cookie>();
            }
            set
            {
                localSettings.Values["cookies"] = Json.Stringify(value);
            }
        }
    }
}
