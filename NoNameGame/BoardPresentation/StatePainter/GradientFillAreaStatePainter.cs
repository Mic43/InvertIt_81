using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NoNameGame.BoardPresentation.StatePainter
{
    public class GradientFillAreaStatePainter : IStatePainter
    {
        private GradientBrush _gradientBrush;

        public GradientFillAreaStatePainter(GradientBrush gradientBrush)
        {            
            _gradientBrush = gradientBrush;
            //this._fillColor = color;
        }

        public void Paint(Shape shape)
        {
            //_gradientBrush.GradientStops[0].Color = _fillColor;
            shape.Fill = _gradientBrush;
        }
    }
}