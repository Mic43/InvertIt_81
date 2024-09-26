using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimationLib
{
    public class AnimationFunction
    {
        public Func<int,int,float> Function { get; private set; }
        private AnimationFunction(Func<int, int, float> function)
        {
            if (function == null)
                throw new ArgumentNullException("function");
            Function = function;
        }
        public static AnimationFunction FromTransformable(TransformableFunction function)
        {
            return new AnimationFunction(function.Build());
        }
    }
}
