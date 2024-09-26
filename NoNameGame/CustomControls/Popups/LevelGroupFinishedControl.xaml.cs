using System.Windows.Controls;
using NoNameGame.Helpers;

namespace NoNameGame.CustomControls.Popups
{
    public partial class LevelGroupFinishedControl : UserControl
    {
        public LevelGroupFinishedControl()
        {                        
            InitializeComponent();
            Themer.EnableThemesForControls(OkButton);
        }        
    }
}
