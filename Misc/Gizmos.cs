using System;
using UnityEngine;

namespace Kit.Unity
{
    public static partial class Misc
    {
        static float previousColorAlpha;
        public static void GizmosSetColorAlpha(float a)
        {
            var color = Gizmos.color;
            previousColorAlpha = color.a;
            color.a = a;
            Gizmos.color = color;
        }

        public static void GizmosRestoreColorAlpha()
        {
            var color = Gizmos.color;
            color.a = 1;
            Gizmos.color = color;
        }

        public static void GizmosWithColor(Color color, Action callback)
        {
            Color before = Gizmos.color;
            Gizmos.color = color;
            callback();
            Gizmos.color = before;
        }





        private static Mesh gizmosDrawMesh;
        private static Mesh GetGizmosDrawMesh() => gizmosDrawMesh ?? (gizmosDrawMesh = new Mesh());

        public static void GizmosDrawTriangle(object key, Vector3 vertex0, Vector3 vertex1, Vector3 vertex2)
        {
            GizmosMeshHandler.Instance.SetColor(key, Gizmos.color);
            GizmosMeshHandler.Instance.Append(key,
                new Vector3[] { vertex0, vertex1, vertex2 },
                new int[] { 0, 1, 2 });
        }

        public static void GizmosDrawTriangle<T>(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2)
            => GizmosDrawTriangle(typeof(T), vertex0, vertex1, vertex2);

        public static void GizmosDrawQuad(object key, Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3)
        {
            GizmosMeshHandler.Instance.SetColor(key, Gizmos.color);
            GizmosMeshHandler.Instance.Append(key,
                new Vector3[] { vertex0, vertex1, vertex2, vertex3 },
                new int[] { 0, 1, 3, 1, 2, 3 });
        }

        public static void GizmosDrawQuad<T>(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3)
            => GizmosDrawQuad(typeof(T), vertex0, vertex1, vertex2, vertex3);

        public static void GizmosDrawCylinder(Vector3 start, Vector3 end, float radius = .2f, int step = 8)
        {
            var (vertices, uv, triangles) = Geom.SimpleCylinder(start, end, radius, step);

            Mesh mesh = GetGizmosDrawMesh();

            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            Gizmos.DrawMesh(mesh);
        }
    }
}
