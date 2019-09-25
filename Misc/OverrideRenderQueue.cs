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
        public bool overrideRenderQueue;
        public int renderQueue;

        private void OnValidate()
        {
            if (overrideRenderQueue)
            {
                foreach(var m in GetComponent<MeshRenderer>().sharedMaterials)
                {
                    m.renderQueue = renderQueue;
                }
            }
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
