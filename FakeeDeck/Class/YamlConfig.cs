using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FakeeDeck.Class
{
    internal class YamlConfig
    {
        public Dictionary<string, dynamic> Data { get; private set; }
        public YamlConfig(string path)
        {
            Data = new Dictionary<string, dynamic>();
            ParseYamlFile(File.ReadAllLines(path));
        }
        private Dictionary<dynamic, dynamic> ParseYamlFile(string[] lines, int levelStart = 0, int linesParsedPreviuse = 0)
        {
            Dictionary<dynamic, dynamic> tempObject = new Dictionary<dynamic, dynamic>();

            int linesParsed = 0;

            if ((linesParsed + linesParsedPreviuse) <= lines.Length)
            {
                int level = 0;
                int iterator = 0;
                foreach (string line in lines)
                {
                    level = line.Count(x => x == '\t');

                    linesParsed++;
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }

                    Console.WriteLine(level + ">"+ levelStart + " level " + linesParsedPreviuse + " Lines handeled previouselly '" + line + "'");
                    if ((level > levelStart || levelStart == 0))
                    {
                        if (line.Contains(":"))
                        {
                            Console.WriteLine("Key:Value Pair");
                            iterator = 0;
                            string[] array = line.Split(":");
                            string key = array[0].Trim('\t');
                            dynamic value = "";

                            if (array.Length > 1 && !string.IsNullOrEmpty(array[1]))
                            {
                                Console.WriteLine("Strict Value Found: " + array[1]);
                                tempObject.Add(key,array[1]);
                            }
                            else if (array.Length == 2 && string.IsNullOrEmpty(array[1]) && lines[linesParsed..lines.Length].Length > 0)
                            {
                                tempObject.Add(key, ParseYamlFile(lines[linesParsed..lines.Length], level, linesParsed));
                            }
                        } else
                        {
                            iterator++;
                            tempObject.Add(iterator,line);
                        }
                    } else if (level < levelStart)
                    {
                        Console.WriteLine("Level DOwns");
                    }
                }
            }

            Debug.WriteLine(JsonSerializer.Serialize(tempObject));
            return tempObject;
        }
    }
}
