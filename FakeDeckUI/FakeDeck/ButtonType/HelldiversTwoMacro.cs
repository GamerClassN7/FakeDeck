using FakeeDeck.Class;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace FakeeDeck.ButtonType
{
    class HelldiversTwoMacro : Button
    {
        const uint Key_Up = 0x68;
        const uint Key_Down = 0x62;
        const uint Key_Left = 0x64;
        const uint Key_Right = 0x66;

        //https://helldivers.wiki.gg/wiki/Stratagems
        //https://helldivers.fandom.com/wiki/Stratagems_(Helldivers_2)

        public static Dictionary<string, uint[]> stratogems = new Dictionary<string, uint[]>
        {
            //Mission
            { "reinforce", new uint[] { 0x65, Key_Up, Key_Down, Key_Right, Key_Left, Key_Up}},
            { "resupply", new uint[] { 0x65, Key_Down, Key_Down, Key_Up, Key_Right }},

            //Support Weapons
            { "anti-material", new uint[] { 0x65, Key_Down, Key_Left, Key_Right, Key_Up, Key_Down}},
            { "flamethrower", new uint[] { 0x65, Key_Down, Key_Left, Key_Up, Key_Down, Key_Up}},
            { "autocannon", new uint[] { 0x65, Key_Down, Key_Left, Key_Down, Key_Up, Key_Up, Key_Right}},
            { "grenade-launcher", new uint[] { 0x65, Key_Down, Key_Left, Key_Up, Key_Left, Key_Down}},

            //Offensive: Orbital Strikes
            { "orbital-precision-strike", new uint[] { 0x65, Key_Right, Key_Right, Key_Up}},

            //Defensive
            { "mortar-sentry", new uint[] { 0x65, Key_Down, Key_Up, Key_Right, Key_Down}},
            { "gatling-sentry", new uint[] { 0x65, Key_Down, Key_Up, Key_Right, Key_Left}},


            //Supply: Backpacks

            //Offensive: Eagle
            { "strafing-run", new uint[] { 0x65, Key_Up, Key_Right, Key_Right}},
            { "airstrike", new uint[] { 0x65, Key_Up, Key_Right, Key_Down, Key_Right}},
            { "cluster-bomb", new uint[] { 0x65, Key_Up, Key_Right, Key_Down, Key_Down, Key_Right}},
            { "napalm-airstrike", new uint[] { 0x65, Key_Up, Key_Right, Key_Down, Key_Up}},
            { "smoke-strike", new uint[] { 0x65, Key_Up, Key_Right, Key_Up, Key_Down}},
            { "rocket-pods", new uint[] { 0x65, Key_Up, Key_Right, Key_Up, Key_Left}},
            { "bomb", new uint[] { 0x65, Key_Up, Key_Right, Key_Up, Key_Right}},

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
            
            //Offensive: Eagle
            { "strafing-run","https://static.wikia.nocookie.net/helldivers_gamepedia/images/3/33/Eagle_Strafing_Run_Icon.png"},
            { "airstrike","https://static.wikia.nocookie.net/helldivers_gamepedia/images/7/7e/Eagle_Airstrike_Icon.png"},
            { "cluster-bomb","https://static.wikia.nocookie.net/helldivers_gamepedia/images/8/89/Eagle_Cluster_Bomb_Icon.png"},
            { "napalm-airstrike","https://static.wikia.nocookie.net/helldivers_gamepedia/images/d/d4/Eagle_Napalm_Airstrike_Icon.png"},
            { "smoke-strike","https://static.wikia.nocookie.net/helldivers_gamepedia/images/1/1a/Eagle_Smoke_Strike_Icon.png"},
            { "rocket-pods","https://static.wikia.nocookie.net/helldivers_gamepedia/images/e/e3/Eagle_110MM_Rocket_Pods_Icon.png"},
            { "bomb","https://static.wikia.nocookie.net/helldivers_gamepedia/images/5/5a/Eagle_500KG_Bomb_Icon.png"},
        };

        public static string getButton(string Key)
        {
            return getButtonHTML(null, stratogemsIcons[Key].ToString(), Key, "button\\HelldiversTwoMacro", new Dictionary<string, string>() { { "stratogem", Key } });
        }

        public static bool invokeAction(string stratogem)
        {
            foreach (var key in stratogems[stratogem])
            {
                KeyboardMacro.SendKey(key);
                Debug.WriteLine(key);
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
