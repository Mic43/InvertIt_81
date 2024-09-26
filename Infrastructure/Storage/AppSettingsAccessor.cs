using System.IO.IsolatedStorage;

namespace Infrastructure.Storage
{
    public static class AppSettingsAccessor
    {
        private static object _padlock = new object();
        public static IsolatedStorageSettings Settings = IsolatedStorageSettings.ApplicationSettings;

        public static void AddOrUpdateValue<T>(string key, T value)
        {
//
            lock (_padlock)
            {
                if (!Settings.Contains(key))
                    Settings.Add(key, value);
                else
                    Settings[key] = value;     
                
            }
        }
        public static T GetValueOrDefault<T>(string key,T defaultValue)
        {
            lock (_padlock)
            {
                T value;
                if (IsolatedStorageSettings.ApplicationSettings.TryGetValue(key, out value))
                    return value;
                else
                    return defaultValue;
            }
        }
        public static Maybe<T> GetValue<T>(string key)
        {
            lock (_padlock)
            {
                T value;
                if (IsolatedStorageSettings.ApplicationSettings.TryGetValue(key, out value))
                    return new Maybe<T>(value);
                else
                {
                    return new Maybe<T>();
                }
            }

        }
        public static void Save()
        {
            lock (_padlock)
            {
                Settings.Save();
            }
        }
      
    }
}