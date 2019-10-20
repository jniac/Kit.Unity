using System;
using UnityEngine;
using System.Linq;
using Kit.Utils;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Kit.Unity
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
            mr.sharedMaterial = clone;
        }

        void OnValidate()
        {
            if (clone && overrideRenderQueue)
                clone.renderQueue = renderQueue;
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(OverrideRenderQueue))]
        class MyEditor : Editor
        {
            OverrideRenderQueue Target => target as OverrideRenderQueue;

            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                if (!Target.overrideRenderQueue)
                    Misc.HelpBox(
                        "Current render queue:",
                        Target.GetComponent<MeshRenderer>().sharedMaterials.Select(m => m.renderQueue).StringJoin(),
                        null);

                if (GUILayout.Button("Remove"))
                    Target.gameObject.RemoveComponent<OverrideRenderQueue>();
            }
        }
#endif
    }
}
