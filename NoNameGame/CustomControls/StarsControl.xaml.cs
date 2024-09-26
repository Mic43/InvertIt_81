using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AnimationLib;
using AnimationLib.AnimationsCreator;
using AnimationLib.AnimationsCreator.MutliAnimation;
using Infrastructure;
using NoNameGame.BoardPresentation.Animations;
using NoNameGame.Configuration;
using NoNameGame.CustomControls.AttachedProperties;
using NoNameGame.Helpers;

namespace NoNameGame.CustomControls
{
    public partial class StarsControl : UserControl
    {
        private const string starContolXaml =
           @"<Viewbox xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" 
                       xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                       xmlns:es=""clr-namespace:Microsoft.Expression.Shapes;assembly=Microsoft.Expression.Drawing"">
                <es:RegularPolygon Height=""77.853"" InnerRadius=""0.6"" PointCount=""5"" Stretch=""Fill"" 
                    Stroke=""Black"" UseLayoutRounding=""False"" Width=""77.853""/>
             </Viewbox>";
        private readonly MultiAnimationCreator _animation;
        private List<Viewbox> _stars = new List<Viewbox>();

        public static readonly DependencyProperty StarsCountProperty = DependencyProperty.Register(
            "StarsCount", typeof(int), typeof(StarsControl), new PropertyMetadata(default(int), StarsCountPropertyChangedCallback));
        private static void StarsCountPropertyChangedCallback(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            ((StarsControl)dependencyObject).CreateStars();
        }

        public int StarsCount
        {
            get { return (int)GetValue(StarsCountProperty); }
            set { 
                SetValue(StarsCountProperty, value);               
            }
        }

        public static readonly DependencyProperty ShowAnimationProperty = DependencyProperty.Register(
            "ShowAnimation", typeof(bool), typeof(StarsControl), new PropertyMetadata(default(bool)));


        public static readonly DependencyProperty ShowStrokeProperty = DependencyProperty.Register(
            "ShowStroke", typeof(bool), typeof(StarsControl), new PropertyMetadata(default(bool), ShowStrokeChanged));

        public bool ShowStroke
        {
            get { return (bool) GetValue(ShowStrokeProperty); }
            set { SetValue(ShowStrokeProperty, value); }
        }

         private static void ShowStrokeChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            ((StarsControl)dependencyObject).SetStroke();
          
        }

        public bool ShowAnimation
        {
            get { return (bool) GetValue(ShowAnimationProperty); }
            set { SetValue(ShowAnimationProperty, value); }
        }

        public static readonly DependencyProperty RepeatAnimationForeverProperty = DependencyProperty.Register(
            "RepeatAnimationForever", typeof (bool), typeof (StarsControl), new PropertyMetadata(false,PropertyChangedCallback));
        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            ((StarsControl) dependencyObject).RecreateAnimation();
            ((StarsControl)dependencyObject).PlayAimation();
        }

        public bool RepeatAnimationForever
        {
            get { return (bool) GetValue(RepeatAnimationForeverProperty); }
            set { SetValue(RepeatAnimationForeverProperty, value); }
        }

        public StarsControl()
        {
            InitializeComponent();                                   
            _animation = RecreateAnimation();          
            
        }
        private  MultiAnimationCreator RecreateAnimation()
        {
            return new MultiAnimationCreator(new SingleAnimationCreator(AnimationsRepository.CreateExpandShrinkAnimation(1.4,
                TimeSpan.FromMilliseconds(500),RepeatAnimationForever)),
                SteppingAnimationDelayFuncion.CreateUpDown(TimeSpan.FromMilliseconds(200)));
        }
        private void SetStroke()
        {
            foreach (Viewbox box in LayoutRoot.Children)
            {
                var shape = ((Shape) box.Child);
                shape.StrokeThickness = ShowStroke ? 1 : 0;
            }
        }
        public void CreateStars()
        {
            LayoutRoot.Children.Clear();
            _stars.Clear();
            for (int i = 0; i < StarsCount; i++)
            {
               // var viewbox =new Viewbox();
//                viewbox.Child = new Image()
//                {
//                    Source =GameResources.Instance.StareBitmapImage,
//                  //  Source = new BitmapImage(new Uri(@"/Assets/star-128.png", UriKind.Relative)),
//                    Stretch = Stretch.UniformToFill
//                };
                
                var viewbox = (Viewbox) XamlReader.Load(starContolXaml);
               // var viewbox = new SingleStarControl();
                var shape = (Shape) viewbox.Child;


                //shape.StrokeThickness  = ShowStroke ? 1 : 0;
                shape.StrokeThickness = 3;

                shape.Stroke = new SolidColorBrush();
                shape.Fill = new SolidColorBrush();
                shape.Margin = new Thickness(2.5, 1, 2.5, 1);
                BindingOperations.SetBinding(shape.Fill, SolidColorBrush.ColorProperty      ,
                        new Binding
                        {
                            Path = new PropertyPath("CheckedColor"), 
                            Source = GameResources.Instance
                        });
                BindingOperations.SetBinding(shape.Stroke, SolidColorBrush.ColorProperty,
                        new Binding
                        {
                            Path = new PropertyPath("StarStrokeColor"),
                            Source = GameResources.Instance
                        });
                _stars.Add(viewbox);
                LayoutRoot.Children.Add(viewbox);
            }
            PlayAimation();
        }
        private void PlayAimation()
        {
            if (ShowAnimation)
            {
                var storyboard = _animation.Create(_stars.ToDegenerated2DArray());
                storyboard.BeginTime = TimeSpan.FromMilliseconds(150);
                storyboard.Begin();
            }
        }
    }
}