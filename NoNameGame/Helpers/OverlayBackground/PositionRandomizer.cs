using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using NoNameGame.CustomControls.OverlayAnimatedBackground;

namespace NoNameGame.Helpers.OverlayBackground
{
    public class PositionRandomizer
    {
        private readonly int _shapeSize;
        private Random _random = new Random();

        public PositionRandomizer(int shapeSize)
        {
            _shapeSize = shapeSize;
        }
        public Point RandomizePosition(NewShapePositionArgument newShapePositionArgument)
        {
            Point point;
            do
            {
                int x = _random.Next(0 + _shapeSize, (int)(newShapePositionArgument.MaxLeft - _shapeSize));
                int y = _random.Next(0 + _shapeSize, (int)(newShapePositionArgument.MaxTop - _shapeSize));
                //int y = -shapeSize;
                point = new Point(x, y);
            } while (IsPositionOverlapping(point, newShapePositionArgument.OtherElementsPositions));
            return point;
        }
        private bool IsPositionOverlapping(Point position, IEnumerable<Point> children)
        {
            return
               children.Any(
                    child =>
                        Math.Abs(child.X - position.X) < _shapeSize &&
                        Math.Abs(child.Y - position.Y) < _shapeSize);
        } 
    }
}