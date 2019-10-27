using System;
using Kit;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Kit
{
    public static partial class Misc
    {
        public static T Instantiate<T>(T source, Transform parent = null, string name = null)
            where T : UnityEngine.Object
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

        public static T InstantiatePrefab<T>(string resourcesPath, Transform parent = null, string name = null)
            where T : UnityEngine.Object
        {
            var rsrc = Resources.Load<T>(resourcesPath);

            if (!rsrc)
                throw new Exception($"oups, no resource for \"{resourcesPath}\"");

            if (name == null)
                name = resourcesPath.Split('/').Last();

            return InstantiatePrefab(rsrc, parent, name);
        }

        public static T InstantiatePrefab<T>(string resourcesPath, GameObject parent = null, string name = null)
            where T : UnityEngine.Object =>
            InstantiatePrefab<T>(resourcesPath, parent.transform, name);

        public static T InstantiatePrefab<T>(string resourcesPath, Component parent = null, string name = null)
            where T : UnityEngine.Object =>
            InstantiatePrefab<T>(resourcesPath, parent.transform, name);

        public static GameObject InstantiatePrefab(string resourcesPath, Transform parent = null, string name = null)
        {
            var rsrc = Resources.Load<GameObject>(resourcesPath);

            if (!rsrc)
                throw new Exception($"oups, no resource for \"{resourcesPath}\"");

            return InstantiatePrefab(rsrc, parent, name);
        }

        public static GameObject InstantiatePrefab(string resourcesPath, GameObject parent = null, string name = null) =>
            InstantiatePrefab(resourcesPath, parent.transform, name);

        public static GameObject InstantiatePrefab(string resourcesPath, Component parent = null, string name = null) =>
            InstantiatePrefab(resourcesPath, parent.transform, name);
    }
}
