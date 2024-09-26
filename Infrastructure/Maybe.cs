using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure
{
    public class Maybe<T> :IEnumerable<T> 
    {
        private readonly IEnumerable<T> _values;

        public Maybe()
        {
            _values = Enumerable.Empty<T>();
        }
        public Maybe(T value)
        {
            _values = value == null ? Enumerable.Empty<T>() : Enumerable.Repeat(value, 1);
        }
        public bool HasValue
        {
            get { return _values.Any(); }
        }
        public IEnumerator<T> GetEnumerator()
        {
            return _values.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
    }
}