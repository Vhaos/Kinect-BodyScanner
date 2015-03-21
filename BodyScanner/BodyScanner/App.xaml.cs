using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BodyScanner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly String POINT_CLOUD_PATH = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); // Desktop
        public static readonly String SCANMEASURE_PATH = "C:\\Program Files (x86)\\Tony Ruto\\Home Scanner Tools\\ScanMeasureCmd.exe";
        public static readonly String GENDER_ARG_MALE = " MKF2";
        public static readonly String GENDER_ARG_FEMALE = " MKF1";
    }
}
