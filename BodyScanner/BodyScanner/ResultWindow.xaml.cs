using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;
using System.IO;
using System.Xml;

namespace BodyScanner
{
    /// <summary>
    /// Interaction logic for ResultWindow.xaml
    /// </summary>
    public partial class ResultWindow : Window
    {
        private enum QR_Quality {High, Medium, Low};

        String measurementRequestID;

        public ResultWindow(String id, GenderWindow.GenderType gender)
        {
            InitializeComponent();
            this.measurementRequestID = id;

           QREncoder qr_encoder = new QREncoder(QREncoder.QR_Quality.Low);

           BitmapImage imageSource = qr_encoder.getQRCodeBitmapImage(measurementRequestID + ";" + Enum.GetName(typeof(GenderWindow.GenderType), gender));
 
           bitmap_qr_code.Source = imageSource;
        }
        

        private void done_btn_Click(object sender, RoutedEventArgs e)
        {
            StartWindow nextRun = new StartWindow();
            nextRun.Show();
            this.Close();
        }

        private void help_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void privacy_btn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
/*CODE DUMP
 *  // Leave commented unless you want to sanity check the bitmap -> I have, it is a bunch of black and white pixels as you'd expect
            // GetPixel is very slow (also as you'd expect) so not for the faint of heart!
            /*for(int y = 0; y < bitmap.Height; y++)
            {
                for(int x = 0; x < bitmap.Width; x++)
                {
                    Log.Write(Log.Tag.VERBOSE, "Pixel at ("+y+","+x+"): " + bitmap.GetPixel(x, y));
                }
            }
            
            // Trying to get it to work inside function first - clearly just going mad at this point
            //BitmapImage imageSource = bitmapToBitmapImage(bitmap);
*/