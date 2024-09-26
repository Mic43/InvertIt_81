using System.Windows.Controls;
using System.Windows.Media;
using NoNameGame.Helpers;

namespace NoNameGame.CustomControls.Popups
{
    public partial class GamePausedControl : UserControl
    {
        public GamePausedControl()
        {                        
            InitializeComponent();
          
            Themer.EnableThemesForControls(GoToMenuButton,ResumeButton);


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
