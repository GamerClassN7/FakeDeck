using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using static System.Text.Json.JsonElement;

namespace FakeDeck.ButtonType
{
    class ProcessMacro : Button
    {
        public static string getButton(string process, string arguments = "", string? icon = null, string? image = null)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>() { { "process", process } };
            if (!string.IsNullOrEmpty(arguments))
            {
                parameters.Add("arguments", arguments);
            }

            if (!string.IsNullOrEmpty(icon) || !string.IsNullOrEmpty(image))
            {
                process = null;
            }

            return getButtonHTML(icon, image, process, "button\\ProcessMacro", null, parameters);
        }

        public static bool invokeAction(string process, string arguments = "")
        {
            if (!System.IO.File.ReadAllText("./configuration.yaml").Contains(process))
            {
                Debug.WriteLine("not known process");
                return true;
            }

            Process notePad = new Process();
            notePad.StartInfo.FileName = process;
            notePad.StartInfo.Arguments = arguments;
            notePad.Start();
            return true;
        }
    }
}
