using UnityEngine;

namespace Kit
{
    public static class Geom
    {
        public static (Vector3[] vertices, int[] triangles) SimpleCylinder(Vector3 start, Vector3 end, float radius = .2f, int step = 8)
        {
            Vector3 v = (end - start).normalized;
            Vector3 u = Vector3.Cross(v, Mathf.Abs(Vector3.Dot(v, Vector3.up)) < .9f ? Vector3.up : Vector3.right).normalized;
            Vector3 w = Vector3.Cross(v, u);

            var vertices = new Vector3[step * 2];
            var uv = new Vector2[step * 2];
            var triangles = new int[step * 3 * 2];

            for (int i = 0; i < step; i++)
            {
                float a = Mathf.PI * 2 * i / step;
                Vector3 r = (u * Mathf.Cos(a) + w * Mathf.Sin(a)) * radius;
                vertices[i] = start + r;
                vertices[step + i] = end + r;

                triangles[i * 3 * 2 + 0] = i;
                triangles[i * 3 * 2 + 1] = (i + 1) % step;
                triangles[i * 3 * 2 + 2] = i + step;

                triangles[i * 3 * 2 + 3] = (i + 1) % step;
                triangles[i * 3 * 2 + 4] = (i + 1) % step + step;
                triangles[i * 3 * 2 + 5] = i + step;
            }

            return (vertices, triangles);
        }

        // https://answers.unity.com/questions/861719/a-fast-triangle-triangle-intersection-algorithm-fo.html
        public static bool Intersect(Vector3 p1, Vector3 p2, Vector3 p3, Ray ray)
        {
            const float Epsilon = float.Epsilon;

            float det, invDet, u, v;
            Vector3 p, q, t;

            Vector3 v12 = p2 - p1;
            Vector3 v13 = p3 - p1;

            p = Vector3.Cross(ray.direction, v13);
            det = Vector3.Dot(v12, p);

            if (det > -float.Epsilon && det < float.Epsilon)
                return false;

            invDet = 1.0f / det;

            t = ray.origin - p1;
            u = Vector3.Dot(t, p) * invDet;

            if (u < 0 || u > 1)
                return false;

            q = Vector3.Cross(t, v12);
            v = Vector3.Dot(ray.direction, q) * invDet;

            if (v < 0 || u + v > 1)
                return false;

            if ((Vector3.Dot(v13, q) * invDet) > Epsilon)
                return true;

            return false;
        }
    }
}
