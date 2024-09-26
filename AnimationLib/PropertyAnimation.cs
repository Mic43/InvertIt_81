using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace AnimationLib
{
    public class PropertyAnimation
    {
        Func<Timeline> timeline;

        public Func<Timeline> Timeline
        {
            get { return timeline; }
        }

        readonly PropertyPath[] _propertiesesPaths;

        public PropertyPath[] PropertyPaths
        {
            get { return _propertiesesPaths; }
        }

        public PropertyAnimation(Func<Timeline> timeline, PropertyPath propertyPath)
        {
            if (timeline == null)
                throw new ArgumentNullException("timeline");
            if (propertyPath == null)
                throw new ArgumentNullException("propertyPath");

            this.timeline = timeline;
            _propertiesesPaths = Enumerable.Repeat(propertyPath,1).ToArray();
        }

        public PropertyAnimation(Func<Timeline> timeline, params PropertyPath[] propertyPaths)
        {
            if (timeline == null)
                throw new ArgumentNullException("timeline");
            if (propertyPaths == null)
                throw new ArgumentNullException("propertyPaths");

            this.timeline = timeline;
            this._propertiesesPaths = propertyPaths;
        }
    }
}
