namespace NoNameGame.Configuration.InAppPurchase
{
    public class LevelPackInAppPurchaseInfo
    {
        public int LevelPackId { get; private set; }
        public bool IsPurchasable
        {
            get { return ProductId != null; }
        }
        public string ProductId { get; private set; }

        public LevelPackInAppPurchaseInfo(int levelPackId,string productId)
        {
            ProductId = productId;
            LevelPackId = levelPackId;
        }
    }
}
