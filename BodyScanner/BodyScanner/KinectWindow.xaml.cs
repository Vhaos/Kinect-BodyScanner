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
        bool countDownTimerRunning = false;  // Flag to check if the countDownTimer is running so we dont restart it again
        private int secondsCounter; // The variable that keeps the seconds passed

        private const int BytesPerPixel = 4; // needed for bitmap calculation

        private bool captureFrame = false; // Flag to say when the Kinect thread needs to capture the frame

        private WriteableBitmap displayedBitmap = null; // Bitmap that gets displayed (from Kinect)

        private GenderWindow.GenderType gender_type;

        KinectSensorWrapper sensor;

       

        public KinectWindow(GenderWindow.GenderType gender_type)
        {
            InitializeComponent();

            // Initialise gender_type for when constructing CalculatingWindow
            this.gender_type = gender_type;

            //Initialize the Sensor
            sensor = new KinectSensorWrapper();
            sensor.startScanning();
            sensor.AllFrameCallback += sensor_AllFrameCallback; // Subscribe to Kinect Frame Callbacks

            //Initalize the Bitmap
            displayedBitmap = new WriteableBitmap(sensor.getBodyIndexFrameDescription().Width, 
                                                  sensor.getBodyIndexFrameDescription().Height, 
                                                  96.0, 96.0, PixelFormats.Bgr32, null);

            // Setup the timer
            countDownTimer = new DispatcherTimer();
            countDownTimer.Tick += new EventHandler(countDownTimer_Tick);
            countDownTimer.Interval = new TimeSpan(0, 0, 0, 1); // 1 second

           // Update the GUI
            updateControls(State.NO_BODY);
        }


        //Called when stop button clicked
        private void stop_btn_Click(object sender, RoutedEventArgs e)
        {
            currentState = State.SCANNING;
            updateControls(currentState);
        }

        /*Updates the GUI*/
        private void updateControls(State state)
        {

            switch (state)
            {
                case State.NO_BODY:
                    //help_text.Text = (string)Application.Current.FindResource("BODY_NOT_ALIGNED_HELP"); <--- Let the BodyTracker do this
                    seconds_text.Visibility = Visibility.Hidden;
                    wait_text.Visibility = Visibility.Hidden;
                    help_btn.Visibility = Visibility.Visible;
                    stop_btn.Visibility = Visibility.Visible;
                    status_text.Content = (string)Application.Current.FindResource("NO_BODY_DETECTED");
                    status_background.Background = getStateColor(currentState);
                    countDownTimer.Stop();
                    countDownTimerRunning = false;
                    break;

                case State.NOT_ALIGNED:
                    //help_text.Text = (string)Application.Current.FindResource("BODY_NOT_ALIGNED_HELP"); <--- Let the BodyTracker do this
                    seconds_text.Visibility = Visibility.Hidden;
                    wait_text.Visibility = Visibility.Hidden;
                    help_btn.Visibility = Visibility.Visible;
                    stop_btn.Visibility = Visibility.Visible;
                    status_text.Content = (string)Application.Current.FindResource("BODY_NOT_ALIGNED");
                    status_background.Background = getStateColor(currentState);
                    countDownTimer.Stop();
                    countDownTimerRunning = false;
                    break;

                case State.SCANNING:
                    //help_text.Text = (string)Application.Current.FindResource("BODY_SCANNING_HELP"); <--- Let the BodyTracker do this
                    seconds_text.Visibility = Visibility.Visible;
                    wait_text.Visibility = Visibility.Visible;
                    help_btn.Visibility = Visibility.Hidden;
                    stop_btn.Visibility = Visibility.Hidden;
                    status_text.Content = (string)Application.Current.FindResource("SCANNING");
                    status_background.Background = getStateColor(currentState);
                    seconds_text.Text = secondsCounter.ToString();
                    if (!countDownTimerRunning) // This to prevent countDownTImer from starting again
                    {
                        Log.Write("Time is ticking...");
                        secondsCounter = COUNT_DOWN_SECONDS;
                        countDownTimer.Start();
                        countDownTimerRunning = true;
                    }

                    break;

            }

        }


        //This method is invoked everytime the countdowntimer ticks 1 second
        private void countDownTimer_Tick(object sender, EventArgs e)
        {
            secondsCounter--;
            Log.Write(Log.Tag.INFO, secondsCounter);
            seconds_text.Text = secondsCounter.ToString();
            if (secondsCounter <= 0) // Check if the countdown has reached zero; -1 as we want to give the user 1 extra second.
            {
                Log.Write(Log.Tag.INFO, "Seconds counter reached 0");
                countDownTimer.Stop();
                afterCountDownFinish();
            }
        }

        //Executed after countdown reaches 0
        private void afterCountDownFinish()
        {
            captureFrame = true;
        }


        // Called when all farmes are available from the sensor
        void sensor_AllFrameCallback(BodyFrame bf, BodyIndexFrame bif, DepthFrame df)
        {
            BodyTracker Tracker = BodyTracker.UpdateInstance(this, bf); // Gets/creates, as well as updates, the singleton instance of BodyTracker
            
            if (Tracker.CorrectPose()) { currentState = State.SCANNING; }

            else { currentState = State.NOT_ALIGNED; }

            updateDisplayedBitmap(bif);
            updateControls(currentState);
            if (captureFrame == true) // if countdown finished and capture of frame is requested
            {
                capturePointCloud(df, bif);
                captureFrame = false; // Reset Tag 
            }
        }

        //Captures Point Cloud
        private void capturePointCloud(DepthFrame df , BodyIndexFrame bif)
        {

            CoordinateMapper coordinateMapper = sensor.getCoordinateMapper();
            if (df != null && bif != null && coordinateMapper != null)
            {
                //Generate pointCloud using the frames.
                PointCloudGenerator pcg = new PointCloudGenerator(coordinateMapper);
                PointCloud pointCloud = pcg.generate(df, bif);
                Log.Write(Log.Tag.INFO,pointCloud.getSize());
   
                saveFile(pointCloud);
                startNextWindow();
            }
            else
            {
                Log.Write(Log.Tag.ERROR, "There was problem capturing the frames. Arrays containg FrameData or/and the CoordinateMapper are NULL");
                
            }

        }

        private void startNextWindow()
        {
            sensor.stopScanning();
            sensor = null;

            WaitingWindow cw = new WaitingWindow(gender_type);
            cw.Show();

            this.Hide();
        }

        private void saveFile(PointCloud pcl)
        {
            PointCloudFormatter pcf = new PointCloudFormatter(PointCloudFormatter.Format.VRML); // Choosing the VRML (.wrl) format to save the pointCloud
            FileManager fm = new FileManager(pcf);
            fm.savePointCloudFile(pcl);
            
        }

        private void updateDisplayedBitmap(BodyIndexFrame bif)
        {
            using (Microsoft.Kinect.KinectBuffer bodyIndexBuffer = bif.LockImageBuffer())
            {
                //Verify if the frame is of right size - not sure why but recommended in tutorials
                if (((sensor.getBodyIndexFrameDescription().Width * sensor.getBodyIndexFrameDescription().Height) == bodyIndexBuffer.Size) &&
                               (sensor.getBodyIndexFrameDescription().Width == this.displayedBitmap.PixelWidth) &&
                               (sensor.getBodyIndexFrameDescription().Height == this.displayedBitmap.PixelHeight))
                {
                    uint[] pixalData = processBIF(bodyIndexBuffer.UnderlyingBuffer, bodyIndexBuffer.Size);

                    displayedBitmap.WritePixels(
                        new Int32Rect(0, 0, displayedBitmap.PixelWidth, displayedBitmap.PixelHeight), pixalData,
                        this.displayedBitmap.PixelWidth * BytesPerPixel, 0);

                    bitmap_feed.Source = displayedBitmap;
                }
            }

        }

        // unsafe because of pointer use - taken from the demo application
        // Checks for human pixels and produces pixel array for the bitmap
        private unsafe uint[] processBIF(IntPtr bodyIndexFrameData, uint bodyIndexFrameDataSize)
        {

            uint[] bodyIndexPixels = new uint[sensor.getBodyIndexFrameDescription().Width * sensor.getBodyIndexFrameDescription().Height];

            byte* frameData = (byte*)bodyIndexFrameData;

            bool bodyPresent = false; //Check if there is atleast one body

            // convert body index to a visual representation
            for (int i = 0; i < (int)bodyIndexFrameDataSize; ++i)
            {
                
                if (frameData[i] != 0xff)
                {
                    bodyPresent = true;

                    //Currently the colours are hard coded - will be changed later

                    uint color = 0x00000000;
                    if(currentState == State.SCANNING){
                        color = 0x0000FF00;
                    }else{
                        color = 0x00FFFF00;
                    }

                    bodyIndexPixels[i] = color; /// colour the pixel with current state colour
                }
                else
                {
                    // this pixel is not part of a human
                    // display black
                    bodyIndexPixels[i] = 0x00000000;
                }
                
            }
            
            if (!bodyPresent)
            {
                currentState = State.NO_BODY;
            }
            else
            {
                if (currentState != State.SCANNING)
                {
                    currentState = State.NOT_ALIGNED; // Only change the state/color if the current state is not scanning 
                }
            }
           

            //Log.Write((uint)getStateColor(currentState).GetHashCode());
            return bodyIndexPixels;

        }



        private SolidColorBrush getStateColor(State state)
        {
            SolidColorBrush color = Brushes.Red;
            switch (state)
            {
                
                 case State.NO_BODY:
                    //color = 0xc8373700; //Red
                    color = Brushes.Red;
                    break;

                case State.NOT_ALIGNED:
                    //color = 0xffcc0000; //Yellow
                    color = Brushes.Yellow;
                    break;

                case State.SCANNING:
                    //color = 0x8dd35f00; //Green
                    color = Brushes.Green;
                    break;
            
            }

            return color;
        }


    }
}
