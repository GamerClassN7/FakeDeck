using FakeeDeck;
using FakeeDeck.Class;
using System.Diagnostics;
using System.Text.Json;


YamlConfig parser = new YamlConfig("configuration.yaml");
var result = parser.Data;

Console.WriteLine(JsonSerializer.Serialize(result));


HttpServer.serv();
