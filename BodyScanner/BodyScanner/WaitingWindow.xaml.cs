using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for WaitingWindow.xaml
    /// </summary>
    public partial class WaitingWindow : Window
    {
        private BackgroundWorker bw = new BackgroundWorker();
        private GenderWindow.GenderType gender_type;

        public WaitingWindow(GenderWindow.GenderType gender_type)
        {
            InitializeComponent();
            this.gender_type = gender_type;
            bw.DoWork += bw_DoWork;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
            bw.RunWorkerAsync();
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            WebRequest webRequest = WebRequest.Create(App.REQUEST_NEW_ID_URL);
            webRequest.Method = "GET";
            WebResponse webResp = webRequest.GetResponse();
            StreamReader sr = new StreamReader(webResp.GetResponseStream());
            e.Result = sanatizeIDResponse(sr.ReadToEnd());
            return;
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Log.Write(e.Result.ToString());

            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }

            ResultWindow rw = new ResultWindow(e.Result.ToString());
            rw.Show();
            this.Hide();

            //Log.Write(e.Result);
        }

        private String sanatizeIDResponse(String response)
        {
            String withoutTags = Regex.Replace(response, "<.*>", string.Empty);
            String withoutQuotes = withoutTags.Replace("\"", string.Empty);
            return withoutQuotes.Trim();
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
/*
String pointCloudPath = FileManager.getPointCloudPath(PointCloudFormatter.Format.XYZ) ;
            String scanMeasureSoftwarePath = App.SCANMEASURE_PATH;

            
             * Arguments:
             * Path for output file
             * MKF{1/2} = female/male
             *
            String arguments = pointCloudPath;
            if (gender_type == GenderWindow.GenderType.Male)
                arguments += App.GENDER_ARG_MALE;

            else if (gender_type == GenderWindow.GenderType.Female)
                arguments += App.GENDER_ARG_FEMALE;

            else // Amend this when we introduce proper error handling
            {
                ErrorWindow ew = new ErrorWindow();
                ew.Show();
                this.Hide();
            }

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
*/