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
        public static GameObject Instantiate(GameObject source, Transform parent = null, string name = null)
        {
            if (source == null)
                throw new Exception("oups, source is null!!!");

            var instance = parent ? 
                UnityEngine.Object.Instantiate(source, parent):
                UnityEngine.Object.Instantiate(source);

            if (name != null)
                instance.name = name;

            return instance;
        }

        public static T InstantiatePrefab<T>(T source, Transform parent = null, string name = null)
            where T : UnityEngine.Object
        {
            if (source == null)
                throw new Exception("oups, source is null!!!");

#if UNITY_EDITOR
            var instance = parent ?
                (T)PrefabUtility.InstantiatePrefab(source, parent) :
                (T)PrefabUtility.InstantiatePrefab(source);
#else
            var instance = parent ? 
                (T)UnityEngine.Object.Instantiate(source, parent):
                (T)UnityEngine.Object.Instantiate(source);
#endif

            if (name != null)
                instance.name = name;

            return instance;
        }

        public static GameObject InstantiatePrefab(string resourcesPath, Transform parent = null, string name = null)
        {
            var r = Resources.Load<GameObject>(resourcesPath);

            return InstantiatePrefab(r, parent, name);
        }
    }
}
