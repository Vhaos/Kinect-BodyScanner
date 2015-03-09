using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Globalization;

namespace BodyScanner
{
    
     /// <summary>
     /// This class holds the structure for a point cloud
     /// Created By: Jack Roper
     /// </summary>

   
    class PointCloud
     {
        public enum Axis { X, Y, Z };
        private List<Point3D> points = null;
        
        // constructor without pre-built point list
        public PointCloud(): this(new List<Point3D>()) {}
        // constructor with pre-built point list
        public PointCloud(List<Point3D> points)
        {
            this.points = points;
        }

        public List<Point3D> getPoints()
        {
            return points;
        }
        public Point3D getPoint(int index)
        {
            return points[index];
        }
        public int getSize()
        {
            return points.Count;
        }

        public String generateString(String pointFormat)
        {
            StringBuilder output = new StringBuilder();
            foreach (Point3D point in points)
            {
                output.Append(String.Format(CultureInfo.InvariantCulture, pointFormat, point.X, point.Y, point.Z));
            }
            return output.ToString();
        }

        /*
         * Commented the TOString Method and instead replaced it with generateString.  WHY YOU ASK? 
         * Because The rational and the irrational complement each other. Individually they're far less powerful.
         * - Raymond Tusk
         * 
       // ToString() method, outputting each point on a new line
       public override String ToString()
       {
           
       }

       // ToString() using supplied string header
       public String ToString(String header)
       {                
           StringBuilder output = new StringBuilder(header);
           if (output[output.Length - 1] != '\n')
           {
               output.Append('\n');
           }
           foreach (Point3D point in points)
           {
               output.Append(String.Format(CultureInfo.InvariantCulture, "{0} {1} {2}\n", point.X, point.Y, point.Z));
           }
           return output.ToString();
       }
       */

        public void addPoint(Point3D point)
        {
            points.Add(point);
        }

        // method to calculate and return centroid. We calculate this alongside the point cloud build process to reduce redundancy
        public Point3D getCentroid()
        {
            double xAccumulator = 0.0, yAccumulator = 0.0, zAccumulator = 0.0, xMean = 0.0, yMean = 0.0, zMean = 0.0;
            float numberOfPoints = (float) points.Count;
            for (int i = 0; i < numberOfPoints; i++)
            {
                xAccumulator += points[i].X;
                yAccumulator += points[i].Y;
                zAccumulator += points[i].Z;
            }
            xMean = xAccumulator / numberOfPoints;
            yMean = yAccumulator / numberOfPoints;
            zMean = zAccumulator / numberOfPoints;
            return (new Point3D(xMean, yMean, zMean));
        }

        // method that subtracts centroid from point cloud, aligning its center with the origin
        // NOTE: for repeated operations involving centroid, you should get and store the centroid and supply it to this method as an argument
        public void subtractCentroid()
        {
            int numberOfPoints = points.Count;
            Point3D centroid = this.getCentroid();
            for (int i = 0; i < numberOfPoints; i++)
            {
                points[i] = (Point3D)Point3D.Subtract(points[i], centroid);
            }
        }
        public void subtractCentroid(Point3D centroid)
        {
            int numberOfPoints = points.Count;
            for(int i = 0; i < numberOfPoints; i++)
            {
                points[i] = (Point3D)Point3D.Subtract(points[i], centroid);
            }
        }
        public void addCentroid()
        {
            Point3D centroid = this.getCentroid();
            int numberOfPoints = points.Count;
            for (int i = 0; i < numberOfPoints; i++)
            {
                points[i] = Point3D.Add(points[i], (Vector3D)centroid);
            }
        }
        public void addCentroid(Point3D centroid)
        {
            int numberOfPoints = points.Count;
            for (int i = 0; i < numberOfPoints; i++)
            {
                points[i] = Point3D.Add(points[i], (Vector3D)centroid);
            }
        }
        private void applyRotation(Matrix3D rotationMatrix)
        {
            int numberOfPoints = points.Count;
            for(int i = 0; i < numberOfPoints; i++)
            {
                points[i] = rotationMatrix.Transform(points[i]); 
            }
        }
        public void rotatePointCloud(double degrees, Axis axis)
        {
            double theta = (Math.PI / 180) * degrees;
            Matrix3D rotationMatrix;
            switch (axis)
            {
                // X-Axis
                case (Axis.X):
                    rotationMatrix = new Matrix3D(1, 0, 0, 0,
                                                     0, Math.Cos(theta), -(Math.Sin(theta)), 0,
                                                     0, Math.Sin(theta), Math.Cos(theta), 0,
                                                     0, 0, 0, 1);
                    break;
                // Y-Axis
                case (Axis.Y):
                    rotationMatrix = new Matrix3D(Math.Cos(theta), 0, -(Math.Sin(theta)), 0,
                                                     0, 1, 0, 0,
                                                     Math.Sin(theta), 0, Math.Cos(theta), 0,
                                                     0, 0, 0, 1);
                    break;
                // Z-Axis
                case (Axis.Z):
                    rotationMatrix = new Matrix3D(Math.Cos(theta), -(Math.Sin(theta)), 0, 0,
                                                     Math.Sin(theta), Math.Cos(theta), 0, 0,
                                                     0, 0, 1, 0,
                                                     0, 0, 0, 1);
                    break;
                default:
                    rotationMatrix = Matrix3D.Identity;
                    break;
            }
            
            applyRotation(rotationMatrix);
        }

        public void subtractFromPointAxis(double value, Axis axis)
        {
            int numberOfPoints = points.Count;
            switch (axis)
            {
                case (Axis.X):
                    for (int i = 0; i < numberOfPoints; i++)
                    {
                        points[i] = new Point3D(points[i].X - value, points[i].Y, points[i].Z);
                    }
                    break;
                case (Axis.Y):
                    for (int i = 0; i < numberOfPoints; i++)
                    {
                        points[i] = new Point3D(points[i].X, points[i].Y - value, points[i].Z);
                    }
                    break;
                case (Axis.Z):
                    for (int i = 0; i < numberOfPoints; i++)
                    {
                        points[i] = new Point3D(points[i].X, points[i].Y, points[i].Z - value);
                    }
                    break;
                default:
                    Log.Write(Log.Tag.ERROR,"Error with point cloud subtraction");
                    break;
            }
        }

        public void rotateOnSpot(double degrees, Axis axis)
        {
            Point3D centroid = getCentroid();
            subtractCentroid(centroid);
            rotatePointCloud(degrees, axis);
            addCentroid(centroid);
        }
        public void rotateOnSpot(double degrees, Axis axis, Point3D centroid)
        {
            subtractCentroid(centroid);
            rotatePointCloud(degrees, axis);
            addCentroid(centroid);
        }
    }
}


