using Microsoft.Web.WebView2.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using DGP.Genshin.GamebarWidget.MiHoYoAPI;
using DGP.Genshin.GamebarWidget.Model;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MyWidget
{
    [ComVisible(true)]
    public class ScriptCallbackObject
    {
        public string apiver { get; set; } = "1.0.0.1";

        SysParams sysParams = new SysParams();
        List<RoleAndNote> roleAndNotes = new List<RoleAndNote>();
        int resinCount = 0;

        string cookiestring = "";
        public ScriptCallbackObject()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // 创建一个1s定时的定时器
            System.Timers.Timer RUtimer = new System.Timers.Timer(1000);    // 参数单位为ms
            RUtimer.Elapsed += OnTimedUEvent;

            // 为true时，定时时间到会重新计时；为false则只定时一次
            RUtimer.AutoReset = true;
            // 使能定时器
            RUtimer.Enabled = true;
            // 开始计时
            RUtimer.Start();


        }
        void OnTimedUEvent(object sender, ElapsedEventArgs e)
        {
            sysParams.getprocessorUtility();
            sysParams.getMemAvailable();
            sysParams.getMemCommitedPerc();
            resinCount++;
            //8分钟
            if (resinCount >= 60 * 8)
            {
                RefreshDailyNotePoolAsync();
                resinCount = 0;
            }

        }

        public void runcmd(string cmd)
        {

            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";         //确定程序名
            p.StartInfo.Arguments = "/c " + cmd;   //确定程式命令行
            p.StartInfo.UseShellExecute = false;      //Shell的使用
            p.StartInfo.CreateNoWindow = true;        //设置置不显示示窗口
            p.Start();

        }
        private async Task RefreshDailyNotePoolAsync()
        {
            DGP.Genshin.GamebarWidget.Model.Cookie cookie = new DGP.Genshin.GamebarWidget.Model.Cookie(cookiestring);
            List<UserGameRole> roles = await new UserGameRoleProvider(cookie.CookieValue).GetUserGameRolesAsync();
            roleAndNotes.Clear();
            if (roles.Count == 0)
            {

                return;
            }
            foreach (UserGameRole role in roles)
            {
                DailyNote note = await new DailyNoteProvider(cookie.CookieValue).GetDailyNoteAsync(role.Region, role.GameUid);
                roleAndNotes.Add(new RoleAndNote { Role = role, Note = note });
            }


        }

        public string getPerformance()
        {
            JObject mem = new JObject();
            JObject o = new JObject();
            mem["avaiable"] = sysParams.MEMAvailable;
            mem["commitedPrec"] = sysParams.MEMCommitedPerc;
            o["cpu"] = sysParams.processorUtility;
            o["mem"] = mem;
            return o.ToString();

        }
        public string getAvator(string name)
        {
            return NeteaseCloudMusic.FindAvator(name);
        }
        public string getSongName()
        {

            return NeteaseCloudMusic.FindName();
        }
        public void setGeshinCookie(string cookie)
        {
            if (cookie == "")
            {
                return ;
            }
            RefreshDailyNotePoolAsync();
            this.cookiestring = cookie;
        }
        public string getResinData()
        {
            return JsonConvert.SerializeObject(roleAndNotes);
        }


    }
}
