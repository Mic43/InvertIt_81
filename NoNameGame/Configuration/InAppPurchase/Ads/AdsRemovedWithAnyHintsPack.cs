using System;
using Infrastructure.Storage;

namespace NoNameGame.Configuration.InAppPurchase.Ads
{
    public class AdsRemovedWithAnyHintsPack : IAdsRemovalProvider
    {
        private readonly IValueStorer<string, bool> _valueStorer;
        public AdsRemovedWithAnyHintsPack(IValueStorer<string,bool> valueStorer )
        {
            if (valueStorer == null) throw new ArgumentNullException("valueStorer");
            _valueStorer = valueStorer;
        }
        public bool AreRemoved()
        {
            return _valueStorer.Read(AppSettingsKeys.IsAnyHintsPackPurchased, false);
        }
    }
}