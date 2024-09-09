using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeeDeck.ButtonType
{
    internal class Button
    {
        public static string getButtonHTML(string icon, string image, string name, string action, Dictionary<string, string> parameters)
        {
            string body = "<div class=\"m-2\">";
            body += "<form style=\"margin-bottom: 0px;\" method=\"post\" action=\"" + action + "\">";

            foreach (var parameter in parameters)
            {
                body += "<input type=\"hidden\" name=\"" + parameter.Key + "\" value=\"" + parameter.Value + "\">";
            }

            body += "<button type=\"submit\" value=\"submit\" style=\"background-size: cover; " + (!string.IsNullOrEmpty(image) ? "background-image: url('" + image + "');" : "") + " width: 150px;height: 150px; background-color: aquamarine;\" >";
            body += (!string.IsNullOrEmpty(icon) ? "<i class=\"fa-solid " + icon + "\"></i>" : name);

            body += "</button>";
            body += "</form>";
            body += "</div>";
            return body;
        }

        public static string getButton(string Key)
        {
            return ""; //getButtonHTML(null, "https://docs.itego.cz/uploads/images/system/2022-04/OdRTPJ4iTTInmhdP-jagq7dfjpi2lilfg-imageedit-2-6604933313.gif");


        }
        public static bool invokeAction(string Key)
        {
            return false;
        }
    }
}
