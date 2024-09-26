using System;

namespace Infrastructure.Storage
{
    public class ApsSettingsValueStorer<TKey,TValue> : IValueStorer<TKey, TValue>
    {
        private readonly Func<TKey, string> _stringKeyGenerator;
        public ApsSettingsValueStorer(Func<TKey, string> stringKeyGenerator)
        {
            if (stringKeyGenerator == null) throw new ArgumentNullException("stringKeyGenerator");
            _stringKeyGenerator = stringKeyGenerator;
        }

        public ApsSettingsValueStorer() : this(key => key.ToString())
        {
        }
        public void Write(TKey id, TValue value)
        {
            AppSettingsAccessor.AddOrUpdateValue(_stringKeyGenerator(id),value);
            AppSettingsAccessor.Save();
        }
        public TValue Read(TKey id, TValue defaultValue)
        {
            return AppSettingsAccessor.GetValueOrDefault(_stringKeyGenerator(id), defaultValue);
        }
    }
}