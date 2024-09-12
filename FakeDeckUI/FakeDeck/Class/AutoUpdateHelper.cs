using AutoUpdaterDotNET;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FakeeDeck.Class
{
    internal class AutoUpdateHelper
    {
        public AutoUpdateHelper() {
            AutoUpdater.ParseUpdateInfoEvent += AutoUpdaterOnParseUpdateInfoEvent;
            AutoUpdater.Synchronous = true;
            AutoUpdater.ShowRemindLaterButton = false;
            AutoUpdater.Start("https://api.github.com/repos/ravibpatel/AutoUpdater.NET/releases/latest");
        }
        private void AutoUpdaterOnParseUpdateInfoEvent(ParseUpdateInfoEventArgs args)
        {
            JsonElement json = JsonDocument.Parse(args.RemoteData).RootElement;
            args.UpdateInfo = new UpdateInfoEventArgs
            {
                CurrentVersion = json.GetProperty("tag_name").ToString().TrimStart('v'),
                DownloadURL = json.GetProperty("zipball_url").ToString(),
            };

            Debug.WriteLine(json.ToString());
        }
    }
}
