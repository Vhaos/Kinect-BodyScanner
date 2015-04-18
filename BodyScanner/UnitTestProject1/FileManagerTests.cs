using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BodyScanner;
using Microsoft.Kinect;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace UnitTestProject
{
    /// <summary>
    /// This class contains methods to test the FileManager Class
    /// </summary>
    [TestClass]
    public class FileManagerTests
    {
        /// <summary>
        /// This method tests the savePointCloud()
        /// </summary>
        [TestMethod]
        public void savePointCloudTest()
        {
            //Creating a Fake Point Cloud           
            Point3D fakePoint = new Point3D(1,2,3);

            PointCloud fakePointCloud = new PointCloud();
            fakePointCloud.addPoint(fakePoint);
           
            // Saving the fake point Cloud
            FileManager fm = new FileManager(null);
            fm.savePointCloudFile(fakePointCloud);

            String expectedResult = "1 2 3 0.00 0.00 0.00";

            //Reading the saved point cloud see if it matches the fake point cloud we created
            String result = System.IO.File.ReadAllText(FileManager.getPointCloudPath(PointCloudFormatter.Format.XYZ));
            Console.WriteLine(result);
            Assert.IsTrue(result.Trim().Equals(expectedResult));
            
        }
    }
}
