using System;
using Microsoft.Devices.Sensors;

namespace Infrastructure.Motion
{
    /// <summary>  
    /// Provides Windows Phone applications information about the device’s orientation and motion.  
    /// </summary>  
    public class MotionWrapper //: SensorBase<MotionReading> // No public constructors, nice one.  
    {
        private Microsoft.Devices.Sensors.Motion motion;

        public event EventHandler<SensorReadingEventArgs<MockMotionReading>> CurrentValueChanged;

        #region Properties
        /// <summary>  
        /// Gets or sets the preferred time between Microsoft.Devices.Sensors.SensorBase<TSensorReading>.CurrentValueChanged events.  
        /// </summary>  
        public virtual TimeSpan TimeBetweenUpdates
        {
            get
            {
                return motion.TimeBetweenUpdates;
            }
            set
            {
                motion.TimeBetweenUpdates = value;
            }
        }

        /// <summary>  
        /// Gets or sets whether the device on which the application is running supports the sensors required by the Microsoft.Devices.Sensors.Motion class.  
        /// </summary>  
        public static bool IsSupported
        {
            get
            {
#if(DEBUG)
                return true;
#else  
                    return Motion.MotionWrapper.IsSupported;  
#endif

            }
        }
        #endregion

        #region Constructors
        protected MotionWrapper()
        {
        }

        protected MotionWrapper(Microsoft.Devices.Sensors.Motion motion)
        {
            this.motion = motion;
            this.motion.CurrentValueChanged += motion_CurrentValueChanged;
        }
        #endregion

        /// <summary>  
        /// Get an instance of the MotionWrappper that supports the Motion API  
        /// </summary>  
        /// <returns></returns>  
        public static MotionWrapper Instance()
        {
#if(DEBUG)
            if (!Microsoft.Devices.Sensors.Motion.IsSupported)
            {
                return new MockMotionWrapper();
            }
#endif
            return new MotionWrapper(new Microsoft.Devices.Sensors.Motion());
        }

        /// <summary>  
        /// The value from the underlying Motion API has changed. Relay it on within a MockMotionReading.  
        /// </summary>  
        /// <param name="sender"></param>  
        /// <param name="e"></param>  
        private void motion_CurrentValueChanged(object sender, SensorReadingEventArgs<MotionReading> e)
        {
            var f = new SensorReadingEventArgs<MockMotionReading>();
            f.SensorReading = new MockMotionReading(e.SensorReading);
            RaiseValueChangedEvent(sender, f);
        }

        protected void RaiseValueChangedEvent(object sender, SensorReadingEventArgs<MockMotionReading> e)
        {
            if (CurrentValueChanged != null)
            {
                CurrentValueChanged(this, e);
            }
        }

        /// <summary>  
        /// Starts acquisition of data from the sensor.  
        /// </summary>  
        public virtual void Start()
        {
            motion.Start();
        }

        /// <summary>  
        /// Stops acquisition of data from the sensor.  
        /// </summary>  
        public virtual void Stop()
        {
            motion.Stop();
        }
    }  
}