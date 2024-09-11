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

namespace FakeeDeck.Class
{
    internal class FakeDeck
    {
        public FakeDeck()
        {
            HttpServer server = new HttpServer();

            foreach (var stratogem in HelldiversTwoMacro.stratogems)
            {
                server.pageData += HelldiversTwoMacro.getButton(stratogem.Key);
            }

            foreach (var control in MediaMacro.mediaControls)
            {
                server.pageData += MediaMacro.getButton(control.Key);
            }

            server.serv();
        }
    }
}
