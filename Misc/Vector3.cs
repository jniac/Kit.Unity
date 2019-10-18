using System;
using UnityEngine;

namespace Kit.Unity
{
    public static partial class Misc
    {
        public static Vector3 WithX(Vector3 v, float x)
        {
            return new Vector3(x, v.y, v.z);
        }

        public static Vector3 WithX(ref Vector3 v, float x)
        {
            v.x = x;

            return v;
        }
    }
}
