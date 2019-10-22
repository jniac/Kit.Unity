using System;
using UnityEngine;

namespace Kit
{
    public static partial class Misc
    {
        public static Vector3 WithX(Vector3 v, float x) =>
            new Vector3(x, v.y, v.z);

        public static Vector3 WithZ(Vector3 v, float z) =>
            new Vector3(v.x, v.y, z);

        public static Vector3 WithX(ref Vector3 v, float x)
        {
            v.x = x;

            return v;
        }
    }
}
