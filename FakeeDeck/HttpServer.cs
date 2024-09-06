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

namespace FakeeDeck
{
    internal class HttpServer
    {
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
            "  <body>"+
            "    <div class=\"d-flex flex-wrap\">" +
            "      <div class=\"m-2\">" +
            "        <p style=\"margin-bottom: 0px; width: 200px;height: 200px;background-color: aquamarine;\" >Page Views: {0}</p>" +
            "      </div>";
        public static string pageFooter =
            "      <div class=\"m-2\">" +
            "        <button style=\"width: 200px;height: 200px;background-color: aquamarine;\" onclick=\"!document.fullscreenElement ? document.documentElement.requestFullscreen() :  document.exitFullscreen();\">"+
            "          <i class=\"fa-solid fa-maximize\"></i>"+
            "        </button>" +
            "      </div>"+
            "    </div>" +
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
                
                Dictionary<string, string> postParams = new Dictionary<string, string>();
                if(req.HttpMethod == "POST"){
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
                if ((req.HttpMethod == "POST") && (req.Url.AbsolutePath == "/keyboard/stratogem"))
                {
                   

                    //new uint[] { 0x65, 0x68, 0x62, 0x66, 0x64, 0x68, 0x0D, 0x0D, }
                    foreach (var key in HelldiversTwoMacro.stratogems[postParams["stratogem"]])
                    {
                        KeyboardMacro.SendKey(key);
                        Console.WriteLine(key);
                    }
                    resp.Redirect("/");
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
                Console.WriteLine(HelldiversTwoMacro.stratogemsIcons[stratogem.Key]);
                pageData +=
                    "<div class=\"m-2\">" +
                    "  <form style=\"margin-bottom: 0px;\" method=\"post\" action=\"keyboard\\stratogem\">" +
                    "    <input type=\"hidden\" value=\""+ stratogem.Key + "\">" +
                    "    <input style=\"background-image: url('"+ HelldiversTwoMacro.stratogemsIcons[stratogem.Key].ToString() + "'); width: 200px;height: 200px;background-color: aquamarine;\" type=\"submit\" value=\"" + FirstLetterToUpper(stratogem.Key) + "\">" +
                    "  </form>" +
                    "</div>";
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

        public static string FirstLetterToUpper(string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }
    }
}
