using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;


namespace NoNameGame.Levels.Entities
{
    public class DisabledAreasEntity
    {    
        public List<Point> AreasList { get; private set; }

        public DisabledAreasEntity(List<Point> areasList)
        {
            if (areasList == null) throw new ArgumentNullException("areasList");     
            AreasList = areasList;
        }
    }
}