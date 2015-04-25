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
     /// </summary> 
     public class PointCloud
     {
        public enum Axis { X, Y, Z };
        private List<Point3D> points = null;
        
        /// <summary>
        /// Constructor without pre-built point list
        /// </summary>
        public PointCloud(): this(new List<Point3D>()) {}

         
        /// <summary>
        /// Constructor with pre-built point list
        /// </summary>
        /// <param name="points">Points of the point cloud</param>
        public PointCloud(List<Point3D> points)
        {
            this.points = points;
        }

         /// <summary>
         /// Returns all the points in the point cloud
         /// </summary>
         /// <returns>List of all points</returns>
        public List<Point3D> getPoints()
        {
            return points;
        }

         /// <summary>
         /// Gets the point in the point cloud with a linear index
         /// </summary>
         /// <param name="index">The index of the point</param>
         /// <returns>The point with that index</returns>
        public Point3D getPoint(int index)
        {
            return points[index];
        }

         /// <summary>
         /// Returns the number of points in the point cloud
         /// </summary>
         /// <returns>No. of points</returns>
        public int getSize()
        {
            return points.Count;
        }

         /// <summary>
         /// Generates a string representation of the point cloud with a format
         /// </summary>
         /// <param name="pointFormat">The format of the string</param>
         /// <returns>String representation of the point cloud</returns>
        public String generateString(String pointFormat)
        {
            StringBuilder output = new StringBuilder();
            foreach (Point3D point in points)
            {
                output.Append(String.Format(CultureInfo.InvariantCulture, pointFormat, point.X, point.Y, point.Z));
            }
            return output.ToString();
        }

         /// <summary>
         /// Adds a point to the point cloud
         /// </summary>
         /// <param name="point"></param>
        public void addPoint(Point3D point)
        {
            points.Add(point);
        }

        /// <summary>
        /// Method to calculate and return centroid. 
        /// This is calculated during point cloud generation <see cref="PointCloudGenerator"/> process to reduce redundancy
        /// </summary>
        /// <returns>The centroid</returns>
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

   
         /// <summary>
         /// Method that subtracts centroid from point cloud, aligning its center with the origin
         /// NOTE: for repeated operations involving centroid, you should get and store the centroid and supply it to this method as an argument
         /// </summary>
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
         /// <summary>
         /// Applies a rotation matrix to the point cloud
         /// </summary>
         /// <param name="rotationMatrix">The Rotation Matrix to be applied</param>
        private void applyRotation(Matrix3D rotationMatrix)
        {
            int numberOfPoints = points.Count;
            for(int i = 0; i < numberOfPoints; i++)
            {
                points[i] = rotationMatrix.Transform(points[i]); 
            }
        }

         /// <summary>
         /// Rotates the point cloud
         /// </summary>
         /// <param name="degrees">No. of Degrees to be rotated</param>
         /// <param name="axis">The axis in which the rotation will occur</param>
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

         /// <summary>
         /// Subtracts a value from every point in a given axis
         /// </summary>
         /// <param name="value">The value to be subtracted</param>
         /// <param name="axis">The axis to be subtracted from</param>
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


        /// <summary>
        /// Rotates the point cloud on the spot
        /// </summary>
        /// <param name="degrees">No. of degrees to be rotated</param>
        /// <param name="axis">The axis to be rotated in</param>
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


