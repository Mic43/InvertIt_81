using System;
using System.Collections.ObjectModel;
using Windows.Devices.Sensors;
using GameLogic.Areas;
using Infrastructure;

namespace GameLogic.Board
{
    interface IAreaFactory
    {
        
    }
    public class AreaMatrix
    {
        private readonly int _size;
        private readonly Area[,] _areas;

        public Area[,] Areas
        {
            get { return _areas; }
        }
        public  int Size
        {
            get { return _size; }
        }        

        public AreaMatrix(int size)
        {
            _size = size;
            _areas = new Area[size, size];            
        }
        public Area GetArea(BoardCoordinate bc)
        {
            return _areas[bc.X, bc.Y];
        }

        public bool IsValid(int x,int y)
        {
            return (x >= 0 && x <= _size && y >= 0 && y < _size);
        }
        public void InsertArea(AreaState areaState,int xCoord,int yCoord)
        {
            if (!IsValid(xCoord,yCoord))
                 throw new ArgumentOutOfRangeException("xCoord or yCoord");
            _areas[xCoord, yCoord] = new StandardArea(this, new BoardCoordinate(xCoord, yCoord)) {AreaState = areaState};
        }

        public AreaMatrix Copy()
        {
            var areaMatrix = new AreaMatrix(_size);
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    areaMatrix._areas[i, j] = _areas[i, j].Copy();
                }
            }
            return areaMatrix;
        }

        public bool Equals(AreaMatrix areaMatrix)
        {
            if (areaMatrix.Size != Size) return false;

            for (int i = 0; i < areaMatrix.Size; i++)
            {
                for (int j = 0; j < areaMatrix.Size; j++)
                {
                    if (!Areas[i, j].Equals(areaMatrix.Areas[i, j]))
                        return false;
                }
            }
            return true;
        }

        public ReadOnlyCollection<Area> GetNeiborghood(BoardCoordinate boardCoordinate)
        {
            return _areas.GetNeiborghood(boardCoordinate.X, boardCoordinate.Y);
        }
    }
}