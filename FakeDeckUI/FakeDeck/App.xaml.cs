using FakeDeck.Class;
using Microsoft.VisualBasic.Logging;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Application;

namespace FakeDeck
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public YamlHelper yaml;

        protected override void OnStartup(StartupEventArgs e)
        {
            yaml = new YamlHelper();

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                FakeDeckMain fakeDeck = new FakeDeckMain(yaml);
            }).Start();

            base.OnStartup(e);
        }
    }
}
