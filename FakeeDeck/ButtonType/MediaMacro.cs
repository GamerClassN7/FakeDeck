using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeeDeck.ButtonType
{
    internal class MediaMacro : Button
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
            return getButtonHTML(mediaIcons[Key], null, Key, "button\\MediaMacro", new Dictionary<string, string>() { { "control_action", Key } });
        }

        public static bool invokeAction(string control_action)
        {
            KeyboardMacro.SendKey(mediaControls[control_action][0]);
            Console.WriteLine(control_action);
            return true;
        }
    }
}
