using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BodyScanner
{
    /// <summary>
    /// Interaction logic for CalculatingWindow.xaml
    /// </summary>
    public partial class CalculatingWindow : Window
    {
        public CalculatingWindow()
        {
            InitializeComponent();
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += bw_DoWork;
            bw.RunWorkerAsync();
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            String pointCloudPath = KinectWindow.POINT_CLOUD_PATH + "\\pointcloud.xyz";
            String scanMeasureSoftwarePath = "C:\\Program Files (x86)\\Tony Ruto\\Home Scanner Tools\\ScanMeasureCmd.exe";

            /*
             * Arguments:
             * Path for output file
             * MKF{1/2} = female/male, TODO: ask user for gender
             */
            String arguments = pointCloudPath + " MKF2";

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = scanMeasureSoftwarePath;
            startInfo.Arguments = arguments;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardOutput = true;

            using (Process proc = Process.Start(startInfo))
            {
                Log.Write("Calculating...");

                while (!proc.StandardOutput.EndOfStream)
                {
                    string line = proc.StandardOutput.ReadLine();
                    Log.Write(Log.Tag.INFO, line);
                }


                proc.WaitForExit();
                Log.Write("Finished");

            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }   

    }
}
