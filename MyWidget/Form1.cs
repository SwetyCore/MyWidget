using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyWidget
{

    public partial class Form1 : Form
    {


        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(Keys vKey);


        CoreWebView2 webView;
        WebView2 webViewControl;

        ResourceHandler resourceHandler;

        InternalApi internalApi;
        public static bool debug = false;
        public static readonly string myhost="http://www.sc.mywidget.com";
        public static string internalUrl = "http://www.sc.mywidget.com";



        public static int count = 0;
        const int target = 70;//长按0.7s
#if DEBUG
        string WebRoot = @"E:\HTML\vuewidgets\dist\";
#else
        string WebRoot = "";
#endif
        public Form1()
        {
            InitializeComponent();
            int workWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int workHeight = Screen.PrimaryScreen.WorkingArea.Height;
            Location = new Point(workWidth-600, 0);
            Size = new Size(600, workHeight);
            WebRoot = Path.Combine(Application.StartupPath, "WebRoot"); 
            if (!File.Exists(Path.Combine(WebRoot,"index.html")))
            {
                MessageBox.Show("资源文件丢失，请重新下载/安装此程序！");
                Environment.Exit(1);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.notifyIcon1.Visible = true;
            webViewControl = new WebView2();
            Task<CoreWebView2Environment> createEnvTask = CoreWebView2Environment.CreateAsync(userDataFolder: Path.GetFullPath("app\\data\\cefdata"));
            createEnvTask.Wait();
            CoreWebView2Environment env = createEnvTask.Result;
            Controls.Add(webViewControl);
            webViewControl.Dock = DockStyle.Fill;
            webViewControl.EnsureCoreWebView2Async(env);
            webViewControl.CoreWebView2InitializationCompleted += WebView2_CoreWebView2InitializationCompleted;

        }
        void initWebView2()
        {
            webView.Settings.IsPinchZoomEnabled = false;
            webView.Settings.IsSwipeNavigationEnabled = false;
            webView.AddWebResourceRequestedFilter("*", CoreWebView2WebResourceContext.All);
            resourceHandler = new ResourceHandler(webView.Environment);
            webView.WebResourceRequested += WebView_WebResourceRequested;
            webView.NavigationStarting += NavigationStarting;



            internalApi = new InternalApi(resourceHandler);

        }
        private void NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            if (!e.Uri.StartsWith(internalUrl))
            {
                e.Cancel = true;
                webView.Navigate(internalUrl);
            }
        }
        private void WebView_WebResourceRequested(object sender, CoreWebView2WebResourceRequestedEventArgs e)
        {
            e.Response = handleRequest(e.Request);
            e.GetDeferral().Complete();
        }
        public CoreWebView2WebResourceResponse handleRequest(CoreWebView2WebResourceRequest request)
        {

            if (request.Uri.StartsWith(internalUrl)){
                string path = request.Uri.Substring(internalUrl.Length, request.Uri.Length - internalUrl.Length);
                if (path.StartsWith("/"))
                {
                    path = path.Substring(1);
                }
                if (path == "" || path.EndsWith("/"))
                {
                    path += "index.html";
                }
                if (path.StartsWith("api/"))
                {
                    return internalApi.ApiReqHandler(request);
                }
                string mimetype = "";
                if (debug)
                {
                    return resourceHandler.FromFilePath("", "", true);
                }
                else
                {
                    return resourceHandler.FromFilePath(Path.Combine(WebRoot, path), mimetype, true);
                }

            }


            return resourceHandler.FromFilePath("", "", true);
        }
        private void WebView2_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                webView = webViewControl.CoreWebView2;
                initWebView2();
                webViewControl.Source = new Uri(internalUrl);
                webViewControl.DefaultBackgroundColor = Color.Transparent;
                //webViewControl.NavigationCompleted += WebViewControl_NavigationCompleted;
            }
            else
            {
                if (e.InitializationException is WebView2RuntimeNotFoundException)
                {
                    MessageBox.Show("此计算机上没有安装WebView2运行时。\r\n访问：https://developer.microsoft.com/zh-cn/microsoft-edge/webview2/consumer/ 获取运行时", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    string runtimeDownloadUrl = "https://developer.microsoft.com/zh-cn/microsoft-edge/webview2/consumer/";
                    Process p = new Process();
                    p.StartInfo.FileName = "cmd.exe";
                    p.StartInfo.Arguments = $"/c start {runtimeDownloadUrl}";
                    p.Start();
                    Application.Exit();
                }
                else
                {
                    MessageBox.Show(e.InitializationException.ToString(), "Web页面初始化失败");
                    Application.Exit();
                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            System.Environment.Exit(0);
        }

        private void 项目地址ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process p=new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = "/c start https://github.com/SwetyCore/MyWidget";
            p.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (GetAsyncKeyState(Keys.RControlKey) != 0)
            {
                count++;
            }
            else
            {
                count = 0;
            }
            if (count > target)
            {
                this.WindowState = FormWindowState.Normal;
                this.TopMost = true;
                this.TopMost=false;
                Thread.Sleep(800);
                count = 0;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.notifyIcon1.Visible = false;
        }
    }
}
