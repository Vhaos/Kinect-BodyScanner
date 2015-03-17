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
    class PointCloudFormatter
    {
        public enum Format {RGB_PLY,PLY,XYZ};

        Format format;

        public PointCloudFormatter(Format format)
        {
            this.format = format;
        }

        public Format getFormat() { return format;}

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

            }

            return pointFormat;

        }


        public String formatPointCloud(PointCloud pointCloud)
        {

            String header = getHeader(pointCloud.getSize());

            String points = pointCloud.generateString(getPointFormat());

            String result = header + points;

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

            }

            return extension;
        }


    }


}
