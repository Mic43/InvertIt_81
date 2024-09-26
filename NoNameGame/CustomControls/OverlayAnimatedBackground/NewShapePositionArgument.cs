using System.Collections.ObjectModel;
using System.Windows;

namespace NoNameGame.CustomControls.OverlayAnimatedBackground
{
    public class NewShapePositionArgument
    {
        public ReadOnlyCollection<Point> OtherElementsPositions { get; private set; }
        public double MaxLeft { get; private  set; }
        public double MaxTop { get; private set; }

        public NewShapePositionArgument(ReadOnlyCollection<Point> otherElements, double maxLeft, double maxTop)
        {
            OtherElementsPositions = otherElements;
            MaxLeft = maxLeft;
            MaxTop = maxTop;
        }
    }
}