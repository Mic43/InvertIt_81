using System.Windows.Controls;
using System.Windows.Media;
using NoNameGame.Helpers;
using NoNameGame.Resources;

namespace NoNameGame.CustomControls.Popups
{
    public partial class PlayEveryDayControl : UserControl
    {
        public PlayEveryDayControl(int hintsToReceive,int daysCount)
        {                        
            InitializeComponent();
          
            Themer.EnableThemesForControls(OkButton);

            TextBlock1.Text  = string.Format(AppResources.StartAppEveryDayMessageDisplayer_Message,daysCount, hintsToReceive);                    

//            GoToMenuButton.Foreground = new SolidColorBrush(GameAccentColorProvider.GetDarker());
//            ResumeButton.Foreground = new SolidColorBrush(GameAccentColorProvider.GetDarker());
//            ResumeButton.BorderBrush = new SolidColorBrush(GameAccentColorProvider.GetDarker());
//            GoToMenuButton.BorderBrush = new SolidColorBrush(GameAccentColorProvider.GetDarker());
//
//            GoToMenuButton.Background = new SolidColorBrush(GameAccentColorProvider.GetLighter());
//            ResumeButton.Background = new SolidColorBrush(GameAccentColorProvider.GetLighter());
            
        }        
    }
}
