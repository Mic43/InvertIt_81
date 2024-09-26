using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Media.Animation;
using AnimationLib.AnimationsCreator.Interfaces;

namespace AnimationLib.AnimationsCreator.MutliAnimation
{
    public class MultiAnimationCreator : IMultiUIElementsAnimationCreator
    {
        private readonly Func<int,int,IUIElementAnimationCreator> _singleElementAnimationCreator;
        private readonly IAnimationDelayFunction _delayFunction;
        
        public MultiAnimationCreator(IUIElementAnimationCreator singleElementAnimationCreator,
            IAnimationDelayFunction delayFunction) : this((x,y) => singleElementAnimationCreator,delayFunction)
        {
         
        }
        public MultiAnimationCreator(Func<int,int,IUIElementAnimationCreator> singleElementAnimationCreator,
            IAnimationDelayFunction delayFunction)
        {
            if (singleElementAnimationCreator == null) throw new ArgumentNullException("singleElementAnimationCreator");
            if (delayFunction == null) throw new ArgumentNullException("delayFunction");

            _singleElementAnimationCreator = singleElementAnimationCreator;
            _delayFunction = delayFunction;
        }


        public Storyboard Create(UIElement[,] elementsToAnimate)
        {
            var alterntiveMultiAnimationCreator = new GenericMultiAnimationCreator<Tuple<int, int>>(
                element => elementsToAnimate.CoordinatesOf(element),
                tuple => _delayFunction.ComputeDelay(tuple.Item1, tuple.Item2),
                tuple => _singleElementAnimationCreator(tuple.Item1, tuple.Item2));

            return alterntiveMultiAnimationCreator.Create(elementsToAnimate.OfType<UIElement>());
//            if (elementsToAnimate == null) throw new ArgumentNullException("elementsToAnimate");
//
//            var sb = new Storyboard();
//
//            for (int i = 0; i < elementsToAnimate.GetLength(0); i++)
//            {
//                for (int j = 0; j < elementsToAnimate.GetLength(1); j++)
//                {
//                    TimeSpan singleElementAnimDelay = _delayFunction.ComputeDelay(i, j);
//                    Storyboard singleElelementAnimation = _singleElementAnimationCreator(i,j).Create(elementsToAnimate[i, j]);
//                    singleElelementAnimation.Children.ToList().ForEach(anim => anim.BeginTime = singleElementAnimDelay);
//
//                    sb.Children.Add(singleElelementAnimation);
//                }
//            }
//            return sb;
        }
    }

    public class GenericMultiAnimationCreator<TCoordinate> : IGenericMultiAnimationCreator
    {
        private readonly Func<UIElement, TCoordinate> _coordinateFunc;
        private readonly Func<TCoordinate, TimeSpan> _delayFunc;
        private readonly Func<TCoordinate, IUIElementAnimationCreator> _singleElementAnimationCreatorProvider;
        public GenericMultiAnimationCreator(Func<UIElement, TCoordinate> coordinateFunc, Func<TCoordinate, TimeSpan> delayFunc,
            Func<TCoordinate, IUIElementAnimationCreator> singleElementAnimationCreatorProvider)
        {
            _coordinateFunc = coordinateFunc;
            _delayFunc = delayFunc;
            _singleElementAnimationCreatorProvider = singleElementAnimationCreatorProvider;
        }
        public virtual Storyboard Create(IEnumerable<UIElement> elementsToAnimate)
        {
            if (elementsToAnimate == null) throw new ArgumentNullException("elementsToAnimate");

            var sb = new Storyboard();
            long coordAcc =0 ;
            long singleElemAcc =0;
            long delayAcc =0;
            Stopwatch sw = new Stopwatch();
            foreach (var element in elementsToAnimate)
            {
                sw.Reset();
                sw.Start();
                var elementCoordinate = _coordinateFunc(element);
                sw.Stop();
                coordAcc += sw.ElapsedMilliseconds;

                sw.Reset();
                sw.Start();
                TimeSpan singleElementAnimDelay = _delayFunc(elementCoordinate);
                sw.Stop();
                delayAcc += sw.ElapsedMilliseconds;

                sw.Reset();
                sw.Start();
                Storyboard singleElelementAnimation = _singleElementAnimationCreatorProvider(elementCoordinate).Create(element);
                sw.Stop();
                singleElemAcc += sw.ElapsedMilliseconds;

                singleElelementAnimation.Children.ToList().ForEach(anim => anim.BeginTime = singleElementAnimDelay);

                sb.Children.Add(singleElelementAnimation); 
            }          
//            Debug.WriteLine("CoordinateFunc {0}",coordAcc);
//            Debug.WriteLine("elemCreation {0}", singleElemAcc);
//            Debug.WriteLine("delayAcc {0}", delayAcc);
            return sb;
        }
    }


    public class CachingMultiAnimationCreator : IGenericMultiAnimationCreator
    {
        private readonly IGenericMultiAnimationCreator _multiAnimationCreator;
        private Storyboard _cachedStoryboard;

        public CachingMultiAnimationCreator(IGenericMultiAnimationCreator multiAnimationCreator)
        {
            _multiAnimationCreator = multiAnimationCreator;
        }
        public Storyboard Create(IEnumerable<UIElement> elements)
        {
            return _cachedStoryboard ?? (_cachedStoryboard = _multiAnimationCreator.Create(elements));
        }
    }
    public static class ArrayExt
    {
        public static Tuple<int, int> CoordinatesOf<T>(this T[,] matrix, T value)
        {
            int w = matrix.GetLength(0); // width
            int h = matrix.GetLength(1); // height

            for (int x = 0; x < w; ++x)
            {
                for (int y = 0; y < h; ++y)
                {
                    if (matrix[x, y].Equals(value))
                        return Tuple.Create(x, y);
                }
            }

            return Tuple.Create(-1, -1);
        }
    }
}