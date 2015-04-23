using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BodyScanner
{

    public delegate void ServerResponseCallback(Tasks task, String response);
    public enum Tasks { NEWID,CREATE_FOLDER,UPLOAD_PC,PROCESS_PC}

   


    class ServerConnect
    {
        public static readonly String REQUEST_NEW_ID_URL = "http://localhost/server/newID"; //+ id for create folder
        public static readonly String PROCESS_POINTCLOUD_URL = "http://localhost/server/ProcessPC";// + Id, + M/F for process as male/female; JSON with file location for uplaoding

        public event ServerResponseCallback responseCallback;
        

        public void requestNewID(){
            WebRequest webRequest = WebRequest.Create(REQUEST_NEW_ID_URL);
            webRequest.Method = "GET";
            connectToServer(webRequest, Tasks.NEWID);
        }

        public void requestCreateFolder(String requestId)
        {
            WebRequest webRequest = WebRequest.Create(REQUEST_NEW_ID_URL + "/" + requestId);
            webRequest.Method = "POST";
            connectToServer(webRequest, Tasks.CREATE_FOLDER);
        }

        public void requestUploadPointCloudFile(String requestId, String fileLocation)
        {
            WebRequest webRequest = WebRequest.Create(PROCESS_POINTCLOUD_URL + "/" + requestId);
            webRequest.Method = "PUT";

            using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
            {
                string json = "{\"file\": \""+ fileLocation.Replace("\\","\\\\") +"\"}";
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            connectToServer(webRequest, Tasks.UPLOAD_PC);
        }

        public void processPointCloudFile(String requestId, GenderWindow.GenderType gender)
        {

            String genderArgument = (gender == GenderWindow.GenderType.Male) ? "M" : "F";

            WebRequest webRequest = WebRequest.Create(PROCESS_POINTCLOUD_URL + "/" + requestId + "/" + genderArgument);
            webRequest.Method = "GET";
            connectToServer(webRequest, Tasks.PROCESS_PC);
        }

        private void connectToServer(WebRequest webRequest, Tasks currentTask)
        {
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            object[] parameters = {webRequest,currentTask};
            Log.Write("Starting Background Worker with TASK : " + Enum.GetName(typeof(Tasks), currentTask));
            backgroundWorker.RunWorkerAsync(parameters);
        }

        void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            object[] result = e.Result as object[];
            String response = (String)result[0];
            Tasks task = (Tasks)result[1];
            responseCallback(task, response);
        }

        void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] parameters = e.Argument as object[];

            WebRequest webRequest = (WebRequest)parameters[0];
            Tasks task = (Tasks)parameters[1];

            Log.Write("Connecting..." + webRequest.RequestUri);

            try
            {
                WebResponse webResp = webRequest.GetResponse();
                StreamReader sr = new StreamReader(webResp.GetResponseStream());
                String response = sr.ReadToEnd();
                object[] result = {response, task};
                e.Result = result; 
            }
            catch (WebException exception)
            {
                Log.Write(Log.Tag.ERROR, exception.ToString());
                Log.Write(Log.Tag.ERROR, "problem connecting");
                object[] result = { null, task };
                e.Result = result; 
            }
           
        }


    }
}
