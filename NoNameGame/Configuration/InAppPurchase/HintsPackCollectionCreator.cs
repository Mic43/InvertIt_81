using NoNameGame.Controllers.Hints.HintsInAppProducts;

namespace NoNameGame.Configuration.InAppPurchase
{
    static class HintsPackCollectionCreator
    {
        public static HintPackInAppProductCollection Create()
        {
            return new HintPackInAppProductCollection()
            {
                new HintPackInAppProduct("HintPack_10", 10),
                new HintPackInAppProduct("HintPack_20", 20),
                new HintPackInAppProduct("HintPack_50", 50),
                new HintPackInAppProduct("HintPack_100", 100)
            };
        }
    }
}