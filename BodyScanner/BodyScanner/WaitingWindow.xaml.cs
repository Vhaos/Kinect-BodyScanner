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
        ServerConnect serverConnect = new ServerConnect();
        String requestID = null;
        public WaitingWindow(GenderWindow.GenderType gender_type)
        {
            InitializeComponent();
            this.gender_type = gender_type;

            serverConnect.responseCallback += serverConnect_Callback;
            serverConnect.requestNewID();
        }

        void serverConnect_Callback(Tasks task, string response)
        {

            Log.Write(Log.Tag.INFO, response);
            
            
            
            switch (task)
            {
                case Tasks.NEWID:
                    
                    if (response == null)
                    {
                        ErrorWindow ew = new ErrorWindow();
                        ew.Show();
                        this.Hide();
                        return;
                    }
                    
                    requestID = sanatizeIDResponse(response);
                    ResultWindow rw = new ResultWindow(requestID, gender_type);
                    rw.Show();
                    this.Hide();
                    serverConnect.requestCreateFolder(requestID);
                    break;

                case Tasks.CREATE_FOLDER:
                    serverConnect.requestUploadPointCloudFile(requestID, FileManager.getPointCloudPath(PointCloudFormatter.Format.VRML));
                    break;

                case Tasks.UPLOAD_PC:
                    serverConnect.processPointCloudFile(requestID,gender_type);
                    break;

                case Tasks.PROCESS_PC:
                    Log.Write("Finished");
                    break;

            }
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