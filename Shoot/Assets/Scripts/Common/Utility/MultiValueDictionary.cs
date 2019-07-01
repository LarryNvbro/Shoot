using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiValueDictionary<TKey, TValue>
{
    Dictionary<TKey, List<TValue>> m_Dic = new Dictionary<TKey, List<TValue>>();

    public IEnumerable<TKey> Keys
    {
        get { return m_Dic.Keys; }
    }

    public List<TValue> this[TKey key]
    {
        get
        {
            List<TValue> list;
            if (!m_Dic.TryGetValue(key, out list))
            {
                list = new List<TValue>();
                m_Dic[key] = list;
            }
            return list;
        }
    }

    public void Add(TKey key, TValue value)
    {
        List<TValue> list;
        if (m_Dic.TryGetValue(key, out list))
        {
            list.Add(value);
        }
        else
        {
            list = new List<TValue>();
            list.Add(value);
            m_Dic[key] = list;
        }
    }

    public void Clear()
    {
        m_Dic.Clear();
    }

    public bool ContainsKey(TKey key)
    {
        return m_Dic.ContainsKey(key);
    }

    public bool Remove(TKey key)
    {
        return m_Dic.Remove(key);
    }
}
