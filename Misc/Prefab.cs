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
            var instance = parent ?
                (GameObject)PrefabUtility.InstantiatePrefab(source, parent) :
                (GameObject)PrefabUtility.InstantiatePrefab(source);
#else
            var instance = parent ? 
                (GameObject)UnityEngine.Object.Instantiate(source, parent):
                (GameObject)UnityEngine.Object.Instantiate(source);
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
