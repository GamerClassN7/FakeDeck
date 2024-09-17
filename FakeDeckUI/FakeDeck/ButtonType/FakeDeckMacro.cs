using FakeDeck.ButtonType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FakeDeck.ButtonType
{
    internal class FakeDeckMacro : Button
    {
        public static Dictionary<string, string> actionIcons = new Dictionary<string, string>
        {
            { "full-screen", "fa-maximize"},
            { "set-page", "fa-page"},
        };

        public static string getButton(string Key, string Image = null, string PageId = null, string Color = null)
        {
            if (Key == "full-screen")
            {
                return getButtonHTML(actionIcons[Key], Image, Key, null, "!document.fullscreenElement?document.documentElement.requestFullscreen():document.exitFullscreen();", null, Color);
            }
            if (Key == "set-page")
            {
                return getButtonHTML(actionIcons[Key], Image, Key, null, "loadPage('"+ PageId + "')", null, Color);
            }
            if (Key == "spacer")
            {
                return getButtonHTML(null, null, null, null, null, null, Color);
            }
            else {
                return "NIC";
            }
        }
    }
}
