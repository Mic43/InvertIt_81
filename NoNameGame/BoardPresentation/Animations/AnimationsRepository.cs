using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using AnimationLib;
using AnimationLib.AnimationDSL;
using AnimationLib.AnimationDSL.Helpers;
using AnimationLib.AnimationsCreator;
using AnimationLib.AnimationsCreator.Interfaces;
using Microsoft.Devices;

namespace NoNameGame.BoardPresentation.Animations
{

    public static class AnimationCreators
    {     
    }
    public class AnimationsRepository
    {
//        private static readonly Action<UIElement> InitAction = (element) =>
//        {
//            var tran = new TransformGroup();
//            tran.Children.Add(element.RenderTransform);
//
//        }
        private static readonly Action<UIElement> ScaleTransformInitAction = (element) =>
        {
            if (!(element.RenderTransform is ScaleTransform))
            {
                element.RenderTransform = new ScaleTransform();
            }
            element.RenderTransformOrigin = new Point(0.5, 0.5);
        };
        private static DoubleKeyFrame CreateExponentialDoubleKeyFrameFrame(double targetValue,TimeSpan duration)
        {
            return new EasingDoubleKeyFrame()
            {
                Value = targetValue,
                KeyTime = KeyTime.FromTimeSpan(duration),
                EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseOut }            
            };
//            return new LinearDoubleKeyFrame() {Value = targetValue, KeyTime = duration};
        }

//        public static UIElementAnimation CreateTranslationWithInitialSpeedAnimation(TimeSpan constSpeedDuration,double constSpeedValue,
//            TimeSpan restDuration, double targetaValue)
//        {
//            Action<UIElement> initAction = element =>
//            {
//                element.RenderTransformOrigin = new Point(0.5, 0.5);
//                if (!(element.RenderTransform is TranslateTransform))
//                    element.RenderTransform = new TranslateTransform() {};
//            };
//
//            Func<Timeline> animCreator = () =>
//            {
//                var doubleAnimationUsingKeyFrames = new DoubleAnimationUsingKeyFrames();
//                doubleAnimationUsingKeyFrames.KeyFrames.Add(new LinearDoubleKeyFrame()
//                {
//                    KeyTime = constSpeedDuration,
//                    Value = constSpeedValue
//                });
//                doubleAnimationUsingKeyFrames.KeyFrames.Add(new EasingDoubleKeyFrame()
//                {
//                    EasingFunction = new QuadraticEase() {EasingMode = EasingMode.EaseIn},
//                    KeyTime = restDuration,
//                    Value = targetaValue
//                });
//                return doubleAnimationUsingKeyFrames;
//            };
//            return new UIElementAnimation(initAction, new PropertyAnimation(animCreator,PropertyPathsRepository.TranslateY));
//        }
        public static UIElementAnimation CreateExpandShrinkAnimation(double expandedSize, TimeSpan duration,bool repeatForever = false)
        {
            var timelineBuilder = AnimationBuilder.Scale().Uniform().To(expandedSize).AutoReverse();
            return repeatForever
                ? timelineBuilder.RepeatForever().WithDuration(duration).Build()
                : timelineBuilder.WithDuration(duration).Build();
//            Func<Timeline> animCreator = () =>
//            {
//                var anim = new DoubleAnimationUsingKeyFrames() { AutoReverse = true};
//                anim.KeyFrames.Add( new LinearDoubleKeyFrame() {Value = expandedSize, KeyTime = duration});                
//                return anim;
//            };
//
//            return new UIElementAnimation(ScaleTransformInitAction,
//                new PropertyAnimation(animCreator, PropertyPathsRepository.ScaleX,
//                    PropertyPathsRepository.ScaleY));
        }
        public static UIElementAnimation CreateGrowPopAnimation(double expandedSize, Duration growTime, Duration popTime)
        {
            Func<Timeline> animCreator = () =>
            {
                var anim = new DoubleAnimationUsingKeyFrames() ;
                anim.KeyFrames.Add(CreateExponentialDoubleKeyFrameFrame(expandedSize, growTime.TimeSpan));
                anim.KeyFrames.Add(new LinearDoubleKeyFrame() {Value = 0,KeyTime = KeyTime.FromTimeSpan(popTime.Add(growTime).TimeSpan)});                
                
                return anim;
            };

            return new UIElementAnimation(ScaleTransformInitAction,
                new PropertyAnimation(animCreator, PropertyPathsRepository.ScaleX,
                    PropertyPathsRepository.ScaleY));
        }
        public static UIElementAnimation CreateShrinkAnimation(double shrinkedSize, TimeSpan duration)
        {
            Func<Timeline> animCreator = () =>
            {
                var anim = new DoubleAnimationUsingKeyFrames();
                anim.KeyFrames.Add(CreateExponentialDoubleKeyFrameFrame(shrinkedSize, duration));                
                return anim;
            };

            return new UIElementAnimation(ScaleTransformInitAction,
                new PropertyAnimation(animCreator, PropertyPathsRepository.ScaleX,
                    PropertyPathsRepository.ScaleY));
        }
        public static UIElementAnimation CreateRadialGradientRadiusAnimation(double targetRadius, TimeSpan duration)
        {            
            Func<Timeline> animCreator = () =>
            {
                var anim = new DoubleAnimation() { AutoReverse = true, To = targetRadius,Duration = duration};                                
                return anim;
            };

            return new UIElementAnimation(element => { },
                new PropertyAnimation(animCreator, new PropertyPath("Shape.Fill.RadiusX"),
                    new PropertyPath("Shape.Fill.RadiusY")));
        }
        public static UIElementAnimation CreateRadialGradientOffsetAnimation(double targetOffset, TimeSpan duration)
        {
            Func<Timeline> animCreator = () =>
            {
                var anim = new DoubleAnimation() { AutoReverse = true, To = targetOffset, Duration = duration };
                return anim;
            };

            return new UIElementAnimation(element => { },
                new PropertyAnimation(animCreator, new PropertyPath("UIElement.Fill.GradientStops[2].Offset")));
        }
        public static UIElementAnimation CreateRadialGradientColorAnimation(Color targetColor, TimeSpan duration)
        {
            Func<Timeline> animCreator = () =>
            {
                var anim = new ColorAnimation() { To = targetColor, Duration = duration };
                return anim;
            };

            return new UIElementAnimation(element => { },
                new PropertyAnimation(animCreator, new PropertyPath("Shape.Fill.GradientStops[0].Color")));
        }
        public static UIElementAnimation CreateFillColorAnimation(Color targetColor, TimeSpan duration)
        {
            Func<Timeline> animCreator = () =>
            {
                var anim = new ColorAnimation() { To = targetColor, Duration = duration };
                return anim;
            };

            return new UIElementAnimation(element => { },
                new PropertyAnimation(animCreator, new PropertyPath("Shape.Fill.Color")));
        }
        public static UIElementAnimation CreateFillColorDiscreteAnimation(Color targetColor, KeyTime changeColorTime)
        {
            Func<Timeline> animCreator = () =>
            {
                var anim = new ColorAnimationUsingKeyFrames();
                anim.KeyFrames.Add(new DiscreteColorKeyFrame() { KeyTime = changeColorTime ,Value = targetColor});
                return anim;
            };

            return new UIElementAnimation(element => { },
                new PropertyAnimation(animCreator, new PropertyPath("Shape.Fill.Color")));
        }
        public static UIElementAnimation CreateStrokeThicknessDiscreteAnimation(double targetThickness, KeyTime changeColorTime)
        {
            Func<Timeline> animCreator = () =>
            {
                var anim = new DoubleAnimationUsingKeyFrames();
                anim.KeyFrames.Add(new DiscreteDoubleKeyFrame() { KeyTime = changeColorTime, Value = targetThickness });
                return anim;
            };

            
            return new UIElementAnimation(element => { },
                new PropertyAnimation(animCreator, new PropertyPath("Shape.StrokeThickness")));
        }
        public static UIElementAnimation CreateVerticalTranslationAnimation(double targetDy, TimeSpan duration, double startDy =0,
                                                                            bool autoreverse = false)
        {
            if (autoreverse)
                return new TranslationBuilder().Vertical()
                    .From(startDy)
                    .WithEasingFunction(new QuadraticEase() { EasingMode = EasingMode.EaseInOut})
                    .To(targetDy)                   
                    .AutoReverse()
                    .WithDuration(duration)
                    .Build();
            else
                return
                    new TranslationBuilder().Vertical()
                        .From(startDy)
                        .WithEasingFunction(new QuadraticEase {EasingMode = EasingMode.EaseInOut})
                        .To(targetDy)
                        .WithDuration(duration)
                        .Build();

//            Action<UIElement> initAction = element =>
//            {
//                element.RenderTransformOrigin = new Point(0.5, 0.5);
//                if (!(element.RenderTransform is TranslateTransform))
//                    element.RenderTransform = new TranslateTransform() {Y = startDy};
//            };
//            Func<Timeline> animCreator = () =>
//            {
//                var anim = new DoubleAnimation()
//                {
//                    AutoReverse = autoreverse,
//                    From = startDy,
//                    To = targetDy,
//                    Duration = duration,
//                    EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseInOut },                  
//
//                };
//                return anim;
//            };
//            return new UIElementAnimation(initAction,new PropertyAnimation(animCreator, PropertyPathsRepository.CreateTranslateY()));
        }
        public static UIElementAnimation CreateHorizontalTranslationAnimation(double targetDx, TimeSpan duration, double startDx = 0, bool autoreverse = false)
        {
//           return new TranslationBuilder().Horizontal()
//                       .From(startDx)
//                       .WithEasingFunction(new QuadraticEase { EasingMode = EasingMode.EaseInOut })
//                       .To(targetDx)
//                       .WithDuration(duration)
//                       .End();
            Action<UIElement> initAction = element =>
            {
                element.RenderTransformOrigin = new Point(0.5, 0.5);
                if (!(element.RenderTransform is TranslateTransform))
                    element.RenderTransform = new TranslateTransform() { X = startDx };
            };
            Func<Timeline> animCreator = () =>
            {
                var anim = new DoubleAnimation()
                {
                    AutoReverse = autoreverse,
                    From = startDx,
                    To = targetDx,
                    Duration = duration,
                    EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseInOut },
//                    EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseIn },             
                };
                return anim;
            };
            return new UIElementAnimation(initAction, new PropertyAnimation(animCreator, PropertyPathsRepository.TranslateX));
        }
        public static UIElementAnimation CreateBounceAnimation(double maxHeight,TimeSpan duration)
        {
            return new TranslationBuilder().Vertical()
                .WithEasingFunction(new QuadraticEase() {EasingMode = EasingMode.EaseOut})
                .To(-maxHeight).RepeatForever().AutoReverse().WithDuration(duration).Build();

//            Action<UIElement> initAction = element =>
//            {
//                element.RenderTransformOrigin = new Point(0.5, 0.5);
//                element.RenderTransform = new TranslateTransform();
//            };
//            Func<Timeline> animCreator = () =>
//            {
//                var anim = new DoubleAnimation()
//                {
//                    From = 0,
//                    To = - maxHeight,
//                    Duration = duration,
//                    EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseOut },
//                    AutoReverse = true,
//                    RepeatBehavior = repeatForever ? RepeatBehavior.Forever : new RepeatBehavior(1)
//                };
//                return anim;
//            };
//
//            return new UIElementAnimation(initAction, new PropertyAnimation(animCreator, PropertyPathsRepository.CreateTranslateY()));
        }
        public static UIElementAnimation CreateRotationProjectionAnimation(TimeSpan duration)
        {            
            Action<UIElement> initAction = element =>
            {
                if(element.Projection == null)
                    element.Projection = new PlaneProjection();
            };
            Func<Timeline> animCreator = () =>
            {
                var anim = new DoubleAnimation()
                {
                    From = 0,
                    To = -180,
                    Duration = duration,                                      
                };
                return anim;
            };
            return new UIElementAnimation(initAction, new PropertyAnimation(animCreator, new PropertyPath("UIElement.Projection.RotationY")));
        }
        public static UIElementAnimation CreateFadeToViewAnimation(double targetOpacity, TimeSpan duration)
        {
            Func<Timeline> animCreator = () =>
            {
                var anim = new DoubleAnimation() { To = targetOpacity, Duration = duration };
                return anim;
            };

            return new UIElementAnimation(element => { },
                new PropertyAnimation(animCreator, new PropertyPath("UIElement.Opacity")));
        }
        public static IUIElementAnimationCreator CreateExplosionAnimationCreator(TimeSpan firstPhaseduration,TimeSpan finishPhaseduration,
                                                                                double finishScale)
        {
            return new SimultanousAnimationsCreator(
                new SingleAnimationCreator(
                    AnimationBuilder.Scale()
                        .Uniform()
                        .To(finishScale)
                        .WithDuration(firstPhaseduration.Add(finishPhaseduration))
                        .Build()),
                new SingleAnimationCreator(
                    AnimationBuilder.OpacityChange()
                        .Start()
                        .From(1.0)
                        .To(0.0)
                        .WithBeginTime(firstPhaseduration)
                        .WithDuration(finishPhaseduration)
                        .Build()));
        }
      
    }
}