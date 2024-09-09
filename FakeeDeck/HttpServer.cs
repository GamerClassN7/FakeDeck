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

namespace FakeeDeck
{
    internal class HttpServer
    {
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

        public static HttpListener listener;
        public static string url = "http://*:8000/";
        public static int pageViews = 0;
        public static int requestCount = 0;
        public static string pageHeader =
            "<!DOCTYPE>" +
            "<html>" +
            "  <head>" +
            "    <title>HttpListener Example</title>" +
            "    <link href=\"https://yarnpkg.com/en/package/normalize.css\" rel=\"stylesheet\">" +
            "    <link href=\"https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css\" rel=\"stylesheet\">" +
            "    <link href=\"https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.2/css/all.min.css\" rel=\"stylesheet\">" +
            "  </head>" +
            "  <body>" +
            "    <div class=\"d-flex flex-wrap\">" +
            "      <div class=\"m-2\">" +
            "        <p style=\"margin-bottom: 0px; width: 150px;height: 150px;background-color: aquamarine;\" >Page Views: {0}</p>" +
            "      </div>";
        public static string pageFooter =
            "      <div class=\"m-2\">" +
            "        <button style=\"width: 150px;height: 150px;background-color: aquamarine;\" onclick=\"!document.fullscreenElement ? document.documentElement.requestFullscreen() :  document.exitFullscreen();\">" +
            "          <i class=\"fa-solid fa-maximize\"></i>" +
            "        </button>" +
            "      </div>" +
            "    </div>" +
            "    <script src=\"StaticFiles/app.js\"></script>" +
            "  </body>" +
            "</html>";
        public static string pageData = "";

        public static async Task HandleIncomingConnections()
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
                Console.WriteLine("Request #: {0}", ++requestCount);
                Console.WriteLine(req.Url.ToString());
                Console.WriteLine(req.HttpMethod);
                Console.WriteLine(req.UserHostName);
                Console.WriteLine(req.UserAgent);
                Console.WriteLine();

                //Parse Port Parameters
                Dictionary<string, string> postParams = new Dictionary<string, string>();
                if (req.HttpMethod == "POST")
                {
                    using (var reader = new StreamReader(req.InputStream, req.ContentEncoding))
                    {
                        string postData = reader.ReadToEnd();
                        var parsedData = HttpUtility.ParseQueryString(postData);
                        foreach (string key in parsedData)
                        {
                            postParams[key] = parsedData[key];
                        }
                    }
                }

                // If `shutdown` url requested w/ POST, then shutdown the server after serving the page
                if ((req.HttpMethod == "POST") && (req.Url.AbsolutePath == "/shutdown"))
                {
                    Console.WriteLine("Shutdown requested");
                    runServer = false;
                }

                // If `shutdown` url requested w/ POST, then shutdown the server after serving the page
                if ((req.HttpMethod == "POST") && (req.Url.AbsolutePath.StartsWith("/button")))
                {
                    try
                    {
                        string module = req.Url.AbsolutePath.Replace("/button", "");
                        Console.WriteLine("Call module " + module);

                        callButtonAction(module, postParams);

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
                    finally
                    {
                        resp.Close();
                    }
                    continue;
                }

                if (req.Url.AbsolutePath.Contains("."))
                {
                    string filename = Path.Combine("./", req.Url.AbsolutePath.Substring(1));
                    if (File.Exists(filename))
                    {
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
                        finally
                        {
                            resp.Close();
                        }
                        continue;
                    }
                }


                // Make sure we don't increment the page views counter if `favicon.ico` is requested
                if (req.Url.AbsolutePath != "/favicon.ico")
                {
                    pageViews += 1;
                }



                // Write the response info
                string disableSubmit = !runServer ? "disabled" : "";
                byte[] data = Encoding.UTF8.GetBytes(String.Format((pageHeader + pageData + pageFooter), pageViews, disableSubmit));
                resp.ContentType = "text/html";
                resp.ContentEncoding = Encoding.UTF8;
                resp.ContentLength64 = data.LongLength;

                // Write out to the response stream (asynchronously), then close it
                await resp.OutputStream.WriteAsync(data, 0, data.Length);
                resp.Close();
            }
        }

        public static void serv()
        {
            foreach (var stratogem in HelldiversTwoMacro.stratogems)
            {
                pageData += HelldiversTwoMacro.getButton(stratogem.Key);
            }

            foreach (var control in MediaMacro.mediaControls)
            {
                pageData += MediaMacro.getButton(control.Key);
            }

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

        private static void callButtonAction(string module, Dictionary<string, string>  postParams)
        {
            string cleanClass = module.Trim('/');

            Type buttonClass = Type.GetType("FakeeDeck.ButtonType." + cleanClass, true);
            MethodInfo method = buttonClass.GetMethod("invokeAction");
            
            ParameterInfo[] pars = method.GetParameters();
            List<object> parameters = new List<object>();
            Console.WriteLine(module);
            foreach (ParameterInfo p in pars)
            {
                if (p == null)
                {
                    continue;
                }

                Console.WriteLine(p.Name);
                Console.WriteLine(postParams[p.Name]);

                if (p.Name != null && postParams.ContainsKey(p.Name))
                {

                    parameters.Insert(p.Position, postParams[p.Name]);
                    Console.WriteLine(postParams[p.Name]);
                }
                else if (p.IsOptional && p.DefaultValue != null)
                {
                    parameters.Insert(p.Position, p.DefaultValue);
                }
            }
            Console.WriteLine(JsonSerializer.Serialize(parameters));

            Console.WriteLine(method);
            Console.WriteLine(method.Invoke(null, parameters.ToArray()).ToString());
        }
    }
}
