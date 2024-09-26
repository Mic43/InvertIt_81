using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Infrastructure.Motion
{
    /// <summary>  
    /// Provides Windows Phone applications mock information about the device’s orientation and motion.  
    /// </summary>  
    public class MockMotionWrapper : MotionWrapper
    {
        /// <summary>  
        /// Use a timer to trigger simulated data updates.  
        /// </summary>  
        private DispatcherTimer timer;

        private MockMotionReading lastCompassReading = new MockMotionReading(true);

        #region Properties
        /// <summary>  
        /// Gets or sets the preferred time between Microsoft.Devices.Sensors.SensorBase<TSensorReading>.CurrentValueChanged events.  
        /// </summary>  
        public override TimeSpan TimeBetweenUpdates
        {
            get
            {
                return timer.Interval;
            }
            set
            {
                timer.Interval = value;
            }
        }
        #endregion

        #region Constructors
        public MockMotionWrapper()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(30);
            timer.Tick += new EventHandler(timer_Tick);
        }
        #endregion

        void timer_Tick(object sender, EventArgs e)
        {
            var reading = new Microsoft.Devices.Sensors.SensorReadingEventArgs<MockMotionReading>();
            lastCompassReading = new MockMotionReading(lastCompassReading);
            reading.SensorReading = lastCompassReading;

            //if (lastCompassReading.HeadingAccuracy > 20)  
            //{  
            //    RaiseValueChangedEvent(this, new CalibrationEventArgs());  
            //}  

            RaiseValueChangedEvent(this, reading);
        }

        /// <summary>  
        /// Starts acquisition of data from the sensor.  
        /// </summary>  
        public override void Start()
        {
            timer.Start();
        }

        /// <summary>  
        /// Stops acquisition of data from the sensor.  
        /// </summary>  
        public override void Stop()
        {
            timer.Stop();
        }

    }  
}
