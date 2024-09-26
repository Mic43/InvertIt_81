using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AnimationLib
{
    public class TransformableFunction
    {
        private Random random = new Random();
        private Expression<Func<int, int, float>> _function;
        public Expression<Func<int, int, float>> Function
        {
            get
            {
                return _function;
            }
        }

        private TransformableFunction(Expression<Func<int, int, float>> function)
        {
            this._function = function;
        }
        public TransformableFunction Translate(int dx, int dy)
        {
            _function = new FunctionTransformer(_function).ApplyXTransform(x => x - dx).ApplyYTransform(y => y - dy).GetExpression();
            return this;
        }        
        public Func<int, int, float> Build()
        {
            return _function.Compile();
        }
        public TransformableFunction SymmetryOX()
        {
            _function = new FunctionTransformer(_function).ApplyYTransform(y => -y).GetExpression();
            return this;
        }
        public TransformableFunction SymmetryOY()
        {
            _function = new FunctionTransformer(_function).ApplyXTransform(x => -x).GetExpression();
            return this;
        }
        public TransformableFunction PartialSymmetryOX()
        {
            _function = new FunctionTransformer(_function).ApplyYTransform(y => Math.Abs(y)).GetExpression();
            return this;
        }
        public TransformableFunction PartialSymmetryOY()
        {
            _function = new FunctionTransformer(_function).ApplyXTransform(x => Math.Abs(x)).GetExpression();
            return this;
        }
        public TransformableFunction RandomizeValue(float randomCoefficient)
        {
            if (randomCoefficient > 1.0 || randomCoefficient < 0.0)
                throw new ArgumentOutOfRangeException("randomCoefficient", "coeeficeint must be between 0.0 and 1.0");
            _function =
                new FunctionTransformer(_function).ApplyFunctionValueTransform(
                    val => val + (float)random.NextDouble()* val*randomCoefficient).GetExpression();
            return this;
        }

        public static TransformableFunction CircularFunction()
        {           
            return new TransformableFunction((x, y) => (float)Math.Sqrt((x) * (x) + (y) * (y)));          
        }
        public static TransformableFunction ArcFunction()
        {
            return new TransformableFunction((x, y) => ((float)(Math.Atan2((double)x, (double)y) + Math.PI)));
        }
        public static TransformableFunction DiagonalFunction()
        {
            return new TransformableFunction((x, y) => (x - y));
        }
        public static TransformableFunction HorizontalFunction()
        {
            return new TransformableFunction((x, y) => x);
        }
        public static TransformableFunction VerticalFunction()
        {
            return new TransformableFunction((x, y) => y);
        }
        public static TransformableFunction ZeroFunction()
        {
            return ConstFunction(0);
        }
        public static TransformableFunction ConstFunction(int value)
        {
            return new TransformableFunction((x, y) => value);
        }
        public static TransformableFunction CustomFunction(Expression<Func<int, int, float>> funExpression)
        {
            return new TransformableFunction(funExpression);
        }
    }
    
}
