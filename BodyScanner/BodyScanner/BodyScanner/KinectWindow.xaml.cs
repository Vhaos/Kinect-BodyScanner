using System;
using System.Collections.Generic;
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
using Microsoft.Kinect;
using System.Windows.Threading;

namespace BodyScanner
{    
    /// <summary>
    /// Interaction logic for KinectWindow.xaml
    /// </summary>
    public partial class KinectWindow : Window
    {
        enum State {NO_BODY,NOT_ALIGNED,SCANNING}; //All the states
        private State currentState = State.NO_BODY; // Default State

        private const int COUNT_DOWN_SECONDS = 3; // 3 seconds count down
        DispatcherTimer countDownTimer; // The timer that notifies every second
        public int secondsCounter; // The variable that keeps the seconds passed

        private bool captureFrame = false; // Flag to say when the Kinect thread needs to capture the frame
        private bool captureFinished = false; // Flag to say when Kinect thread has finished capturing so UI Thread can take over

        KinectSensorWrapper sensor;


        public KinectWindow()
        {
            InitializeComponent();
            updateControls(currentState);
            sensor = new KinectSensorWrapper();
            sensor.startScanning();
            sensor.AllFrameCallback += sensor_AllFrameCallback;
        }


        //Called when stop button clicked
        private void stop_btn_Click(object sender, RoutedEventArgs e)
        {
            updateControls(State.SCANNING);
        }

        /*Updates the GUI*/
        private void updateControls(State state)
        {

            switch (state)
            {
                case State.NO_BODY:
                    help_text.Text = (string)Application.Current.FindResource("BODY_NOT_ALIGNED_HELP");
                    seconds_text.Visibility = Visibility.Hidden;
                    wait_text.Visibility = Visibility.Hidden;
                    help_btn.Visibility = Visibility.Visible;
                    stop_btn.Visibility = Visibility.Visible;
                    status_text.Content = (string)Application.Current.FindResource("NO_BODY_DETECTED");
                    status_background.Background = Brushes.Red;
                    break;

                case State.NOT_ALIGNED:
                    help_text.Text = (string)Application.Current.FindResource("BODY_NOT_ALIGNED_HELP");
                    seconds_text.Visibility = Visibility.Hidden;
                    wait_text.Visibility = Visibility.Hidden;
                    help_btn.Visibility = Visibility.Visible;
                    stop_btn.Visibility = Visibility.Visible;
                    status_text.Content = (string)Application.Current.FindResource("BODY_NOT_ALIGNED");
                    status_background.Background = Brushes.Yellow;
                    break;

                case State.SCANNING:
                    help_text.Text = (string)Application.Current.FindResource("BODY_SCANNING_HELP");
                    seconds_text.Visibility = Visibility.Visible;
                    wait_text.Visibility = Visibility.Visible;
                    help_btn.Visibility = Visibility.Hidden;
                    stop_btn.Visibility = Visibility.Hidden;
                    status_text.Content = (string)Application.Current.FindResource("SCANNING");
                    status_background.Background = Brushes.Green;
                    secondsCounter = COUNT_DOWN_SECONDS;
                    seconds_text.Text = secondsCounter.ToString();
                    startTheTimer();
                    break;

            }

        }

        private void startTheTimer(){
            countDownTimer = new DispatcherTimer();
            countDownTimer.Tick += new EventHandler(countDownTimer_Tick);
            countDownTimer.Interval = new TimeSpan(0,0,0,1); // 1 second
            countDownTimer.Start(); 
        }

        //This method is invoked everytime the countdowntimer ticks 1 second
        private void countDownTimer_Tick(object sender, EventArgs e)
        {
            secondsCounter--;
            seconds_text.Text = secondsCounter.ToString();
            if (secondsCounter <= 0) // Check if the countdown has reached zero; -1 as we want to give the user 1 extra second.
            {
                Log.Write(System.Threading.Thread.CurrentThread.ManagedThreadId);
                countDownTimer.Stop();
                Log.Write(Log.Tag.INFO, "Seconds counter reached 0");
                afterCountDownFinish();
            }
        }

        //Executed after countdown reaches 0
        private void afterCountDownFinish()
        {
            captureFrame = true;
        }


        // Called when all farmes are available from the sensor
        void sensor_AllFrameCallback(BodyIndexFrame bif, DepthFrame df)
        {
             //updateDisplayedBitmap() -- Needs to be implemented

            if (captureFrame == true) // if countdown finished and capture of frame is requested
            {
                capturePointCloud(df, bif);
                captureFrame = false; // Reset Tag 
            }
        }

        //Captures
        private void capturePointCloud(DepthFrame df , BodyIndexFrame bif)
        {
            captureFinished = false; // Reset tag

            CoordinateMapper cm = sensor.getCoordinateMapper();
            if (df != null && bif != null && cm != null)
            {
                PointCloudGenerator pcg = new PointCloudGenerator(cm);
                PointCloud pointCloud = pcg.generate(df, bif);
                Log.Write(Log.Tag.INFO, pcg.getNumberOfPoints());
                saveFile(pointCloud.ToString(PointCloudFormats.getFormatHeader(PointCloudFormats.Format.PLY, pcg.getNumberOfPoints())), "ply", "pointCloud");
            }
            else
            {
                Log.Write(Log.Tag.ERROR, "There was problem capturing the frames. Arrays containg FrameData or/and the CoordinateMapper are null");
            }

        }

        private void saveFile(String text, String extension, String fileName)
        {
            if (!extension.StartsWith("."))
                extension = "." + extension;

            String path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + fileName + extension;
            System.IO.StreamWriter file = new System.IO.StreamWriter(path);
            file.WriteLine(text);
            file.Close();
            Log.Write(Log.Tag.INFO, "File Saved to " + path);
        }


    }
}
