using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework;

namespace Infrastructure.Motion
{
    public struct MockAttitudeReading : ISensorReading
    {
        public MockAttitudeReading(AttitudeReading attitudeReading)
            : this()
        {
            Pitch = attitudeReading.Pitch;
            Quaternion = attitudeReading.Quaternion;
            Roll = attitudeReading.Roll;
            RotationMatrix = attitudeReading.RotationMatrix;
            Timestamp = attitudeReading.Timestamp;
            Yaw = attitudeReading.Yaw;
        }

        /// <summary>  
        /// Gets the pitch of the attitude reading in radians.  
        /// </summary>  
        public float Pitch { get; set; }

        /// <summary>  
        /// Gets the quaternion representation of the attitude reading.  
        /// </summary>  
        public Quaternion Quaternion { get; set; }

        /// <summary>  
        /// Gets the roll of the attitude reading in radians.  
        /// </summary>  
        public float Roll { get; set; }

        /// <summary>  
        /// Gets the matrix representation of the attitude reading.  
        /// </summary>  
        public Matrix RotationMatrix { get; set; }

        /// <summary>  
        /// Gets a timestamp indicating the time at which the accelerometer reading was  
        ///     taken. This can be used to correlate readings across sensors and provide  
        ///     additional input to algorithms that process raw sensor data.  
        /// </summary>  
        public DateTimeOffset Timestamp { get; set; }

        /// <summary>  
        /// Gets the yaw of the attitude reading in radians.  
        /// </summary>  
        public float Yaw { get; set; }
    }  
}
