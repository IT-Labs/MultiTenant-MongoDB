using System;
using System.Collections.Concurrent;

namespace ItLabs.MultiTenant.Core
{
    /// <summary>
    /// Concurrent Dictionary cache implementation 
    /// </summary>
    public class ConcurrentDictionaryCache : ICache
    {
        private readonly ConcurrentDictionary<string, object> _dictionary;

        public ConcurrentDictionaryCache(ConcurrentDictionary<string, object> dictionary)
        {
            _dictionary = dictionary;
        }

        public bool Exists(string key)
        {
            return _dictionary.ContainsKey(key);
        }

        public T Get<T>(string key)
        {
            if (_dictionary.ContainsKey(key))
                return (T)_dictionary[key];

            return default;
        }

        public T Set<T>(string key, T value)
        {
            return (T)_dictionary.AddOrUpdate(key, value, (k, v) => { return value; });
        }

        public T GetOrSet<T>(string key, Func<T> valueBuilder)
        {
            return Exists(key) ? Get<T>(key) : Set(key, valueBuilder());
        }

        public void Remove(string key)
        {
            _dictionary.TryRemove(key, out _);
        }

        public void Clear()
        {
            _dictionary.Clear();
        }

        public int Count()
        {
            return _dictionary.Count;
        }
    }
}
