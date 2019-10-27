using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Kit
{
    /// <summary>
    /// Get query, based on Transform.Get() extension
    /// </summary>
    public static class GameObjectGet
    {
        // Get()
        public static IEnumerable<T> Get<T>(this GameObject gameObject,
            Func<Transform, bool> test = null, bool includeSelf = false, int recursiveLimit = -1)
            where T : Component =>
            gameObject.transform.Get<T>(test, includeSelf, recursiveLimit);

        public static IEnumerable<T> Get<T>(this GameObject gameObject,
            string query, bool includeSelf = false, int recursiveLimit = -1)
            where T : Component =>
            gameObject.transform.Get<T>(query, includeSelf, recursiveLimit);

        public static IEnumerable<GameObject> Get(this GameObject gameObject,
            string query, bool includeSelf = false, int recursiveLimit = -1) =>
            gameObject.transform.Get<Transform>(query, includeSelf, recursiveLimit)
            .Select(transform => transform.gameObject);

        // First()
        public static T First<T>(this GameObject gameObject,
            Func<Transform, bool> test = null, bool includeSelf = false, int recursiveLimit = -1)
            where T : Component =>
            gameObject.transform.First<T>(test, includeSelf, recursiveLimit);

        public static T First<T>(this GameObject gameObject,
            string query, bool includeSelf = false, int recursiveLimit = -1)
            where T : Component =>
            gameObject.transform.First<T>(query, includeSelf, recursiveLimit);

        public static GameObject First(this GameObject gameObject,
            string query, bool includeSelf = false, int recursiveLimit = -1) =>
            gameObject.transform.First<Transform>(query, includeSelf, recursiveLimit)
            .gameObject;
    }
}
