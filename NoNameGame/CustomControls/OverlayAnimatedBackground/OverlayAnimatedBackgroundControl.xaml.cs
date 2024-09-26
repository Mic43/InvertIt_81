using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using AnimationLib.AnimationsCreator;
using AnimationLib.AnimationsCreator.Interfaces;
using NoNameGame.BoardPresentation;
using NoNameGame.BoardPresentation.Animations;

namespace NoNameGame.CustomControls.OverlayAnimatedBackground
{
    public partial class OverlayAnimatedBackgroundControl : UserControl
    {
        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private readonly Random _random = new Random();

        private IUIElementAnimationCreator _animationCreator;

        public IUIElementAnimationCreator AnimationCreator
        {
            get { return _animationCreator; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                _animationCreator = value;
            }
        }

        private Func<NewShapePositionArgument, Point> _newShapePosition;
        private NewShapeAppearanceTime _newShapeAppearanceTime;

        public NewShapeAppearanceTime NewShapeAppearanceTime
        {
            get { return _newShapeAppearanceTime; }
            set
            {
                _newShapeAppearanceTime = value;
                UpdateTimerInterval();
            }
        }

        public Func<NewShapePositionArgument, Point> NewShapePosition
        {
            get { return _newShapePosition; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                _newShapePosition = value;
            }
        }

        private Func<Shape> _shapeCreator;
        private Storyboard _animation;

        public Func<Shape> ShapeCreator
        {
            get { return _shapeCreator; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                _shapeCreator = value;
            }
        }

        public OverlayAnimatedBackgroundControl()
        {
            InitializeComponent();

            CreateDefaultSetup();
            SetupTimer();

            Canvas.Loaded += Canvas_Loaded;
            Unloaded += OverlayAnimatedBackgroundControl_Unloaded;
        }

        private void OverlayAnimatedBackgroundControl_Unloaded(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
		    if(_animation!=null)
	            _animation.Stop();
        }

        private void SetupTimer()
        {
            _timer.Interval = TimeSpan.FromMilliseconds(1);
            _timer.Tick -= _timer_Tick;
            _timer.Tick += _timer_Tick;
        }

        private void CreateDefaultSetup()
        {
            NewShapePosition = arg => new Point(0,0);
            AnimationCreator = new EmptyAnimationCreator();
            NewShapeAppearanceTime = new NewShapeAppearanceTime(TimeSpan.FromMilliseconds(500),
                TimeSpan.FromMilliseconds(1000));
            ShapeCreator = () => new Ellipse();
        }

        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {            
            _timer.Start();
        }      

        private void _timer_Tick(object sender, EventArgs e)
        {
            UpdateTimerInterval();

            var shape = ShapeCreator();
            PrepareShapeToShow(shape);

            _animation = AnimationCreator.Create(shape);
            _animation.Begin();
            _animation.Completed += (o, args1) => Canvas.Children.Remove(shape);
        }

        private void UpdateTimerInterval()
        {
            _timer.Interval =
                TimeSpan.FromMilliseconds(_random.Next((int) NewShapeAppearanceTime.Minimum.TotalMilliseconds,
                    (int) NewShapeAppearanceTime.Maximum.TotalMilliseconds));
        }

        private void PrepareShapeToShow(Shape s)
        {
            if (ActualWidth > s.ActualHeight)
            { 
                var position =
                    NewShapePosition(new NewShapePositionArgument(new ReadOnlyCollection<Point>(Canvas.Children.Select(
                        x => x.TransformToVisual(Canvas)
                          .Transform(new Point(0, 0))).ToList()),
                        ActualWidth, ActualHeight));

                Canvas.SetLeft(s, position.X);
                Canvas.SetTop(s, position.Y);

                s.Visibility = Visibility.Visible;

                Canvas.Children.Add(s);
            }
        }
    }
}
