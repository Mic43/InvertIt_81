using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using AnimationLib;
using NoNameGame.BoardPresentation.StatePainter;

namespace NoNameGame.BoardPresentation.AreaPainterFactory
{

    public static class AnimationPainterCreator
    {
        private static Func<Timeline> CreateColorAnimation(Color from,Color to,int duration = 500,int offset =0)
        {
            return () => new ColorAnimation()
            {
                Duration = TimeSpan.FromMilliseconds(duration),
                From = from,
                To = to,
                BeginTime = TimeSpan.FromMilliseconds(offset)
            }; 
        }
        private static Func<Timeline> CreateDoubleAnimation(double from, double to, int duration = 500)
        {
            return () => new DoubleAnimation()
            {
                Duration = TimeSpan.FromMilliseconds(duration),
                From = from,
                To = to,
            };
        }
        public static AnimatingStatePainter CreateFillColorAnimator(Color from,Color to)
        {
            return new AnimatingStatePainter(new PropertyAnimation(CreateColorAnimation(from, to), new PropertyPath("Shape.Fill.Color")));       
        }      
        public static AnimatingStatePainter CreateRadialGradientColorAnimator(Color from,Color to)
        {
            return new AnimatingStatePainter(new PropertyAnimation(CreateColorAnimation(from, to,200,0), new PropertyPath("Shape.Fill.GradientStops[0].Color")));            
        }
    }
}
