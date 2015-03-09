using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Windows.Media.Media3D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BodyScanner
{
    static class BodyTracker
    {
        const double tolerance = 0.2, targetDistanceFromCam = 2;
        
        public static bool CorrectPose(BodyFrame bf)
        {
            Body[] bodyArray = new Body[bf.BodyFrameSource.BodyCount];
            bf.GetAndRefreshBodyData(bodyArray);
            Body subject = FindClosestBody(bodyArray);

            if (subject == null) { return false; }
            
            if(!CorrectDistance(subject)) 
            { 
                Log.Write(actualDistanceFromCam + "<----- Should be " + targetDistanceFromCam + " meter(s) away");  
                return false; 
            }

            // Check difference between hand heights relative to arm length && diff between feet height relative to leg length
            if ( CompareJointsAxis(PointCloud.Axis.Y, leftHand, rightHand) < tolerance * forearmLength &&
               CompareJointsAxis(PointCloud.Axis.Y, leftFoot, rightFoot) < tolerance * lowerLegLength)
            {
                return true;
            }
          
            return false;
        }
        
        public static bool DefaultPose(Body subject)
        {
            Joint rightKnee = subject.Joints[JointType.KneeRight]; // currently used to measure arm length
            Joint rightElbow = subject.Joints[JointType.ElbowRight]; // currently used to measure arm length
            Joint rightHand = subject.Joints[JointType.HandRight], leftHand = subject.Joints[JointType.HandLeft];
            Joint rightFoot = subject.Joints[JointType.FootRight], leftFoot = subject.Joints[JointType.FootLeft];
            Joint rightHip = subject.Joints[JointType.HipRight], leftHip = subject.Joints[JointType.HipLeft];
            
            double forearmLength = DistanceBetween(rightElbow, rightHand);
            double lowerLegLength = DistanceBetween(rightKnee, rightFoot);
            
            // Check that hands are below hips &&
            // small diff between hand heights relative to arm length && 
            // small diff between feet height relative to leg length
            
            return( rightHip.Position.Y < rightHand.Position.Y                                                  &&
                    leftHip.Position.Y < leftHand.Position.Y                                                    &&
                    (CompareJointsAxis(PointCloud.Axis.Y, leftHand, rightHand) < tolerance * forearmLength)     &&
                    (CompareJointsAxis(PointCloud.Axis.Y, leftFoot, rightFoot) < tolerance * lowerLegLength)    &&
              );
        }
        
        public static bool CorrectDistance(Body subject)
        {
            return Math.Abs(targetDistanceFromCam - GetDistanceFromCam(subject) > 0.2);
        }
        
        public static double CompareJointsAxis(PointCloud.Axis axis, Joint jointA,  Joint jointB)
        {
            double result = 0;
            switch(axis)
            {
                case (PointCloud.Axis.X):
                    result = Math.Abs(jointA.Position.X - jointB.Position.X); break;
                case (PointCloud.Axis.Y):
                    result = Math.Abs(jointA.Position.Y - jointB.Position.Y); break;
                case (PointCloud.Axis.Z):
                    result = Math.Abs(jointA.Position.Z - jointB.Position.Z); break;
            }
            return result;
        }

        public static Body FindClosestBody(Body[] bodies)
        {
            Body closestBody = null;
            double lowestDist = double.MaxValue;
            foreach (Body b in bodies)
            {
                if (b != null && b.IsTracked)
                {
                    CameraSpacePoint bodyPosition = b.Joints[JointType.SpineBase].Position;

                    double dist = Math.Pow(bodyPosition.X, 2) + Math.Pow(bodyPosition.Y, 2) + Math.Pow(bodyPosition.Z, 2);
                    if (dist < lowestDist)
                    {
                        lowestDist = dist;
                        closestBody = b;
                    }
                }
            }
            return closestBody;
        }

        public static Vector3D GetJointPosition(Body body, JointType joint)
        {
            return new Vector3D(body.Joints[joint].Position.X,
                                body.Joints[joint].Position.Y,
                                body.Joints[joint].Position.Z);
        }

        public static double DistanceBetween(Joint jointA, Joint jointB)
        {
            return Math.Sqrt(   Math.Pow((jointA.Position.X - jointB.Position.X), 2) +
                                Math.Pow((jointA.Position.Y - jointB.Position.Y), 2) +
                                Math.Pow((jointA.Position.Z - jointB.Position.Z), 2)        );
        }
        
        public static GetDistanceFromCam(Body body)
        {
            return body.Joints[JointType.SpineBase].Position.Z;
        }
    }
}
