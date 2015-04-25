using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyScanner
{
    /// <summary>
    /// Class to format a pointCloud to a string representative using a format
    /// </summary>
    public class PointCloudFormatter
    {
        public enum Format {RGB_PLY,PLY,XYZ, VRML};

        private Format format;

        /// <summary>
        /// Constructor for Point Cloud Formatter
        /// </summary>
        /// <param name="format">Format in which the point cloud will be formatted</param>
        public PointCloudFormatter(Format format)
        {
            this.format = format;
        }

        /// <summary>
        /// Returns the Format the pointcloud will get formatted to 
        /// </summary>
        /// <returns> Returns the Format</returns>
        public Format getFormat() { return format;}

        private String getVRMLFooter()
        {
            return"]\n}\n}\n}\n]\n}\n";
        }

        private String getHeader(int numberOfPoints){

            String header = "";
            switch (format)
            {
                case Format.PLY:
                    header = "ply \n" +
                                  "format ascii 1.0 \n" +
                                  "element vertex " + numberOfPoints + "\n" +
                                  "property float x \n" +
                                  "property float y \n" +
                                  "property float z \n" +
                                  "end_header \n";
                    break;
                case Format.RGB_PLY:

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
                    break;

                case Format.VRML:

                    header = "#VRML V2.0 utf8\n" +
                             "#Scan pointcloud\n" +
                                "Transform {\n" +
                                    "children [\n" +
                                        "geometry PointSet {\n" +
                                            "coord Coordinate {\n" +
                                                "point [\n";
                    break;
            }

            return header;

        }

        private String getPointFormat()
        {

            String pointFormat = "";

            switch (format)
            {
                case Format.PLY:
                    pointFormat = "{0} {1} {2}\n";
                    break;
                case Format.RGB_PLY:
                    pointFormat = "{0} {1} {2}\n";
                    break;

                case Format.XYZ:
                    pointFormat = "{0} {1} {2} 0.00 0.00 0.00\n";
                    break;
                
                case Format.VRML:
                    pointFormat = "{1} {0} {2},\n";
                    break;
            }

            return pointFormat;

        }

        /// <summary>
        /// Formats the pointcloud with a given Format
        /// </summary>
        /// <param name="pointCloud">Point Cloud to be formatter</param>
        /// <returns>String representation of the formatted Point Cloud</returns>
        public String formatPointCloud(PointCloud pointCloud)
        {
            String header = getHeader(pointCloud.getSize());

            String points = pointCloud.generateString(getPointFormat());

            String result = header + points;

            if(format == Format.VRML)
            {
                result += getVRMLFooter();
            }

            return result;

        }

        /// <summary>
        /// Static Method to get the Format extension
        /// </summary>
        /// <param name="format">The format of the extenion</param>
        /// <returns>String with the extension (without the dot)</returns>
        public static String getFormatExtension(Format format)
        {
            String extension = "";
            switch (format)
            {
                case Format.PLY:
                    extension = "ply";
                    break;
                case Format.RGB_PLY:
                    extension = "ply";
                    break;
                case Format.XYZ:
                    extension = "xyz";
                    break;
                case Format.VRML:
                    extension = "wrl";
                    break;
            }

            return extension;
        }


    }


}
