using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeeDeck.ButtonType
{
    class HelldiversTwoMacro
    {
        public static Dictionary<string, uint[]> stratogems = new Dictionary<string, uint[]>
        {
            { "reinforce", new uint[] { 0x65, 0x68, 0x62, 0x66, 0x64, 0x68}},
            { "resupply", new uint[] { 0x65, 0x62, 0x62, 0x68, 0x66 }},
            { "orbital-precision-strike", new uint[] { 0x65, 0x66, 0x66, 0x66}},
            { "anti-material", new uint[] { 0x65, 0x62, 0x66, 0x64, 0x68, 0x62}},
            { "mortar-sentry", new uint[] { 0x65, 0x62, 0x68, 0x66, 0x62}},
            { "gatling-sentry", new uint[] { 0x65, 0x62, 0x68, 0x66, 0x64}},
        };

        public static Dictionary<string, string> stratogemsIcons = new Dictionary<string, string>
        {
            { "reinforce", "https://static.wikia.nocookie.net/helldivers_gamepedia/images/5/5a/HD2_Reinforce.png"},
            { "resupply", "https://static.wikia.nocookie.net/helldivers_gamepedia/images/7/72/HD2_Resupply.png"},
            { "orbital-precision-strike", "https://static.wikia.nocookie.net/helldivers_gamepedia/images/4/47/Orbital_Precision_Strike_Icon.png"},
            { "anti-material", "https://static.wikia.nocookie.net/helldivers_gamepedia/images/c/c3/APW-1_Anti-Materiel_Rifle_Icon.png"},
            { "mortar-sentry", "https://static.wikia.nocookie.net/helldivers_gamepedia/images/1/1d/A_M-12_Mortar_Sentry_Icon.png"},
            { "gatling-sentry","https://static.wikia.nocookie.net/helldivers_gamepedia/images/4/48/A_G-16_Gatling_Sentry_Icon.png"},
        };
    }
}
