using Newtonsoft.Json;

namespace DGP.Genshin.GamebarWidget.Helper
{
    public static class Json
    {
        /// <summary>	
        /// 将JSON反序列化为指定的.NET类型
        /// </summary>	
        /// <typeparam name="T">要反序列化的对象的类型</typeparam>	
        /// <param name="value">要反序列化的JSON</param>	
        /// <returns>JSON字符串中的反序列化对象, 如果反序列化失败则返回 <see cref="null"/></returns>	
        public static T ToObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        /// <summary>	
        /// 将指定的对象序列化为JSON字符串	
        /// </summary>	
        /// <param name="value">要序列化的对象</param>	
        /// <returns>对象的JSON字符串表示形式</returns>	
        public static string Stringify(object value, bool indented = true)
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
            {
                //NullValueHandling = NullValueHandling.Ignore,
                //兼容原神api格式
                DateFormatString = "yyyy'-'MM'-'dd' 'HH':'mm':'ss.FFFFFFFK",
                Formatting = indented ? Formatting.Indented : Formatting.None,
            };
            return JsonConvert.SerializeObject(value, jsonSerializerSettings);
        }
    }
}
