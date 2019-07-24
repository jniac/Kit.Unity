using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Kit.Unity
{
    [ExecuteInEditMode, RequireComponent(typeof(MeshFilter))]
    public class MeshInspector : MonoBehaviour
    {
        public Color color = Color.red;
        [Range(0, 1)]
        public float alpha = .1f;

        [NonSerialized]
        public bool inspectTriangle = true;
        [NonSerialized]
        public int triangleIndex = 0;

        public Mesh Mesh { get { return GetComponent<MeshFilter>().sharedMesh; } }
        public int TriangleCount { get { return Mesh.triangles.Length / 3; } }
        public int TriangleIndexMax { get { return TriangleCount - 1; } }
        public void TriangleIndexNext() { triangleIndex = (triangleIndex + 1) % TriangleCount; }
        public void TriangleIndexPrev() { triangleIndex = triangleIndex > 0 ? triangleIndex - 1 : TriangleIndexMax; }

        public string GetMeshInfo()
        {
            Mesh mesh = Mesh;

            if (mesh == null)
                return "no Mesh!";

            return $"Mesh \"{mesh.name}\"" +
                $"\nTriangles: {mesh.triangles.Length / 3}" +
                $"\nVertices: {mesh.vertices.Length}" +
                $"\nsubMeshCount: {mesh.subMeshCount}";
        }

        public (Vector3, Vector3, Vector3) GetTriangle(Mesh mesh, int index)
        {
            return (
                mesh.vertices[mesh.triangles[index * 3]],
                mesh.vertices[mesh.triangles[index * 3 + 1]],
                mesh.vertices[mesh.triangles[index * 3 + 2]]
            );
        }

        private void OnDrawGizmos()
        {
            Mesh mesh = Mesh;

            if (mesh == null || inspectTriangle == false)
                return;

            if (triangleIndex < 0 || triangleIndex >= mesh.triangles.Length * 3)
                return;

            Gizmos.color = color;
            Gizmos.matrix = transform.localToWorldMatrix;

            var (p0, p1, p2) = GetTriangle(mesh, triangleIndex);
            var C = (p0 + p1 + p2) / 3;
            var N = Vector3.Cross(p1 - p0, p2 - p0);
            Gizmos.DrawLine(p0, p1);
            Gizmos.DrawLine(p1, p2);
            Gizmos.DrawLine(p2, p0);
            Gizmos.DrawRay(C, N);

            GizmosMeshHandler.Instance.Append(this, Gizmos.color,
                new Vector3[] { p0, p1, p2 },
                new int[] { 0, 1, 2 });
        }

    }


#if UNITY_EDITOR
    [CustomEditor(typeof(MeshInspector))]
    public class MeshInspectorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var Target = target as MeshInspector;

            EditorGUILayout.LabelField("Mesh Info:");
            EditorGUILayout.HelpBox(Target.GetMeshInfo(), MessageType.None);

            Mesh mesh = Target.Mesh;

            GUI.changed = false;
            int maxTriangleIndex = mesh ? Target.Mesh.triangles.Length / 3 - 1 : 0;

            GUI.enabled = mesh;
            Target.inspectTriangle = EditorGUILayout.Toggle("Inspect triangle", Target.inspectTriangle);
            Target.triangleIndex = EditorGUILayout.IntSlider($"Triangle index ({Target.triangleIndex}/{maxTriangleIndex}):", Target.triangleIndex, 0, maxTriangleIndex);
            if (GUILayout.Button("Next"))
                Target.TriangleIndexNext();
            if (GUILayout.Button("Previous"))
                Target.TriangleIndexPrev();

            GUI.enabled = true;
        }
    }
#endif
}
