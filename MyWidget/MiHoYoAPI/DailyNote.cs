using DGP.Genshin.MiHoYoAPI.Record.DailyNote;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DGP.Genshin.GamebarWidget.MiHoYoAPI
{

    //public class DailyNote
    //{
    //    /// <summary>
    //    /// 当前树脂
    //    /// </summary>
    //    [JsonProperty("current_resin")] public int CurrentResin { get; set; }
    //    /// <summary>
    //    /// 最大树脂
    //    /// </summary>
    //    [JsonProperty("max_resin")] public int MaxResin { get; set; }
    //    /// <summary>
    //    /// 树脂恢复时间<see cref="string"/>类型的秒数
    //    /// </summary>
    //    [JsonProperty("resin_recovery_time")] public string ResinRecoveryTime { get; set; }

    //    public string ResinFormatted
    //    {
    //        get
    //        {
    //            return $"{CurrentResin} / {MaxResin}";
    //        }
    //    }

    //    public string ResinRecoveryTargetTimeFormatted
    //    {
    //        get
    //        {
    //            if (ResinRecoveryTime != null)
    //            {
    //                DateTime tt = DateTime.Now.AddSeconds(int.Parse(ResinRecoveryTime));
    //                int totalDays = (tt - DateTime.Today).Days;
    //                string day;
    //                switch (totalDays)
    //                {
    //                    case 0: 
    //                        day = "今天"; 
    //                        break;
    //                    case 1: 
    //                        day = "明天"; 
    //                        break;
    //                    case 2: 
    //                        day = "后天"; 
    //                        break;
    //                    default: 
    //                        day = $"{totalDays}天"; 
    //                        break;
    //                };
    //                return $"将于 {day} {tt:HH:mm} 完全恢复";
    //            }
    //            return null;
    //        }
    //    }
    //}



    public class DailyNote
    {
        /// <summary>
        /// 当前树脂
        /// </summary>
        [JsonProperty("current_resin")] public int CurrentResin { get; set; }
        /// <summary>
        /// 最大树脂
        /// </summary>
        [JsonProperty("max_resin")] public int MaxResin { get; set; }
        /// <summary>
        /// 树脂恢复时间 <see cref="string"/>类型的秒数
        /// </summary>
        [JsonProperty("resin_recovery_time")] public string? ResinRecoveryTime { get; set; }

        public string? ResinRecoveryTargetTimeFormatted
        {
            get
            {
                if (ResinRecoveryTime is not null)
                {
                    DateTime tt = DateTime.Now.AddSeconds(int.Parse(ResinRecoveryTime));
                    int totalDays = (tt - DateTime.Today).Days;
                    string day = totalDays switch
                    {
                        0 => "今天",
                        1 => "明天",
                        2 => "后天",
                        _ => $"{totalDays}天"
                    };
                    return $"{day} {tt:HH:mm}";
                }
                return null;
            }
        }

        /// <summary>
        /// 委托完成数
        /// </summary>
        [JsonProperty("finished_task_num")] public int FinishedTaskNum { get; set; }
        /// <summary>
        /// 委托总数
        /// </summary>
        [JsonProperty("total_task_num")] public int TotalTaskNum { get; set; }
        /// <summary>
        /// 4次委托额外奖励是否领取
        /// </summary>
        [JsonProperty("is_extra_task_reward_received")] public bool IsExtraTaskRewardReceived { get; set; }

        public string ExtraTaskRewardDescription =>
            IsExtraTaskRewardReceived
            ? "已领取「每日委托」奖励"
            : FinishedTaskNum == TotalTaskNum
            ? "「每日委托」奖励待领取"
            : "今日完成委托次数不足";

        /// <summary>
        /// 剩余周本折扣次数
        /// </summary>
        [JsonProperty("remain_resin_discount_num")] public int RemainResinDiscountNum { get; set; }

        public int ResinDiscountUsedNum => ResinDiscountNumLimit - RemainResinDiscountNum;
        /// <summary>
        /// 周本折扣总次数
        /// </summary>
        [JsonProperty("resin_discount_num_limit")] public int ResinDiscountNumLimit { get; set; }
        /// <summary>
        /// 当前派遣数
        /// </summary>
        [JsonProperty("current_expedition_num")] public int CurrentExpeditionNum { get; set; }
        /// <summary>
        /// 最大派遣数
        /// </summary>
        [JsonProperty("max_expedition_num")] public int MaxExpeditionNum { get; set; }
        /// <summary>
        /// 派遣
        /// </summary>
        [JsonProperty("expeditions")] public List<Expedition>? Expeditions { get; set; }

        /// <summary>
        /// 当前洞天宝钱
        /// </summary>
        [JsonProperty("current_home_coin")] public int CurrentHomeCoin { get; set; }
        /// <summary>
        /// 最大洞天宝钱
        /// </summary>
        [JsonProperty("max_home_coin")] public int MaxHomeCoin { get; set; }
        /// <summary>
        /// 洞天宝钱恢复时间 <see cref="string"/>类型的秒数
        /// </summary>
        [JsonProperty("home_coin_recovery_time")] public string? HomeCoinRecoveryTime { get; set; }

        public string? HomeCoinRecoveryTargetTimeFormatted
        {
            get
            {
                if (HomeCoinRecoveryTime is not null)
                {
                    DateTime tt = DateTime.Now.AddSeconds(int.Parse(HomeCoinRecoveryTime));
                    int totalDays = (tt - DateTime.Today).Days;
                    string day = totalDays switch
                    {
                        0 => "今天",
                        1 => "明天",
                        2 => "后天",
                        _ => $"{totalDays}天"
                    };
                    return $"{day} {tt:HH:mm}";
                }
                return null;
            }
        }
    }

}
