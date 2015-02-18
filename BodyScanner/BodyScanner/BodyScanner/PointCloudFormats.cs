using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyScanner
{
    /// <summary>
    /// Static class for all Point cloud Formats
    /// </summary>
    static class PointCloudFormats
    {
        public enum Format {RGB_PLY,PLY};

        public static String getFormatHeader(Format format, int numberOfPoints){

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

    }


}
