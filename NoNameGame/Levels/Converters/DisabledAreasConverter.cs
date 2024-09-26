using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using NoNameGame.Levels.DB;
using NoNameGame.Levels.Entities;

namespace NoNameGame.Levels.Converters
{
    public class DisabledAreasConverter
    {
        public static DisabledAreasEntity CreateEntity(DisabledAreas disabledAreas)
        {
            List<Point> coordinates =
                Common.MovesCollectionFormatter.ParseString(disabledAreas.Coordinates).Select(x => new Point(x.X, x.Y)).ToList();
            return new DisabledAreasEntity(coordinates);
        }
    }
}