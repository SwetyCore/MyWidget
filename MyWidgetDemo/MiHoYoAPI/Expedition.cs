using Newtonsoft.Json;
using System;

namespace DGP.Genshin.MiHoYoAPI.Record.DailyNote
{
    /// <summary>
    /// 探索派遣
    /// </summary>
    public class Expedition
    {
        /// <summary>
        /// 图标
        /// </summary>
        [JsonProperty("avatar_side_icon")] public string? AvatarSideIcon { get; set; }
        /// <summary>
        /// 状态 Ongoing:派遣中 Finished:已完成
        /// </summary>
        [JsonProperty("status")] public string? Status { get; set; }
        /// <summary>
        /// 剩余时间
        /// </summary>
        [JsonProperty("remained_time")] public string? RemainedTime { get; set; }

        public string? RemainedTimeFormatted
        {
            get
            {
                if (Status == "Finished")
                {
                    return "已完成";
                }
                if (RemainedTime is not null)
                {
                    TimeSpan ts = new(0, 0, int.Parse(RemainedTime));
                    return ts.Days > 0 ? $"{ts.Days}天{ts.Hours}时{ts.Minutes}分" : $"{ts.Hours}时{ts.Minutes}分";
                }
                return null;
            }
        }
    }
}
