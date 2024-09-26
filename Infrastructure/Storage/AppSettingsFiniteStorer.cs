using System;
using System.Collections.Generic;

namespace Infrastructure.Storage
{
    public class AppSettingsFiniteStorer<TKey, TValue> : IFiniteTypeStorer<TKey, TValue>
    {     
        private readonly Func<TKey, string> _stringKeyGenerator;
        private readonly Dictionary<TKey, TValue> _defaultValues;

        public AppSettingsFiniteStorer(Func<TKey, string> stringKeyGenerator,Dictionary<TKey, TValue> defaultValues)
        {
          
            if (stringKeyGenerator == null) throw new ArgumentNullException("stringKeyGenerator");
            if (defaultValues == null) throw new ArgumentNullException("defaultValues");
            _stringKeyGenerator = stringKeyGenerator;
            _defaultValues = defaultValues;
        }
        public TValue Read(TKey key)
        {
            return AppSettingsAccessor.GetValueOrDefault(_stringKeyGenerator(key), _defaultValues[key]);
        }
        public void Write(TKey key, TValue value)
        {
            AppSettingsAccessor.AddOrUpdateValue(_stringKeyGenerator(key), value);
            AppSettingsAccessor.Save();
        }
    }
    

//    public class Unlocker<T> :IUnlocker<T>
//    {
//        private readonly Dictionary<T, bool> _defaultLockStateCollection;
//        private readonly Func<T, string> _keyGenerator;
//        
//        public Unlocker(Dictionary<T, bool> defaultLockStateCollection, Func<T, string> keyGenerator)
//        {
//            if (defaultLockStateCollection == null) throw new ArgumentNullException("defaultLockStateCollection");
//            if (keyGenerator == null) throw new ArgumentNullException("keyGenerator");
//
//            _defaultLockStateCollection = defaultLockStateCollection;
//            _keyGenerator = keyGenerator;
//          
//        }
//        public void Unlock(T item)
//        {
//            if (!_defaultLockStateCollection.ContainsKey(item))
//                throw new ArgumentException("Invalid item. It must be present in defaultLockStateCollection passsed in costructor", "item");
//            
//             SettingsAccessor.AddOrUpdateValue(_keyGenerator(item), false);
//        }
//        public bool IsLocked(T item)
//        {
//            if (!_defaultLockStateCollection.ContainsKey(item))
//                throw new ArgumentException("Invalid item. It must be present in defaultLockStateCollection passsed in costructor", "item");
//
//            return SettingsAccessor.GetValue<bool>(_keyGenerator(item))
//                .DefaultIfEmpty(_defaultLockStateCollection[item])
//                .Single();
//        }
//    }
//
//    public interface IUnlocker<T>
//    {
//        void Unlock(T item);
//        bool IsLocked(T item);
//    }
}