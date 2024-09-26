using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using NoNameGame.Helpers;

namespace NoNameGame.CustomControls
{


    public partial class GameWonControl : UserControl
    {

        public int Points
        {
            get { return (int)GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }        
        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register("Points", typeof(int), typeof(GameWonControl), new PropertyMetadata(0));

        public int BestPoints
        {
            get { return (int)GetValue(BestPointsProperty); }
            set { SetValue(BestPointsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BestPoints.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BestPointsProperty =
            DependencyProperty.Register("BestPoints", typeof(int), typeof(GameWonControl), new PropertyMetadata(0));

        public static readonly DependencyProperty MovesCountProperty = DependencyProperty.Register(
            "MovesCount", typeof(int), typeof(GameWonControl), new PropertyMetadata(default(int)));

        public int MovesCount
        {
            get { return (int)GetValue(MovesCountProperty); }
            set { SetValue(MovesCountProperty, value); }
        }
       
        public GameWonControl()
        {                        
            InitializeComponent();
            Themer.EnableThemesForControls(GoToMenuButton,PlayAgainButton);
        }        
    }
}
