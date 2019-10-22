using System;
using System.Collections.Generic;
using System.Linq;

namespace Kit
{
    public partial class Anim
    {
        // NOTE: same role as Event.Register (less secure implementation),
        // should be merged ?

        class DictionarySet<TKey, TValue>
        {
            Dictionary<TKey, HashSet<TValue>> dict = new Dictionary<TKey, HashSet<TValue>>();

            public void Add(TKey key, TValue value)
            {
                if (!dict.TryGetValue(key, out HashSet<TValue> values))
                {
                    values = new HashSet<TValue>();
                    dict.Add(key, values);
                }

                values.Add(value);
            }

            public void Remove(TKey key, TValue value)
            {
                var values = dict[key];
                values.Remove(value);

                if (values.Count == 0)
                    dict.Remove(key);
            }

            public TValue[] Get(TKey key, bool returnEmptyArrayIfNull)
            {
                if (dict.TryGetValue(key, out HashSet<TValue> values))
                    return values.ToArray();

                return returnEmptyArrayIfNull ? new TValue[0] : null;
            }
        }
    }
}
