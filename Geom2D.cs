using UnityEngine;

namespace Kit
{
    public static class Geom2D
    {
        // https://github.com/jniac/particles-js/blob/master/src/geom.js
        public static (bool intersects, float k1, float k2) GetK(
            float p1x, float p1y, 
            float v1x, float v1y, 
            float p2x, float p2y, 
            float v2x, float v2y)
        {
            float det = v1x * v2y - v1y * v2x;

            if (det == 0)
                return (false, 0, 0);

            float ppx = p2x - p1x;
            float ppy = p2y - p1y;

            float k1 = (ppx * v2y - ppy * v2x) / det;
            float k2 = (ppx * v1y - ppy * v1x) / det;

            return (true, k1, k2);
        }
        public static (bool intersects, float k1, float k2) GetK(Vector2 p1, Vector2 v1, Vector2 p2, Vector2 v2) =>
            GetK(p1.x, p1.y, v1.x, v1.y, p2.x, p2.y, v2.x, v2.y);

        public static bool RayIntersectsSegment(
            float p1x, float p1y, 
            float v1x, float v1y, 
            float p2x, float p2y, 
            float v2x, float v2y)
        {
            var (intersects, k1, k2) = GetK(p1x, p1y, v1x, v1y, p2x, p2y, v2x, v2y);

            return k1 >= 0 && k2 >= 0 && k2 <= 1;
        }
        public static bool RayIntersectsSegment(Vector2 p1, Vector2 v1, Vector2 p2, Vector2 v2) =>
            RayIntersectsSegment(p1.x, p1.y, v1.x, v1.y, p2.x, p2.y, v2.x, v2.y);

    }
}
