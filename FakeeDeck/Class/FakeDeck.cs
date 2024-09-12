using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Reflection;
using FakeeDeck.ButtonType;
using YamlDotNet.Serialization;

namespace FakeeDeck.Class
{
    internal class FakeDeck
    {
        public FakeDeck()
        {

            YamlHelper yaml = new YamlHelper();
            HttpServer server = new HttpServer();

            foreach (JsonElement item in yaml.getData().GetProperty("pages").EnumerateArray())
            {
                Debug.WriteLine("PAGE: " + item.GetProperty("page"));
                foreach (JsonElement button in item.GetProperty("buttons").EnumerateArray())
                {
                    server.pageData += AbstractionHelper.getButtonVisual(button);
                }
            }

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

    
    }
}
