using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using NoNameGame.Configuration;

namespace NoNameGame.CustomControls.OverlayAnimatedBackground
{
    public interface IOverlayBackgroundShapeFactory
    {
        Shape CreateShape();
    }

    public class EllipseOverlayBackgroundShapeFactory : IOverlayBackgroundShapeFactory
    {
        private readonly Random _random = new Random();
        private const double ShapeSize = 60;

        public Shape CreateShape()
        {
            var ellipse = new Ellipse { };
            ellipse.Visibility = Visibility.Visible;
          //  ellipse.Opacity = 0;
            ellipse.Width = ellipse.Height = ShapeSize;

            var propertyPaths = new[] { new PropertyPath("CheckedAreaGradientBrush"), new PropertyPath("UnCheckedAreaGradientBrush") };
            var binding = new Binding()
            {
                Source = GameResources.Instance,
                Path = propertyPaths[_random.Next(propertyPaths.Length)]
            };
            ellipse.SetBinding(Shape.FillProperty, binding);
            ellipse.CacheMode = new BitmapCache();
            return ellipse;
        }
    }
}