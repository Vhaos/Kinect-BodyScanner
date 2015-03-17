using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;

namespace BodyScanner
{
    /// <summary>
    /// Interaction logic for CalculatingWindow.xaml
    /// </summary>
    public partial class CalculatingWindow : Window
    {
        private BackgroundWorker bw = new BackgroundWorker();

        public CalculatingWindow()
        {
            InitializeComponent();
            bw.DoWork += bw_DoWork;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
            bw.RunWorkerAsync();
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            String pointCloudPath = FileManager.getPointCloudPath(PointCloudFormatter.Format.XYZ) ;
            String scanMeasureSoftwarePath = App.SCANMEASURE_PATH;

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

            Process process = Process.Start(startInfo);

            Log.Write("Calculating");
            process.Start();
            process.WaitForExit();
            Log.Write("Finshed");
            process.Dispose();

            String xmlMeasurementsFile = new FileManager(null).getMeasurementsFile();

            XmlDocument xmlDoc = new XmlDocument(); // Create an XML document object
            xmlDoc.LoadXml(xmlMeasurementsFile); // Load the XML document from the specified file

            // Get elements
            XmlNode height = xmlDoc.SelectSingleNode("/MSV_Measures/measure[@name='Height']");
            XmlNode hip = xmlDoc.SelectSingleNode("/MSV_Measures/measure[@name='Hip']");
            XmlNode chest = xmlDoc.SelectSingleNode("/MSV_Measures/measure[@name='Chest | Bust']");
            XmlNode waist = xmlDoc.SelectSingleNode("/MSV_Measures/measure[@name='Waist']");
            XmlNode insideLeg = xmlDoc.SelectSingleNode("/MSV_Measures/measure[@name='Inside Leg']");

            String measurements = "Height: " + height.InnerText + "\n" +
                                  "Hip: " + hip.InnerText + "\n" +
                                  "Chest: " + chest.InnerText + "\n" +
                                  "Waist: " + waist.InnerText + "\n" +
                                  "InsideLeg: " + insideLeg.InnerText;

            Log.Write(Log.Tag.IMP, measurements);

            Log.Write("Finished");


            //e.Result = true;

            return;
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Log.Write("HERE Changing to results window");
            // First, handle the case where an exception was thrown. 
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }

            ResultWindow rw = new ResultWindow();
            rw.Show();
            this.Hide();

            //Log.Write(e.Result);
        }

      

    }
}

/*
 using ()
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
*/