using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Globalization;
 

namespace BodyScanner
{
    class PointCloudGenerator
    {

        CoordinateMapper coordinateMapper;
        private CameraSpacePoint[] cameraPoints = null;

        private readonly int bytesPerPixel = (PixelFormats.Bgr32.BitsPerPixel + 7) / 8;
        private const double depthLimit = 8.0;
        private const double unitScale = 100.0;// scale from m to cm etc.

        private byte[] bodyIndexFrameData = null;
        private ushort[] depthFrameData = null;




        public PointCloudGenerator(CoordinateMapper coordinateMapper)
        {
            this.coordinateMapper = coordinateMapper;

        }

        public PointCloud generate(DepthFrame df, BodyIndexFrame bif)
        {
            Log.Write("Creating point Cloud");

            /* 
             * used to calculate centroid, as well as lowest x value for later on -> 
             * saves us looping later, though PointCloud methods do allow you to do that
             */  
            double xAccumulator = 0.0, yAccumulator = 0.0, zAccumulator = 0.0, xMean = 0.0, yMean = 0.0, zMean = 0.0, xMinimum = (1/0.0);

            int depthFrameWidth = df.FrameDescription.Width;
            int depthFrameHeight = df.FrameDescription.Height;

            this.depthFrameData = new ushort[depthFrameWidth * depthFrameHeight];
            this.bodyIndexFrameData = new byte[depthFrameWidth * depthFrameHeight];
            this.cameraPoints = new CameraSpacePoint[depthFrameWidth * depthFrameHeight];


            df.CopyFrameDataToArray(depthFrameData);
            bif.CopyFrameDataToArray(bodyIndexFrameData);


          

            coordinateMapper.MapDepthFrameToCameraSpace(depthFrameData, cameraPoints);

            // Create new point cloud for storing points and operating on later
            PointCloud pointCloud = new PointCloud();
            int numberOfPoints = 0;

            // loop over each row and column of the depth
            for (int y = 0; y < depthFrameHeight; y++)
            {
                for (int x = 0; x < depthFrameWidth; x++)
                {
                    // calculate index into depth array
                    int depthIndex = (y * depthFrameWidth) + x;

                    byte humanPoint = bodyIndexFrameData[depthIndex];



                    if (humanPoint == 0xff) // Check if human point empty
                    {
                        continue;
                    }
                    
                        

                    CameraSpacePoint p = this.cameraPoints[depthIndex];

                    if (!(Double.IsInfinity(p.X)) && !(Double.IsInfinity(p.Y)) && !(Double.IsInfinity(p.Z)))
                    {
                        if (p.X < depthLimit && p.Y < depthLimit && p.Z < depthLimit)
                        {
                            Point3D scaledPoint = new Point3D(p.X * unitScale, p.Y * unitScale, p.Z * unitScale);
                            
                            pointCloud.addPoint(scaledPoint); 

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

            Log.Write("Finished calculating point cloud");
            
            return pointCloud;

        }

       

     }

}
/*
 * ==============CODE DUMP================
 * 
 * -------------Color Points Stuff---------------
 * 
 * // retrieve the depth to color mapping for the current depth pixel
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
*/