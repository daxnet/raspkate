using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspkate.Controllers.Routing
{
    internal sealed class RouteValueCollection : IDictionary<string, object>
    {
        private readonly Dictionary<string, object> values = new Dictionary<string, object>();

        public void Add(string key, object value)
        {
            values.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return values.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get { return values.Keys; }
        }

        public bool Remove(string key)
        {
            return values.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return values.TryGetValue(key, out value);
        }

        public ICollection<object> Values
        {
            get { return values.Values; }
        }

        public object this[string key]
        {
            get
            {
                return values[key];
            }
            set
            {
                values[key] = value;
            }
        }

        public void Add(KeyValuePair<string, object> item)
        {
            values.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            values.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return values.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, object>>)values).CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return values.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return values.Remove(item.Key);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return values.GetEnumerator();
        }
    }
}
