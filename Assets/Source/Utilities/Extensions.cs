using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    /// <summary>
    /// Sets the value at a key in the dictionary if it exists.
    /// Adds the key and value if the key does not exist.
    /// </summary>
    /// <typeparam name="TKey">
    /// The key's type.
    /// </typeparam>
    /// <typeparam name="TValue">
    /// The value's type.
    /// </typeparam>
    /// <param name="dictionary">
    /// The dictionary that is being set.
    /// </param>
    /// <param name="key">
    /// The key to access/insert.
    /// </param>
    /// <param name="value">
    /// The value to set.
    /// </param>
    public static void Set<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        if (dictionary.TryGetValue(key, out TValue _))
        {
            dictionary[key] = value;
        }
        else
        {
            dictionary.Add(key, value);
        }
    }
}
