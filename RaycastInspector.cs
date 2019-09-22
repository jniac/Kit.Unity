using System;
using UnityEngine;
using System.Linq;
using Kit.Utils;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Kit.Unity
{
    [ExecuteInEditMode]
    public class RaycastInspector : MonoBehaviour
    {
        const string NAME = "RayCast Inspector";

#if UNITY_EDITOR
        [MenuItem("GameObject/Kit/" + NAME)]
        static void CreateInstance()
        {
            var go = new GameObject(NAME);
            go.AddComponent<RaycastInspector>();
        }
#endif

        public enum Mode { FORWARD, BACKWARD, BOTH }

        public LayerMask layerMask = ~0;

        public Mode mode = Mode.BOTH;
        public Vector3 p0 = Vector3.zero;
        public Vector3 p1 = Vector3.right * 4;

        public int maxPrintCount = 10;

        bool selected;

        RaycastHit[] hits = new RaycastHit[0];

        private void OnEnable()
        {
            Compute();
        }

        public void Compute()
        {
            var ray = new Ray(p0, p1 - p0);
            var backRay = new Ray(p1, p0 - p1);
            var maxDistance = (p1 - p0).magnitude;

            switch(mode)
            {
                default:
                case Mode.FORWARD:
                    hits = Physics.RaycastAll(ray, maxDistance, layerMask);
                    break;

                case Mode.BACKWARD:
                    hits = Physics.RaycastAll(backRay, maxDistance, layerMask);
                    break;

                case Mode.BOTH:
                    hits = Physics.RaycastAll(ray, maxDistance, layerMask)
                        .Concat(Physics.RaycastAll(backRay, maxDistance, layerMask))
                        .OrderBy(hit => hit.distance)
                        .ToArray();
                    break;
            }
        }

        private void OnValidate()
        {
            Compute();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            selected = Selection.activeGameObject == gameObject;

            Gizmos.color = selected ? Color.yellow : Color.white;

            Gizmos.DrawLine(p0, p1);

            if (!selected)
            {
                Gizmos.DrawSphere(p0, .05f);
                Gizmos.DrawSphere(p1, .05f);
            }

            foreach (var hit in hits)
                Gizmos.DrawSphere(hit.point, selected ? .1f : .075f);

            GUIStyle style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
            style.normal.textColor = Gizmos.color;
            style.fixedWidth = 300;
            Handles.Label(p0 + Vector3.up * .25f, selected ? $"P0{p0.ToString("0.0")}" : "P0", style);
            Handles.Label(p1 + Vector3.up * .25f, selected ? $"P1{p1.ToString("0.0")}" : "P1", style);
        }


        [CustomEditor(typeof(RaycastInspector))]
        public class MyEditor : Editor
        {
            RaycastInspector Target { get => target as RaycastInspector; }

            private void OnEnable()
            {
                Tools.hidden = true;
            }

            private void OnDisable()
            {
                Tools.hidden = false;
            }

            private void OnSceneGUI()
            {
                Handles.color = Target.selected ? Color.yellow : Color.white;
                Handles.DrawLine(Target.p0, Target.p1);

                EditorGUI.BeginChangeCheck();
                var p0 = Handles.DoPositionHandle(Target.p0, Quaternion.identity);
                if (EditorGUI.EndChangeCheck())
                    Target.p0 = p0;

                EditorGUI.BeginChangeCheck();
                var p1 = Handles.DoPositionHandle(Target.p1, Quaternion.identity);
                if (EditorGUI.EndChangeCheck())
                    Target.p1 = p1;

                if (GUI.changed)
                    Target.Compute();
            }

            string ColliderInfo(Collider collider)
            {
                if (collider == null)
                    return null;

                return collider.ToString() + 
                    " layer: " + LayerMask.LayerToName(collider.gameObject.layer);
            }

            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                int max = Target.maxPrintCount;
                bool tooLong = Target.hits.Length > max;
                string colliderMessage =
                    (tooLong ? Target.hits.Take(max) : Target.hits)
                    .Select(h => ColliderInfo(h.collider) ?? "Collider has been destroyed")
                    .Concat(tooLong ? new string[] { $"(+{Target.hits.Length - max})" } : new string[0])
                    .StringJoin("\n");

                string message = $"hits: {Target.hits.Length} (mask: {(int)Target.layerMask})\n{colliderMessage}";
                EditorGUILayout.HelpBox(message, MessageType.None);
            }
        }
#endif
    }
}
