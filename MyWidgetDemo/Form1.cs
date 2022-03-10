using Microsoft.Web.WebView2.Core;
using MyWidget;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MyWidget.WindowStyle;

namespace MyWidget
{
    public partial class Form1 : Form
    {


        ScriptCallbackObject internalApi;




        public static bool autoHide = false;
        public static bool debug = false;
        public static string WebRoot = @"E:\HTML\vuewidgets\dist";
        public static string internalHost = "widgets.sc";
        public static string imageHost = "pictures.sc";

        public static string internalUrl = "https://widgets.sc/";
        public Form1()
        {
            InitializeComponent();

            IntPtr hWnd = this.Handle;


            var attribute = DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE;
            var preference = DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUND;
            DwmSetWindowAttribute(hWnd, attribute, ref preference, sizeof(uint));

            int workWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int workHeight = Screen.PrimaryScreen.WorkingArea.Height;
            Location = new Point(workWidth - 605, 5);
            Size = new Size(600, workHeight - 10);

            InitializeAsync();

            WebRoot = Path.Combine(Application.StartupPath, "WebRoot");

            this.自动隐藏ToolStripMenuItem.Text = $"自动隐藏:{autoHide}";
        }
        async void InitializeAsync()
        {
            await webView.EnsureCoreWebView2Async(null);

            webView.DefaultBackgroundColor = Color.Transparent;

            webView.CoreWebView2.SetVirtualHostNameToFolderMapping(
            internalHost, WebRoot, CoreWebView2HostResourceAccessKind.DenyCors);

            webView.CoreWebView2.SetVirtualHostNameToFolderMapping(
            imageHost, "图片", CoreWebView2HostResourceAccessKind.DenyCors);


            Graphics currentGraphics = Graphics.FromHwnd(this.Handle);
            double dpixRatio = currentGraphics.DpiX / 96;
            webView.ZoomFactor = 1.0 / dpixRatio;

            webView.CoreWebView2.AddHostObjectToScript("webBrowserObj", new ScriptCallbackObject());
            //注册全局变量winning
            webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("var apiHost= window.chrome.webview.hostObjects.webBrowserObj;");


#if DEBUG
            webView.Source = new Uri("http://localhost:8080/");

#else
            
            webView.Source = new Uri("https://widgets.sc/index.html");
#endif
            ScriptCallbackObject internalApi = new ScriptCallbackObject();

        }



        private void Form1_Load(object sender, EventArgs e)
        {
            this.TransparencyKey = Color.Snow;

            EnableBlur(this.Handle);
        }

        private void 自动隐藏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            autoHide = !autoHide;
            this.自动隐藏ToolStripMenuItem.Text = $"自动隐藏:{autoHide}";

        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            notifyIcon1.Visible = false;
            System.Environment.Exit(0);
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            if (autoHide)
            {
                this.Visible = false;
            }
        }

        private void 项目地址ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = "/c start https://github.com/SwetyCore/MyWidget";
            p.Start();

        }
        public void activeWindow()
        {

            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
            this.TopMost = true;
            this.TopMost = false;
            this.Activate();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            activeWindow();
        }


        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(Keys vKey);


        public static int count = 0;
        const int target = 50;//长按0.5s

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
                this.Visible = true;
                this.WindowState = FormWindowState.Normal;
                this.TopMost = true;
                this.TopMost = false;
                Thread.Sleep(800);
                count = 0;
            }
        }
    }
}
