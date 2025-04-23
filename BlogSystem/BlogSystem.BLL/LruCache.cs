using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.BLL
{
    public class LruCache<TKey, TValue>
    {
        private readonly int _maxCapacity;
        private Dictionary<TKey, LinkedListNode<KeyValuePair<TKey, TValue>>> _cacheMap;
        private LinkedList<KeyValuePair<TKey, TValue>> _lruList;

        public LruCache(int capacity)
        {
            _maxCapacity = capacity;
            _cacheMap = new Dictionary<TKey, LinkedListNode<KeyValuePair<TKey, TValue>>>();
            _lruList = new LinkedList<KeyValuePair<TKey, TValue>>();
        }

        public TValue Get(TKey key)
        {
            if (_cacheMap.TryGetValue(key, out var node))
            {
                // Item has been accessed, move it to the front of the list
                _lruList.Remove(node);
                _lruList.AddFirst(node);
                return node.Value.Value;
            }
            return default(TValue); // Return default value if key not found
        }

        public void Set(TKey key, TValue value)
        {
            if (_cacheMap.TryGetValue(key, out var node))
            {
                // Update the value and move it to the front
                _lruList.Remove(node);
                _lruList.AddFirst(new LinkedListNode<KeyValuePair<TKey, TValue>>(new KeyValuePair<TKey, TValue>(key, value)));
                _cacheMap[key] = _lruList.First;
            }
            else
            {
                if (_cacheMap.Count >= _maxCapacity)
                {
                    // Remove least recently used item from cache
                    _cacheMap.Remove(_lruList.Last.Value.Key);
                    _lruList.RemoveLast();
                }
                // Add new item to cache
                _lruList.AddFirst(new KeyValuePair<TKey, TValue>(key, value));
                _cacheMap[key] = _lruList.First;
            }
        }
    }

}
