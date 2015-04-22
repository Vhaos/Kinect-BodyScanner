
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyScanner
{
    public class FileManager
    {

        PointCloudFormatter pointCloudFormatter;
        private static String pointCloudFileName = "pointcloud";

        public FileManager(PointCloudFormatter pointCloudFormatter)
        {
            if (pointCloudFormatter != null)
            {
                this.pointCloudFormatter = pointCloudFormatter;
            }
            else
            {
                //Log.Write(Log.Tag.INFO, "Point Cloud Formatter is NULL. Creating new with XYZ"); Not helpful if we're using it to get XML document as well
                this.pointCloudFormatter = new PointCloudFormatter(PointCloudFormatter.Format.VRML);
            }
           
        }


        public void savePointCloudFile(PointCloud pcl)
        {

            String path = getPointCloudPath(pointCloudFormatter.getFormat());

            System.IO.StreamWriter file = new System.IO.StreamWriter(path);
            file.WriteLine(pointCloudFormatter.formatPointCloud(pcl));
            file.Close();
            Log.Write(Log.Tag.INFO, "Point Cloud Saved to " + path);

        }

        public String getMeasurementsFile()
        {
            System.IO.StreamReader measurementsFile = new System.IO.StreamReader(getMeasurementsFilePath());
            String file = measurementsFile.ReadToEnd();
            return file;
        }


        public String getMeasurementsFilePath()
        {
            String path = App.POINT_CLOUD_PATH + "\\" + pointCloudFileName + "." + "xml";
            return path;
        }

        public static String getPointCloudPath(PointCloudFormatter.Format format)
        {
            String path = App.POINT_CLOUD_PATH + "\\" + pointCloudFileName + "." + PointCloudFormatter.getFormatExtension(format);
            return path;
        }

    }
}
