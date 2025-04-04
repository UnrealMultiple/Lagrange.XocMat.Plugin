using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CircularDictionary<TKey, TValue> : IDictionary<TKey, TValue>
{
    private readonly Dictionary<TKey, TValue> _dictionary;
    private readonly Queue<TKey> _keyQueue;
    private readonly int _maxSize;

    public CircularDictionary(int maxSize)
    {
        if (maxSize <= 0)
        {
            throw new ArgumentException("Max size must be greater than zero.", nameof(maxSize));
        }

        _maxSize = maxSize;
        _dictionary = new Dictionary<TKey, TValue>(maxSize);
        _keyQueue = new Queue<TKey>(maxSize);
    }

    public TValue this[TKey key]
    {
        get => _dictionary[key];
        set
        {
            if (_dictionary.ContainsKey(key))
            {
                _dictionary[key] = value;
            }
            else
            {
                Add(key, value);
            }
        }
    }

    public ICollection<TKey> Keys => _dictionary.Keys;
    public ICollection<TValue> Values => _dictionary.Values;
    public int Count => _dictionary.Count;
    public bool IsReadOnly => false;

    public void Add(TKey key, TValue value)
    {
        if (_dictionary.ContainsKey(key))
        {
            throw new ArgumentException("An item with the same key has already been added.", nameof(key));
        }

        if (_keyQueue.Count >= _maxSize)
        {
            var oldestKey = _keyQueue.Dequeue();
            _dictionary.Remove(oldestKey);
        }

        _dictionary.Add(key, value);
        _keyQueue.Enqueue(key);
    }

    public void Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);

    public void Clear()
    {
        _dictionary.Clear();
        _keyQueue.Clear();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item) => 
        _dictionary.Contains(item);

    public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).CopyTo(array, arrayIndex);
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => 
        _dictionary.GetEnumerator();

    public bool Remove(TKey key)
    {
        if (_dictionary.Remove(key))
        {
            // 从队列中移除key，这需要重建队列
            _keyQueue.Clear();
            foreach (var k in _dictionary.Keys)
            {
                _keyQueue.Enqueue(k);
            }
            return true;
        }
        return false;
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        if (((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).Remove(item))
        {
            // 从队列中移除key，这需要重建队列
            _keyQueue.Clear();
            foreach (var k in _dictionary.Keys)
            {
                _keyQueue.Enqueue(k);
            }
            return true;
        }
        return false;
    }

    public bool TryGetValue(TKey key, out TValue value) => 
        _dictionary.TryGetValue(key, out value);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public int MaxSize => _maxSize;
}