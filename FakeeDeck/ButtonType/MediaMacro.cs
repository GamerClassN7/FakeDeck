using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeeDeck.ButtonType
{
    internal class MediaMacro
    {
        public static Dictionary<string, uint[]> mediaControls = new Dictionary<string, uint[]>
        {
            { "play/pause", new uint[] { 0xB3}},
            { "mute", new uint[] { 0xAD }},
        };

        public static Dictionary<string, string> mediaIcons = new Dictionary<string, string>
        {
            { "play/pause", "fa-play"},
            { "mute", "fa-volume-xmark"},
        };

        public static string getButton(string Key)
        {
            return
                "<div class=\"m-2\">" +
                "  <form style=\"margin-bottom: 0px;\" method=\"post\" action=\"button\\MediaMacro\">" +
                "    <input type=\"hidden\" name=\"control_action\" value=\"" + Key + "\">" +
                "    <button type=\"submit\" value=\"" + Key + "\" style=\"width: 150px;height: 150px;background-color: aquamarine;\" >" +
                "      <i class=\"fa-solid "+ mediaIcons[Key] +"\"></i>" +
                "    </button>" +
                "  </form>" +
                "</div>";
        }

        public static bool invokeAction(string control_action)
        {
            KeyboardMacro.SendKey(mediaControls[control_action][0]);
            Console.WriteLine(control_action);
            return true;
        }
    }
}
