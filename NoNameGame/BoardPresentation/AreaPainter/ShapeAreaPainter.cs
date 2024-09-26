using System;
using System.Windows.Shapes;
using GameLogic;
using GameLogic.Areas;
using NoNameGame.BoardPresentation.StatePainter;

namespace NoNameGame.BoardPresentation.AreaPainter
{
    public class ShapeAreaPainter : AreaPainter
    {
        protected IStatePainter checkedStatePainter;
        protected IStatePainter uncheckedStatePainter;

        public ShapeAreaPainter(Area areaToPaint, Shape areaVisualisation,IStatePainter checkedStatePainter,IStatePainter uncheckedStatePainter)
            :base (areaToPaint, areaVisualisation)
        {
           
            if (checkedStatePainter == null)
                throw new ArgumentNullException("checkedStatePainter");
            if (uncheckedStatePainter == null)
                throw new ArgumentNullException("uncheckedStatePainter");
          
            this.checkedStatePainter = checkedStatePainter;
            this.uncheckedStatePainter = uncheckedStatePainter;
        }
        public override void Paint()
        {
            if(AreaToPaint.Checked)            
                checkedStatePainter.Paint(areaVisualisation);            
            else            
                uncheckedStatePainter.Paint(areaVisualisation);            
        }
    }
}