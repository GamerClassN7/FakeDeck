using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;

namespace FakeeDeck.Class
{
    internal class FakeDeckMain
    {
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
        public string pageData = "";
        public FakeDeckMain(YamlHelper yaml)
        {
            HttpServer server = new HttpServer(yaml.getData().GetProperty("server").GetProperty("port").ToString());

            foreach (JsonElement item in yaml.getData().GetProperty("pages").EnumerateArray())
            {
                Debug.WriteLine("PAGE: " + item.GetProperty("page"));
                foreach (JsonElement button in item.GetProperty("buttons").EnumerateArray())
                {
                    pageData += AbstractionHelper.getButtonVisual(button);
                }
            }

            server.addRoute(servViewResponseAsync, "GET", "/");
            server.addRoute(servButtonResponseAsync, "POST", "/button/");

            /*foreach (var stratogem in HelldiversTwoMacro.stratogems)
            {
                server.pageData += HelldiversTwoMacro.getButton(stratogem.Key);
            }

            foreach (var control in MediaMacro.mediaControls)
            {
                server.pageData += MediaMacro.getButton(control.Key);
            }*/

            server.serv();
        }

        private static void callButtonAction(string module, Dictionary<string, string> postParams)
        {
            string cleanClass = "FakeeDeck.ButtonType." + module.Trim('/');

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
        }
    }
}
