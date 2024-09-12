using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using FakeeDeck.ButtonType;
using System.Web;
using static System.Net.WebRequestMethods;
using File = System.IO.File;
using System.Reflection.Metadata;
using System.Xml.Linq;
using System.Reflection;
using System.Text.Json;
using System.Collections;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace FakeeDeck.Class
{
    internal class HttpServer
    {
        public string port = "8000";
        public HttpServer(string port)
        {
            port = port;
        }

        private static IDictionary<string, string> mimeTypes = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase) {
                {".asf", "video/x-ms-asf"},
                {".asx", "video/x-ms-asf"},
                {".avi", "video/x-msvideo"},
                {".bin", "application/octet-stream"},
                {".cco", "application/x-cocoa"},
                {".crt", "application/x-x509-ca-cert"},
                {".css", "text/css"},
                {".deb", "application/octet-stream"},
                {".der", "application/x-x509-ca-cert"},
                {".dll", "application/octet-stream"},
                {".dmg", "application/octet-stream"},
                {".ear", "application/java-archive"},
                {".eot", "application/octet-stream"},
                {".exe", "application/octet-stream"},
                {".flv", "video/x-flv"},
                {".gif", "image/gif"},
                {".hqx", "application/mac-binhex40"},
                {".htc", "text/x-component"},
                {".htm", "text/html"},
                {".html", "text/html"},
                {".ico", "image/x-icon"},
                {".img", "application/octet-stream"},
                {".iso", "application/octet-stream"},
                {".jar", "application/java-archive"},
                {".jardiff", "application/x-java-archive-diff"},
                {".jng", "image/x-jng"},
                {".jnlp", "application/x-java-jnlp-file"},
                {".jpeg", "image/jpeg"},
                {".jpg", "image/jpeg"},
                {".js", "application/x-javascript"},
                {".mml", "text/mathml"},
                {".mng", "video/x-mng"},
                {".mov", "video/quicktime"},
                {".mp3", "audio/mpeg"},
                {".mpeg", "video/mpeg"},
                {".mpg", "video/mpeg"},
                {".msi", "application/octet-stream"},
                {".msm", "application/octet-stream"},
                {".msp", "application/octet-stream"},
                {".pdb", "application/x-pilot"},
                {".pdf", "application/pdf"},
                {".pem", "application/x-x509-ca-cert"},
                {".pl", "application/x-perl"},
                {".pm", "application/x-perl"},
                {".png", "image/png"},
                {".prc", "application/x-pilot"},
                {".ra", "audio/x-realaudio"},
                {".rar", "application/x-rar-compressed"},
                {".rpm", "application/x-redhat-package-manager"},
                {".rss", "text/xml"},
                {".run", "application/x-makeself"},
                {".sea", "application/x-sea"},
                {".shtml", "text/html"},
                {".sit", "application/x-stuffit"},
                {".swf", "application/x-shockwave-flash"},
                {".tcl", "application/x-tcl"},
                {".tk", "application/x-tcl"},
                {".txt", "text/plain"},
                {".war", "application/java-archive"},
                {".wbmp", "image/vnd.wap.wbmp"},
                {".wmv", "video/x-ms-wmv"},
                {".xml", "text/xml"},
                {".xpi", "application/x-xpinstall"},
                {".zip", "application/zip"},
            };
        private Dictionary<string, Dictionary<string, Delegate>> routes = new Dictionary<string, Dictionary<string, Delegate>>();

        public static HttpListener listener;
        public static int pageViews = 0;
        public static int requestCount = 0;

        public void addRoute(Delegate callback, string method = "GET", string route = "/")
        {
            if (!routes.ContainsKey(method))
            {
                routes.Add(method, new Dictionary<string, Delegate>());
            }

            routes[method].Add(route, callback);
        }
        public async Task HandleIncomingConnections()
        {
            bool runServer = true;

            // While a user hasn't visited the `shutdown` url, keep on handling requests
            while (runServer)
            {
                // Will wait here until we hear from a connection
                HttpListenerContext ctx = await listener.GetContextAsync();

                // Peel out the requests and response objects
                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse resp = ctx.Response;

                // Print out some info about the request
                /*Debug.WriteLine("Request #: {0}", ++requestCount);
                 Debug.WriteLine(req.Url.ToString());
                 Debug.WriteLine(req.HttpMethod);
                 Debug.WriteLine(req.UserHostName);
                 Debug.WriteLine(req.UserAgent);*/

                if (req.HttpMethod == "GET" && req.Url.AbsolutePath.Contains("."))
                {
                    await servFileResponseAsync(req, resp);
                }
                else
                {
                    bool isMatch = false;
                    foreach (var route in routes[req.HttpMethod])
                    {
                        isMatch = Regex.IsMatch(req.Url.AbsolutePath, route.Key, RegexOptions.IgnoreCase);
                        if (isMatch)
                        {
                            Debug.WriteLine(route.Key);
                            Delegate gelegate = route.Value;
                            if (req.HttpMethod == "POST")
                            {
                                Dictionary<string, string> postParams = parsePostRequestParameters(req);
                                gelegate.DynamicInvoke([req, resp, postParams]);
                            }
                            else
                            {
                                gelegate.DynamicInvoke([req, resp]);
                            }
                        }
                    }

                    if (!isMatch)
                    {
                        resp.StatusCode = (int)HttpStatusCode.NotFound;
                        await resp.OutputStream.FlushAsync();
                    }
                }
                resp.Close();
            }
        }

        public void serv()
        {
            string url = "http://*:" + port + "/";
            // Create a Http server and start listening for incoming connections
            listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Start();
            Console.WriteLine("Listening for connections on {0}", url);

            // Handle requests
            Task listenTask = HandleIncomingConnections();
            listenTask.GetAwaiter().GetResult();

            // Close the listener
            listener.Close();
        }

        private static async Task servFileResponseAsync(HttpListenerRequest req, HttpListenerResponse resp)
        {
            string filename = Path.Combine("./", req.Url.AbsolutePath.Substring(1));
            if (!File.Exists(filename))
            {
                resp.StatusCode = (int)HttpStatusCode.NotFound;
                return;
            }

            try
            {
                Stream input = new FileStream(filename, FileMode.Open);

                string mime;
                resp.ContentType = mimeTypes.TryGetValue(Path.GetExtension(filename), out mime)
                    ? mime
                    : "application/octet-stream";
                resp.ContentLength64 = input.Length;
                resp.AddHeader("Date", DateTime.Now.ToString("r"));
                resp.AddHeader("Last-Modified", File.GetLastWriteTime(filename).ToString("r"));

                byte[] buffer = new byte[1024 * 32];
                int nbytes;
                while ((nbytes = input.Read(buffer, 0, buffer.Length)) > 0)
                    resp.OutputStream.Write(buffer, 0, nbytes);
                input.Close();
                resp.OutputStream.Flush();
                resp.StatusCode = (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                byte[] errorData = Encoding.UTF8.GetBytes(ex.Message);
                resp.ContentType = "text/html";
                resp.ContentEncoding = Encoding.UTF8;
                resp.ContentLength64 = errorData.LongLength;
                resp.StatusCode = (int)HttpStatusCode.InternalServerError;
                await resp.OutputStream.WriteAsync(errorData, 0, errorData.Length);
            }
        }

        private static Dictionary<string, string> parsePostRequestParameters(HttpListenerRequest req)
        {
            Dictionary<string, string> postParams = new Dictionary<string, string>();

            using (var reader = new StreamReader(req.InputStream, req.ContentEncoding))
            {
                string postData = reader.ReadToEnd();
                var parsedData = HttpUtility.ParseQueryString(postData);
                foreach (string key in parsedData)
                {
                    postParams[key] = parsedData[key];
                }
            }

            return postParams;
        }
    }
}
