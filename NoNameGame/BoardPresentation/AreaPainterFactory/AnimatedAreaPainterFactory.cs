using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using GameLogic;
using GameLogic.Areas;
using NoNameGame.BoardPresentation.AreaPainter;
using NoNameGame.BoardPresentation.StatePainter;
using NoNameGame.Configuration;

namespace NoNameGame.BoardPresentation.AreaPainterFactory
{
    public class AnimatedAreaPainterFactory : IAreaPainterFactory
    {
        private IStatePainter CreateCheckedPainter()
        {
            return AnimationPainterCreator.CreateRadialGradientColorAnimator(GameResources.Instance.UnCheckedColor, GameResources.Instance.CheckedColor);
        }
        
        private IStatePainter CreateUnCheckedPainter()
        {
            return AnimationPainterCreator.CreateRadialGradientColorAnimator(GameResources.Instance.CheckedColor, GameResources.Instance.UnCheckedColor);
        }
        public AreaPainter.AreaPainter Create(Area areaToPaint, Shape areaVisualisation)
        {
            return new ShapeAreaPainter(areaToPaint, areaVisualisation, CreateCheckedPainter(), CreateUnCheckedPainter());                
        }
    }
}