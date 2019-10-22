using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Kit
{
    public class GizmosMeshHandler : MonoBehaviour
    {
        static T[] Concat<T>(T[] a, T[] b)
        {
            var r = new T[a.Length + b.Length];

            a.CopyTo(r, 0);
            b.CopyTo(r, a.Length);

            return r;
        }

        public class Combo
        {
            public Mesh mesh;
            public Color color = Color.red;
            public Matrix4x4 matrix = Matrix4x4.identity;
            public int count;
        }

        public const int FramesBeforeDispose = 20;

        static GizmosMeshHandler instance;
        public static GizmosMeshHandler Instance
        {
            get
            {
                instance = instance ?? FindObjectOfType<GizmosMeshHandler>();

                if (instance == null)
                {
                    var existing = GameObject.Find(typeof(GizmosMeshHandler).Name);
                    if (existing)
                        DestroyImmediate(existing);

                    GameObject gameObject = new GameObject(typeof(GizmosMeshHandler).Name);
                    //gameObject.hideFlags = HideFlags.DontSaveInEditor; // causes strange lags???
                    instance = gameObject.AddComponent<GizmosMeshHandler>();
                }

                return instance;
            }
        }

        int drawCount;

        public Dictionary<object, Combo> dictionary =
            new Dictionary<object, Combo>();

        Combo GetCombo(object key) 
        {
            if (!dictionary.TryGetValue(key, out Combo combo))
            {
                combo = new Combo
                {
                    mesh = new Mesh { name = key.ToString() },
                    color = Color.red,
                    matrix = Matrix4x4.identity,
                    count = drawCount,
                };

                dictionary.Add(key, combo);
            }

            return combo;
        }

        public void SetColor(object key, Color color)
            => GetCombo(key).color = color;

        Combo DoAppend(object key, Vector3[] vertices, int[] triangles)
        {
            Combo combo = GetCombo(key);
            Mesh mesh = combo.mesh;

            if (combo.count < drawCount)
                mesh.Clear();

            combo.count = drawCount;

            int offset = mesh.vertices.Length;
            mesh.vertices = Concat(mesh.vertices, vertices);
            mesh.triangles = Concat(mesh.triangles, triangles.Select(index => index + offset).ToArray());

            return combo;
        }

        public void Append(object key, Vector3[] vertices, int[] triangles)
            => DoAppend(key, vertices, triangles);

        public void Append(object key, Color color, Vector3[] vertices, int[] triangles)
        {
            Combo combo = DoAppend(key, vertices, triangles);
            combo.color = color;
        }

        private void OnDrawGizmos()
        {
            foreach (Combo combo in dictionary.Values)
            {
                if (combo.count == drawCount)
                {
                    combo.mesh.RecalculateNormals();

                    Gizmos.color = combo.color;
                    Gizmos.matrix = combo.matrix;
                    Gizmos.DrawMesh(combo.mesh);
                }


            }

            if (drawCount % FramesBeforeDispose == 0)
            {
                foreach (var (key, combo) in dictionary.Select(pair => (pair.Key, pair.Value)).ToArray())
                    if (drawCount - combo.count > FramesBeforeDispose)
                        dictionary.Remove(key);
            }

            drawCount++;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(GizmosMeshHandler))]
    public class GizmosMeshHandlerEditor : Editor
    {
        GizmosMeshHandler Target { get { return target as GizmosMeshHandler; } }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.LabelField($"{Target.GetType().Name} Info:");

            string message =
                $"FrameBeforeDispose meshes: {GizmosMeshHandler.FramesBeforeDispose}\n" +
                $"Number of meshes: {Target.dictionary.Count}";

            EditorGUILayout.HelpBox(message, MessageType.None);
        }
    }
#endif
}
