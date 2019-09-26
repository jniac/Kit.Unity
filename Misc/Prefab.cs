using System;
using Kit.Utils;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Kit.Unity
{
    public static partial class Misc
    {
        public static GameObject InstantiatePrefab(GameObject source, Transform parent = null, string name = null)
        {
            if (source == null)
                throw new Exception("oups, source is null!!!");

            #if UNITY_EDITOR
            var instance = (GameObject)PrefabUtility.InstantiatePrefab(source);
            #else
            var instance =  (GameObject)UnityEngine.Object.Instantiate(source);
            #endif

            if (parent)
                instance.transform.SetParent(parent, false);

            if (name != null)
                instance.name = name;

            return instance;
        }
    }
}
