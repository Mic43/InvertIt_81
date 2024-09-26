using System.Linq;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using AnimationLib;

namespace NoNameGame.BoardPresentation.StatePainter
{
    public class AnimatingStatePainter : IStatePainter
    {
        //IStatePainter painterToAnimate;
        PropertyAnimation propertyAnimation;
        
        public AnimatingStatePainter(PropertyAnimation propertyAnimation)
        {
            //   this.painterToAnimate = painterToAnimate;
            this.propertyAnimation = propertyAnimation;
        }
        public void Paint(Shape shape)
        {
            // painterToAnimate.Paint(shape);

            var sb = new Storyboard();
            sb.Children.Add(propertyAnimation.Timeline());

            Storyboard.SetTarget(sb, shape);
            foreach (var propPath in  propertyAnimation.PropertyPaths)
            {
                Storyboard.SetTargetProperty(sb, propPath);
            }            

            sb.Begin();                     
        }
    }
}