using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using NoNameGame.Controllers.DomainEvents;
using NoNameGame.Controllers.DomainEvents.Achievements;

namespace NoNameGame.CustomControls.NewAchievements
{
    public partial class NewAchievementsList : UserControl
    {
        private AchievementsModel _achievementsModel;

        public AchievementsModel AchievementsModel
        {
            get { return _achievementsModel; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                DataContext = _achievementsModel = value;
            }
        }
        public NewAchievementsList()
        {
            InitializeComponent();
        }
    }
}
