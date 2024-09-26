using System;
using System.Windows.Shapes;
using GameLogic;
using GameLogic.Areas;

namespace NoNameGame.BoardPresentation.AreaPainter
{
    public abstract class AreaPainter
    {
        private Area areaToPaint;

        public Area AreaToPaint
        {
            get { return areaToPaint; }         
        }
        protected Shape areaVisualisation;
        public Shape AreaVisualisation
        {
            get { return areaVisualisation; }
        }
        public AreaPainter(Area areaToPaint, Shape areaVisualisation)
        {
            if (areaVisualisation == null)
                throw new ArgumentNullException("areaVisualisation");
            if (areaToPaint == null)
                throw new ArgumentNullException("areaToPaint");

            this.areaToPaint = areaToPaint;
            this.areaVisualisation = areaVisualisation;
        }
        public abstract void Paint();
    }
}