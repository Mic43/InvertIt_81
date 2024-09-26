using System.Linq;
using System.Windows;
using Infrastructure.Storage;
using NoNameGame.Configuration;
using NoNameGame.Resources;
using Infrastructure;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using NoNameGame.CustomControls.Popups;

namespace NoNameGame.Helpers
{
    internal class PlayEveryDayMessageDisplayer
    {
        private int hintsToReceive;
        private int daysCount;
        private int _showMessageOnNthRun;
        public PlayEveryDayMessageDisplayer(int hintsToReceive, int daysCount, int showMessageOnNthRun)
        {
            this.hintsToReceive = hintsToReceive;
            this.daysCount = daysCount;
            _showMessageOnNthRun = showMessageOnNthRun;
        }
        public void ShowDialogIfNeeded(PhoneApplicationPage page )
        {            
            var currentRunCount = AppSettingsAccessor.GetValue<int>(AppSettingsKeys.RunCount);

            if (currentRunCount.HasValue && currentRunCount.Single() == _showMessageOnNthRun)
            {
                var control = new PlayEveryDayControl(hintsToReceive, daysCount);
                    new PopupWindowService(page,control ,
                        new UIElementWithTappedAction(control.OkButton,(el) => {})).Show();
                //MessageBox.Show(
                //    string.Format(AppResources.StartAppEveryDayMessageDisplayer_Message,
                //        daysCount, hintsToReceive),AppResources.StartAppEveryDayMessageDisplayer_Caption,MessageBoxButton.OK);
            }
        }
    }
}