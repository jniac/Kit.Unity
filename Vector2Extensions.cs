using System;
using UnityEngine;

namespace Kit.Unity
{
    public static class Vector2Extensions
    {
        public static float GetArea(this Vector2 v) =>
            v.x * v.y;

        public static float GetAngle(this Vector2 v) =>
            Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }
}
