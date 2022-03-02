using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyWidget
{
    internal class ResourceHandler
    {

        private CoreWebView2Environment env;

        public ResourceHandler(CoreWebView2Environment env)
        {
            this.env = env;
        }

        internal CoreWebView2WebResourceResponse ForErrorMessage(string errMsg, HttpStatusCode errCode)
        {
            return env.CreateWebResourceResponse(null, (int)errCode, errMsg, "");
        }

        internal CoreWebView2WebResourceResponse FromFilePath(string path, string mimeType, bool autoClose)
        {
            FileStream fileStream = File.OpenRead(path);
            return env.CreateWebResourceResponse(fileStream, 200, "OK", "Content-Type: " + mimeType);
        }

        internal CoreWebView2WebResourceResponse FromStream(Stream s, string mimetype, bool autoDisposeStream)
        {
            return env.CreateWebResourceResponse(s, 200, "OK", "Content-Type: " + mimetype);
        }

        internal CoreWebView2WebResourceResponse FromString(string msg, Encoding encoding, string contentType)
        {
            MemoryStream ms = new MemoryStream(encoding.GetBytes(msg));
            return env.CreateWebResourceResponse(ms, 200, "OK", "Content-Type: " + contentType + ", charset=" + encoding.EncodingName);
        }
    }
}
