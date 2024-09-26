using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using AnimationLib;
using NoNameGame.Controllers.PeriodicAnimations;

namespace NoNameGame.Configuration.Animations.Periodic
{
    public sealed class AnimationDirectionDictionary : ReadOnlyDictionary<AnimationDirectionType, AnimationDirection>
    {
        public static AnimationDirectionType DefaultAnimationDirection = AnimationDirectionType.Circle;

        private static readonly List<AnimationDirection> _values = new List<AnimationDirection>
        {
            new AnimationDirection(
                boardSize => SteppingAnimationDelayFuncion.CreateDiagonal(TimeSpan.FromMilliseconds(150), boardSize),
                AnimationDirectionType.Diagonal,
                "Diagonal"),
            new AnimationDirection(
                boardSize => SteppingAnimationDelayFuncion.CreateDownUp(TimeSpan.FromMilliseconds(150), boardSize),
                AnimationDirectionType.Vertical,
                "Vertical"),
            new AnimationDirection(
                boardSize => SteppingAnimationDelayFuncion.CreateCircular(TimeSpan.FromMilliseconds(150), new Point(boardSize/2,boardSize/2)),
                AnimationDirectionType.Circle,
                "Circular"),
            new AnimationDirection(
                boardSize => SteppingAnimationDelayFuncion.CreateArc(TimeSpan.FromMilliseconds(150), new Point(boardSize/2,boardSize/2)),
                AnimationDirectionType.Arc,
                "Clock"),
            new AnimationDirection(
                boardSize => SteppingAnimationDelayFuncion.CreateLinear(TimeSpan.FromMilliseconds(50),boardSize),
                AnimationDirectionType.Linear,
                "Linear")
        };
        private static Dictionary<AnimationDirectionType, AnimationDirection> CreateDirections()
        {
            return _values.ToDictionary(x => x.AnimationDirectionType);
        }
        public AnimationDirectionDictionary()
            : base(CreateDirections())
        {
        }
    }
}