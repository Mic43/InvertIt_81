using System;
using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework;

namespace Infrastructure.Motion
{
    //Microsoft.Devices.Sensors.MotionReading  
    /// <summary>  
    /// Contains information about the orientation and movement of the device.  
    /// </summary>  
    public struct MockMotionReading : Microsoft.Devices.Sensors.ISensorReading
    {
        public static bool RequiresCalibration = false;

        #region Properties
        /// <summary>  
        /// Gets the attitude (yaw, pitch, and roll) of the device, in radians.  
        /// </summary>  
        public MockAttitudeReading Attitude { get; internal set; }

        /// <summary>  
        ///  Gets the linear acceleration of the device, in gravitational units.  
        /// </summary>  
        public Vector3 DeviceAcceleration { get; internal set; }

        /// <summary>  
        /// Gets the rotational velocity of the device, in radians per second.  
        /// </summary>  
        public Vector3 DeviceRotationRate { get; internal set; }

        /// <summary>  
        /// Gets the gravity vector associated with the Microsoft.Devices.Sensors.MotionReading.  
        /// </summary>  
        public Vector3 Gravity { get; internal set; }

        /// <summary>  
        /// Gets a timestamp indicating the time at which the accelerometer reading was  
        ///     taken. This can be used to correlate readings across sensors and provide  
        ///     additional input to algorithms that process raw sensor data.  
        /// </summary>  
        public DateTimeOffset Timestamp { get; internal set; }
        #endregion

        #region Constructors

        /// <summary>  
        /// Initialize an instance from an actual MotionReading  
        /// </summary>  
        /// <param name="cr"></param>  
        public MockMotionReading(MotionReading cr)
            : this()
        {
            this.Attitude = new MockAttitudeReading(cr.Attitude);
            this.DeviceAcceleration = cr.DeviceAcceleration;
            this.DeviceRotationRate = cr.DeviceRotationRate;
            this.Gravity = cr.Gravity;
            this.Timestamp = cr.Timestamp;
        }

        /// <summary>  
        /// Create an instance initialized with testing data  
        /// </summary>  
        /// <param name="test"></param>  
        public MockMotionReading(bool test)
            : this()
        {
            float pitch = 0.01f;
            float roll = 0.02f;
            float yaw = 0.03f;

            this.Attitude = new MockAttitudeReading()
            {
                Pitch = pitch,
                Roll = roll,
                Yaw = yaw,
                RotationMatrix = Matrix.CreateFromYawPitchRoll(yaw, pitch, roll),
                Quaternion = Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll),

                Timestamp = DateTimeOffset.Now
            };

            // TODO: pull data from the Accelerometer  
            this.Gravity = new Vector3(0, 0, 1f);
        }

        /// <summary>  
        /// Create a new mock instance based on the previous mock instance  
        /// </summary>  
        /// <param name="lastCompassReading"></param>  
        public MockMotionReading(MockMotionReading lastCompassReading)
            : this()
        {
            // Adjust the pitch, roll, and yaw as required.  

            // -90 to 90 deg  
            float pitchDegrees = MathHelper.ToDegrees(lastCompassReading.Attitude.Pitch) - 0.5f;
            //pitchDegrees = ((pitchDegrees + 90) % 180) - 90;  

            // -90 to 90 deg  
            float rollDegrees = MathHelper.ToDegrees(lastCompassReading.Attitude.Roll);
            //rollDegrees = ((rollDegrees + 90) % 180) - 90;  

            // 0 to 360 deg  
            float yawDegrees = MathHelper.ToDegrees(lastCompassReading.Attitude.Yaw) + 0.5f;
            //yawDegrees = yawDegrees % 360;  

            float pitch = MathHelper.ToRadians(pitchDegrees);
            float roll = MathHelper.ToRadians(rollDegrees);
            float yaw = MathHelper.ToRadians(yawDegrees);

            this.Attitude = new MockAttitudeReading()
            {
                Pitch = pitch,
                Roll = roll,
                Yaw = yaw,
                RotationMatrix = Matrix.CreateFromYawPitchRoll(yaw, pitch, roll),
                Quaternion = Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll),

                Timestamp = DateTimeOffset.Now
            };

            this.DeviceAcceleration = lastCompassReading.DeviceAcceleration;
            this.DeviceRotationRate = lastCompassReading.DeviceRotationRate;
            this.Gravity = lastCompassReading.Gravity;
            Timestamp = DateTime.Now;

        }
        #endregion



    }  
}