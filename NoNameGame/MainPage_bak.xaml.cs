using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows.ApplicationModel.Store;
using AnimationLib;
using AnimationLib.AnimationsCreator;
using GameLogic;
using GameLogic.Board;
using GameLogic.MovesSequentionGenerators;
using GameLogic.MovesSequentionGenerators.Evaluators;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using NoNameGame.BoardPresentation;
using NoNameGame.BoardPresentation.Animations;
using NoNameGame.BoardPresentation.AreaPainterFactory;
using NoNameGame.Configuration;
using NoNameGame.Controllers;
using NoNameGame.Controllers.GameLogic;
using NoNameGame.CustomControls.OverlayAnimatedBackground;
using NoNameGame.Helpers;
using NoNameGame.Storage;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace NoNameGame
{
    public partial class MainPage : PhoneApplicationPage
    {
        private GameController _controller;
        private Random _random = new Random();
        const int shapeSize = 60;
        public MainPage()
        {
            InitializeComponent();
            _controller = this.CurrentApp().GameController;
            // Overlay.Loaded += InitializeOverlayBackgroundControl;            
        }

        private void  InitializeOverlayBackgroundControl(object sender, RoutedEventArgs routedEventArgs)
        {
//            Overlay.AnimationCreator =
////                new SingleAnimationCreator(AnimationsRepository.CreateVerticalTranslationAnimation(0,
////                    Overlay.ActualHeight,
////                    TimeSpan.FromMilliseconds(5000)));
//                new SingleAnimationCreator(AnimationsRepository.CreateGrowPopAnimation(shapeSize,
//                    TimeSpan.FromMilliseconds(1000), TimeSpan.FromMilliseconds(50)));
//
//            Overlay.NewShapeAppearanceTime = new NewShapeAppearanceTime(TimeSpan.FromMilliseconds(200),
//                                                                          TimeSpan.FromMilliseconds(600));
//            Overlay.ShapeCreator = CreateShape;
//            Overlay.NewShapePosition = RandomizePosition;
        }

        private Shape CreateShape()
        {
            
            var ellipse = new Ellipse { };
            ellipse.Visibility = Visibility.Collapsed;
            ellipse.Width = ellipse.Height = 1;

            var colors = new Brush[]
            {
                GameResources.Instance.OverlayGradientBlue,
                GameResources.Instance.OverlayGradientRed,
            };
            ellipse.Fill = colors[_random.Next(colors.Length)];
            return ellipse;
        }
        private Point RandomizePosition(NewShapePositionArgument newShapePositionArgument)
        {
            Point point;
            do
            {
                int x = _random.Next(0 + shapeSize / 2, (int)(newShapePositionArgument.MaxLeft - shapeSize / 2));
                int y = _random.Next(0 + shapeSize / 2, (int)(newShapePositionArgument.MaxTop - shapeSize / 2));
                //int y = -shapeSize;
                point = new Point(x,y);
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

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {         
//            _controller.StartNewGame(0);            
//            NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
            var movesCount = 5;
            int levelsCount = 50;
            var boardSize = 7;

            var res = new List<LevelStorageData>();
            for (int i = 0; i < levelsCount; i++)
            {
                var evaluatingGenerator = new EvaluatingGenerator(new RandomUniqueGenerator(),
                    new SimpleMovesSequentionEvaluator(LevelGenerator.heuristicFunc1,
                        new LevelGenerator().GetEstimator(1 - i * (1 / (float)levelsCount), movesCount)));

                var hashSet = new HashSet<BoardCoordinate>();
                List<BoardCoordinate> boardCoordinates;
                do
                {
                    boardCoordinates = new List<BoardCoordinate>(
                        evaluatingGenerator.Generate(boardSize, movesCount));

                    hashSet = new HashSet<BoardCoordinate>(boardCoordinates);
                } while (res.Any(x => hashSet.SetEquals(x.MovesList.Select(m=> new BoardCoordinate(m.X,m.Y)))));

                res.Add(new LevelStorageData
                {
                    BoardSize = boardSize,
                    Id = i,
                    IsAvailable = true,
                    MovesList =   boardCoordinates.Select(
                            x => new SingleMoveStorageData {X = x.X, Y = x.Y}).ToList(),
                    OrderNo = i
                });

            }

            using (MemoryStream memStm = new MemoryStream())
            {
                var serializer = new DataContractSerializer(typeof(List<LevelStorageData>));
                serializer.WriteObject(memStm, res);

                memStm.Seek(0, SeekOrigin.Begin);

                using (var streamReader = new StreamReader(memStm))
                {
                    string result = streamReader.ReadToEnd();
                }
            }

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            while (NavigationService.CanGoBack) NavigationService.RemoveBackEntry();
        }

        private void HelpButton_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PanoramaPage1.xaml", UriKind.Relative));
        }
        private void SettingsButton_OnClick(object sender, RoutedEventArgs e)
        {
           // this.CurrentApp().ThemeController.ChangeTheme(ThemesDictionary.Instance["green-orange"]);
        }      
    }
}