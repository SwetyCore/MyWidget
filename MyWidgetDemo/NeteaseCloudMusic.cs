using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MyWidget
{
    public class NeteaseCloudMusic
    {
        public IntPtr playerhwnd;

        #region Native导入

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow([MarshalAs(UnmanagedType.LPTStr)] string lpClassName, [MarshalAs(UnmanagedType.LPTStr)] string lpWindowName);

        [DllImport("user32")]
        private static extern IntPtr FindWindowEx(IntPtr hWnd1, IntPtr hWnd2, string lpsz1, string lpsz2);

        private delegate bool WndEnumProc(IntPtr hWnd, int lParam);

        [DllImport("user32")]
        private static extern bool EnumWindows(WndEnumProc lpEnumFunc, int lParam);

        [DllImport("user32")]
        private static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lptrString, int nMaxCount);

        [DllImport("user32")]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32")]
        private static extern bool GetWindowRect(IntPtr hWnd, ref LPRECT rect);

        [StructLayout(LayoutKind.Sequential)]
        private readonly struct LPRECT
        {
            public readonly int Left;
            public readonly int Top;
            public readonly int Right;
            public readonly int Bottom;
        }

        #endregion



        /// <summary>
        /// 指定Url地址使用Get 方式获取全部字符串
        /// </summary>
        /// <param name="url">请求链接地址</param>
        /// <returns></returns>
        public static string Get(string url)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            try
            {
                //获取内容
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }
            finally
            {
                stream.Close();
            }
            return result;
        }
        /// <summary>
        /// 发送Get请求
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="dic">请求参数定义</param>
        /// <returns></returns>
        public static string GetWithData(string url, Dictionary<string, string> dic)
        {
            string result = "";
            StringBuilder builder = new StringBuilder();
            builder.Append(url);
            if (dic.Count > 0)
            {
                builder.Append("?");
                int i = 0;
                foreach (var item in dic)
                {
                    if (i > 0)
                        builder.Append("&");
                    builder.AppendFormat("{0}={1}", item.Key, item.Value);
                    i++;
                }
            }
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(builder.ToString());
            //添加参数
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            try
            {
                //获取内容
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }
            finally
            {
                stream.Close();
            }
            return result;
        }

        public static IntPtr FindPlayer()
        {
            return FindWindow("icon", null);
        }

        public static string FindName()
        {
            StringBuilder builder = new StringBuilder(512);
            GetWindowText(FindPlayer(), builder, builder.Capacity);
            return builder.ToString();
        }

        public static string FindAvator(string name)
        {
    //        data ={
    //            "csrf_token": "hlpretag",
    //"hlposttag": "",
    //"s": name,
    //"type": 1,
    //"offset": 0,
    //"total": True,
    //"limit": 20
    //}
            Dictionary<string, string> form=new Dictionary<string, string>();
            form["csrf_token"] = "hlpretag";
            form[""] = "";
            form["s"] = name;
            form["type"] = "1";
            string respdata= GetWithData("http://music.163.com/api/search/get/web", form);
            JObject jo = JObject.Parse(respdata);
            string id=  (string)jo.SelectToken("result.songs[0].id");

            respdata = Get($"http://music.163.com/api/song/detail/?id={id}&ids=[{id}]");
            //respdata = (string)jo.SelectToken("songs[0][album][picUrl]");
            jo = JObject.Parse(respdata);
            string src = (string)jo.SelectToken("songs[0].album.picUrl");
            return src;
        }
    }
}
