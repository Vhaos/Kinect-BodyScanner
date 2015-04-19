using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BodyScanner;
using Microsoft.Kinect;
using System.Collections.Generic;
using System.Windows.Media.Media3D;
using System.Diagnostics;

namespace UnitTestProject
{
    /// <summary>
    /// This class contains methods to test the PointCloud class
    /// </summary>
    [TestClass]
    public class PointCloudTests
    {
        /// <summary>
        /// This method tests the getSize() method
        /// </summary>
        [TestMethod]
        public void getSizeTestLoop()
        {
            PointCloud cloud = new PointCloud();
            int i = 0;
            do 
            { 
                cloud.addPoint(new Point3D(i, i, i));
                i++;
            } while (i < 10);
            
            int size = cloud.getSize();

            Assert.IsTrue(size == i);
        }
        [TestMethod]
        public void getSizeTestZero()
        {
            PointCloud cloud = new PointCloud();
            int size = cloud.getSize();
            Assert.IsTrue(size == 0);
        }

        /// <summary>
        /// This method tests the getPoint() method
        /// </summary>
        [TestMethod]
        public void getPointTest()
        {
            List<Point3D> pointList = new List<Point3D>();
            for (int i = 0; i < 5; i++)
            {
                Point3D point = new Point3D(i, i, i);
                pointList.Add(point);
            }

            PointCloud cloud = new PointCloud(pointList);
            for (int i = 0; i < cloud.getSize(); i++)
            {
                Point3D point = cloud.getPoint(i);
                Assert.IsTrue(point.X == i && point.Y == i && point.Z == i);
            }
        }

        /// <summary>
        /// This method tests the addPoint() method
        /// </summary>
        [TestMethod]
        public void addPointTest()
        {
            PointCloud cloud = new PointCloud();
            Point3D point = new Point3D(0, 0, 0);
            cloud.addPoint(point);
            Assert.ReferenceEquals(cloud.getPoint(0), point);
        }

        /// <summary>
        /// This method tests the generateString() method
        /// </summary>
        [TestMethod]
        public void generateStringTest()
        {
            PointCloud cloud = new PointCloud();

            for (int i = 0; i < 10; i++)
            {
                cloud.addPoint(new Point3D(i, i+1, i+2));
            }

            string result = cloud.generateString("{0} {1} {2} 0.00 0.00 0.00\n");
            string[] lines = result.Trim().Split('\n');

            bool matched = true;
            for (int i = 0; i < 10; i++)
            {
                matched = matched && (lines[i].Equals(i + " " + (i + 1) + " " + (i + 2) + " 0.00 0.00 0.00"));
            }
            Assert.IsTrue(matched);
        }

        /// <summary>
        /// This method tests the getCentroid() method
        /// </summary>
        [TestMethod]
        public void getCentroidTest()
        {
            Point3D point1 = new Point3D(0, 0, 0);
            Point3D point2 = new Point3D(100, 100, 100);
            PointCloud cloud = new PointCloud();
            cloud.addPoint(point1); 
            cloud.addPoint(point2);


            Point3D centroid = cloud.getCentroid();
            Assert.IsTrue(centroid.X == 50 && centroid.Y == 50 && centroid.Z == 50);
        }

        /// <summary>
        /// This method tests the subtractCentroid() method
        /// </summary>
        [TestMethod]
        public void subtractCentroidTest()
        {
            Point3D point1 = new Point3D(0, 0, 0);
            Point3D point2 = new Point3D(100, 100, 100);
            PointCloud cloud = new PointCloud();
            cloud.addPoint(point1);
            cloud.addPoint(point2);

            Point3D centroid = cloud.getCentroid();

            cloud.subtractCentroid();

            Assert.IsTrue(     cloud.getPoint(0) == (Point3D) Point3D.Subtract(point1, centroid)
                            && cloud.getPoint(1) == (Point3D) Point3D.Subtract(point2, centroid));
        }

        /// <summary>
        /// This method tests the addCentroid() method
        /// </summary>

        [TestMethod]
        public void addCentroidTest()
        {
            Point3D point1 = new Point3D(0, 0, 0);
            Point3D point2 = new Point3D(100, 100, 100);
            PointCloud cloud = new PointCloud();
            cloud.addPoint(point1);
            cloud.addPoint(point2);

            Point3D centroid = cloud.getCentroid();

            cloud.addCentroid();

            Assert.IsTrue(cloud.getPoint(0) == Point3D.Add(point1, (Vector3D)centroid)
                            && cloud.getPoint(1) == Point3D.Add(point2, (Vector3D)centroid));
        }

        /// <summary>
        /// This method tests the rotatePointCloud() method
        /// </summary>

        [TestMethod]
        public void rotatePointCloudZeroTest()
        {
            Point3D point1 = new Point3D(0, 0, 0);
            Point3D point2 = new Point3D(100, 100, 100);
            PointCloud cloud = new PointCloud();
            cloud.addPoint(point1);
            cloud.addPoint(point2);

            cloud.rotatePointCloud(0, PointCloud.Axis.Y);

            Point3D p1 = cloud.getPoint(0);
            Point3D p2 = cloud.getPoint(1);
            Assert.IsTrue(   p1.X == 0   && p1.Y == 0   && p1.Z == 0
                          && p2.X == 100 && p2.Y == 100 && p2.Z == 100);
        }
		
        /// <summary>
        /// This method tests the subtractFromPointAxis() method
        /// </summary>

        [TestMethod]
        public void subtractFromPointAxisTest()
        {	
			PointCloud cloud = new PointCloud();
			for(int i = 0; i < 10; i++)
			{
				Point3D p = new Point3D(i,i,i);
				cloud.add(p);
			}

			cloud.subtractFromPointAxis(1.23, PointCloud.Axis.X);
			cloud.subtractFromPointAxis(2.34, PointCloud.Axis.Y);
			cloud.subtractFromPointAxis(3.45, PointCloud.Axis.Z);

			bool result = true;
			for(int i = 0; i < 10; i++)
			{
				Point3D point = cloud.getPoint(i);
				result = (point.X == (i-1.23) && point.Y == (i-2.34) && point.Z == (i-3.45));
			}
            Assert.IsTrue(result);
        }
    }
}
