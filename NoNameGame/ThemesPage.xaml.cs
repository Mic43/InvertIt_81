using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AnimationLib;
using AnimationLib.AnimationDSL;
using AnimationLib.AnimationsCreator;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using NoNameGame.BoardPresentation.Animations;
using NoNameGame.Configuration;
using NoNameGame.Controllers.PeriodicAnimations;
using NoNameGame.Controllers.Sound;
using NoNameGame.CustomControls;
using NoNameGame.CustomControls.OverlayAnimatedBackground;
using NoNameGame.Helpers;
using NoNameGame.Models;

namespace NoNameGame
{
    public partial class ThemesPage : BasePage
    {
        private Random _random = new Random();
        private int shapeSize = Constants.OverlayShapeSize;
        void SetupOverlayBackground()
        {

            Overlay.AnimationCreator =
                                new SingleAnimationCreator(
                                    AnimationBuilder.Translate()
                                        .Vertical()
                                        .WithEasingFunction(new QuadraticEase{ EasingMode = EasingMode.EaseIn})
                                        .To(Overlay.ActualHeight + shapeSize)
                                        .WithDuration(5000)
                                        .Build());


            Overlay.NewShapeAppearanceTime = new NewShapeAppearanceTime(TimeSpan.FromMilliseconds(300),
                TimeSpan.FromMilliseconds(500));
            Overlay.ShapeCreator = CreateShape;
            Overlay.NewShapePosition = RandomizePosition;
        }

        private Shape CreateShape()
        {
            var ellipse = new Ellipse { };
            ellipse.Visibility = Visibility.Collapsed;
            ellipse.Width = ellipse.Height = shapeSize;
            ellipse.CacheMode = new BitmapCache();

            var colors = new Brush[]
            {
                GameResources.Instance.OverlayGradientRed,
                GameResources.Instance.OverlayGradientBlue
            };
            ellipse.Fill = colors[_random.Next(colors.Length)];
            return ellipse;
        }
        private Point RandomizePosition(NewShapePositionArgument newShapePositionArgument)
        {           
            Point point;
            int tries = 0;
            do
            {
                int x = _random.Next(0 + shapeSize, (int)(newShapePositionArgument.MaxLeft - shapeSize));
                //int y = _random.Next(0 + shapeSize, (int)(newShapePositionArgument.MaxTop - shapeSize));
                int y = -shapeSize;
                point = new Point(x, y);
                tries++;
            } while (IsPositionOverlapping(point, newShapePositionArgument.OtherElementsPositions) && tries < 50);
            return point;
        }
        private bool IsPositionOverlapping(Point position, IEnumerable<Point> children)
        {
            return
               children.Any(
                    child =>
                        Math.Abs(child.X - position.X) < shapeSize &&
                        Math.Abs(child.Y - position.Y) < shapeSize);
        }

        public ThemesPage()
        {
            InitializeComponent();
        }
        private void UnlocksPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.CurrentApp().NewItemUnlockedStorer.ClearNewItemUnlocked();

            var themeModels = new ObservableCollection<ThemeModel>(
                this.CurrentApp()
                    .ThemeController.GetAllThemes()
                    .Select(
                        x =>
                            new ThemeModel(x.ThemeType, x.Name,
                                this.CurrentApp().ThemeUnlocker.Read(x.ThemeType).IsLocked,
                                this.CurrentApp().ThemeBrushesProvider.GetThemeMainBrush(x.ThemeType),
                                this.CurrentApp().ThemeBrushesProvider.GetThemeSecondBrush(x.ThemeType),
                                this.CurrentApp().ThemeStarsToUnlockProvider.GetStarsToUnlockCount(x.ThemeType)))
                                                 .OrderBy(x=>x.StarsToUnlock));
            
            SelectThemeControl.SelectThemeModel = new SelectThemeModel(themeModels,
                themeModels.Single(x => x.ThemeType == this.CurrentApp().ThemeController.CurrentTheme.ThemeType));
                   
            StarsSummary.StarsCount = this.CurrentApp().PlayerStatsController.GetStarsProgress().CurrentStarsCount;

            SetupOverlayBackground();          

        }
        private void SelectThemeControl_OnThemeChanged(object sender, ThemeModel newtheme)
        {   
            this.CurrentApp().ThemeController.ChangeTheme(newtheme.ThemeType);                      
        }         
    }
}