using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace AnimationLib
{
    public class FunctionTransformer
    {
        private Expression<Func<int, int, float>> function;
        
        private ParameterExpression GetXParamExp()
        {
            return function.Parameters.First();
        }
        private ParameterExpression GetYParamExp()
        {
            return function.Parameters[1];
        }
        public FunctionTransformer(Expression<Func<int, int, float>> functionToTransform)
        {
            function = functionToTransform;
        }
        public Func<int,int,float> Build()
        {
            return function.Compile();
        }
        public Expression<Func<int, int, float>> GetExpression()
        {
            return function;
        }
        public FunctionTransformer ApplyXTransform(Expression<Func<int, int>> xTransfrom)
        {
            Expression trasform = new ParametersExpressionReplacer(GetXParamExp()).Visit(xTransfrom.Body);
            function = Expression.Lambda<Func<int, int, float>>(new ParamWithExpressionReplacer(GetXParamExp(), trasform).Visit(function.Body), function.Parameters);
            return this;
        }
        public FunctionTransformer ApplyYTransform(Expression<Func<int, int>> yTransfrom)
        {
            Expression trasform = new ParametersExpressionReplacer(GetYParamExp()).Visit(yTransfrom.Body);
            function = Expression.Lambda<Func<int, int, float>>(new ParamWithExpressionReplacer(GetYParamExp(), trasform).Visit(function.Body), function.Parameters);
            return this;
        }
        public FunctionTransformer ApplyFunctionValueTransform(Expression<Func<float, float>> funValueTransform)
        {
            Expression transformed = new ParamWithExpressionReplacer(funValueTransform.Parameters.Single(), function.Body).Visit(funValueTransform.Body);
            function = Expression.Lambda<Func<int, int, float>>(transformed, function.Parameters);
            return this;
        }
    }
    public class ParamWithExpressionReplacer: ExpressionVisitor
    {
        private ParameterExpression parameter;
        private Expression parameterTransform;
        public ParamWithExpressionReplacer(ParameterExpression parameter,Expression parameterTransform)
        {
            this.parameter = parameter;
            this.parameterTransform = parameterTransform;
        }
        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (node == parameter)
                return parameterTransform;
            return base.VisitParameter(node);
        }
    }
    public class ParametersExpressionReplacer : ExpressionVisitor
    {
        ParameterExpression newParamExp;
        public ParametersExpressionReplacer(ParameterExpression newParamExp)
        {
            this.newParamExp = newParamExp;
        }
        protected override Expression VisitParameter(ParameterExpression node)
        {
            return newParamExp;            
        }
    }    
}
