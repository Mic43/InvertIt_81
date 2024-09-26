using System.Collections.Generic;
using System.Windows.Shapes;

namespace NoNameGame.BoardPresentation.StatePainter
{
    public class CompositeStatePainter : IStatePainter
    {
        public IEnumerable<IStatePainter> Painters{ get; private set; }
        public CompositeStatePainter(IEnumerable<IStatePainter> painters)
        {
            Painters = painters;

        }
        public void Paint(Shape shape)
        {
            foreach (var painter in Painters)
            {
                painter.Paint(shape);
            }
        }
    }
}