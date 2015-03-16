using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;
using System.IO;

namespace BodyScanner
{
    /// <summary>
    /// Interaction logic for ResultWindow.xaml
    /// </summary>
    public partial class ResultWindow : Window
    {
        public ResultWindow()
        {
            InitializeComponent();
            
            QRCodeEncoder encoder = new QRCodeEncoder();
            Log.Write(Log.Tag.INFO, encoder.ToString() + ": initialised");
            Bitmap bitmap = encoder.Encode("132464654654");
            BitmapImage imageSource = bitmapToBitmapImage(bitmap);
            bitmap_qr_code.Source = imageSource; // try to get it to work here first, as not quite sure whether the using directive will throw away our image after it closes
            Log.Write(Log.Tag.INFO, "Bitmap generated with H: " + bitmap.Height + " and W: " + bitmap.Width);
            
           
            
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