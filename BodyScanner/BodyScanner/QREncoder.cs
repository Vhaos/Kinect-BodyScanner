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
    class QREncoder
    {
        
        public enum QR_Quality { High, Medium, Low };

        QR_Quality qualitySetting;

        public QREncoder(QR_Quality qualitySetting)
        {
            this.qualitySetting = qualitySetting; 
        }


        public Bitmap getQRCodeBitmap(String message)
        {
            int scale = 25;
            QRCodeEncoder encoder = new QRCodeEncoder();
            encoder.QRCodeScale = scale;

            switch (qualitySetting)
            {
                case (QR_Quality.High):
                    encoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H; 
                    break;
                case (QR_Quality.Medium):
                    encoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M; 
                    break;
                case (QR_Quality.Low):
                    encoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L; 
                    break;
            }

            return encoder.Encode(message);
        }

        public BitmapImage getQRCodeBitmapImage(String message)
        {
            return bitmapToBitmapImage(getQRCodeBitmap(message));
        }

        private BitmapImage bitmapToBitmapImage(Bitmap bitmap)
        {
            using (MemoryStream memoryStream = new MemoryStream())
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


    }
}
