using System.Windows.Shapes;

namespace NoNameGame.BoardPresentation.StatePainter
{
    public interface IStatePainter
    {
        void Paint(Shape shape);
    }
}