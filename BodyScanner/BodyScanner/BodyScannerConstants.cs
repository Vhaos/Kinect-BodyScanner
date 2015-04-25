using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyScanner
{

    /// <summary>
    /// This class contains all constants releavant in BodyScanner
    /// </summary>
    public static class BodyScannerConstants
    {

        public static readonly String POINT_CLOUD_PATH = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); //MyDocuments
        public static readonly String SCANMEASURE_PATH = "C:\\Program Files (x86)\\Tony Ruto\\Home Scanner Tools\\ScanMeasureCmd.exe";
        public static readonly String GENDER_ARG_MALE = "MKF2";
        public static readonly String GENDER_ARG_FEMALE = "MKF1";

    }
}
