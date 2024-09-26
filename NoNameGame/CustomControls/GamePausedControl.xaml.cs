using System.Windows.Controls;
using NoNameGame.Helpers;

namespace NoNameGame.CustomControls
{
    public partial class GamePausedControl : UserControl
    {
        public GamePausedControl()
        {                        
            InitializeComponent();
            Themer.EnableThemesForControls(GoToMenuButton,ResumeButton);
        }        
    }
}
