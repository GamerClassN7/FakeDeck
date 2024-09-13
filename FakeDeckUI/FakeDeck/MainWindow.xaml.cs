using FakeDeck.Class;
using FakeDeck.Class;
using QRCoder;
using System.Reflection.Emit;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static QRCoder.QRCodeGenerator;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using System.Drawing;
using Color = System.Drawing.Color;
using AutoUpdaterDotNET;

namespace FakeDeck
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void FakeDeckUI_Activated(object sender, EventArgs e)
        {
            string port = ((App)Application.Current).yaml.getData().GetProperty("server").GetProperty("port").ToString();
            string url = "http://localhost:" + port;

            PayloadGenerator.Url qrCodePayload = new PayloadGenerator.Url(url);
            QRCodeGenerator qrCodeGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(qrCodePayload.ToString(), 0);
            QRCode qrCode = new QRCode(qrCodeData);
            qr_code.Source = GeneralHelper.BitmapToImageSource(qrCode.GetGraphic(20, Color.Black, Color.White, false));

            AutoUpdateHelper updater = new AutoUpdateHelper();
        }
    }
}