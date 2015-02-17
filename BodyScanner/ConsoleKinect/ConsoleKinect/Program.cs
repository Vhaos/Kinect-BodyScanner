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
    using System.Windows.Media.Media3D;
    using Microsoft.Kinect;
    using System.Reflection;
    using System.Text;
    using System.Threading;


    public class Program
    {
        private const double depthLimit = 3.0;
        private const double unitScale = 100.0; // scale from m to cm etc.
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

            /* 
             * used to calculate centroid, as well as lowest x value for later on -> 
             * saves us looping later, though PointCloud methods do allow you to do that
            */
            int numberOfPoints = 0;
            double xAccumulator = 0.0, yAccumulator = 0.0, zAccumulator = 0.0, xMean = 0.0, yMean = 0.0, zMean = 0.0, xMinimum = (1/0.0);

            // LEGACY StringBuilders -> remove once no longer needed
            StringBuilder pc_ply = new StringBuilder(); // This is for the PLY file
            StringBuilder pc_csv = new StringBuilder(); // This is for the CSV file

            FrameDescription depthFrameDes = df.FrameDescription;
            int depthFrameWidth = depthFrameDes.Width;
            int depthFrameHeight = depthFrameDes.Height;

            FrameDescription colorFrameDes = cf.FrameDescription;
            int colorFrameWidth = colorFrameDes.Width;
            int colorFrameHeight = colorFrameDes.Height;

            bif.CopyFrameDataToArray(bodyPoints);

            coordinateMapper.MapDepthFrameToColorSpace(depthFrameData, colorPoints);
            coordinateMapper.MapDepthFrameToCameraSpace(depthFrameData, cameraPoints);

            // Create new point cloud for storing points and operating on later
            PointCloud pointCloud = new PointCloud();

            // loop over each row and column of the depth
            for (int y = 0; y < depthFrameHeight; y++)
            {
                for (int x = 0; x < depthFrameWidth; x++)
                {
                    // calculate index into depth array
                    int depthIndex = (y * depthFrameWidth) + x;

                    byte human = bodyPoints[depthIndex];

                    if (human == 0xff)
                        continue;

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
                        if (p.X < depthLimit && p.Y < depthLimit && p.Z < depthLimit)
                        {
                            Point3D scaledPoint = new Point3D(p.X * unitScale, p.Y * unitScale, p.Z * unitScale);
                            if (NO_COLOUR == true)
                            {
                                // legacy StringBuilder appends -> remove once no longer needed
                                pc_ply.Append(String.Format(CultureInfo.InvariantCulture, "{0} {1} {2}\n", scaledPoint.X, scaledPoint.Y, scaledPoint.Z));
                                pc_csv.Append(String.Format(CultureInfo.InvariantCulture, "{0},{1},{2}\n", scaledPoint.X, scaledPoint.Y, scaledPoint.Z));

                                pointCloud.addPoint(scaledPoint);
                            }
                            else
                            {
                                pc_ply.Append(String.Format(CultureInfo.InvariantCulture, "{0} {1} {2} {3} {4} {5}\n", scaledPoint.X, scaledPoint.Y, scaledPoint.Z, r, g, b));

                                pointCloud.addPoint(scaledPoint);
                            }

                            xAccumulator += scaledPoint.X;
                            yAccumulator += scaledPoint.Y;
                            zAccumulator += scaledPoint.Z;
                            numberOfPoints++;

                            if (scaledPoint.X < xMinimum)
                            {
                                xMinimum = scaledPoint.X;
                            }
                        }
                    }
                } // end of for(int x..) loop over points
            } // end of for(int y..) loop over points

            xMean = xAccumulator / numberOfPoints;
            yMean = yAccumulator / numberOfPoints;
            zMean = zAccumulator / numberOfPoints;

            // centroid calculated on the fly so we don't have to loop again unnecessarily
            Point3D centroid = new Point3D(xMean, yMean, zMean);

            pointCloud.subtractFromPointAxis(xMinimum, 0);
            pointCloud.rotateOnSpot(180, PointCloud.Axis.Y, centroid);
            
            String transformed_pc_ply = pointCloud.ToString();

            // legacy header -> remove once no longer needed and use pointCloud.ToString() which uses default header, or use pointCloud.ToString(header)
            // note that the new PointCloud doesn't support
            String header = "";
            if (NO_COLOUR == true)
            {
                header = "ply \n" +
                                  "format ascii 1.0 \n" +
                                  "element vertex " + numberOfPoints + "\n" +
                                  "property float x \n" +
                                  "property float y \n" +
                                  "property float z \n" +   
                                  "end_header \n";
            }
            else
            {
                header = "ply \n" +
                            "format ascii 1.0 \n" +
                            "element vertex " + numberOfPoints + "\n" +
                            "property float x \n" +
                            "property float y \n" +
                            "property float z \n" +
                            "property uchar red \n" +
                            "property uchar green \n" +
                            "property uchar blue \n" +
                            "end_header \n";
            }
            
            // LEGACY outputs -> remove once no longer necessary
            saveFile(header + pc_ply.ToString(), "ply", "pointcloud");
            saveFile(pc_csv.ToString(), "txt", "pointcloud_csv");

            // new output using PointCloud class
            saveFile(transformed_pc_ply, "ply", "transformed_pointcloud");

            Environment.Exit(0); //Quit the application -- not really thread safe or a good practice
        }

        private void saveFile(String text, String extension, String fileName)
        {
            if (!extension.StartsWith("."))
                extension = "." + extension;

            String path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + fileName + extension;
            System.IO.StreamWriter file = new System.IO.StreamWriter(path);
            file.WriteLine(text);
            file.Close();
            Console.WriteLine("File Saved to " + path);
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
