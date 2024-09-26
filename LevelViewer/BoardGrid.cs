using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using AnimationLib.AnimationsCreator;
using AnimationLib.AnimationsCreator.Interfaces;
using AnimationLib.AnimationsCreator.MutliAnimation;
using GameLogic.Areas;
using GameLogic.Board;
using Infrastructure;
namespace LevelViewer
{
    public class BoardGrid : Grid
    {
        private readonly GameBoard _gameBoard;
    //    private readonly AreaStateTransitionsManager _areaStateTransitionsManager;

        private readonly Shape[,] _areaVisualizations;
//
//        private readonly SoundEffect _popSoundEffect;
//        private readonly BouncingShapeMarker _bouncingShapeMarker;

        #region Periodic animation timer

        private TimeSpan _periodicAnimationInterval = TimeSpan.FromMilliseconds(5000);

        public TimeSpan PeriodicAnimationInterval
        {
            get { return _periodicAnimationInterval; }
            set
            {
                if (value < TimeSpan.Zero) throw new ArgumentOutOfRangeException("value", "Must be positive");
                _periodicAnimationInterval = value;
                _animationTimer.Interval = value;
            }
        }

        #endregion

        #region Animations

        private DispatcherTimer _animationTimer;

        private IUIElementAnimationCreator _areaTappedAnimationCreator;

  //      private IEndGameAnimationFactory _endGameAnimationFactory;

        private IMultiUIElementsAnimationCreator _periodicAnimationCreator;

        private readonly IMultiUIElementsAnimationCreator _newGameAnimationCreator;

        public IUIElementAnimationCreator AreaTappedAnimationCreator
        {
            get { return _areaTappedAnimationCreator; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                _areaTappedAnimationCreator = value;
            }
        }

//        public IEndGameAnimationFactory EndGameAnimationFactory
//        {
//            get { return _endGameAnimationFactory; }
//            set
//            {
//                if (value == null) throw new ArgumentNullException("value");
//                _endGameAnimationFactory = value;
//            }
//        }
        public IMultiUIElementsAnimationCreator PeriodicAnimationCreator
        {
            get { return _periodicAnimationCreator; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                _periodicAnimationCreator = value;
            }
        }
        public IMultiUIElementsAnimationCreator NewGameAnimationCreator
        {
            get { return _newGameAnimationCreator; }
        }

        #endregion

        public event EventHandler<BoardCoordinate> AreaTapped;
        public event EventHandler EndGameAnimationFinished;

        #region Private methods

        private void SetupPeriodicAnimationTimer()
        {
            _animationTimer = new DispatcherTimer {Interval = PeriodicAnimationInterval};
            _animationTimer.Tick += PerformTimerAnimation;
            _animationTimer.Start();
        }
        private void PerformTimerAnimation(object state, EventArgs e)
        {
           // PeriodicAnimationCreator.Create(_areaVisualizations).Begin();
        }
        private void CreateContent()
        {
            for (int i = 0; i < _gameBoard.Size; i++)
            {
                RowDefinitions.Add(new RowDefinition());
                ColumnDefinitions.Add(new ColumnDefinition());
            }

            _gameBoard.ForEachArea((area, i, j) =>
            {
                var shape = CreateAreaShape(i, j);

                var cell = CreateGridCell();
                cell.Children.Add(shape);
                Children.Add(cell);

                SetColumn(cell, i);
                SetRow(cell, j);

                // _painters.Add(_painterFactory.Create(area, shape));
                _areaVisualizations[i, j] = shape;
            });
        }
        private Grid CreateGridCell()
        {
            var grid = new Grid
            {
                Margin = new Thickness(1),
                //Background = new SolidColorBrush() {Color = Color.FromArgb(40, 0, 0, 0)},
            };
            return grid;
        }
        private Shape CreateAreaShape(int coordX, int coordY)
        {
            Area area = _gameBoard.AreaMatrix.Areas[coordX, coordY];
            var ellipse = new Ellipse {Margin = new Thickness(1)};
            ellipse.MouseDown += (sender, args) => OnEllipseTap(sender);
            ellipse.StrokeThickness = 0;
            ellipse.Tag = area;
            ellipse.StrokeThickness = 0;


            ellipse.Fill = GetBrushForAreaState(area);
     

            //new GradientFillAreaStatePainter(color,new RadialGradientBrush() {GradientStops = new GradientStopCollection() { new GradientStop() {Color = color}}}).Paint(ellipse);         
            return ellipse;
        }
        private Brush GetBrushForAreaState(Area area)
        {
            SolidColorBrush brush = null;
            switch (area.AreaState)
            {
                case AreaState.Checked:
                    brush = new SolidColorBrush(Colors.Green);
                    break;
                case AreaState.UnChecked:
                    brush = new SolidColorBrush(Colors.Gold);
                    break;
                case AreaState.Disabled:
                    brush = new SolidColorBrush(Colors.Gray);
                    break;
            }
            return brush;
        }

        private void OnEllipseTap(object sender)
        {
            var tappedcoords = GetBoardCoords((Ellipse) sender);

            PerformAreaTappedAnimation(tappedcoords.Item1, tappedcoords.Item2);

           // Refresh();
            if (AreaTapped != null)
                AreaTapped(this, new BoardCoordinate(tappedcoords.Item1, tappedcoords.Item2));
        }

        private Tuple<int, int> GetBoardCoords(Shape shape)
        {
            int coordX = 0, coordY = 0;
            _areaVisualizations.ForEach((s, i, j) =>
            {
                if (s == shape)
                {
                    coordX = i;
                    coordY = j;
                }
            });
            return new Tuple<int, int>(coordX, coordY);
        }
        private Shape GetAreaVisualisation(Area area)
        {
            return _areaVisualizations[area.BoardCoordinate.X, area.BoardCoordinate.Y];
        }

        #endregion

        public BoardGrid(GameBoard gameBoard)
            : this(gameBoard, new EmptyMultiAnimationCreator())
        {
        }

        public BoardGrid(GameBoard gameBoard,
            IMultiUIElementsAnimationCreator newGameAnimationCreator)
        {
            if (gameBoard == null) throw new ArgumentNullException("gameBoard");
          
            if (newGameAnimationCreator == null) throw new ArgumentNullException("newGameAnimationCreator");

            _newGameAnimationCreator = newGameAnimationCreator;
            _gameBoard = gameBoard;
          

//            EndGameAnimationFactory = new EmptyEndGameAnimationFactory();
//            AreaTappedAnimationCreator = new EmptyAnimationCreator();
//            PeriodicAnimationCreator = new EmptyMultiAnimationCreator();
//            _bouncingShapeMarker = new BouncingShapeMarker(TimeSpan.FromMilliseconds(200), _popSoundEffect);

            _areaVisualizations = new Shape[gameBoard.Size, gameBoard.Size];

            // In order to remove sound lag
//            _popSoundEffect = SoundEffect.FromStream(TitleContainer.OpenStream(@"Sounds\pop.wav"));
//            _popSoundEffect.Play(0.0f,1.0f,1.0f);

            SetupPeriodicAnimationTimer();
            CreateContent();
          
            Loaded+=(sender, args) => PerformNewGameAnimation();
        }

        
        private void PerformNewGameAnimation()
        {
           
        }
        public void PerformAreaTappedAnimation(int boardCoordX, int boardCoordY)
        {
                      
        }
        public void PerformResetAnimation()
        {
           
        }
        public void PerformEndGameAnimation(BoardCoordinate tappedAreaCoord)
        {
          
        }
        public void PausePeriodicAnimation()
        {
            _animationTimer.Stop();
        }
        public void ResumePeriodicAnimation()
        {
            _animationTimer.Start();
        }

        public void Refresh(List<Area> areasToRefresh)
        {
            areasToRefresh.ForEach(area =>
            {
                if (_gameBoard.Level.WinningMovesSequention.Contains(area.BoardCoordinate))
                    GetAreaVisualisation(area).Fill = new SolidColorBrush(Colors.LightGreen);
                else
                {

                    GetAreaVisualisation(area).Fill = GetBrushForAreaState(area);
                }
            });

        }
        public void Refresh()
        {
            Refresh(_gameBoard.AreaMatrix.Areas.OfType<Area>().ToList());
        }

        public void Mark(BoardCoordinate boardCoordinate)
        {
          //  _bouncingShapeMarker.Mark(_areaVisualizations[boardCoordinate.X, boardCoordinate.Y]);
        }
    }
}