using System;
using RotaryHeart.Lib.SerializableDictionary;

namespace Database
{
    [Serializable]
    public abstract class SDictionary<Key, Value> : SerializableDictionaryBase<Key, Value>
    {
    }
}