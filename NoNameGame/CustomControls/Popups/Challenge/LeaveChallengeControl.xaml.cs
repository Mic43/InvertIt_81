using System.Windows.Controls;
using NoNameGame.Helpers;

namespace NoNameGame.CustomControls.Popups.Challenge
{
    public partial class LeaveChallengeControl : UserControl
    {
        public LeaveChallengeControl()
        {            
            InitializeComponent();
            Themer.EnableThemesForControls(OkButton,CancelButton);
        }     
    }
}
