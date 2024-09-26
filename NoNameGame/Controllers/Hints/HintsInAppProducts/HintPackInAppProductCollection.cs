using System.Collections.ObjectModel;

namespace NoNameGame.Controllers.Hints.HintsInAppProducts
{
    public class HintPackInAppProductCollection : KeyedCollection<string, HintPackInAppProduct>
    {
        protected override string GetKeyForItem(HintPackInAppProduct item)
        {
            return item.AppProductId;
        }
    }
}