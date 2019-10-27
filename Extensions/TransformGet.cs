using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Kit
{
    public static class TransformGet
    {
        // Get() replace Collect()
        // Get() is based on IEnumerable, so it's better, since IEnumerable<T> are lazy.
        public static IEnumerable<T> Get<T>(this Transform transform,
            Func<Transform, bool> test = null, bool includeSelf = false, int recursiveLimit = -1)
            where T : Component
        {
            if (includeSelf)
                if (test == null || test(transform))
                    foreach (T c in transform.GetComponents<T>())
                        yield return c;

            if (recursiveLimit != 0)
            {
                foreach (Transform child in transform)
                    if (test == null || test(child))
                        foreach (T c in child.GetComponents<T>())
                            yield return c;

                foreach (Transform child in transform)
                    foreach (T c in Get<T>(child, test, false, recursiveLimit - 1))
                        yield return c;
            }
        }

        // NOTE: standby
        public static Func<string, bool> Match(string mask) =>
            name => mask[0] == '!' ? name != mask.Substring(1) : name == mask;

        public static IEnumerable<T> Get<T>(this Transform transform,
            string query, bool includeSelf = false, int recursiveLimit = -1)
            where T : Component
        {
            if (query.Contains(','))
            {
                var parts = new Regex(@"\s*,\s*").Split(query);

                return Get<T>(transform, t => parts.Contains(t.gameObject.name), includeSelf, recursiveLimit);
            }

            if (query[0] == '!')
            {
                query = query.Substring(1);
                return Get<T>(transform, t => t.gameObject.name != query, includeSelf, recursiveLimit);
            }

            return Get<T>(transform, t => t.gameObject.name == query, includeSelf, recursiveLimit);
        }

        public static IEnumerable<Transform> Get(this Transform transform,
            string query, bool includeSelf = false, int recursiveLimit = -1) =>
            Get<Transform>(transform, query, includeSelf, recursiveLimit);





        public static T First<T>(this Transform transform,
            Func<Transform, bool> test = null, bool includeSelf = false, int recursiveLimit = -1, bool throwError = true)
            where T : Component
        {
            var result = Get<T>(transform, test, includeSelf, recursiveLimit);

            if (result.Any())
                return result.First();

            if (throwError)
                throw new Exception($"oups, no child match \"{test}\"");

            return null;
        }

        public static T First<T>(this Transform transform,
            string query, bool includeSelf = false, int recursiveLimit = -1, bool throwError = true)
            where T : Component
        {
            var result = Get<T>(transform, query, includeSelf, recursiveLimit);

            if (result.Any())
                return result.First();

            if (throwError)
                throw new Exception($"oups, no child match \"{query}\"");

            return null;
        }

        public static Transform First(this Transform transform,
            string query, bool includeSelf = false, int recursiveLimit = -1, bool throwError = true) =>
            First<Transform>(transform, query, includeSelf, recursiveLimit, throwError);
    }
}
