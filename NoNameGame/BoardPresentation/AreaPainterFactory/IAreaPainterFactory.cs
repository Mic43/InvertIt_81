using System.Windows.Shapes;
using GameLogic;
using GameLogic.Areas;

namespace NoNameGame.BoardPresentation.AreaPainterFactory
{
    public interface IAreaPainterFactory
    {
        AreaPainter.AreaPainter Create(Area areaToPaint, Shape areaVisualisation);
    }
}