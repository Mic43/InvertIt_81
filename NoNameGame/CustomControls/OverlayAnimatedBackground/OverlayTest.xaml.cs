using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using AnimationLib;
using AnimationLib.AnimationDSL;
using AnimationLib.AnimationsCreator;
using AnimationLib.AnimationsCreator.MutliAnimation;
using Microsoft.Expression.Interactivity.Media;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using NoNameGame.BoardPresentation.Animations;
using NoNameGame.BoardPresentation.Animations.CircularTransaltion;
using NoNameGame.Configuration;
using NoNameGame.CustomControls.AttachedProperties;

namespace NoNameGame.CustomControls.OverlayAnimatedBackground
{

//    public class Randomizer
//    {
//        private readonly int _elementsCountX;
//        private readonly int _planeWidth;
//        private readonly int _planeHeight;
//        private readonly int _elementSize;
//        private readonly int _elementsCountY;
//
//        private double _areaHeight;
//        private double _areaWidth;
//
//        private List<Point>_areaCenterCache;
//        private readonly Random _random;
//        public Randomizer(int elementSize, int elementsCountX, int elementsCountY,int planeWidth, int planeHeight)
//        {
//            if (elementsCountX <=0)
//                throw new ArgumentOutOfRangeException("elementsCountX");
//            if (elementsCountY <= 0)
//                throw new ArgumentOutOfRangeException("elementsCountX");
//            if (elementSize <= 0)
//                throw new ArgumentOutOfRangeException("elementSize");
//            if (planeWidth <= 0)
//                throw new ArgumentOutOfRangeException("planeWidth");
//            if (planeHeight <= 0)
//                throw new ArgumentOutOfRangeException("planeHeight");
//
//            _elementsCountX = elementsCountX;
//            _planeWidth = planeWidth;
//            _planeHeight = planeHeight;
//            _elementSize = elementSize;
//            _elementsCountY = elementsCountY;
//
//            PrepareAreas();
//            _random = new Random();
//        }
//        private void PrepareAreas()
//        {
//            _areaHeight = _planeHeight / (double)_elementsCountY;
//            _areaWidth = _planeWidth / (double)_elementsCountX;
//
//            if (_areaWidth < _elementSize || _areaHeight < _elementSize)
//                throw new InvalidOperationException("There are to many shapes of this size to fit in provided plane");
//
//            int areasCountY = _elementsCountX;
//            int areasCountX = _elementsCountY;
//
//            _areaCenterCache = new List<Point>(_elementsCountX);
//            for (int i = 0; i < areasCountX; i++)
//            {
//                for (int j = 0; j < areasCountY; j++)
//                {
//                    _areaCenterCache.Add(GetAreaCenter(i,j));
//                }
//            }
//        }
//        private Point GetAreaCenter(int xIndex, int yIndex)
//        {
//            return new Point(xIndex*_areaWidth + _areaWidth/2, yIndex*_areaHeight + _areaHeight/2);
//        }
//        public List<Point> Get()
//        {
//            int availableSpaceY = (int)((_areaHeight - _elementSize)/2.0);
//            int availableSpaceX = (int)((_areaWidth - _elementSize) / 2.0);
//
//            return _areaCenterCache.Select(center => new Point(center.X + _random.Next(availableSpaceX),
//                                                               center.Y + _random.Next(availableSpaceY)))
//                                    .ToList();
//        }
//    }
    public partial class OverlayTest : UserControl
    {      
        private Random _random = new Random();
        private const int shapeSize = Constants.OverlayShapeSize;
        private GenericMultiAnimationCreator<Point> _moveShapesAnimationCreator;
        private readonly DispatcherTimer _moveShapesAnimationTimer = new DispatcherTimer() {Interval = TimeSpan.FromMilliseconds(500)};
        private Storyboard _moveShapesStoryboard;
        private readonly TimeSpan _automaticTransaltionDuration = TimeSpan.FromMilliseconds(800);
        private const int ShapesCount = 40;
        private Storyboard _fadeAnimation;    
        public bool IsLoaded { get; set; }

        private static List<Point> _randomizedPositions;
        private Ellipse _explosionEllipse;

        static OverlayTest ()
        {
            CreateRandomizedPositions();
        }
        private static void CreateRandomizedPositions()
        {
            _randomizedPositions = new List<Point>(ShapesCount);

            Point point;
            Random random = new Random();

            for (int i = 0; i < ShapesCount; i++)
            {
                int k = 0;
                do
                {
                    int x = random.Next(0 + shapeSize/2, (int) Application.Current.Host.Content.ActualWidth - shapeSize/2);
                    int y = random.Next(0 + shapeSize/2, (int) (Application.Current.Host.Content.ActualHeight - shapeSize/2));
                    point = new Point(x, y);
                    k++;
                } while (IsPositionOverlapping(point, _randomizedPositions) && k <= 100);
                _randomizedPositions.Add(point);
            }
        }
        private static bool IsPositionOverlapping(Point position, IEnumerable<Point> children)
        {
            return
               children.Any(
                    child =>
                        Math.Abs(child.X - position.X) < shapeSize &&
                        Math.Abs(child.Y - position.Y) < shapeSize);
        }
        public OverlayTest()
        {          
            InitializeComponent();
            CreateShapes();
            CreateFadeAnimation();
            SetupMoveAnimationTimer();           

            Canvas.Loaded += OverlayTEst_Loaded;
            Canvas.Unloaded+=CanvasOnUnloaded;
        }
        private void SetupMoveAnimationTimer()
        {
            _moveShapesAnimationTimer.Tick -= MoveShapesAnimationTimerOnTick;
            _moveShapesAnimationTimer.Tick += (MoveShapesAnimationTimerOnTick);
        }
        private void MoveShapesAnimationTimerOnTick(object s, EventArgs args)
        {
            if (_moveShapesStoryboard != null && IsLoaded && _moveShapesStoryboard.GetCurrentState() != ClockState.Active && _fadeAnimation.GetCurrentState() == ClockState.Filling)
            {
                PlayMoveAnimation(new Point(_random.Next((int) ActualWidth), _random.Next((int) ActualHeight)), _automaticTransaltionDuration);
            }
        }
        private void CreateFadeAnimation()
        {        
            _fadeAnimation = new GenericMultiAnimationCreator<int>(element =>(int)((Shape)element).Tag,
                i => TimeSpan.FromMilliseconds(20*i).Add(TimeSpan.FromMilliseconds(500)),
                i =>
                    new SingleAnimationCreator(AnimationsRepository.CreateFadeToViewAnimation(1.0,
                        TimeSpan.FromMilliseconds(200)))).Create(Canvas.Children);
            _fadeAnimation.Completed += (sender, args) => _moveShapesAnimationTimer.Start();
        }
        private void CreateShapes()
        {
            var shapes = Enumerable.Range(0, ShapesCount).Select(CreateShape).ToList();
            shapes.ForEach(Canvas.Children.Add);                
        }
        private Shape CreateShape(int index)
        {
            var ellipse = new Ellipse { };
            ellipse.Visibility = Visibility.Visible;
            ellipse.Opacity = 0;
            ellipse.Tag = index;
            ellipse.Width = ellipse.Height = shapeSize;

            var propertyPaths = new PropertyPath[] { new PropertyPath("CheckedAreaGradientBrush"), new PropertyPath("UnCheckedAreaGradientBrush") };
            var binding = new Binding()
            {
                Source = GameResources.Instance,
                Path = propertyPaths[_random.Next(propertyPaths.Length)]
            };
            ellipse.SetBinding(Shape.FillProperty, binding);
            ellipse.CacheMode = new BitmapCache();
            return ellipse;
        }
        private void CanvasOnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _moveShapesAnimationTimer.Stop();
            _moveShapesStoryboard.Stop();       
        }
        void OverlayTEst_Loaded(object sender, RoutedEventArgs e)
        {
            RecreateShapes();
            IsLoaded = true;
        }        
        private void SyncPositions()
        {
            var enumerable = (from child in GetMovableShapes()              
                where child.RenderTransform is TranslateTransform
                let c = child
                select                 
                    new
                    {
                        element = c,                                            
                        tranX = ((TranslateTransform) c.RenderTransform).X,
                        tranY = ((TranslateTransform) c.RenderTransform).Y
                    });

            enumerable.ToList().ForEach(x =>
            {            
                Canvas.SetLeft(x.element, Canvas.GetLeft(x.element) + x.tranX);
                Canvas.SetTop(x.element, Canvas.GetTop(x.element) + x.tranY);
            });
        }
        public void PlayMoveAnimation(Point center, TimeSpan duration)
        {
          //  PlayExplosionAnim(center);

            if (_moveShapesStoryboard != null)
            {
                SyncPositions();
                _moveShapesStoryboard.Stop();         
            }
            CreateMoveStoryboard(center, duration);
            _moveShapesStoryboard.Begin();
        }
//        private void PlayExplosionAnim(Point center)
//        {
//            _explosionEllipse = new Ellipse() {Width = 20, Height = 20, Fill = new SolidColorBrush(Colors.Gray)};
//            Canvas.Children.Add(_explosionEllipse);
//            Canvas.SetLeft(_explosionEllipse,center.X - 10 );
//            Canvas.SetTop(_explosionEllipse, center.Y - 10);
//            var uiElementAnimationCreator = AnimationsRepository.CreateExplosionAnimationCreator(TimeSpan.FromMilliseconds(200), TimeSpan.FromMilliseconds(500), 2);
//            var storyboard = uiElementAnimationCreator.Create(_explosionEllipse);
//            storyboard.Completed += (sender, args) => Canvas.Children.Remove(_explosionEllipse);
//            storyboard.Begin();
//        }
        private void CreateMoveStoryboard(Point center, TimeSpan duration)
        {
            _moveShapesAnimationCreator = new GenericMultiAnimationCreator<Point>(
                element => new Point(Canvas.GetLeft(element) + shapeSize/2 , Canvas.GetTop(element) + shapeSize/2),
                point =>
                    SteppingAnimationDelayFuncion.CreateCircular(TimeSpan.FromMilliseconds(1.5), center)
                        .ComputeDelay((int) point.X, (int) point.Y),
                point =>
                {
                    double dx = point.X - center.X;
                    double dy = point.Y - center.Y;
                    double distanceFromCenter = Math.Sqrt(dx*dx + dy*dy);
                    if (distanceFromCenter == 0) distanceFromCenter = 1;

                    double dx2 = dx/distanceFromCenter;
                    double dy2 = dy/distanceFromCenter;
                    ;
                    double initialDistance = 50; //+ _random.Next(100);
                    var translationLen = new Point(dx2/Math.Pow(distanceFromCenter, 0.33)*initialDistance,
                        dy2/Math.Pow(distanceFromCenter, 0.33)*initialDistance);

                    //return
//                        new SingleAnimationCreator(
//                            AnimationBuilder.Scale().Uniform().To(0.5).AutoReverse().WithDuration(500).Build());
                    return new SimultanousAnimationsCreator(
                        new SingleAnimationCreator(
                            AnimationBuilder.Translate()
                                .Horizontal()
                                .WithEasingFunction(new QuadraticEase())
                                .To(translationLen.X)
                                .AutoReverse()
                                .WithDuration(duration).Build()),
                        new SingleAnimationCreator(AnimationBuilder.Translate()
                            .Vertical()
                            .WithEasingFunction(new QuadraticEase())
                            .To(translationLen.Y)
                            .AutoReverse()
                            .WithDuration(duration).Build()));
                });
            _moveShapesStoryboard = _moveShapesAnimationCreator.Create(GetMovableShapes());         
        }
        private IEnumerable<UIElement> GetMovableShapes()
        {
            return Canvas.Children.OfType<Shape>().Where(x => x.Tag !=null );
        }
        private void RandomizeShapes(IList<UIElement> shapes)
        {                        
            for (int i = 0; i < shapes.Count; i++)
            {
                shapes[i].Opacity = 0;
                Point position = _randomizedPositions[i];
                Canvas.SetLeft(shapes[i], position.X - shapeSize / 2.0);
                Canvas.SetTop(shapes[i], position.Y - shapeSize / 2.0);    
                          
            }
//            foreach (var shape in shapes)
//            {             
//                Point position = 
//                    RandomizePosition(new NewShapePositionArgument(new ReadOnlyCollection<Point>(Canvas.Children.Select(
//                        x => x.TransformToVisual(Canvas)
//                            .Transform(new Point(0, 0))).ToList()),
//                        ActualWidth, ActualHeight));
//                Canvas.SetLeft(shape, position.X - shapeSize / 2.0);
//                Canvas.SetTop(shape, position.Y - shapeSize / 2.0);      
//            }
        }
        public void RecreateShapes()
        {
           // _randomizer = new Randomizer(shapeSize, _shapesCountX,_shapesCountY, (int)ActualWidth, (int)ActualHeight);
            RandomizeShapes(Canvas.Children);

            CreateMoveStoryboard(new Point(_random.Next((int)ActualWidth), _random.Next((int)ActualHeight)),
                      _automaticTransaltionDuration);                 
            _fadeAnimation.Begin();
        }
    }
}
