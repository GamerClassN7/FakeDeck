using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace FakeeDeck.ButtonType
{
    class HelldiversTwoMacro : Button
    {
        //https://helldivers.wiki.gg/wiki/Stratagems
        //https://helldivers.fandom.com/wiki/Stratagems_(Helldivers_2)

        public static Dictionary<string, uint[]> stratogems = new Dictionary<string, uint[]>
        {
            { "reinforce", new uint[] { 0x65, 0x68, 0x62, 0x66, 0x64, 0x68}},
            { "resupply", new uint[] { 0x65, 0x62, 0x62, 0x68, 0x66 }},
            //Patriotic Administration Center
            { "anti-material", new uint[] { 0x65, 0x62, 0x64, 0x66, 0x68, 0x62}},
            { "flamethrower", new uint[] { 0x65, 0x62, 0x64, 0x68, 0x62, 0x68}},
            { "autocannon", new uint[] { 0x65, 0x62, 0x64, 0x62, 0x68, 0x68, 0x66}},
            { "grenade-launcher", new uint[] { 0x65, 0x62, 0x64, 0x68, 0x64, 0x62}},

            //Orbital Cannons
            { "orbital-precision-strike", new uint[] { 0x65, 0x66, 0x66, 0x68}},
            //Robotics Workshop
            { "mortar-sentry", new uint[] { 0x65, 0x62, 0x68, 0x66, 0x62}},
            { "gatling-sentry", new uint[] { 0x65, 0x62, 0x68, 0x66, 0x64}},
            //Hangar
            { "rocket-pods", new uint[] { 0x65, 0x68, 0x66, 0x68, 0x64}},
            { "bomb", new uint[] { 0x65, 0x68, 0x66, 0x68, 0x64}},
        };

        public static Dictionary<string, string> stratogemsIcons = new Dictionary<string, string>
        {
            { "reinforce", "https://static.wikia.nocookie.net/helldivers_gamepedia/images/5/5a/HD2_Reinforce.png"},
            { "resupply", "https://static.wikia.nocookie.net/helldivers_gamepedia/images/7/72/HD2_Resupply.png"},
            { "anti-material", "https://static.wikia.nocookie.net/helldivers_gamepedia/images/c/c3/APW-1_Anti-Materiel_Rifle_Icon.png"},
            { "flamethrower", "https://static.wikia.nocookie.net/helldivers_gamepedia/images/c/cc/FLAM-40_Flamethrower_Icon.png"},
            { "autocannon", "https://static.wikia.nocookie.net/helldivers_gamepedia/images/c/c6/AC-8_Autocannon_Icon.png"},
            { "grenade-launcher","https://static.wikia.nocookie.net/helldivers_gamepedia/images/6/66/GL-21_Grenade_Launcher_Icon.png"},
            { "orbital-precision-strike", "https://static.wikia.nocookie.net/helldivers_gamepedia/images/4/47/Orbital_Precision_Strike_Icon.png"},
            { "mortar-sentry", "https://static.wikia.nocookie.net/helldivers_gamepedia/images/1/1d/A_M-12_Mortar_Sentry_Icon.png"},
            { "gatling-sentry","https://static.wikia.nocookie.net/helldivers_gamepedia/images/4/48/A_G-16_Gatling_Sentry_Icon.png"},
            { "rocket-pods","https://static.wikia.nocookie.net/helldivers_gamepedia/images/e/e3/Eagle_110MM_Rocket_Pods_Icon.png"},
            { "bomb","https://static.wikia.nocookie.net/helldivers_gamepedia/images/5/5a/Eagle_500KG_Bomb_Icon.png"},
        };

        public static string getButton(string Key)
        {
            return
                "<div class=\"m-2\">" +
                "  <form style=\"margin-bottom: 0px;\" method=\"post\" action=\"button\\HelldiversTwoMacro\">" +
                "    <input type=\"hidden\" name=\"stratogem\" value=\"" + Key + "\">" +
                "    <input style=\"background-size: cover; background-image: url('" + stratogemsIcons[Key].ToString() + "'); width: 150px;height: 150px;background-color: aquamarine;\" type=\"submit\" value=\"" + FirstLetterToUpper(Key) + "\">" +
                "  </form>" +
                "</div>";
        }

        public static bool invokeAction(string stratogem)
        {
            foreach (var key in stratogems[stratogem])
            {
                KeyboardMacro.SendKey(key);
                Console.WriteLine(key);
            }
            return true;
        }

        private static string FirstLetterToUpper(string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }
    }
}
