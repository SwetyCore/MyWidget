using Newtonsoft.Json;
using System.Collections.Generic;

namespace DGP.Genshin.GamebarWidget.MiHoYoAPI
{
    /// <summary>
    /// 列表对象包装器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListWrapper<T>
    {
        [JsonProperty("list")] public List<T> List { get; set; }
    }
}
