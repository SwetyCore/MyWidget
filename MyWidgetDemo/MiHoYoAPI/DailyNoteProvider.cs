using System.Threading.Tasks;

namespace DGP.Genshin.GamebarWidget.MiHoYoAPI
{
    public class DailyNoteProvider
    {
        private const string ApiTakumiRecord = @"https://api-takumi-record.mihoyo.com/game_record/app/genshin/api";
        private const string Referer = @"https://webstatic.mihoyo.com/app/community-game-records/index.html?v=6";

        private readonly Requester requester;

        public DailyNoteProvider(string cookie)
        {
            requester = new Requester(new RequestOptions
            {
                {"Accept", RequestOptions.Json },
                {"x-rpc-app_version", DynamicSecretProvider2.AppVersion },
                {"User-Agent", RequestOptions.CommonUA2161 },
                {"x-rpc-client_type", "5" },
                {"Referer",Referer },
                {"Cookie", cookie },
                {"X-Requested-With", RequestOptions.Hyperion }
            });
        }

        /// <summary>
        /// 获取实时便笺信息
        /// </summary>
        /// <param name="server"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public async Task<DailyNote> GetDailyNoteAsync(string server, string uid)
        {
            return await requester.GetWhileUpdateDynamicSecret2Async<DailyNote>(
                $"{ApiTakumiRecord}/dailyNote?server={server}&role_id={uid}");
        }
    }
}
