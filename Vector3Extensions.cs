using System;
using UnityEngine;

namespace Kit.Unity
{
    public static class Vector3Extensions
    {
        public static Vector2 XZ(this Vector3 v) => new Vector2(v.x, v.z);
    }
}
