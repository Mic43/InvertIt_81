using System.Windows.Media;
using System.Windows.Shapes;
using GameLogic;
using GameLogic.Areas;
using NoNameGame.BoardPresentation.AreaPainter;
using NoNameGame.BoardPresentation.StatePainter;

namespace NoNameGame.BoardPresentation.AreaPainterFactory
{
    public class SimpleAreaPainterFactory : IAreaPainterFactory
    {
        public AreaPainter.AreaPainter Create(Area areaToPaint, Shape areaVisualisation)
        {
            return new ShapeAreaPainter(areaToPaint, areaVisualisation, new SolidFillAreaStatePainter(Colors.Blue),
                new SolidFillAreaStatePainter(Colors.Green));
        }
    }
}