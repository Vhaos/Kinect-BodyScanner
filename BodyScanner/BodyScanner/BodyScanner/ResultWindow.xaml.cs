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

        public ResultWindow()
        {
            InitializeComponent();

            Bitmap qr_code = get_qr_code(QR_Quality.Low);

            BitmapImage imageSource = bitmapToBitmapImage(qr_code);
 
            bitmap_qr_code.Source = imageSource;
        }
        
        private Bitmap encodeResults(QRCodeEncoder encoder)
        {
            String xmlMeasurementsFile = new FileManager(null).getMeasurementsFile();

            XmlDocument xmlDoc = new XmlDocument(); // Create an XML document object
            xmlDoc.LoadXml(xmlMeasurementsFile); // Load the XML document from the specified file

            XmlNode height = xmlDoc.SelectSingleNode("/MSV_Measures/measure[@name='Height']");
            XmlNode hip = xmlDoc.SelectSingleNode("/MSV_Measures/measure[@name='Hip']");
            XmlNode chest = xmlDoc.SelectSingleNode("/MSV_Measures/measure[@name='Chest | Bust']");
            XmlNode waist = xmlDoc.SelectSingleNode("/MSV_Measures/measure[@name='Waist']");
            XmlNode insideLeg = xmlDoc.SelectSingleNode("/MSV_Measures/measure[@name='Inside Leg']");

            String measurements = "Height: " + height.InnerText + "\n" +
                                  "Hip: " + hip.InnerText + "\n" +
                                  "Chest: " + chest.InnerText + "\n" +
                                  "Waist: " + waist.InnerText + "\n" +
                                  "InsideLeg: " + insideLeg.InnerText;

            return encoder.Encode(measurements);
        }

        private Bitmap get_qr_code(QR_Quality qualitySetting, int scale = 25)
        {
            QRCodeEncoder encoder = new QRCodeEncoder();
            encoder.QRCodeScale = scale;

            switch(qualitySetting)
            {
                case(QR_Quality.High):
                    encoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H; break;
                case(QR_Quality.Medium):
                    encoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M; break;
                case(QR_Quality.Low):
                    encoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L; break;
            }

            return encodeResults(encoder);
        }

        private BitmapImage bitmapToBitmapImage(Bitmap bitmap)
        {
            using(MemoryStream memoryStream = new MemoryStream())
            {
                bitmap.Save(memoryStream, ImageFormat.Png);
                memoryStream.Position = 0;
                BitmapImage imageSource = new BitmapImage();
                imageSource.BeginInit();
                imageSource.CacheOption = BitmapCacheOption.OnLoad;
                imageSource.StreamSource = memoryStream;
                imageSource.EndInit();
                imageSource.Freeze();

                // Attempted to validate the imageSource by comparing pixelHeight and width, but doesn't add up :(
                Log.Write("image source with H: " + imageSource.Height + " and W: " + imageSource.Width);
                return imageSource;
            }
        }

        private void done_btn_Click(object sender, RoutedEventArgs e)
        {

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