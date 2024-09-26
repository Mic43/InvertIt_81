using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;

namespace AnimationLib
{
    public interface IAnimationDelayFunction
    {
        TimeSpan ComputeDelay(int coordX, int cooordY);    
    }

    public class SteppingAnimationDelayFuncion : IAnimationDelayFunction
    {
        private AnimationFunction animationFunction;        
        public TimeSpan DelayStepValue { get; private set; }
        public TimeSpan InitialDelayValue { get; set; }
        

        private SteppingAnimationDelayFuncion(TimeSpan delayStepValue,TimeSpan initialDelayValue)
        {
            InitialDelayValue = initialDelayValue;
            DelayStepValue = delayStepValue;
        }
        private SteppingAnimationDelayFuncion(TimeSpan delayStepValue)
            : this(delayStepValue, TimeSpan.Zero)
        {
            
        }

        public SteppingAnimationDelayFuncion SetInitialDelay(TimeSpan initialDelayValue)
        {
            InitialDelayValue = initialDelayValue;
            return this;
        }

        public TimeSpan ComputeDelay(int coordX, int coordY)
        {
            return TimeSpan.FromTicks((long)(animationFunction.Function(coordX, coordY) * DelayStepValue.Ticks + InitialDelayValue.Ticks));
        }
        public static SteppingAnimationDelayFuncion CreateUniform()
        {
            var ret = new SteppingAnimationDelayFuncion(TimeSpan.Zero);
            ret.animationFunction = AnimationFunction.FromTransformable(TransformableFunction.ZeroFunction());            
            return ret; 
        }
        public static SteppingAnimationDelayFuncion CreateUpDown(TimeSpan stepValue, int initialDelayMs = 0,int startVerticalOffset = 0)
        {
            var ret = new SteppingAnimationDelayFuncion(stepValue,TimeSpan.FromMilliseconds(initialDelayMs));            
            ret.animationFunction = AnimationFunction.FromTransformable(TransformableFunction.VerticalFunction().
                Translate(0,startVerticalOffset));
            return ret; 
        }
        public static SteppingAnimationDelayFuncion CreateDownUp(TimeSpan stepValue,int startVerticalOffset)
        {
            var ret = new SteppingAnimationDelayFuncion(stepValue);
            ret.animationFunction = AnimationFunction.FromTransformable(TransformableFunction.VerticalFunction()             
                .SymmetryOX()
                .Translate(0,startVerticalOffset));
            return ret;
        }
        public static SteppingAnimationDelayFuncion CreateCircular(TimeSpan stepValue,Point center)
        {
            var ret = new SteppingAnimationDelayFuncion(stepValue);
            ret.animationFunction = AnimationFunction.FromTransformable(TransformableFunction.CircularFunction().Translate((int)center.X, (int)center.Y));
            return ret;
        }
        public static SteppingAnimationDelayFuncion CreateArc(TimeSpan stepValue, Point center,bool clockwise = true)
        {
            var ret = new SteppingAnimationDelayFuncion(stepValue);

            var tranFun = TransformableFunction.ArcFunction();
            if (clockwise)
                tranFun = tranFun.SymmetryOX();
            tranFun = tranFun.Translate((int)center.X, (int)center.Y);
            ret.animationFunction = AnimationFunction.FromTransformable(tranFun);
            return ret;
        }
        public static SteppingAnimationDelayFuncion CreateDiagonal(TimeSpan stepValue, int maxOffset)
        {
            var ret = new SteppingAnimationDelayFuncion(stepValue);
            var tranFun = TransformableFunction.DiagonalFunction().SymmetryOY().Translate(maxOffset, maxOffset);
            ret.animationFunction = AnimationFunction.FromTransformable(tranFun);
            return ret;            
        }
        public static SteppingAnimationDelayFuncion CreateRandom(TimeSpan stepValue,float randomCoef)
        {
            var ret = new SteppingAnimationDelayFuncion(stepValue);
            var tranFun = TransformableFunction.ConstFunction(1).RandomizeValue(randomCoef);
            ret.animationFunction = AnimationFunction.FromTransformable(tranFun);
            return ret;
        }
        public static SteppingAnimationDelayFuncion CreateLinear(TimeSpan stepValue, int maxOffset,bool randomize=false)
        {
            var ret = new SteppingAnimationDelayFuncion(stepValue);
            var tranFun = TransformableFunction.CustomFunction((x, y) => maxOffset * y + x).SymmetryOX().Translate(0, maxOffset);
            if (randomize)
                tranFun = tranFun.RandomizeValue(0.2f);
            ret.animationFunction = AnimationFunction.FromTransformable(tranFun);
            return ret;
//            + Math.Sign(((y % 2) -1)
        }
        


    }
    //public class VerticalAnimationDelayFunction : SteppingAnimationDelayFuncion
    //{
    //    public VerticalAnimationDelayFunction(TimeSpan delayStepValue)
    //        : base(delayStepValue)
    //    {

    //    }
    //    public override TimeSpan ComputeDelay(int coordX, int coordY)
    //    {
    //        return TimeSpan.FromTicks(coordY * DelayStepValue.Ticks); 
    //    }
    //}
    //public class HorizontalAnimationDelayFunction : SteppingAnimationDelayFuncion
    //{
    //    public HorizontalAnimationDelayFunction(TimeSpan delayStepValue)
    //        : base(delayStepValue)
    //    {

    //    }
    //    public override TimeSpan ComputeDelay(int coordX, int coordY)
    //    {
    //        return TimeSpan.FromTicks(coordX * DelayStepValue.Ticks);
    //    }
    //}
    //public class UniformAnimationDelayFunction : IAnimationDelayFunction
    //{
    //    public TimeSpan DelayValue { get; private set; }
    //    public UniformAnimationDelayFunction(TimeSpan delayValue)
    //    {
    //        DelayValue = delayValue;
    //    }
    //    public TimeSpan ComputeDelay(int coordX, int coordY)
    //    {
    //        return DelayValue;
    //    }
    //}
    //public class DiagonalAnimationDelayFunction : SteppingAnimationDelayFuncion
    //{
    //    public DiagonalAnimationDelayFunction(TimeSpan delayStepValue)
    //        : base(delayStepValue)
    //    {

    //    }
    //    public override TimeSpan ComputeDelay(int coordX, int coordY)
    //    {
    //        return TimeSpan.FromTicks((coordY - coordX +5) * DelayStepValue.Ticks);
    //    }
    //}
    //public class CircularAnimationDelayFunction : SteppingAnimationDelayFuncion
    //{
    //    public CircularAnimationDelayFunction(TimeSpan delayStepValue)
    //        : base(delayStepValue)
    //    {

    //    }
    //    public override TimeSpan ComputeDelay(int coordX, int coordY)
    //    {
    //       return TimeSpan.FromTicks((int)Math.Sqrt((coordX -5) *(coordX - 5) +(coordY - 5)*(coordY - 5)) * DelayStepValue.Ticks);
    //    }
    //}
    //public class AbsAnimationDelayFunction : SteppingAnimationDelayFuncion
    //{
    //    public AbsAnimationDelayFunction(TimeSpan delayStepValue)
    //        : base(delayStepValue)
    //    {

    //    }
    //    public override TimeSpan ComputeDelay(int coordX, int coordY)
    //    {
    //        return TimeSpan.FromTicks((int)Math.Abs(coordX - coordY) * DelayStepValue.Ticks);
    //    }
    //}


}
