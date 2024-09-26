using System;
using Windows.Phone.Devices.Notification;
using Microsoft.Devices;

namespace NoNameGame.Controllers.Vibrator
{
    class ShortPhoneVibrator : PhoneVibrator
    {
        public override void Vibrate()
        {
            VibrateController.Default.Start(TimeSpan.FromMilliseconds(50));
        }
    }
}