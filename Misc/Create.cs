using System;
using UnityEngine;

namespace Kit
{
    public static partial class Misc
    {
        public static T Create<T>(Transform parent, string name = null) 
            where T : Component
        {
            var go = new GameObject(name);

            go.transform.SetParent(parent, false);

            return go.AddComponent<T>();
        }
    }
}
