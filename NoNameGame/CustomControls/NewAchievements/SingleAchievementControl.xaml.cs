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
    public partial class SingleAchievementControl : UserControl
    {
        private SingleAchievementModel _singleAchievementModel;

        public SingleAchievementModel SingleAchievementModel
        {
            get { return _singleAchievementModel; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                DataContext = _singleAchievementModel = value;
            }
        }
        public SingleAchievementControl()
        {
            InitializeComponent();
           // SingleAchievementModel = new SingleAchievementModel("nazwa","opis",true,DateTime.Now,5,0);
        }
    }
}
