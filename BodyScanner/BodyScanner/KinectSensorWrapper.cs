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
    /// <summary>
    /// Callback Delegate for when Body Index Frame is returned by the sensor
    /// </summary>
    /// <param name="bif">The BodyIndexFrame object from the Kinect sensor</param>
    public delegate void BIFRecievedCallback(BodyIndexFrame bif);

    /// <summary>
    /// Callback Delegate for when all frames are returned by the sensor
    /// </summary>
    /// <param name="bf">The BodyFrame object from the Kinect sensor</param>
    /// <param name="bif">The BodyIndexFrame object from the Kinect sensor</param>
    /// <param name="df">The DepthFrame object from the Kinect sensor</param>
    public delegate void AllFrameRecievedCallback(BodyFrame bf, BodyIndexFrame bif, DepthFrame df);

    /// <summary>
    /// This class essentially wraps the KinectSensor class and simplifies it.
    /// reducing the need to dupolicate same code when connecting to sensor
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
            if (kinectSensor == null) { return; }

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

        /// <summary>
        /// Starts the Kinect Sensor
        /// </summary>
        public void startScanning(){
            kinectSensor.Open();
        }

        /// <summary>
        /// Stops the Kinect Sensor
        /// </summary>
        public void stopScanning()
        {
            kinectSensor.Close();
            kinectSensor = null;
        }

        /// <summary>
        /// Gets the Coordinaate Mapper of the Kienct Sensor
        /// </summary>
        /// <returns> CoordinateMapper of the Kinect Sensor</returns>
        public CoordinateMapper getCoordinateMapper()
        {
            return kinectSensor.CoordinateMapper;
        }

        /// <summary>
        /// Gets the Descripion of the Depth Frame
        /// </summary>
        /// <returns>Description of the depth frame </returns>
        public FrameDescription getDepthFrameDescription()
        {
            return this.kinectSensor.DepthFrameSource.FrameDescription;
        }

        /// <summary>
        /// Gets the Descripion of the Body Index Frame
        /// </summary>
        /// <returns>Description of the Body Index Frame</returns>
        public FrameDescription getBodyIndexFrameDescription()
        {
            return this.kinectSensor.BodyIndexFrameSource.FrameDescription;
        }
    }
}
