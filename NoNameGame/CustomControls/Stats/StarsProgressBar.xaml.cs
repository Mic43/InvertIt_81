using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using NoNameGame.Controllers.PlayerStats;

namespace NoNameGame.CustomControls.Stats
{
    public partial class StarsProgressBar : UserControl
    {
        private StarsProgressModel _starsProgressModel;
        private DoubleAnimation _animation;

        public StarsProgressModel StarsProgressModel
        {
            get { return _starsProgressModel; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                DataContext = _starsProgressModel = value;
             
               // PlayAnimation();
            }
        }
        public void PlayAnimation()
        {
            int toValue = _starsProgressModel.CurrentStarsCount;
            float velocity = 170;
            _animation = new DoubleAnimation()
            {                
                To = toValue,
                From = 0,
                Duration = TimeSpan.FromMilliseconds(500 + toValue / velocity *1000),
            };
            Storyboard.SetTarget(_animation,ProgressBar);
            Storyboard.SetTargetProperty(_animation,new PropertyPath("ProgressBar.Value"));
            var storyboard = new Storyboard();
            storyboard.Children.Add(_animation);
            storyboard.Begin();
        }
        public StarsProgressBar()
        {
            InitializeComponent();
            StarsProgressModel = new StarsProgressModel(0, 0);            
        }
    }
}
