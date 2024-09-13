using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Windows;
using static System.Text.Json.JsonElement;

namespace FakeDeck.Class
{
    internal class FakeDeckMain
    {
        private static string cachePath = "./cache/";
        public static string pageHeader =
    "<!DOCTYPE>" +
    "<html lang=\"en\">" +
    "  <head>" +
    "    <title>HttpListener Example</title>" +
    "    <meta charset=\"utf-8\">" +
    "    <meta name = \"viewport\" content=\"width=device-width, initial-scale=1, user-scalable=yes\">" +
    "    <link href=\"https://yarnpkg.com/en/package/normalize.css\" rel=\"stylesheet\">" +
    "    <link href=\"https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css\" rel=\"stylesheet\">" +
    "    <link href=\"https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.2/css/all.min.css\" rel=\"stylesheet\">" +
    "    <link href=\"StaticFiles/style.css\" rel=\"stylesheet\">" +
    "  </head>" +
    "  <body>" +
    "    <div id=\"main\" class=\"d-flex flex-wrap\" style=\"transform-origin: left top;\">";
        public static string pageFooter =
            "    </div>" +
            "    <script src=\"StaticFiles/app.js\"></script>" +
            "  </body>" +
            "</html>";
        public string pageData = "";
        private ArrayEnumerator pages;
        public FakeDeckMain(YamlHelper yaml)
        {

            HttpServer server = new HttpServer(yaml.getData().GetProperty("server").GetProperty("port").ToString());
            pages = yaml.getData().GetProperty("pages").EnumerateArray();

            //ClearCache
            if (Directory.Exists(cachePath))
            {
                DirectoryInfo di = new DirectoryInfo(cachePath);
                foreach (FileInfo file in di.EnumerateFiles())
                {
                    file.Delete();
                }
            }

            pageData = renderPageView();

            server.addRoute(servViewResponseAsync, "GET", "/");
            server.addRoute(servButtonResponseAsync, "POST", "/button/");
            server.addRoute(servPageResponseAsync, "POST", "/page");
            server.serv();
        }

        private static void callButtonAction(string module, Dictionary<string, string> postParams)
        {
            string cleanClass = "FakeDeck.ButtonType." + module.Trim('/');

            Type? buttonClass = Type.GetType(cleanClass, true);

            if (buttonClass is null)
                return;

            MethodInfo? method = buttonClass.GetMethod("invokeAction");

            if (method is null)
                return;

            ParameterInfo[] pars = method.GetParameters();
            List<object> parameters = new List<object>();

            foreach (ParameterInfo p in pars)
            {
                if (p == null)
                {
                    continue;
                }

                if (p.Name != null && postParams.ContainsKey(p.Name))
                {
                    parameters.Insert(p.Position, postParams[p.Name]);
                }
                else if (p.IsOptional && p.DefaultValue != null)
                {
                    parameters.Insert(p.Position, p.DefaultValue);
                }
            }

            _ = method.Invoke(null, [.. parameters]).ToString();
        }

        private async Task servViewResponseAsync(HttpListenerRequest req, HttpListenerResponse resp)
        {
            string disableSubmit = false ? "disabled" : "";
            byte[] data = Encoding.UTF8.GetBytes(string.Format(pageHeader + this.pageData + pageFooter, 0, disableSubmit));
            resp.ContentType = "text/html";
            resp.ContentEncoding = Encoding.UTF8;
            resp.ContentLength64 = data.LongLength;
            await resp.OutputStream.WriteAsync(data, 0, data.Length);
        }

        private async Task servButtonResponseAsync(HttpListenerRequest req, HttpListenerResponse resp, Dictionary<string, string> postParams)
        {
            try
            {
                string module = req.Url.AbsolutePath.Replace("/button", "");
                Debug.WriteLine("Call module " + module);
                callButtonAction(module, postParams);
                resp.StatusCode = (int)HttpStatusCode.OK;
                await resp.OutputStream.FlushAsync();
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

        private async Task servPageResponseAsync(HttpListenerRequest req, HttpListenerResponse resp, Dictionary<string, string> postParams)
        {
            string pageContent = "";
            try
            {
                pageContent = renderPageView(postParams["Key"]);
                resp.StatusCode = (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                pageContent = ex.Message;
                resp.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            byte[] errorData = Encoding.UTF8.GetBytes(pageContent);
            resp.ContentType = "text/html";
            resp.ContentEncoding = Encoding.UTF8;
            resp.ContentLength64 = errorData.LongLength;
            await resp.OutputStream.WriteAsync(errorData, 0, errorData.Length);
        }

        private string renderPageView(string page = null)
        {
            JsonElement selectedPage = pages.First();

            if (page != null)
                selectedPage = pages.SingleOrDefault(item => item.GetProperty("page").ToString() == page);

            string SelectedPageName = selectedPage.GetProperty("page").ToString();
            if (File.Exists(cachePath + SelectedPageName + ".html"))
                return File.ReadAllText(cachePath + SelectedPageName + ".html");

            string pageContent = "";
            foreach (JsonElement button in selectedPage.GetProperty("buttons").EnumerateArray())
            {
                pageContent += AbstractionHelper.getButtonVisual(button);
            }

            if (Directory.Exists(cachePath))
                Directory.CreateDirectory(cachePath);

            File.WriteAllText(cachePath + SelectedPageName + ".html", pageContent);
            return pageContent;
        }
    }
}
