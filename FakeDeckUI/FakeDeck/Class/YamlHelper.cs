﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace FakeDeck.Class
{
    public  class YamlHelper
    {
        JsonDocument jsonObjecz;
        public YamlHelper()
        {
            var r = new StreamReader("./configuration.yaml");
            var deserializer = new Deserializer();
            object yamlObject = deserializer.Deserialize(r);
            string json = System.Text.Json.JsonSerializer.Serialize(yamlObject);
            jsonObjecz = JsonDocument.Parse(json);
        }

        public JsonElement getData()
        {
            return jsonObjecz.RootElement;
        }
    }
}
