using System.Windows.Media;
using System.Windows.Shapes;
using GameLogic;
using GameLogic.Areas;
using NoNameGame.BoardPresentation.AreaPainter;
using NoNameGame.BoardPresentation.StatePainter;
using NoNameGame.Configuration;

namespace NoNameGame.BoardPresentation.AreaPainterFactory
{
    public class GradientAreaPainterFactory : IAreaPainterFactory
    {
        public AreaPainter.AreaPainter Create(Area areaToPaint, Shape areaVisualisation)
        {
            return new ShapeAreaPainter(areaToPaint, areaVisualisation, new GradientFillAreaStatePainter(GameResources.Instance.CheckedAreaGradientBrush),
                new GradientFillAreaStatePainter(GameResources.Instance.UnCheckedAreaGradientBrush));         
        }
    }
}