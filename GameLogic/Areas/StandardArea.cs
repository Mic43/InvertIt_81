using System.Collections.ObjectModel;
using System.Linq;
using GameLogic.Board;

namespace GameLogic.Areas
{
    public class StandardArea : Area
    {
        public StandardArea(AreaMatrix areaMatrix,BoardCoordinate boardCoordinate)
            : base(areaMatrix, boardCoordinate)
        {
        }

        public override ReadOnlyCollection<Area> OnEnter()
        {
            if (AreaState == AreaState.Disabled)
                return new ReadOnlyCollection<Area>(Enumerable.Empty<Area>().ToList());

            ReadOnlyCollection<Area> neighbours = AreaMatrix.GetNeiborghood(BoardCoordinate);
            foreach (Area area in neighbours.Where(a=>a.AreaState!=AreaState.Disabled))
            {
                area.TryInvertState();
            }
            return neighbours;  
        }
    }

}