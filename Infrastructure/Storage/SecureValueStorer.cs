using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Storage
{
    public class SecureValueStorer<TKey, TValue> : IValueStorer<TKey, TValue>
    {
        private readonly IValueStorer<TKey, byte[]> _innerValueStorer;
        private readonly Converter<string, TValue> _converter;
        /// <summary>
        /// Converter that can extract value of Type TValue from string
        /// </summary>
        /// <param name="innerValueStorer"></param>
        /// <param name="converter"></param>
        public SecureValueStorer(IValueStorer<TKey, byte[]> innerValueStorer, Converter<string, TValue> converter)
        {
            if (innerValueStorer == null) throw new ArgumentNullException("innerValueStorer");
            if (converter == null) throw new ArgumentNullException("converter");
            _innerValueStorer = innerValueStorer;
            _converter = converter;
        }
        public void Write(TKey id, TValue value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value.ToString());
            byte[] protect = ProtectedData.Protect(bytes, null);

            _innerValueStorer.Write(id, protect);
        }
        public TValue Read(TKey id, TValue defaultValue)
        {
            var bytes = AppSettingsAccessor.GetValue<byte[]>(id.ToString())
                .SingleOrDefault();
            if (bytes == null)
                return defaultValue;

            var unprotected = ProtectedData.Unprotect(bytes, null);
            var str = Encoding.UTF8.GetString(unprotected, 0, unprotected.Length);
            return _converter.Invoke(str);
        }
    }
}