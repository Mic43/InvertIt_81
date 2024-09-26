using System;

namespace NoNameGame.Controllers.Hints.HintsInAppProducts
{
    public class HintPackInAppProduct
    {
        public string AppProductId { get; private set; }
        public int HintsCount { get; private set; }

        public HintPackInAppProduct(string appProductId, int hintsCount)
        {
            if (appProductId == null) throw new ArgumentNullException("appProductId");
            if(hintsCount <=0) throw new ArgumentOutOfRangeException("hintsCount");

            AppProductId = appProductId;
            HintsCount = hintsCount;
        }
    }
}