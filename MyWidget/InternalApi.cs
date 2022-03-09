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
using DGP.Genshin.GamebarWidget.Helper;
using Newtonsoft.Json;
using DGP.Genshin.GamebarWidget.Model;

namespace MyWidget
{
    internal class InternalApi
    {
        ResourceHandler resourceHandler;
        SysParams sysParams =new SysParams();
        List<RoleAndNote> roleAndNotes = new List<RoleAndNote>();
        int resinCount=0;

        string cookiestring = "";
        public InternalApi(ResourceHandler resourceHandler)
        {
            this.resourceHandler = resourceHandler;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // 创建一个100ms定时的定时器
            Timer RUtimer = new Timer(1000);    // 参数单位为ms
                                                       // 定时时间到，处理函数为OnTimedUEvent(...)
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

        private static void runcmd(string cmd)
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

        public CoreWebView2WebResourceResponse ApiReqHandler(CoreWebView2WebResourceRequest req)
        {
            string path = req.Uri.Substring(Form1.internalUrl.Length, req.Uri.Length - Form1.internalUrl.Length);

            JObject resp = new JObject();
            switch (req.Method)
            {


                case "POST":
                    {

                        Stream s = req.Content;
                        StreamReader reader = new StreamReader(s);
                        string text = reader.ReadToEnd();
                        JObject o = JObject.Parse(text);

                        switch (path)
                        {
                            case "/api/runcmd":
                                {
                                    string cmd = (string)o.SelectToken("cmd");
                                    runcmd(cmd);
                                    return resourceHandler.FromString("ok", Encoding.Default, "");
                                }
                                break;

                            case "/api/debug":
                                {
                                    bool debug = (bool)o.SelectToken("debug");
                                    string url = (string)o.SelectToken("url");
                                    Form1.debug = debug;
                                    if (debug)
                                    {
                                        Form1.internalUrl = url;

                                    }
                                    else
                                    {
                                        Form1.internalUrl = Form1.myhost;
                                    }
                                    return resourceHandler.FromString("ok", Encoding.Default, "");
                                }
                                break;
                            case "/api/getAvator":
                                {
                                    string name = (string)o.SelectToken("name");

                                    return resourceHandler.FromString(NeteaseCloudMusic.FindAvator(name), Encoding.Default, "");

                                }
                                break;
                            case "/api/setResinCookie":
                                {

                                    string cookie = (string)o.SelectToken("cookie");

                                    //cookiestring = cookie;
                                    cookiestring = cookie;
                                    RefreshDailyNotePoolAsync();
                                    //RefreshDailyNotePoolAsync();
                                    //JObject response = new JObject();
                                    //response["ResinFormatted"] = "";
                                    return resourceHandler.FromString("ok", Encoding.Default, "");
                                }
                                break;
                        }
                    }break;

                case "GET":
                    {
                        switch (path)
                        {

                            case "/api/performance":
                                {
                                    JObject o = new JObject();
                                    JObject mem = new JObject();
                                    mem["avaiable"] = sysParams.MEMAvailable;
                                    mem["commitedPrec"] = sysParams.MEMCommitedPerc;
                                    o["cpu"] = sysParams.processorUtility;
                                    o["mem"] = mem;
                                    return resourceHandler.FromString(o.ToString(), Encoding.Default, "application/json");
                                }
                                break;
                            case
                                "/api/getSongName":
                                {
                                    return resourceHandler.FromString(NeteaseCloudMusic.FindName(), Encoding.Default, "");
                                }
                                break;
                               case "/api/getResin":{
                                    if (cookiestring=="")
                                    {
                                        return resourceHandler.ForErrorMessage("error", HttpStatusCode.BadRequest);
                                    }
                                    //RefreshDailyNotePoolAsync();
                                    return resourceHandler.FromString(JsonConvert.SerializeObject(roleAndNotes), Encoding.Default, "application/json");
                                }
                                break;
                            case "/api/debug":
                                {
                                    resp["debug"] = Form1.debug;
                                    return resourceHandler.FromString(resp.ToString(), Encoding.Default, "");
                                } break;
                        }



                    }
                    break;
            }
            return resourceHandler.ForErrorMessage("error", HttpStatusCode.Forbidden);
        }
    }
}
