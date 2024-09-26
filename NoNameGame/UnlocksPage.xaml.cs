using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using AnimationLib.AnimationDSL;
using AnimationLib.AnimationsCreator;
using Microsoft.Phone.Controls;
using NoNameGame.Configuration;
using NoNameGame.Controllers.Sound;
using NoNameGame.CustomControls;
using NoNameGame.CustomControls.OverlayAnimatedBackground;
using NoNameGame.Helpers;
using NoNameGame.Models;

namespace NoNameGame
{
    public partial class UnlocksPage : BasePage
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
            do
            {
                int x = _random.Next(0 + shapeSize, (int)(newShapePositionArgument.MaxLeft - shapeSize));
                //int y = _random.Next(0 + shapeSize, (int)(newShapePositionArgument.MaxTop - shapeSize));
                int y = -shapeSize;
                point = new Point(x, y);
            } while (IsPositionOverlapping(point, newShapePositionArgument.OtherElementsPositions));
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

        public UnlocksPage()
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
                                this.CurrentApp().ThemeStarsToUnlockProvider.GetStarsToUnlockCount(x.ThemeType))).OrderBy(x=>x.StarsToUnlock));
            
            SelectThemeControl.SelectThemeModel = new SelectThemeModel(themeModels,
                themeModels.Single(x => x.ThemeType == this.CurrentApp().ThemeController.CurrentTheme.ThemeType));

            var animationDirectionModels = new ObservableCollection<AnimationDirectionModel>(
                this.CurrentApp()
                    .AnimationDirectionController.GetAllDirections()
                    .Select(x=>
                        new AnimationDirectionModel(x.Name, 
                                                   this.CurrentApp().AnimationDirectionUnlocker.Read(x.AnimationDirectionType).IsLocked,
                                                   string.Empty,x.AnimationDirectionType)));

            SelectAnimationDirectionControl.AnimationDirectionModel = new SelectAnimationDirectionModel(
                animationDirectionModels,animationDirectionModels.Single(
                    x => x.AnimationDirectionType == this.CurrentApp().AnimationDirectionController
                                                         .CurrentAnimationDiretion
                                                         .AnimationDirectionType));
            StarsSummary.StarsCount = this.CurrentApp().PlayerStatsController.GetStarsProgress().CurrentStarsCount;

            SetupOverlayBackground();

        }
        private void SelectThemeControl_OnThemeChanged(object sender, ThemeModel newtheme)
        {
            this.CurrentApp().ThemeController.ChangeTheme(newtheme.ThemeType);
            SelectAnimationDirectionControl.ReselectSelectedItem(); // in order to refresh seelcted item with new theme foreground
        }
        private void SelectAnimationDirectionControl_OnAnimationDirectionChanged(object sender, AnimationDirectionModel selectedanimationdirection)
        {
            this.CurrentApp().AnimationDirectionController.ChangeAnimationDirection(selectedanimationdirection.AnimationDirectionType);
        }
        private void Pivot_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            
        }
        private void GestureListener_OnFlick(object sender, FlickGestureEventArgs e)
        {
            SoundEffectsPlayer.Current.SwypeEffect.Play();
        }
    }
}