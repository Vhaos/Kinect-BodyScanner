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
    /// This class contains methods to test the PointCloudFormatter class
    /// </summary>
    [TestClass]
    public class PointCloudFormatterTests
    {
        /// <summary>
        /// This method tests the getFormat() method
        /// </summary>
        [TestMethod]
        public void getFormatTest()
        {
            PointCloudFormatter xyz = new PointCloudFormatter(PointCloudFormatter.Format.XYZ);
            Assert.IsTrue(xyz.getFormat().GetType() == typeof(PointCloudFormatter.Format));
        }

        /// <summary>
        /// This method tests the formatPointCloud() method
        /// </summary>
        [TestMethod]
        public void formatPointCloudTest()
        {
            PointCloud cloud = new PointCloud();
            Point3D point = new Point3D(1, 2, 3);
            cloud.addPoint(point);
            PointCloudFormatter xyz = new PointCloudFormatter(PointCloudFormatter.Format.XYZ);
            String expected = "1 2 3 0.00 0.00 0.00\n";
            String result = xyz.formatPointCloud(cloud);
            Assert.IsTrue(result.Equals(expected));
        }
    }

        
}
