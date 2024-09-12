using AutoUpdaterDotNET;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace FakeeDeck.Class
{
    public class AutoUpdateHelper
    {
        public AutoUpdateHelper() {
            AutoUpdater.ParseUpdateInfoEvent += AutoUpdaterOnParseUpdateInfoEvent;
            AutoUpdater.Synchronous = true;
            AutoUpdater.ShowRemindLaterButton = false;
            AutoUpdater.ReportErrors = Debugger.IsAttached;
            AutoUpdater.HttpUserAgent = ("FakeDeck-v" + Assembly.GetExecutingAssembly().GetName().Version);
            AutoUpdater.Start("https://api.github.com/repos/ravibpatel/AutoUpdater.NET/releases/latest");
        }
        private void AutoUpdaterOnParseUpdateInfoEvent(ParseUpdateInfoEventArgs args)
        {
            JsonElement json = JsonDocument.Parse(args.RemoteData).RootElement;
            args.UpdateInfo = new UpdateInfoEventArgs
            {
                CurrentVersion = json.GetProperty("tag_name").ToString().TrimStart('v')+ ".0",
                DownloadURL = json.GetProperty("zipball_url").ToString(),
            };
            Debug.WriteLine("calling Updater");
        }
    }
}
