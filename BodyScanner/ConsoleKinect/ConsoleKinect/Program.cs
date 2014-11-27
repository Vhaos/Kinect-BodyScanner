using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleKinect
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Microsoft.Kinect;
    using System.Reflection;
    using System.Text;
    using System.Threading;


    public class Program
    {

        private const int OpaquePixel = -1;
        private readonly int bytesPerPixel = (PixelFormats.Bgr32.BitsPerPixel + 7) / 8;
        private KinectSensor kinectSensor = null;
        private CoordinateMapper coordinateMapper = null;
        private MultiSourceFrameReader multiFrameSourceReader = null;
        private ushort[] depthFrameData = null;
        private byte[] colorFrameData = null;
        private ColorSpacePoint[] colorPoints = null;
        private CameraSpacePoint[] cameraPoints = null;
        private byte[] bodyPoints = null;
        private bool NO_COLOUR = true;
        public Program()
        {
            this.kinectSensor = KinectSensor.GetDefault();
            this.multiFrameSourceReader = this.kinectSensor.OpenMultiSourceFrameReader(FrameSourceTypes.Depth | FrameSourceTypes.Color | FrameSourceTypes.BodyIndex);
            this.multiFrameSourceReader.MultiSourceFrameArrived += this.Reader_MultiSourceFrameArrived;
            this.coordinateMapper = this.kinectSensor.CoordinateMapper;

            FrameDescription depthFrameDescription = this.kinectSensor.DepthFrameSource.FrameDescription;

            int depthWidth = depthFrameDescription.Width;
            int depthHeight = depthFrameDescription.Height;

            this.depthFrameData = new ushort[depthWidth * depthHeight];
            this.colorPoints = new ColorSpacePoint[depthWidth * depthHeight];
            this.cameraPoints = new CameraSpacePoint[depthWidth * depthHeight];
            this.bodyPoints = new byte[depthWidth * depthHeight];

            FrameDescription colorFrameDescription = this.kinectSensor.ColorFrameSource.FrameDescription;

            int colorWidth = colorFrameDescription.Width;
            int colorHeight = colorFrameDescription.Height;
            this.colorFrameData = new byte[colorWidth * colorHeight * this.bytesPerPixel];

            // open the sensor
            this.kinectSensor.Open();

            Console.ReadLine();

            this.multiFrameSourceReader.Dispose();
            this.multiFrameSourceReader = null;

            if (this.kinectSensor != null)
            {
                this.kinectSensor.Close();
                this.kinectSensor = null;
            }

        }


        private void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {

            MultiSourceFrame msf = e.FrameReference.AcquireFrame();

            if (msf != null)
            {
                Console.WriteLine("MSF OK");
                using (BodyIndexFrame bif = msf.BodyIndexFrameReference.AcquireFrame())
                {
                    Console.WriteLine("BIF OK");
                    using (DepthFrame df = msf.DepthFrameReference.AcquireFrame())
                    {
                        Console.WriteLine("DF OK");
                        using (ColorFrame cf = msf.ColorFrameReference.AcquireFrame())
                        {
                            Console.WriteLine("CF OK");
                            if (df != null && bif != null && cf != null)
                            {
                                df.CopyFrameDataToArray(depthFrameData);
                                Console.WriteLine("All Frames are valid");
                                cf.CopyConvertedFrameDataToArray(colorFrameData, ColorImageFormat.Bgra);
                                pointCloud(df, cf, bif);
                               

                            }
                        }
                    }

                }
            }
        }

        private void pointCloud(DepthFrame df, ColorFrame cf, BodyIndexFrame bif)
        {
            
            Console.WriteLine("Creating point Cloud");
            int len = 0;
            StringBuilder pc_ply = new StringBuilder(); // This is for the PLY file
            StringBuilder pc_csv = new StringBuilder(); // This for the CSV file

            FrameDescription depthFrameDes = df.FrameDescription;
            int depthFrameWidth = depthFrameDes.Width;
            int depthFrameHeight = depthFrameDes.Height;

            FrameDescription colorFrameDes = cf.FrameDescription;
            int colorFrameWidth = colorFrameDes.Width;
            int colorFrameHeight = colorFrameDes.Height;

            bif.CopyFrameDataToArray(bodyPoints);

            coordinateMapper.MapDepthFrameToColorSpace(depthFrameData, colorPoints);
            coordinateMapper.MapDepthFrameToCameraSpace(depthFrameData, cameraPoints);

            // loop over each row and column of the depth
            for (int y = 0; y < depthFrameHeight; y++)
            {
                for (int x = 0; x < depthFrameWidth; x++)
                {
                    // calculate index into depth array
                    int depthIndex = (y * depthFrameWidth) + x;

                    byte human = bodyPoints[depthIndex];

                    if (human != 0xff) // Check if pixel is part of human Body
                    {
                        CameraSpacePoint p = this.cameraPoints[depthIndex];

                        // retrieve the depth to color mapping for the current depth pixel
                        ColorSpacePoint colorPoint = this.colorPoints[depthIndex];

                        byte r = 0;
                        byte g = 0;
                        byte b = 0;

                        // make sure the depth pixel maps to a valid point in color space
                        int colorX = (int)Math.Floor(colorPoint.X + 0.5);
                        int colorY = (int)Math.Floor(colorPoint.Y + 0.5);
                        if ((colorX >= 0) && (colorX < colorFrameWidth) && (colorY >= 0) && (colorY < colorFrameHeight))
                        {
                            // calculate index into color array
                            int colorIndex = ((colorY * colorFrameWidth) + colorX) * this.bytesPerPixel;

                            // set source for copy to the color pixel
                            int displayIndex = depthIndex * this.bytesPerPixel;

                            b = this.colorFrameData[colorIndex++];
                            g = this.colorFrameData[colorIndex++];
                            r = this.colorFrameData[colorIndex++];

                        }


                        if (!(Double.IsInfinity(p.X)) && !(Double.IsInfinity(p.Y)) && !(Double.IsInfinity(p.Z)))
                        {
                            if (p.X < 3.0 && p.Y < 3.0 && p.Z < 3.0)
                            {
                                if (NO_COLOUR == true)
                                {
                                    pc_ply.Append(String.Format(CultureInfo.InvariantCulture, "{0} {1} {2}\n", p.X, p.Y, p.Z));
                                    pc_csv.Append(String.Format(CultureInfo.InvariantCulture, "{0},{1},{2}\n", p.X, p.Y, p.Z));
                                }
                                else
                                {
                                    pc_ply.Append(String.Format(CultureInfo.InvariantCulture, "{0} {1} {2} {3} {4} {5}\n", p.X, p.Y, p.Z, r, g, b));
                                }
                               

                                len++;
                            }
                        }
                    }

                   
                }
            }

            String header = "";
            
            if (NO_COLOUR == true)
            {
                header = "ply \n" +
                                  "format ascii 1.0 \n" +
                                  "element vertex " + len + "\n" +
                                  "property float x \n" +
                                  "property float y \n" +
                                  "property float z \n" +   
                                  "end_header \n";
            }
            else
            {
                header = "ply \n" +
                            "format ascii 1.0 \n" +
                            "element vertex " + len + "\n" +
                            "property float x \n" +
                            "property float y \n" +
                            "property float z \n" +
                            "property uchar red \n" +
                            "property uchar green \n" +
                            "property uchar blue \n" +
                            "end_header \n";
            }
            
           
            
            saveFile(header + pc_ply.ToString(), "ply");
            saveFile(pc_csv.ToString(), "txt");
            Environment.Exit(0); //Quit the application -- not really thread safe or a good practice

        }

        private void saveFile(String text, String extension)
        {
            String path = "s:\\pointcloud." + extension;
            System.IO.StreamWriter file = new System.IO.StreamWriter(path);
            file.WriteLine(text);
            file.Close();
            Console.WriteLine("File Saved to" + path);

        }

        public static void Main()
        {
            for (int i = 10; i > 0; i--)
            {
                Thread.Sleep(1000);
                Console.WriteLine (i);
            }
            
            new Program();
        }

    }


}
