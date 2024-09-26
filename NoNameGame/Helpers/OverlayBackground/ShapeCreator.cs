using System;
using System.Windows;
using System.Windows.Shapes;
using NoNameGame.Configuration;

namespace NoNameGame.Helpers.OverlayBackground
{
    public class ShapeCreator
    {
        private Random _random =new Random();
        public Ellipse CreateMainColorsVisibleEllipse(int shapeSize)
        {
            var ellipse = new Ellipse {};
            ellipse.Visibility = Visibility.Visible;
            ellipse.Width = ellipse.Height = shapeSize; //shapeSize;

            var colors = new[]
            {
                GameResources.Instance.OverlayGradientRed,
                GameResources.Instance.OverlayGradientBlue
            };
            ellipse.Fill = colors[_random.Next(colors.Length)];

            return ellipse;
        }
        public Ellipse CreateMainColorsCollapsedEllipse(int shapeSize)
        {
            var ellipse = new Ellipse { };
            ellipse.Visibility = Visibility.Collapsed;
            ellipse.Width = ellipse.Height = shapeSize; //shapeSize;

            var colors = new[]
            {
                GameResources.Instance.OverlayGradientRed,
                GameResources.Instance.OverlayGradientBlue
            };
            ellipse.Fill = colors[_random.Next(colors.Length)];

            return ellipse;
        }
    }
}