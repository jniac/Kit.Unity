using System;
using UnityEngine;
using System.Linq;
using Kit;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Kit
{
    [ExecuteAlways]
    public class OverrideRenderQueue : MonoBehaviour
    {
        public Material original;
        public Material clone;

        public bool overrideRenderQueue;
        public int renderQueue;

        MeshRenderer mr;

        void Awake()
        {
            mr = GetComponent<MeshRenderer>();
            original = mr.sharedMaterial;
            clone = new Material(original) { name = $"{original.name}(clone)" };
            mr.sharedMaterial = clone;

            if (overrideRenderQueue)
                clone.renderQueue = renderQueue;
        }

        private void OnEnable()
        {
            mr.sharedMaterial = overrideRenderQueue ? clone : original;
        }

        void OnValidate()
        {
            if (clone)
            {
                mr.sharedMaterial = overrideRenderQueue ? clone : original;
                clone.renderQueue = renderQueue;
            }
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(OverrideRenderQueue))]
        class MyEditor : Editor
        {
            OverrideRenderQueue Target => target as OverrideRenderQueue;

            public override void OnInspectorGUI()
            {
                EditorGUI.BeginChangeCheck();

                Target.overrideRenderQueue =
                    EditorGUILayout.Toggle("Override", Target.overrideRenderQueue);

                GUI.enabled = Target.overrideRenderQueue;

                if (!Target.overrideRenderQueue)
                {
                    var actual = Target.GetComponent<MeshRenderer>().sharedMaterials.Select(m => m.renderQueue).StringJoin();
                    EditorGUILayout.TextField("Render Queue", actual);
                }
                else
                {
                    Target.renderQueue =
                        EditorGUILayout.IntField("Render Queue", Target.renderQueue);
                }

                //base.OnInspectorGUI();

                //if (!Target.overrideRenderQueue)
                Misc.HelpBox(
                "Current render queue:",
                Target.GetComponent<MeshRenderer>().sharedMaterials.Select(m => m.renderQueue).StringJoin(),
                null);

                if (EditorGUI.EndChangeCheck())
                    Target.OnValidate();

                if (GUILayout.Button("Remove"))
                    Target.gameObject.RemoveComponent<OverrideRenderQueue>();
            }
        }
#endif
    }
}
