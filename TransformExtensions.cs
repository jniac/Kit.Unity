using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Kit.Unity
{
    public static class TransformExtensions
    {
        public static bool IsParentOf(this Transform parent, Transform child)
            => child.IsChildOf(parent);

        public static bool IsAncestorOf(this Transform parent, Transform child)
        {
            while (child)
            {
                if (child.parent == parent)
                    return true;

                child = child.parent;
            }

            return false;
        }

        public static (Vector3 position, Vector3 scale, Quaternion rotation) GetPSR(this Transform transform)
            => (transform.localPosition, transform.localScale, transform.localRotation);

        public static void SetPSR(this Transform transform, (Vector3 position, Vector3 scale, Quaternion rotation) psr)
        {
            transform.localPosition = psr.position;
            transform.localScale = psr.scale;
            transform.localRotation = psr.rotation;
        }








        public static List<Transform> Collect(
            this Transform transform, 
            Func<Transform, bool> test, 
            List<Transform> list = null, 
            int recursiveLimit = -1, 
            bool includeSelf = false)
        {
            if (list == null)
                list = new List<Transform>();

            if (includeSelf && test(transform))
                list.Add(transform);

            foreach (Transform child in transform)
            {
                if (test(child))
                    list.Add(child);

                if (recursiveLimit != 0)
                    child.Collect(test, list, recursiveLimit--, includeSelf);
            }

            return list;
        }

        // regex
        public static List<Transform> Collect(
            this Transform transform, 
            Regex regex, 
            List<Transform> list = null, 
            int recursiveLimit = -1, 
            bool includeSelf = false)
            => transform.Collect(child => regex.Match(child.gameObject.name).Success, list, recursiveLimit, includeSelf);

        // string
        public static List<Transform> Collect(
            this Transform transform, 
            string name, 
            List<Transform> list = null, 
            int recursiveLimit = -1, 
            bool includeSelf = false)
        {
            if (name == "*")
                return transform.Collect(child => true, list, recursiveLimit, includeSelf);

            if (name.Length > 6 && name.Substring(0, 6) == "regex:")
                return transform.Collect(new Regex(name.Substring(6)), list, recursiveLimit, includeSelf);

            return transform.Collect(child => child.gameObject.name == name, list, recursiveLimit, includeSelf);
        }

        public static List<Transform> Collect(
            this Transform transform,
            List<Transform> list = null,
            int recursiveLimit = -1,
            bool includeSelf = false)
            => Collect(transform, "*", null, recursiveLimit, includeSelf);

        public static List<T> Collect<T>(
            this Transform transform,
            string name,
            List<Transform> list = null,
            int recursiveLimit = -1,
            bool includeSelf = false)
            where T : Component
            => Collect(transform, name, null, recursiveLimit, includeSelf)
                .Select(t => t.GetComponent<T>()).Where(c => c).ToList();

        public static List<T> Collect<T>(
            this Transform transform,
            List<Transform> list = null,
            int recursiveLimit = -1,
            bool includeSelf = false)
            where T : Component
            => Collect(transform, "*", null, recursiveLimit, includeSelf)
                .Select(t => t.GetComponent<T>()).Where(c => c).ToList();
    }
}
