using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using GameLogic.Board;

namespace GameLogic.Areas
{    
    public abstract class Area 
    {
        protected AreaMatrix AreaMatrix;
        private readonly BoardCoordinate _boardCoordinate;

        protected Area(AreaMatrix areaMatrix,BoardCoordinate boardCoordinate)
        {
            if (areaMatrix == null) throw new ArgumentNullException("areaMatrix");

          //  Checked = false;
            AreaState = AreaState.UnChecked;
            AreaMatrix = areaMatrix;
            _boardCoordinate = boardCoordinate;
        }

        public AreaState AreaState { get; set; }

        public void TryInvertState()
        {
            if (AreaState == AreaState.Checked)
                AreaState = AreaState.UnChecked;
            else if (AreaState == AreaState.UnChecked)
                AreaState = AreaState.Checked;
        }
//        public bool Checked { get; set; }

        public BoardCoordinate BoardCoordinate
        {
            get { return _boardCoordinate; }
        }

        public Area Copy()
        {
            var clone = (Area) MemberwiseClone();
            
            return clone;
        
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var area = (Area) obj;
            return area.AreaState == AreaState && _boardCoordinate.Equals(area._boardCoordinate);
        }
        public override int GetHashCode()
        {
            return AreaState.GetHashCode() ^ BoardCoordinate.GetHashCode();
        }

        public abstract ReadOnlyCollection<Area> OnEnter();
    }

    [DataContract]
    public enum AreaState
    {
        [EnumMember]
        Checked,
        [EnumMember]
        UnChecked,
        [EnumMember]
        Disabled
    }
}