using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace FakeDeck.ButtonType
{
    internal class Button
    {
        public static string getButtonHTML(string icon, string image, string name, string action = null, string jsAction = null, Dictionary<string, string> parameters = null, string color = null)
        {
            string body = "";
            string styles = "style=\"background-size: cover; " + (!string.IsNullOrEmpty(image) ? "background-image: url('" + image + "');" : "") + " width: 150px; height: 150px; " + (!string.IsNullOrEmpty(image) ? "background-image: url('" + image + "');" : "") + (!string.IsNullOrEmpty(color) ? " background-color: " + color + "; " : "") + "\"";

            body += "<div>";
            if (action != null)
            {
                body += "<form style=\"margin-bottom: 0px;\" method=\"post\" action=\"" + action + "\">";

                if (parameters is not null)
                {
                    foreach (var parameter in parameters)
                    {
                        body += "<input type=\"hidden\" name=\"" + parameter.Key + "\" value=\"" + parameter.Value + "\">";
                    }
                }

                body += "<button class=\"button\" type=\"submit\" value=\"submit\" " + styles + ">";
                body += (!string.IsNullOrEmpty(icon) ? "<i class=\"fa-solid " + icon + "\"></i>" : name);
                body += "</button>";
                body += "</form>";
            }
            else if (jsAction != null)
            {
                body += "<button class=\"button\" onclick=\"" + jsAction + "\" " + styles + " >";
                body += (!string.IsNullOrEmpty(icon) ? "<i class=\"fa-solid " + icon + "\"></i>" : name);
                body += "</button>";
            }
            else
            {
                body += "<div class=\"button\" " + styles + "></div>";
            }
            body += "</div>";

            return body;
        }

        public static string getButton(string Key)
        {
            return getButtonHTML(null, "https://docs.itego.cz/uploads/images/system/2022-04/OdRTPJ4iTTInmhdP-jagq7dfjpi2lilfg-imageedit-2-6604933313.gif", "Test", "test/test");
        }

        public static bool invokeAction(string Key)
        {
            return false;
        }
    }
}
