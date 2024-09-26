using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using AnimationLib;
using AnimationLib.AnimationDSL;
using AnimationLib.AnimationsCreator;
using AnimationLib.AnimationsCreator.Interfaces;
using AnimationLib.AnimationsCreator.MutliAnimation;
using GameLogic.Areas;
using GameLogic.Board;
using Infrastructure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using NoNameGame.BoardPresentation.Animations;
using NoNameGame.BoardPresentation.AreaVisualisation;
using NoNameGame.Configuration;
using NoNameGame.Configuration.Animations.AreaStateTransition;
using NoNameGame.Configuration.Animations.Periodic;
using NoNameGame.Configuration.Animations.Periodic.Interfaces;
using NoNameGame.Configuration.Animations.Reset;
using NoNameGame.Controllers.GameLogic;
using NoNameGame.Controllers.PeriodicAnimations;
using NoNameGame.Controllers.Sound;
using NoNameGame.Models;
using Point = System.Windows.Point;

namespace NoNameGame.BoardPresentation
{
    public class BoardGrid : Grid
    {
        private readonly BoardModel _gameBoard;
        private readonly IAreaStateTransitionsManager _areaStateTransitionsManager;

        private Shape[,] _areaVisualizations;
        
        private readonly BouncingShapeMarker _bouncingShapeMarker;

        #region Periodic animation timer

        private TimeSpan _periodicAnimationInterval = TimeSpan.FromMilliseconds(5000);

        public TimeSpan PeriodicAnimationInterval
        {
            get { return _periodicAnimationInterval; }
            set
            {
                if (value < TimeSpan.Zero) throw new ArgumentOutOfRangeException("value", "Must be positive");
                _periodicAnimationInterval = value;
                _periodicAnimationTimer.Interval = value;
            }
        }

        #endregion

        #region Animations

        private DispatcherTimer _periodicAnimationTimer;

        private IUIElementAnimationCreator _areaTappedAnimationCreator;

        private IEndGameAnimationFactory _endGameAnimationFactory;

        private IMultiUIElementsAnimationCreator _periodicAnimationCreator;

        private readonly IMultiUIElementsAnimationCreator _newGameAnimationCreator;
        private readonly IResetAnimationFactory _resetAnimationFactory;
        private readonly IAreaVisualisationFactory _areaVisualisationFactory;
        private Storyboard _newGameAnimationStoryboard = new Storyboard() ;
        private Storyboard _endGameAnimationStoryboard = new Storyboard();
        private Storyboard _resetAnimationStoryboard = new Storyboard();
        private Storyboard _areaTappedAnimationStoryboard = new Storyboard();

        public IUIElementAnimationCreator AreaTappedAnimationCreator
        {
            get { return _areaTappedAnimationCreator; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                _areaTappedAnimationCreator = value;
            }
        }

        public IEndGameAnimationFactory EndGameAnimationFactory
        {
            get { return _endGameAnimationFactory; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                _endGameAnimationFactory = value;
            }
        }
    //    private Storyboard _periodicAnimationStoryboard;
        public IMultiUIElementsAnimationCreator PeriodicAnimationCreator
        {
            get { return _periodicAnimationCreator; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                _periodicAnimationCreator = value;
              //  _periodicAnimationStoryboard = _periodicAnimationCreator.Create(_areaVisualizations);
            }
        }
        public IMultiUIElementsAnimationCreator NewGameAnimationCreator
        {
            get { return _newGameAnimationCreator; }
        }
        public Shape[,] AreaVisualizations
        {
            get { return _areaVisualizations; }
        }

        #endregion

        public event EventHandler<BoardCoordinate> AreaTapped;
        public event EventHandler EndGameAnimationFinished;
        public event EventHandler NewGameAnimationFinished;
        public event EventHandler<BoardCoordinate> AreaTappedAnimationFinished;

        #region Private methods

        private void SetupPeriodicAnimationTimer()
        {
            _periodicAnimationTimer = new DispatcherTimer {Interval = PeriodicAnimationInterval};
            _periodicAnimationTimer.Tick += PerformTimerPeriodicAnimation;
        }
        private void PerformTimerPeriodicAnimation(object state, EventArgs e)
        {
      //      _periodicAnimationStoryboard.Begin();
      //      PeriodicAnimationCreator.Create(AreaVisualizations).Begin();
        }
        private void CreateContent()
        {
            _areaVisualizations = new Shape[_gameBoard.Size, _gameBoard.Size];
            for (int i = 0; i < _gameBoard.Size; i++)
            {
                RowDefinitions.Add(new RowDefinition());
                ColumnDefinitions.Add(new ColumnDefinition());
            }

            _gameBoard.Areas.ForEach((area, i, j) =>
            {
                var shape = _areaVisualisationFactory.CreateAreaVisualization(area);                
                shape.Tap += OnEllipseTap;
               
                Children.Add(shape);

                SetColumn(shape, i);
                SetRow(shape, j);
                
                AreaVisualizations[i, j] = shape;
            });
        }
   
        private void OnEllipseTap(object sender, GestureEventArgs e)
        {
            var tappedcoords = GetBoardCoordinate((Shape) sender);

            if (_gameBoard.Areas[tappedcoords.X, tappedcoords.Y].AreaState == AreaState.Disabled)
                return;
          
            PerformAreaTappedAnimation(tappedcoords.X, tappedcoords.Y);

            if (AreaTapped != null)
                AreaTapped(this, tappedcoords);
        }
        public BoardCoordinate GetBoardCoordinate(Shape shape)
        {
            return GetAreaFromShape(shape).Coordinates;
        }
        private AreaModel GetAreaFromShape(Shape shape)
        {
            return ((AreaModel) shape.Tag);
        }
        private Shape GetAreaVisualisation(AreaModel area)
        {
            return AreaVisualizations[area.Coordinates.X, area.Coordinates.Y];
        }

        #endregion

        public BoardGrid(BoardModel gameBoard, IAreaStateTransitionsManager areaStateTransitionsManager,
            IMultiUIElementsAnimationCreator newGameAnimationCreator,IResetAnimationFactory resetAnimationFactory,
            IAreaVisualisationFactory areaVisualisationFactory)
        {
            if (gameBoard == null) throw new ArgumentNullException("gameBoard");
            if (areaStateTransitionsManager == null) throw new ArgumentNullException("areaStateTransitionsManager");
            if (newGameAnimationCreator == null) throw new ArgumentNullException("newGameAnimationCreator");
            if (resetAnimationFactory == null) throw new ArgumentNullException("resetAnimationFactory");
            if (areaVisualisationFactory == null) throw new ArgumentNullException("areaVisualisationFactory");

            _newGameAnimationCreator = newGameAnimationCreator;
            _resetAnimationFactory = resetAnimationFactory;
            _areaVisualisationFactory = areaVisualisationFactory;
            _gameBoard = gameBoard;
            _areaStateTransitionsManager = areaStateTransitionsManager;

            EndGameAnimationFactory = new EmptyEndGameAnimationFactory();
            AreaTappedAnimationCreator = new EmptyAnimationCreator();
            PeriodicAnimationCreator = new EmptyMultiAnimationCreator();
            _bouncingShapeMarker = new BouncingShapeMarker(TimeSpan.FromMilliseconds(200));                    

            SetupPeriodicAnimationTimer();
            CreateContent();
           
            PerformNewGameAnimation();           
           
            Unloaded += (sender, args) => PauseAnimations();
            Loaded += (sender, args) => ResumeAnimations();
        }
        ~BoardGrid()
        {
            Debug.WriteLine("GameBoard destructor");
        }
        public void ResumeAnimations()
        {
          //  _periodicAnimationTimer.Start();
            _newGameAnimationStoryboard.Resume();
            _endGameAnimationStoryboard.Resume();
            _resetAnimationStoryboard.Resume();
            _areaTappedAnimationStoryboard.Resume();
        }
        public void PauseAnimations()
        {
          //  _periodicAnimationTimer.Stop();            
            _newGameAnimationStoryboard.Pause();
            _endGameAnimationStoryboard.Pause();
            _resetAnimationStoryboard.Pause();
            _areaTappedAnimationStoryboard.Pause();
        }

        private void PerformNewGameAnimation()
        {
            IsHitTestVisible = false;
            _newGameAnimationStoryboard = NewGameAnimationCreator.Create(AreaVisualizations);

          //  var effects = storyboard.Children.Select(x => _popSoundEffect.CreateInstance()).ToArray();

            for (int i = 0; i < _newGameAnimationStoryboard.Children.Count; i += 2)
            {
                int i1 = i;
                _newGameAnimationStoryboard.Children[i].Completed += (sender, args) => SoundEffectsPlayer.Current.NewGameEffect.Play();
            }
            _newGameAnimationStoryboard.Completed += (sender, args) =>
            {
                IsHitTestVisible = true; 
                if(NewGameAnimationFinished!=null)
                    NewGameAnimationFinished.Invoke(this,EventArgs.Empty);
            };
            _newGameAnimationStoryboard.Begin();
        }
        public void PerformAreaTappedAnimation(int boardCoordX, int boardCoordY)
        {         
            var tappedEllipse = AreaVisualizations[boardCoordX, boardCoordY];
            _areaTappedAnimationStoryboard = _areaTappedAnimationCreator.Create(tappedEllipse);

            var list = AreaVisualizations.GetNeiborghood(boardCoordX, boardCoordY)
                .Where(shape => GetAreaFromShape(shape).AreaState!=AreaState.Disabled)
                .ToList();

            Func<BoardCoordinate, IUIElementAnimationCreator> animationProvider =
                (coord =>
                {
                    int dx = coord.X - boardCoordX;
                    int dy = coord.Y - boardCoordY;
                    double coeff = 0.25*0.5*tappedEllipse.ActualWidth;
                    TimeSpan duration = TimeSpan.FromMilliseconds(Constants.TapAnimationTimeMs);
                    return new SimultanousAnimationsCreator(
                        new SingleAnimationCreator(AnimationsRepository.CreateVerticalTranslationAnimation(dy*coeff,
                            duration, 0, true)),
                        new SingleAnimationCreator(
                            AnimationsRepository.CreateHorizontalTranslationAnimation(dx*coeff,
                                duration, 0, true)));
                });
            _areaTappedAnimationStoryboard.Children.Add(
                new GenericMultiAnimationCreator<BoardCoordinate>(
                    element => GetBoardCoordinate((Shape) element),
                    coord =>
                        SteppingAnimationDelayFuncion.CreateCircular(TimeSpan.FromMilliseconds(1),
                            new Point(boardCoordX, boardCoordY)).ComputeDelay(coord.X, coord.Y), animationProvider)
                    .Create(list));
         //   _areaTappedAnimationStoryboard.Children.Add(sb);
            _areaTappedAnimationStoryboard.Completed +=
                (sender, args) =>
                {
                   // this.Children.Remove(tmpShape);
                    if (AreaTappedAnimationFinished != null) 
                        AreaTappedAnimationFinished.Invoke(this, new BoardCoordinate(boardCoordX,boardCoordY));
                };

            _areaTappedAnimationStoryboard.Begin();                    
        }
        public void PerformResetAnimation()
        {
            _bouncingShapeMarker.UnMark();
            IsHitTestVisible = false;
         //   _periodicAnimationTimer.Stop();
                     
            var resetAnimationCreator = _resetAnimationFactory.CreateAnimationCreator(el => GetBoardCoordinate((Shape)el));
            _resetAnimationStoryboard = resetAnimationCreator.Create(AreaVisualizations.OfType<Shape>().ToList());
            _resetAnimationStoryboard.Completed += (sender, args) =>
            {
                IsHitTestVisible = true;
              //  _periodicAnimationTimer.Start();
            };
              
            _resetAnimationStoryboard.Begin();
        }
        public void PerformEndGameAnimation(BoardCoordinate tappedAreaCoord)
        {
          //  _periodicAnimationTimer.Stop();
            IsHitTestVisible = false;
            _endGameAnimationStoryboard = EndGameAnimationFactory.CreateAnimationCreator(tappedAreaCoord).Create(AreaVisualizations);
            _endGameAnimationStoryboard.Completed += (sender, e) =>
            {
                IsHitTestVisible = true;
                if (EndGameAnimationFinished != null)
                    EndGameAnimationFinished(this, EventArgs.Empty);
            };
            _endGameAnimationStoryboard.Begin();
        }            
        public void Refresh (List<AreaModel> areasToRefresh)
        {
            _bouncingShapeMarker.UnMark();
            areasToRefresh.ForEach(area =>
            {
                var storyboard = _areaStateTransitionsManager.GetAnimationCreatorForArea(area).Create(GetAreaVisualisation(area));
                storyboard.BeginTime = TimeSpan.FromMilliseconds(Constants.TapAnimationTimeMs);
                storyboard.Begin();
                // _gameBoard.Areas[area.Coordinates.X, area.Coordinates.Y].AreaState = area.AreaState;
            });
        }
        public void Refresh()
        {
            Refresh(_gameBoard.Areas.OfType<AreaModel>().ToList());
        }
        public void Mark (BoardCoordinate boardCoordinate)
        {
            _bouncingShapeMarker.TryMark(AreaVisualizations[boardCoordinate.X, boardCoordinate.Y]);
        }
        public bool IsAnyShapeMarked()
        {
            return _bouncingShapeMarker.IsAnyShapeMarked();
        }
    }
}