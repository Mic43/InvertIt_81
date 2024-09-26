using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
//using Microsoft.AdMediator.Core.Models;

namespace NoNameGame.CustomControls
{
    public partial class AdControl : UserControl
    {
        public AdControl()
        {
            InitializeComponent();

          //  AdMediatorControl.AdSdkError += AdMediator_Bottom_AdError;
//            AdMediatorControl.AdMediatorFilled += AdMediator_Bottom_AdFilled;
            //AdMediatorControl.AdMediatorError += AdMediator_Bottom_AdMediatorError;
            //AdMediatorControl.AdSdkEvent += AdMediator_Bottom_AdSdkEvent;

            //AdMediatorControl.AdSdkTimeouts[AdSdkNames.GoogleAdMob] = TimeSpan.FromSeconds(20);
           

        }        
        void AdMediator_Bottom_AdSdkEvent(object sender, Microsoft.AdMediator.Core.Events.AdSdkEventArgs e)
        {
            Debug.WriteLine("AdSdk event {0} by {1}", e.EventName, e.Name);
        }

        void AdMediator_Bottom_AdMediatorError(object sender, Microsoft.AdMediator.Core.Events.AdMediatorFailedEventArgs e)
        {
            Debug.WriteLine("AdMediatorError:" + e.Error + " " + e.ErrorCode);
            // if (e.ErrorCode == AdMediatorErrorCode.NoAdAvailable)
            // AdMediator will not show an ad for this mediation cycle
        }

        void AdMediator_Bottom_AdFilled(object sender, Microsoft.AdMediator.Core.Events.AdSdkEventArgs e)
        {
            Debug.WriteLine("AdFilled:" + e.Name);
        }

        void AdMediator_Bottom_AdError(object sender, Microsoft.AdMediator.Core.Events.AdFailedEventArgs e)
        {
            Debug.WriteLine("AdSdkError by {0} ErrorCode: {1} ErrorDescription: {2} Error: {3}", e.Name, e.ErrorCode, e.ErrorDescription, e.Error);
        }
    }
}
