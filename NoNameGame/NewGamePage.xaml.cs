using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Infrastructure;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using NoNameGame.BoardPresentation;
using NoNameGame.BoardPresentation.Animations;
using NoNameGame.Configuration;
using NoNameGame.CustomControls;
using NoNameGame.CustomControls.ClickSound;
using NoNameGame.CustomControls.Levels;
using NoNameGame.CustomControls.OverlayAnimatedBackground;
using NoNameGame.Helpers;
using NoNameGame.Helpers.OverlayBackground;
using NoNameGame.Models;

namespace NoNameGame
{
    public partial class NewGamePage : BasePage
    {
        private int _levelPackId;
        private bool isNewInstance;

        private readonly PositionRandomizer _positionRandomizer = new PositionRandomizer(Constants.OverlayShapeSize);
        private readonly Random _random = new Random();
        void SetupOverlayBackground()
        {
            //            Overlay.AnimationCreator = new SingleAnimationCreator(AnimationsRepository.CreateGrowPopAnimation(Constants.OverlayShapeSize,
            //                                                                 TimeSpan.FromMilliseconds(1000), TimeSpan.FromMilliseconds(50)));
//            Overlay.AnimationCreator =
//                AnimationsRepository.CreateExplosionAnimationCreator(TimeSpan.FromMilliseconds(300), TimeSpan.FromMilliseconds(800), 10);
//
//            Overlay.NewShapeAppearanceTime = new NewShapeAppearanceTime(TimeSpan.FromMilliseconds(50),
//                                                                         TimeSpan.FromMilliseconds(100));
//            Overlay.ShapeCreator = CreateShape;
//            Overlay.NewShapePosition = _positionRandomizer.RandomizePosition;
        }

        private Shape CreateShape()
        {
            var ellipse = new Ellipse { };
            ellipse.Visibility = Visibility.Visible;
            ellipse.Width = ellipse.Height = 10;//shapeSize;

            var colors = new[]
            {
                GameResources.Instance.OverlayGradientRed,
                GameResources.Instance.OverlayGradientBlue
            };
            ellipse.Fill = colors[_random.Next(colors.Length)];
            return ellipse;
        }

        public NewGamePage()
        {
            InitializeComponent();           
            LevelPackControl.LevelSelected += LevelPackControl_LevelSelected;
          //  LevelPackControl.LevelPackControlModel = this.CurrentApp().LevelsController.GetLevelPackControlModel(_levelPackId);            
          
            Loaded += NewGamePage_Loaded;
            isNewInstance = true;
        }
        ~NewGamePage()
        {
            Debug.WriteLine("NewGamePage destructor");
        }

        void NewGamePage_Loaded(object sender, RoutedEventArgs e)
        {
          //  SetupOverlayBackground();
            // LayoutRoot.Background = new SolidColorBrush(ColorManipulation.LightenColor(GameResources.Instance.CheckedColor, 0.85f));
            //  Debug.WriteLine("Loadtime: " + this.CurrentApp().Stopwatch.ElapsedMilliseconds);
//            this.CurrentApp().Stopwatch.Reset();
        }      
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _levelPackId = int.Parse(NavigationContext.QueryString["LevelPackId"]);
            LevelPackControl.LevelPackControlModel = this.CurrentApp().LevelsController.GetLevelPackControlModel(_levelPackId);
//            if (isNewInstance)
//            {
//                isNewInstance = false;
//                LevelPackControl.RecreatePivot();                
//            }
//            else
//            {
//                LevelPackControl.RecreateCurentPivotItem();
//            }
            //_areaVisualizations = new Shape[_gameBoard.Size, _gameBoard.Size];           
        }       

        void LevelPackControl_LevelSelected(object sender, LevelModel selectedLevelModel)
        {
            this.CurrentApp().GameController.StartNewGame(selectedLevelModel.Id);
            NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
        }
    }
}