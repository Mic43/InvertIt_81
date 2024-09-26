using System.Windows.Media;
using System.Windows.Shapes;

namespace NoNameGame.BoardPresentation.StatePainter
{
    public class SolidFillAreaStatePainter : IStatePainter
    {
        Color fillColor;
        public SolidFillAreaStatePainter(Color color)
        {
            this.fillColor = color;
        }
        public void Paint(Shape shape)
        {
            shape.Fill = new SolidColorBrush(fillColor);
        }
    }
}