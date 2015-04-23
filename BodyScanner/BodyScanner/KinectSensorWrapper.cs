using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyScanner
{
    /*
     * Delegates for Callbacks. They act like a one method interface (in Java), enforcing the parmateres and the method names for callback.
     */
    public delegate void BIFRecievedCallback(BodyIndexFrame bif);
    public delegate void AllFrameRecievedCallback(BodyFrame bf, BodyIndexFrame bif, DepthFrame df);

    /// <summary>
    /// This class essentially wraps the KinectSensor class and simplifies it.
    /// So we dont need to write the same code again.
    /// </summary>
    class KinectSensorWrapper
    {

        /*
         * These events essentially keep track of pointers to functions that need to be called back after a frame has been recieved.
         */
        public event BIFRecievedCallback BIFCallback;
        public event AllFrameRecievedCallback AllFrameCallback;

        KinectSensor kinectSensor;
        MultiSourceFrameReader multiFrameSourceReader;

        public KinectSensorWrapper(){

            this.kinectSensor = KinectSensor.GetDefault();
            this.multiFrameSourceReader = this.kinectSensor.OpenMultiSourceFrameReader(FrameSourceTypes.Depth | FrameSourceTypes.BodyIndex | FrameSourceTypes.Body);
            this.multiFrameSourceReader.MultiSourceFrameArrived += this.multiSourceFrameArrived;

        }

        private void multiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            MultiSourceFrame msf = e.FrameReference.AcquireFrame();

            if (msf != null)
            {
                using (BodyFrame bf = msf.BodyFrameReference.AcquireFrame())
                {
                    using (BodyIndexFrame bif = msf.BodyIndexFrameReference.AcquireFrame())
                    {
                        using (DepthFrame df = msf.DepthFrameReference.AcquireFrame())
                        {
                            if (df != null && bif != null && bf != null)
                            {
                                AllFrameCallback(bf, bif, df); // Pass these on to anyone who is subscribed to this event
                            }
                        }
                    }
                }
            }
        }

        public void startScanning()
        {
            kinectSensor.Open();

            Log.Write("ID: "+ kinectSensor.UniqueKinectId);
            Log.Write(kinectSensor.IsAvailable);
        }

        public void stopScanning()
        {
            kinectSensor.Close();
        }

        public CoordinateMapper getCoordinateMapper()
        {
            return kinectSensor.CoordinateMapper;
        }

        public FrameDescription getDepthFrameDescription()
        {
            return this.kinectSensor.DepthFrameSource.FrameDescription;
        }

        public FrameDescription getBodyIndexFrameDescription()
        {
            return this.kinectSensor.BodyIndexFrameSource.FrameDescription;
        }
    }
}
