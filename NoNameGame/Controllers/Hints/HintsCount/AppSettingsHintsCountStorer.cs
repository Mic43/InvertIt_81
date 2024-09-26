using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Infrastructure.Storage;
using NoNameGame.Configuration;

namespace NoNameGame.Controllers.Hints.HintsCount
{
    public class AppSettingsHintsCountStorer : IHintsCountDecreaser,IHintsCountIncreaser,IHintsCountProvider   
    {

        private int GetCurrentHintsCount()
        {
            var bytes = AppSettingsAccessor.GetValue<byte[]>(AppSettingsKeys.HintsCount)                    
                    .SingleOrDefault();
            if (bytes == null)
                return Constants.DefaultHintsCount;

            var unprotected = ProtectedData.Unprotect(bytes, null);
            var str = Encoding.UTF8.GetString(unprotected, 0, unprotected.Length);
            return int.Parse(str);
        }
        private void SetCurrentHintsCount(int value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value.ToString());
            byte[] protect = ProtectedData.Protect(bytes, null);

            AppSettingsAccessor.AddOrUpdateValue(AppSettingsKeys.HintsCount, protect);
        }

        public void Decrement()
        {
            SetCurrentHintsCount(GetCurrentHintsCount() - 1);

            //AppSettingsAccessor.AddOrUpdateValue(AppSettingsKeys.HintsCount,Get() -1);
        }
        public void Increase(int value)
        {
            SetCurrentHintsCount(GetCurrentHintsCount() + value);
//            AppSettingsAccessor.AddOrUpdateValue(AppSettingsKeys.HintsCount, Get() + value);
            
        }
        public int Get()
        {
            return GetCurrentHintsCount();

//            return
//                AppSettingsAccessor.GetValue<int>(AppSettingsKeys.HintsCount)
//                    .DefaultIfEmpty(Constants.DefaultHintsCount)
//                    .Single();
        }
}
}