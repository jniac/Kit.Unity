using System;
using UnityEngine;

namespace Kit.Unity
{
    public static class Vector3Extensions
    {
        public static Vector2 XY(this Vector3 v) => new Vector2(v.x, v.y);
        public static Vector2 XY(this Vector3 v, float x, float y) => new Vector2(v.x * x, v.y * y);

        public static Vector2 XZ(this Vector3 v) => new Vector2(v.x, v.z);

        public static Vector2 YZ(this Vector3 v) => new Vector2(v.y, v.z);

        public static Vector2 ZY(this Vector3 v) => new Vector2(v.z, v.y);
        public static Vector2 ZY(this Vector3 v, float z, float y) => new Vector2(v.z * z, v.y * y);
    }
}
