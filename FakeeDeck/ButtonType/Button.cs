using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeeDeck.ButtonType
{
    internal class Button
    {
        public static string getButton(string Key)
        {
            return
                "<div class=\"m-2\">" +
                "  <form style=\"margin-bottom: 0px;\" method=\"post\" action=\"keyboard\\stratogem\">" +
                "    <input type=\"hidden\" name=\"stratogem\" value=>" +
                "    <input style=\"background-size: cover; ; width: 150px;height: 150px;background-color: aquamarine;\" type=\"submit\" value=\"\">" +
                "  </form>" +
                "</div>";
        }

        public static bool invokeAction(string Key)
        {
            return false;
        }
    }
}
