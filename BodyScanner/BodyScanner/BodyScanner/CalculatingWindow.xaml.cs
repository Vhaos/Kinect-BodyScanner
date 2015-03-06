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
using System.Xml;

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

            String measurements = "\nheight: " + height.InnerText + "\n " +
                                  "hip: " + hip.InnerText + "\n " +
                                  "chest: " + chest.InnerText + "\n " +
                                  "waist: " + waist.InnerText + "\n " +
                                  "insideLeg: " + insideLeg.InnerText;

            Log.Write(Log.Tag.IMP, measurements);

            Log.Write("Finished");
            Application.Current.Shutdown();
 
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