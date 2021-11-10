using UnityEngine;

namespace Utils
{
    class MoreRandom
    {
        public static T RandomChoice<T>(T[] items)
        {
            if (items.Length == 0) {
                Debug.LogWarning("Can't make choice of zero items");
            }
            return items[Mathf.Min(Mathf.FloorToInt(Random.value * items.Length), items.Length - 1)];
        }
    }
}